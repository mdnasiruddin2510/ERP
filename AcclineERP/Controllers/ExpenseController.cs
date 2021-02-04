using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Data.Context;
using System.Data.SqlClient;
using AcclineERP.Models;
using Newtonsoft.Json;
using System.Transactions;

namespace AcclineERP.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ICashOperationAppService _CashOperationService;
        private readonly ICashReceiptAppService _cashReceiptAppService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly IExpenseAppService _ExpenseService;
        private readonly IDepositToBankAppService _DepositToBankService;
        private readonly INewChartAppService _NewChartService;
        private readonly ISubsidiaryInfoAppService _subsidiaryService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IItemInfoAppService _ItemService;
        private readonly ILocationAppService _LocationService;
        private readonly IUnitAppService _UnitService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly IExpenseSubDetailsAppService _expenseSubDetailsService;
        private readonly IPVchrMainAppService _pVchrMainAppService;
        private readonly IPVchrDetailAppService _pVchrDetailAppService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IGsetAppService _gsetService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ILotDTAppService _lotDTService;
        private readonly IPayInvRecvAppService _PayInvRecvService;
        private readonly IAcBRAppService _AcBrService;
        public ExpenseController(ICashOperationAppService _CashOperationService, ICashReceiptAppService _cashReceiptAppService,
            IWithdrawAppService _withdrawService, IExpenseAppService _ExpenseService, IDepositToBankAppService _DepositToBankService,
            IEmployeeAppService _EmployeeService, ISubsidiaryInfoAppService _subsidiaryService, IItemInfoAppService _ItemService,
            INewChartAppService _NewChartService, ILocationAppService _LocationService, IUnitAppService _UnitService,
            ITransactionLogAppService _transactionLogService, IReceiveAppService _receiveMainService,
            IExpenseSubDetailsAppService _expenseSubDetailsService, IPVchrMainAppService _pVchrMainAppService,
            IPVchrDetailAppService _pVchrDetailAppService, IJarnalVoucherAppService _jarnalVoucherService,
            ISysSetAppService _sysSetService, IGsetAppService _gsetService, ICurrentStockAppService _currentStockService,
            ILotDTAppService _lotDTService, IPayInvRecvAppService _PayInvRecvService, IReceiveDetailsAppService _receiveDetailsService,
            IAcBRAppService _AcBrService)
        {
            this._CashOperationService = _CashOperationService;
            this._cashReceiptAppService = _cashReceiptAppService;
            this._withdrawService = _withdrawService;
            this._ExpenseService = _ExpenseService;
            this._DepositToBankService = _DepositToBankService;
            this._subsidiaryService = _subsidiaryService;
            this._NewChartService = _NewChartService;
            this._EmployeeService = _EmployeeService;
            this._subsidiaryService = _subsidiaryService;
            this._ItemService = _ItemService;
            this._LocationService = _LocationService;
            this._UnitService = _UnitService;
            this._transactionLogService = _transactionLogService;
            this._receiveMainService = _receiveMainService;
            this._receiveDetailsService = _receiveDetailsService;
            this._expenseSubDetailsService = _expenseSubDetailsService;
            this._pVchrMainAppService = _pVchrMainAppService;
            this._pVchrDetailAppService = _pVchrDetailAppService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._gsetService = _gsetService;
            this._currentStockService = _currentStockService;
            this._lotDTService = _lotDTService;
            this._PayInvRecvService = _PayInvRecvService;
            this._AcBrService = _AcBrService;
        }


        [HttpPost]
        public ActionResult SaveExpense(Expense expense, ReceiveMain recvInfo, List<ReceiveDetails> recvDetails, List<CurrentStock> currentStock)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CashPayment_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();

            try
            {
                var IfExit = _ExpenseService.All().Where(x => x.ExpenseNo == expense.ExpenseNo).FirstOrDefault();
                if (IfExit == null)
                {
                    if (!string.IsNullOrEmpty(expense.expPurAccode) && expense.expPurAccode.Trim() != "Select Purpose")
                    {

                        List<Expense> expenselist = new List<Expense>();
                        expense.BranchCode = Session["BranchCode"].ToString();

                        var GCa = _AcBrService.All().Where(s => s.BranchCode == expense.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        expense.FinYear = Session["FinYear"].ToString();
                        expense.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == expense.expPurAccode.Trim());
                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        CashOperation cashOperation = new CashOperation(expense.ExpenseDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, expense.FinYear, false, expense.BranchCode);
                        var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == expense.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == expense.ExpenseDate.ToString("MM-dd-yyyy"));
                        if (isAlreadyClosed == null)
                        {

                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _CashOperationService.Add(cashOperation);
                                    _CashOperationService.Save();

                                    _ExpenseService.Add(expense);
                                    _ExpenseService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                    CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);

                                    #region for Receive
                                    var ExitRecv = _receiveMainService.All().Where(x => x.RcvNo == recvInfo.RcvNo).FirstOrDefault();
                                    if (recvDetails != null && ExitRecv == null)
                                    {
                                        var todayDate = DateTime.Now;
                                        ReceiveMain recvMain = new ReceiveMain();
                                        recvMain.RcvNo = recvInfo.RcvNo;
                                        recvMain.BranchCode = Session["BranchCode"].ToString();
                                        recvMain.RcvDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.FromLocCode = "";
                                        recvMain.ToLocCode = "";
                                        recvMain.Source = "";
                                        recvMain.Accode = expense.expPurAccode.Trim();
                                        recvMain.RefNo = "";
                                        recvMain.RefDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.Remarks = recvInfo.Remarks;
                                        recvMain.RcvdByCode = "";
                                        recvMain.AppByCode = "";
                                        recvMain.RcvdTime = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.StoreLocCode = recvInfo.StoreLocCode;
                                        recvMain.RecvFromSubCode = "";
                                        recvMain.VchrNo = recvInfo.VchrNo;
                                        recvMain.FinYear = Session["FinYear"].ToString();
                                        recvMain.GLPost = false;
                                        recvMain.RcvdDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.expenseStatus = false;
                                        recvMain.CreditPur = false;


                                        double amount = 0.0;

                                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                                        foreach (var recvDetailsItem in recvDetails)
                                        {
                                            ReceiveDetails receivDetails = new ReceiveDetails();
                                            receivDetails.RcvNo = recvDetailsItem.RcvNo;
                                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                            receivDetails.SubCode = "";
                                            receivDetails.Description = recvDetailsItem.Description;
                                            receivDetails.Qty = recvDetailsItem.Qty;
                                            receivDetails.Price = recvDetailsItem.Price;
                                            receivDetails.ExQty = recvDetailsItem.ExQty;
                                            receivDetails.LotNo = recvDetailsItem.LotNo;

                                            amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                            receveDetailsList.Add(receivDetails);
                                        }

                                        recvMain.Amount = amount;

                                        foreach (var currentItem in currentStock)
                                        {
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                m.LocCode == recvMain.StoreLocCode &&
                                                m.LotNo == currentItem.LotNo);
                                            if (currentStocks != null)
                                            {
                                                currentStocks.CurrQty = currentItem.CurrQty + currentStocks.CurrQty;
                                                currentStocks.UnitPrice = currentItem.UnitPrice;
                                                _currentStockService.Update(currentStocks);
                                            }
                                            else
                                            {
                                                CurrentStock currStock = new CurrentStock();
                                                currStock.LocCode = recvInfo.StoreLocCode;
                                                currStock.LotNo = currentItem.LotNo;
                                                currStock.ItemCode = currentItem.ItemCode;
                                                currStock.CurrQty = currentItem.CurrQty;
                                                currStock.UnitPrice = currentItem.UnitPrice;
                                                _currentStockService.Add(currStock);
                                            }

                                            var lotNO = _lotDTService.All().ToList().Where(m => m.LotNo == currentItem.LotNo).FirstOrDefault();
                                            if (lotNO == null)
                                            {
                                                LotDT lotDt = new LotDT();
                                                lotDt.LotNo = currentItem.LotNo;
                                                lotDt.LotDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                                lotDt.RefNo = recvInfo.RcvNo;
                                                lotDt.RefSource = "Receive";
                                                _lotDTService.Add(lotDt);
                                            }
                                        }
                                        recvMain.RcvDetails = receveDetailsList;
                                        _receiveMainService.Add(recvMain);
                                        _currentStockService.Save();
                                        _receiveMainService.Save();
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Save", recvMain.RcvNo, Session["UserName"].ToString());
                                        LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), "01", Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());

                                        PayInvRecv PInvRecv = new PayInvRecv();
                                        PInvRecv.TranNo = expense.ExpenseNo;
                                        PInvRecv.RefNo = recvInfo.RcvNo;
                                        PInvRecv.PType = "C";
                                        _PayInvRecvService.Add(PInvRecv);
                                        _PayInvRecvService.Save();
                                    }
                                    #endregion

                                    transaction.Complete();
                                    return Json(new { CashOPVM, data = 4 }, JsonRequestBehavior.AllowGet);
                                }
                                catch (Exception ex)
                                {
                                    transaction.Dispose();
                                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else if (isAlreadyClosed != null && sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _ExpenseService.Add(expense);
                                    _ExpenseService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), expense.BranchCode, expense.FinYear, Convert.ToDecimal(expense.Amount), expense.ExpenseDate);                                       
                                    }                                    
                                    CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);

                                    #region for Receive
                                    var ExitRecv = _receiveMainService.All().Where(x => x.RcvNo == recvInfo.RcvNo).FirstOrDefault();
                                    if (recvDetails != null && ExitRecv == null)
                                    {
                                        var todayDate = DateTime.Now;
                                        ReceiveMain recvMain = new ReceiveMain();
                                        recvMain.RcvNo = recvInfo.RcvNo;
                                        recvMain.BranchCode = Session["BranchCode"].ToString();
                                        recvMain.RcvDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.FromLocCode = "";
                                        recvMain.ToLocCode = "";
                                        recvMain.Source = "";
                                        recvMain.Accode = expense.expPurAccode.Trim();
                                        recvMain.RefNo = "";
                                        recvMain.RefDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.Remarks = recvInfo.Remarks;
                                        recvMain.RcvdByCode = "";
                                        recvMain.AppByCode = "";
                                        recvMain.RcvdTime = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.StoreLocCode = recvInfo.StoreLocCode;
                                        recvMain.RecvFromSubCode = "";
                                        recvMain.VchrNo = recvInfo.VchrNo;
                                        recvMain.FinYear = Session["FinYear"].ToString();
                                        recvMain.GLPost = false;
                                        recvMain.RcvdDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        recvMain.expenseStatus = false;
                                        recvMain.CreditPur = false;


                                        double amount = 0.0;

                                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                                        foreach (var recvDetailsItem in recvDetails)
                                        {
                                            ReceiveDetails receivDetails = new ReceiveDetails();
                                            receivDetails.RcvNo = recvDetailsItem.RcvNo;
                                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                            receivDetails.SubCode = "";
                                            receivDetails.Description = recvDetailsItem.Description;
                                            receivDetails.Qty = recvDetailsItem.Qty;
                                            receivDetails.Price = recvDetailsItem.Price;
                                            receivDetails.ExQty = recvDetailsItem.ExQty;
                                            receivDetails.LotNo = recvDetailsItem.LotNo;

                                            amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                            receveDetailsList.Add(receivDetails);
                                        }

                                        recvMain.Amount = amount;

                                        foreach (var currentItem in currentStock)
                                        {
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                m.LocCode == recvMain.StoreLocCode &&
                                                m.LotNo == currentItem.LotNo);
                                            if (currentStocks != null)
                                            {
                                                currentStocks.CurrQty = currentItem.CurrQty + currentStocks.CurrQty;
                                                currentStocks.UnitPrice = currentItem.UnitPrice;
                                                _currentStockService.Update(currentStocks);
                                            }
                                            else
                                            {
                                                CurrentStock currStock = new CurrentStock();
                                                currStock.LocCode = recvInfo.StoreLocCode;
                                                currStock.LotNo = currentItem.LotNo;
                                                currStock.ItemCode = currentItem.ItemCode;
                                                currStock.CurrQty = currentItem.CurrQty;
                                                currStock.UnitPrice = currentItem.UnitPrice;
                                                _currentStockService.Add(currStock);
                                            }

                                            var lotNO = _lotDTService.All().ToList().Where(m => m.LotNo == currentItem.LotNo).FirstOrDefault();
                                            if (lotNO == null)
                                            {
                                                LotDT lotDt = new LotDT();
                                                lotDt.LotNo = currentItem.LotNo;
                                                lotDt.LotDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                                lotDt.RefNo = recvInfo.RcvNo;
                                                lotDt.RefSource = "Receive";
                                                _lotDTService.Add(lotDt);
                                            }
                                        }
                                        recvMain.RcvDetails = receveDetailsList;
                                        _receiveMainService.Add(recvMain);
                                        _currentStockService.Save();
                                        _receiveMainService.Save();
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Save", recvMain.RcvNo, Session["UserName"].ToString());
                                        LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), "01", Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());

                                        PayInvRecv PInvRecv = new PayInvRecv();
                                        PInvRecv.TranNo = expense.ExpenseNo;
                                        PInvRecv.RefNo = recvInfo.RcvNo;
                                        PInvRecv.PType = "C";
                                        _PayInvRecvService.Add(PInvRecv);
                                        _PayInvRecvService.Save();
                                    }

                                    #endregion
                                    transaction.Complete();
                                    return Json(new { CashOPVM, data = 4 }, JsonRequestBehavior.AllowGet);
                                }
                                catch (Exception ex)
                                {
                                    transaction.Dispose();
                                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                        else
                        {
                            return Json("2", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("3", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateExpense(Expense expense, DateTime dateTime, ReceiveMain recvInfo, List<ReceiveDetails> recvDetails, List<CurrentStock> currentStock)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CashPayment_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            if (ModelState.IsValid)
            {
                expense.BranchCode = Session["BranchCode"].ToString();

                var GCa = _AcBrService.All().Where(s => s.BranchCode == expense.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                if (GCa == null)
                {
                    var gset = _gsetService.All().LastOrDefault();
                    GCa = gset.GCa;
                }
                var sysSet = _sysSetService.All().FirstOrDefault();
                expense.FinYear = Session["FinYear"].ToString();
                expense.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == expense.expPurAccode.Trim());
                CashOperation cashOperation = new CashOperation();
                var isClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == expense.BranchCode && Convert.ToDateTime(x.TrDate) <= Convert.ToDateTime(dateTime));
                if (sysSet.CashRule == "O" ? true: isClosed.IsClosed == false)
                {

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            _ExpenseService.Update(expense);
                            _ExpenseService.Save();
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Update", expense.ExpenseNo, Session["UserName"].ToString());
                            //delete to provision                           
                            var entryNo = expense.ExpenseNo;
                            LoadDropDown.journalVoucherRemoval("CP", entryNo, expense.VoucherNo, expense.FinYear);
                            //insert to provision 
                            LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                            CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);

                            var recvMain = _receiveMainService.All().FirstOrDefault(x => x.RcvNo == recvInfo.RcvNo);
                            if (recvMain != null && recvDetails != null)
                            {
                                var todayDate = DateTime.Now;
                                recvMain.RcvNo = recvInfo.RcvNo;
                                recvMain.BranchCode = Session["BranchCode"].ToString();
                                recvMain.RcvDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.FromLocCode = "";
                                recvMain.ToLocCode = "";
                                recvMain.Source = "";
                                recvMain.Accode = expense.expPurAccode.Trim();
                                recvMain.RefNo = expense.RefNo;
                                recvMain.RefDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.Remarks = recvInfo.Remarks;
                                recvMain.RcvdByCode = "";
                                recvMain.AppByCode = "";
                                recvMain.RcvdTime = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.StoreLocCode = recvInfo.StoreLocCode;
                                recvMain.RecvFromSubCode = "";
                                recvMain.VchrNo = recvInfo.VchrNo;
                                recvMain.FinYear = Session["FinYear"].ToString();
                                recvMain.GLPost = false;
                                recvMain.RcvdDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.expenseStatus = false;
                                recvMain.CreditPur = false;
                                double amount = 0.0;
                                foreach (var recvDetailsItem in recvDetails)
                                {
                                    ReceiveDetails receivDetails = new ReceiveDetails();
                                    receivDetails.RcvNo = recvMain.RcvNo;
                                    receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                    receivDetails.Description = recvDetailsItem.Description;
                                    receivDetails.Qty = recvDetailsItem.Qty;
                                    receivDetails.Price = recvDetailsItem.Price;
                                    receivDetails.ExQty = recvDetailsItem.ExQty;
                                    receivDetails.LotNo = recvDetailsItem.LotNo;
                                    amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                    recvMain.RcvDetails.Add(receivDetails);
                                }
                                recvMain.Amount = amount;
                                var recvDetail = _receiveDetailsService.All().ToList().Where(m => m.RcvNo == recvMain.RcvNo).ToList();
                                foreach (var item in recvDetail)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == item.ItemCode &&
                                        m.LocCode == recvMain.StoreLocCode &&
                                        m.LotNo == item.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty - item.Qty;
                                        _currentStockService.Update(currentStocks);
                                        _currentStockService.Save();
                                    }
                                }
                                foreach (var currentItem in currentStock)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                        m.LocCode == recvMain.StoreLocCode &&
                                        m.LotNo == currentItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty + currentItem.CurrQty;
                                        currentStocks.UnitPrice = currentItem.UnitPrice;
                                        _currentStockService.Update(currentStocks);
                                    }
                                    else
                                    {
                                        CurrentStock currStock = new CurrentStock();
                                        currStock.LocCode = recvMain.StoreLocCode;
                                        currStock.LotNo = currentItem.LotNo;
                                        currStock.ItemCode = currentItem.ItemCode;
                                        currStock.CurrQty = currentItem.CurrQty;
                                        currStock.UnitPrice = currentItem.UnitPrice;

                                        _currentStockService.Add(currStock);
                                    }

                                }
                                _receiveMainService.Update(recvMain);
                                _receiveDetailsService.All().ToList().Where(y => !string.IsNullOrEmpty(y.RcvNo)).ToList().ForEach(x => _receiveDetailsService.Delete(x));
                                _receiveMainService.Save();
                                _currentStockService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Update", recvMain.RcvNo, Session["UserName"].ToString());
                                //delete to provition
                                var entNo = Convert.ToInt64(recvInfo.VchrNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                LoadDropDown.journalVoucherRemoval("IR", entNo, recvInfo.VchrNo, Session["FinYear"].ToString());
                                LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), "01", Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), expense.BranchCode, expense.FinYear, Convert.ToDecimal(expense.Amount), expense.ExpenseDate);
                                } 
                            }
                            transaction.Complete();
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

            }
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveExWithReceive(Expense expense, ReceiveMain recvInfo, List<ReceiveDetails> recvDetails)
        {
            try
            {
                List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
                if (!string.IsNullOrEmpty(expense.expPurAccode) && expense.expPurAccode.Trim() != "Select Purpose")
                {
                    expense.BranchCode = Session["BranchCode"].ToString();
                    expense.FinYear = Session["FinYear"].ToString();
                    expense.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == expense.expPurAccode.Trim());
                    //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                    CashOperation cashOperation = new CashOperation(expense.ExpenseDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, expense.FinYear, false, expense.BranchCode);
                    var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == expense.ExpenseDate.ToString("MM-dd-yyyy"));
                    if (isAlreadyClosed == null)
                    {
                        ReceiveMain recvMain = new ReceiveMain();
                        recvMain.RcvNo = recvInfo.RcvNo;
                        recvMain.BranchCode = Session["BranchCode"].ToString();
                        recvMain.RcvDate = recvInfo.RcvDate;
                        recvMain.FromLocCode = recvInfo.FromLocCode;
                        recvMain.ToLocCode = recvInfo.ToLocCode;
                        recvMain.Source = recvInfo.Source;
                        recvMain.Accode = recvInfo.Accode;
                        recvMain.RefNo = recvInfo.RefNo;
                        recvMain.RefDate = recvInfo.RefDate;
                        recvMain.Remarks = recvInfo.Remarks;
                        recvMain.RcvdByCode = recvInfo.RcvdByCode;
                        recvMain.AppByCode = recvInfo.AppByCode;
                        recvMain.RcvdTime = DateTime.Now;
                        recvMain.CreditPur = false;
                        recvMain.ChallanNo = null;
                        recvMain.FinYear = Session["FinYear"].ToString();
                        recvMain.GLPost = false;
                        recvMain.RcvdDate = recvInfo.RcvdDate;
                        recvMain.expenseStatus = true;

                        double amount = 0.0;

                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                        foreach (var recvDetailsItem in recvDetails)
                        {
                            ReceiveDetails receivDetails = new ReceiveDetails();
                            receivDetails.RcvNo = recvDetailsItem.RcvNo;
                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                            receivDetails.Description = recvDetailsItem.Description;
                            receivDetails.Qty = recvDetailsItem.Qty;
                            receivDetails.BadQty = recvDetailsItem.BadQty;
                            receivDetails.Price = recvDetailsItem.BadQty * recvDetailsItem.Price;
                            receivDetails.ExQty = recvDetailsItem.ExQty;
                            receivDetails.LotNo = recvDetailsItem.LotNo;
                            amount = amount + recvDetailsItem.Price;
                            receveDetailsList.Add(receivDetails);
                        }

                        recvMain.Amount = amount;

                        recvMain.RcvDetails = receveDetailsList;
                        _receiveMainService.Add(recvMain);

                        _CashOperationService.Add(cashOperation);

                        _ExpenseService.Add(expense);

                        _CashOperationService.Save();
                        _receiveMainService.Save();
                        _ExpenseService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Save", recvMain.RcvNo, Session["UserName"].ToString());
                        return Json(GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode), JsonRequestBehavior.AllowGet);
                    }
                    else if (isAlreadyClosed != null && isAlreadyClosed.IsClosed == false)
                    {
                        ReceiveMain recvMain = new ReceiveMain();
                        recvMain.RcvNo = recvInfo.RcvNo;
                        recvMain.BranchCode = Session["BranchCode"].ToString();
                        recvMain.RcvDate = recvInfo.RcvDate;
                        recvMain.FromLocCode = recvInfo.FromLocCode;
                        recvMain.ToLocCode = recvInfo.ToLocCode;
                        recvMain.Source = recvInfo.Source;
                        recvMain.Accode = recvInfo.Accode;
                        recvMain.RefNo = recvInfo.RefNo;
                        recvMain.RefDate = recvInfo.RefDate;
                        recvMain.Remarks = recvInfo.Remarks;
                        recvMain.RcvdByCode = recvInfo.RcvdByCode;
                        recvMain.AppByCode = recvInfo.AppByCode;
                        recvMain.RcvdTime = DateTime.Now;
                        recvMain.CreditPur = false;
                        recvMain.ChallanNo = null;
                        recvMain.FinYear = Session["FinYear"].ToString();
                        recvMain.GLPost = false;
                        recvMain.RcvdDate = recvInfo.RcvdDate;
                        recvMain.expenseStatus = true;

                        double amount = 0.0;

                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                        foreach (var recvDetailsItem in recvDetails)
                        {
                            ReceiveDetails receivDetails = new ReceiveDetails();
                            receivDetails.RcvNo = recvDetailsItem.RcvNo;
                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                            receivDetails.Description = recvDetailsItem.Description;
                            receivDetails.Qty = recvDetailsItem.Qty;
                            receivDetails.BadQty = recvDetailsItem.BadQty;
                            receivDetails.Price = recvDetailsItem.BadQty * recvDetailsItem.Price;
                            receivDetails.ExQty = recvDetailsItem.ExQty;
                            receivDetails.LotNo = recvDetailsItem.LotNo;
                            amount = amount + recvDetailsItem.Price;
                            receveDetailsList.Add(receivDetails);
                        }

                        recvMain.Amount = amount;

                        recvMain.RcvDetails = receveDetailsList;
                        _receiveMainService.Add(recvMain);

                        _ExpenseService.Add(expense);

                        _receiveMainService.Save();
                        _ExpenseService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Save", recvMain.RcvNo, Session["UserName"].ToString());
                        return Json(GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveExWithSubsidiary(Expense expense, List<ExpenseSubDetails> expDetails)
        {
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();

            try
            {
                if (!string.IsNullOrEmpty(expense.expPurAccode) && expense.expPurAccode.Trim() != "Select Purpose")
                {
                    expense.BranchCode = Session["BranchCode"].ToString();
                    var GCa = _AcBrService.All().Where(s => s.BranchCode == expense.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                    if (GCa == null)
                    {
                        var gset = _gsetService.All().LastOrDefault();
                        GCa = gset.GCa;
                    }
                    var sysSet = _sysSetService.All().FirstOrDefault();
                    expense.FinYear = Session["FinYear"].ToString();
                    expense.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == expense.expPurAccode.Trim());
                    //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                    CashOperation cashOperation = new CashOperation(expense.ExpenseDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, expense.FinYear, false, expense.BranchCode);
                    var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == expense.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == expense.ExpenseDate.ToString("MM-dd-yyyy"));
                    if (isAlreadyClosed == null)
                    {
                        List<ExpenseSubDetails> expSubDetails = new List<ExpenseSubDetails>();
                        foreach (var subsidiary in expDetails)
                        {
                            ExpenseSubDetails subDetails = new ExpenseSubDetails();
                            subDetails.ExpensesubCode = Convert.ToInt32(expense.ExpenseNo);
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = expense.expPurAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;
                            expSubDetails.Add(subDetails);
                        }

                        expense.ExpenseSubDetails = expSubDetails;
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _CashOperationService.Add(cashOperation);

                                _ExpenseService.Add(expense);

                                _CashOperationService.Save();
                                _ExpenseService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                            }
                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else if (isAlreadyClosed != null && sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                    {
                        List<ExpenseSubDetails> expSubDetails = new List<ExpenseSubDetails>();
                        foreach (var subsidiary in expDetails)
                        {
                            ExpenseSubDetails subDetails = new ExpenseSubDetails();
                            subDetails.ExpenseNo = expense.ExpenseNo;
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = expense.expPurAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;

                            expSubDetails.Add(subDetails);
                        }

                        expense.ExpenseSubDetails = expSubDetails;
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _ExpenseService.Add(expense);
                                _ExpenseService.Save();

                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), expense.BranchCode, expense.FinYear, Convert.ToDecimal(expense.Amount), expense.ExpenseDate);
                                } 
                                CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                            }
                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateExWithSubsidiary(Expense expense, List<ExpenseSubDetails> expDetails)
        {
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            try
            {
                if (!string.IsNullOrEmpty(expense.expPurAccode) && expense.expPurAccode.Trim() != "Select Purpose")
                {
                    expense.BranchCode = Session["BranchCode"].ToString();
                    var GCa = _AcBrService.All().Where(s => s.BranchCode == expense.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                    if (GCa == null)
                    {
                        var gset = _gsetService.All().LastOrDefault();
                        GCa = gset.GCa;
                    }
                    var sysSet = _sysSetService.All().FirstOrDefault();

                    expense.FinYear = Session["FinYear"].ToString();
                    expense.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == expense.expPurAccode.Trim());
                    //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                    CashOperation cashOperation = new CashOperation(expense.ExpenseDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, expense.FinYear, false, expense.BranchCode);
                    var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == expense.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == expense.ExpenseDate.ToString("MM-dd-yyyy"));
                    if (isAlreadyClosed == null)
                    {
                        List<ExpenseSubDetails> expSubDetails = new List<ExpenseSubDetails>();
                        foreach (var subsidiary in expDetails)
                        {
                            ExpenseSubDetails subDetails = new ExpenseSubDetails();
                            subDetails.ExpensesubCode = Convert.ToInt32(expense.ExpenseNo);
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = expense.expPurAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;
                            expSubDetails.Add(subDetails);
                        }
                        _expenseSubDetailsService.All().ToList().Where(y => !string.IsNullOrEmpty(y.ExpenseNo)).ToList().ForEach(x => _expenseSubDetailsService.Delete(x));

                        //var _expense = _expenseSubDetailsService.All().ToList().FirstOrDefault(x => x.ExpenseNo == expense.ExpenseNo.Trim());
                        //_expenseSubDetailsService.Delete(_expense);

                        expense.ExpenseSubDetails = expSubDetails;

                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _CashOperationService.Add(cashOperation);

                                _ExpenseService.Update(expense);

                                _CashOperationService.Save();
                                _ExpenseService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                //delete to provision
                                // var entryNo = Convert.ToInt64(expense.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                var entryNo = expense.ExpenseNo;
                                LoadDropDown.journalVoucherRemoval("CP", entryNo, expense.VoucherNo, expense.FinYear);
                                LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), expense.BranchCode, expense.FinYear, Convert.ToDecimal(expense.Amount), expense.ExpenseDate);
                                } 
                                CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else if (isAlreadyClosed != null && sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                    {
                        _expenseSubDetailsService.All().ToList().Where(y => y.ExpenseNo == expense.ExpenseNo).ToList().ForEach(x => _expenseSubDetailsService.Delete(x));
                        _expenseSubDetailsService.Save();
                        List<ExpenseSubDetails> expSubDetails = new List<ExpenseSubDetails>();
                        foreach (var subsidiary in expDetails)
                        {
                            ExpenseSubDetails subDetails = new ExpenseSubDetails();
                            subDetails.ExpenseNo = expense.ExpenseNo;
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = expense.expPurAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;

                            //expSubDetails.Add();
                            _expenseSubDetailsService.Add(subDetails);
                        }

                        //expense.ExpenseSubDetails = expSubDetails;
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _ExpenseService.Update(expense);
                                _ExpenseService.Save();
                                _expenseSubDetailsService.Save();

                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Expense", "Save", expense.ExpenseNo, Session["UserName"].ToString());
                                //delete to provision
                                // var entryNo = Convert.ToInt64(expense.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                var entryNo = expense.ExpenseNo;
                                LoadDropDown.journalVoucherRemoval("CP", entryNo, expense.VoucherNo, expense.FinYear);
                                LoadDropDown.journalVoucherSave("CP", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, GCa, Session["UserName"].ToString());
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), expense.BranchCode, expense.FinYear, Convert.ToDecimal(expense.Amount), expense.ExpenseDate);
                                } 
                                CashOPVM = GetAllExpenseAndDeposit(expense.ExpenseDate, expense.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetExpenseNo(string branchCode)
        {
            return Json(GenerateExpenseNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateExpenseNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string expenseNo = "";

            var expenses = _ExpenseService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var expense = _ExpenseService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (expense != null)
                {

                    expenseNo = "00" + (Convert.ToInt64(expense.ExpenseNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    expenseNo = "00000001";
                }
            }
            else
            {
                if (expenses != null)
                {
                    expenseNo = branchCode + (Convert.ToInt64(expenses.ExpenseNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    expenseNo = branchCode + "000001";
                }

            }

            return expenseNo;

        }

        public ActionResult GetExpense(string receiptNo)
        {
            var expense = _expenseSubDetailsService.All().Where(x => x.ExpenseNo == receiptNo.Trim()).ToList();//shahin Change x => x.SubCode == re
            if (expense.Count > 0)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }
        }
        //calling  off by shahin (For subsidiary edit )
        public ActionResult GetAllByExpenseNo(string expenseNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + expenseNo + "' and EntrySource='CP'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            AcclineERPContext DbContext = new AcclineERPContext();

            DbContext.Expense.AsNoTracking().FirstOrDefault();

            ExpenseVModel expense = new ExpenseVModel();
            //CRSubDetailsVModel vmode = new CRSubDetailsVModel();
            expense.Main = _ExpenseService.All().ToList().FirstOrDefault(x => x.ExpenseNo == expenseNo.Trim());

            //AcclineERPContext _dbcon = new AcclineERPContext();

            //_dbcon.Configuration.ProxyCreationEnabled = false;

            var items = _expenseSubDetailsService.All().Where(x => x.ExpenseNo == expenseNo.Trim()).ToList();//shahin Change x => x.SubCode == receiptNo.Trim())
            if (items.Count > 0)
            {
                //int i = 0;
                DbContext.ExpenseSubDetails.AsNoTracking().FirstOrDefault();
                foreach (var item in items)
                {
                    expense.Details.Add(new EXSubDetailsVModel(item.SubCode, item.Amount, item.Description, item.SubsidiaryInfo.SubName));
                }
            }

            List<EXSubDetailsVModel> Details = new List<EXSubDetailsVModel>();
            Details = expense.Details;
            Expense Main = new Expense();
            Main = expense.Main;

            //jsonconvert()

            //PreserveReferencesHandling fd = PreserveReferencesHandling.Objects;
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

            string json = JsonConvert.SerializeObject(expense, Formatting.Indented, serializerSettings);

            var check = Json(json, JsonRequestBehavior.AllowGet);
            if (json != null && IsExit.Count != 0)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            // return Json(json, JsonRequestBehavior.AllowGet);

        }

        #region Generate Voucher No
        public ActionResult GetNewVoucherNo(string VType)
        {

            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            //string VLen = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sql = string.Format("exec [GetNewVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sql).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExistVoucherNo(string VType, DateTime TrDate)
        {

            string voucherNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sqls = string.Format("exec [GetExistVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + TrDate.ToString("MM-dd-yyyy") + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sqls).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //public ActionResult GetAllCashReceipt()
        //{
        //    var cashReceipts = new SelectList(_MoneyReceiptAppService.All().ToList().Where(x => x.ReceiptDate == DateTime.Now && x.PayMode == "Ca"));
        //    return Json(cashReceipts, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult DelMoneyReceiptOrRemittance(string tag, string receiptNo)
        //{
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetExpenseBYExpenseNo(Expense expens, string expenseNo)
        {

            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + expens.ExpenseNo + "' and EntrySource='CP'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            var expense = _ExpenseService.All().FirstOrDefault(x => x.ExpenseNo == expenseNo.Trim());

            var row = _PayInvRecvService.All().FirstOrDefault(s => s.TranNo == expenseNo);
            string rcvNo = row != null ? row.RefNo : string.Empty;
            RecvMainVM recvInfo = new RecvMainVM();
            RecvDetailsVM recvDetail = new RecvDetailsVM();
            if (rcvNo != "")
            {
                recvInfo.Main = _receiveMainService.All().ToList().FirstOrDefault(x => x.RcvNo == rcvNo.Trim());
                var items = _receiveDetailsService.All().ToList().Where(x => x.RcvNo == rcvNo.Trim()).ToList();

                recvInfo.Main.RcvDetails = null;
                if (items.Count > 0)
                {
                    int i = 0;
                    foreach (var item in items)
                    {
                        recvInfo.Details.Add(new RecvDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, item.Qty, item.Price, item.LotNo, item.RcvNo, item.Price, item.Description));
                    }
                }
            }
            if (expense != null && IsExit.Count != 0)
            {
                return Json(new { expense = expense, recvInfo = recvInfo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public List<CashOperationVModel> GetAllExpenseAndDeposit(DateTime dateTime, string branchCode)
        {
            var cash = new List<CashOperationVModel>();
            int i = 0;
            _ExpenseService.All().ToList().Where(x => x.BranchCode == branchCode && x.ExpenseDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                 new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.ExpenseNo, "CP", x.VoucherNo)
                ));

            _DepositToBankService.All().ToList().Where(x => x.BranchCode == branchCode && x.DepositDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.DepositNo, "BD", x.VoucherNo)));
            return cash;
        }




        public void JvCallInsert(Expense expense)
        {
            LoadDropDown.journalVoucherSave("Cp", "I", expense.ExpenseNo, expense.VoucherNo, expense.FinYear, "01", expense.BranchCode, expense.ExpenseDate, expense.expPurAccode, Session["UserName"].ToString());
        }

        public ActionResult ExpenseNoIncreement(Expense model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var expenseNo = _ExpenseService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int exNo = Convert.ToInt32(expenseNo.ExpenseNo);
                string ExpenseNo = (exNo + 1).ToString();
                string IncInvcNo = "";
                if (ExpenseNo.Length > 7)
                {
                    IncInvcNo = ExpenseNo.ToString();
                }
                else
                {
                    IncInvcNo = "0" + ExpenseNo.ToString();
                }

                return Json(IncInvcNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }


    }
}
