using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class WithdrawController : Controller
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
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;

        private readonly IOpnBalAppService _OpnBalService;
        private readonly IPaymentAppService _paymentService;
        private readonly IAcBRAppService _AcBrService;
        private readonly IHORemitAppService _HORemitService;
        private readonly IBankReceiptAppService _BankReceiptAppService;
        private readonly IBankOperationAppService _bankOperationService;
        private readonly IGsetAppService _gsetService;


        public WithdrawController(ICashOperationAppService _CashOperationService, ICashReceiptAppService _cashReceiptAppService,
            IWithdrawAppService _withdrawService, IExpenseAppService _ExpenseService, IDepositToBankAppService _DepositToBankService,
            IEmployeeAppService _EmployeeService, ISubsidiaryInfoAppService _subsidiaryService, IItemInfoAppService _ItemService,
            INewChartAppService _NewChartService, ILocationAppService _LocationService, IUnitAppService _UnitService,
            ITransactionLogAppService _transactionLogService,
            IJarnalVoucherAppService _jarnalVoucherService,
            ISysSetAppService _sysSetService,
             IOpnBalAppService _OpnBalService,
            IPaymentAppService _paymentService,
            IHORemitAppService _HORemitService,IAcBRAppService _AcBrService,
            IBankReceiptAppService _BankReceiptAppService,
            IBankOperationAppService _bankOperationService, IGsetAppService _gsetService
            )
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
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._OpnBalService = _OpnBalService;
            this._paymentService = _paymentService;
            this._HORemitService = _HORemitService;
            this._BankReceiptAppService = _BankReceiptAppService;
            this._bankOperationService = _bankOperationService;
            this._gsetService = _gsetService;
            this._AcBrService = _AcBrService;
        }


        [HttpPost]
        public ActionResult SaveWithdraw(Withdraw withdraw)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankWithdraw_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            
            try
            {
                var IfExit = _withdrawService.All().Where(x => x.WithdrawNo == withdraw.WithdrawNo).FirstOrDefault();
                if (IfExit == null)
                {
                    if (!string.IsNullOrEmpty(withdraw.bankAccode) && withdraw.bankAccode.Trim() != "Select Bank A/C")
                    {
                        withdraw.BranchCode = Session["BranchCode"].ToString();
                        var GCa = _AcBrService.All().Where(s => s.BranchCode == withdraw.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }
                        withdraw.FinYear = Session["FinYear"].ToString();
                        withdraw.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == withdraw.bankAccode.Trim());
                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        CashOperation cashOperation = new CashOperation(withdraw.WithdrawDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, withdraw.FinYear, false, withdraw.BranchCode);
                        var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == withdraw.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == withdraw.WithdrawDate.ToString("MM-dd-yyyy"));


                        var openBal = GetOpenBal(withdraw.WithdrawDate, withdraw.bankAccode);
                        var rcecBal = GetAllRemittances(withdraw.WithdrawDate, withdraw.BranchCode, withdraw.bankAccode).Sum(x => x.Amount);
                        var payTotal = GetAllPayment(withdraw.WithdrawDate, withdraw.BranchCode, withdraw.bankAccode).Sum(x => x.Amount);
                        var closBal = openBal + rcecBal - payTotal;

                        BankOperation bankOperation = new BankOperation(withdraw.WithdrawDate, openBal, 0.0, withdraw.Amount, closBal, withdraw.BranchCode, withdraw.FinYear, false, withdraw.bankAccode);
                        // BankOperation bankOperation = new BankOperation(payment.PayDate, 0.0, 0.0, 0.0, 0.0, payment.BranchCode, payment.FinYear, false, payment.bankAccode);
                        var isAlreadySaved = _bankOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == withdraw.WithdrawDate.ToString("MM-dd-yyyy") && x.BranchCode == withdraw.BranchCode && x.bankAccode == withdraw.bankAccode);

                        var sysSet = _sysSetService.All().FirstOrDefault();

                        if (isAlreadyClosed == null || isAlreadySaved == null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _bankOperationService.Add(bankOperation);
                                    _bankOperationService.Save();

                                    if (isAlreadyClosed == null)
                                    {
                                        _CashOperationService.Add(cashOperation);
                                        _CashOperationService.Save();
                                    }
                                    
                                    _withdrawService.Add(withdraw);
                                    _withdrawService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Withdraw", "Save", withdraw.WithdrawNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("BW", "I", withdraw.WithdrawNo, withdraw.VoucherNo, withdraw.FinYear, "01", withdraw.BranchCode, withdraw.WithdrawDate, GCa, Session["UserName"].ToString());
                                    CashOPVM = GetAllCashReceiptAndWithdraw(withdraw.WithdrawDate, withdraw.BranchCode);
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


                            var payTotals = GetAllPayment(withdraw.WithdrawDate, withdraw.BranchCode, withdraw.bankAccode).Sum(x => x.Amount);
                            isAlreadySaved.PayTotal = payTotals + withdraw.Amount;
                            isAlreadySaved.CloseBal = isAlreadySaved.OpenBal + isAlreadySaved.RecTotal - isAlreadySaved.PayTotal;


                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _bankOperationService.Update(isAlreadySaved);
                                    _bankOperationService.Save();


                                    _withdrawService.Add(withdraw);
                                    _withdrawService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Withdraw", "Save", withdraw.WithdrawNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("BW", "I", withdraw.WithdrawNo, withdraw.VoucherNo, withdraw.FinYear, "01", withdraw.BranchCode, withdraw.WithdrawDate, GCa, Session["UserName"].ToString());
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), withdraw.BranchCode, withdraw.FinYear, Convert.ToDecimal(withdraw.Amount), withdraw.WithdrawDate);
                                    }
                                    CashOPVM = GetAllCashReceiptAndWithdraw(withdraw.WithdrawDate, withdraw.BranchCode);
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
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateWithdraw(Withdraw withdraw, DateTime dateTime)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankWithdraw_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            if (ModelState.IsValid)
            {
                withdraw.BranchCode = Session["BranchCode"].ToString();

                var GCa = _AcBrService.All().Where(s => s.BranchCode == withdraw.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                if (GCa == null)
                {
                    var gset = _gsetService.All().LastOrDefault();
                    GCa = gset.GCa;
                }

                withdraw.FinYear = Session["FinYear"].ToString();
                withdraw.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == withdraw.bankAccode.Trim());
                CashOperation cashOperation = new CashOperation();
                var isClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == withdraw.BranchCode && Convert.ToDateTime(x.TrDate.ToShortDateString()) <= Convert.ToDateTime(dateTime.ToShortDateString()));
                if (isClosed.IsClosed == false)
                {

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            _withdrawService.Update(withdraw);
                            _withdrawService.Save();
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Withdraw", "Update", withdraw.WithdrawNo, Session["UserName"].ToString());

                            //delete to provision
                            //var entryNo = Convert.ToInt64(withdraw.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                            var entryNo = withdraw.WithdrawNo;
                            LoadDropDown.journalVoucherRemoval("BW", entryNo, withdraw.VoucherNo, withdraw.FinYear);
                             var sysSet = _sysSetService.All().FirstOrDefault();
                            if (sysSet.CashRule == "O")
                            {
                                LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), withdraw.BranchCode, withdraw.FinYear, Convert.ToDecimal(withdraw.Amount), withdraw.WithdrawDate);
                            }
                            //insert to provition
                            LoadDropDown.journalVoucherSave("BW", "I", withdraw.WithdrawNo, withdraw.VoucherNo, withdraw.FinYear, "01", withdraw.BranchCode, withdraw.WithdrawDate, GCa, Session["UserName"].ToString());
                            CashOPVM = GetAllCashReceiptAndWithdraw(withdraw.WithdrawDate, withdraw.BranchCode);
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
        

        public ActionResult GetWithdrawNo(string branchCode)
        {
            return Json(GenerateWithdrawNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateWithdrawNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string withdrawNo = "";

            var withdraws = _withdrawService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var withdraw = _withdrawService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (withdraw != null)
                {

                    withdrawNo = "00" + (Convert.ToInt64(withdraw.WithdrawNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    withdrawNo = "00000001";
                }
            }
            else
            {
                if (withdraws != null)
                {
                    withdrawNo = branchCode + (Convert.ToInt64(withdraws.WithdrawNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    withdrawNo = branchCode + "000001";
                }

            }

            return withdrawNo;

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
            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sqls = string.Format("exec [GetExistVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + TrDate + "','" + finYear + "'");
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
        public ActionResult GetWithdrawBYWithdrawNo(string withdrawNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + withdrawNo + "' and EntrySource='BW'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            var withdraw = _withdrawService.All().FirstOrDefault(x => x.WithdrawNo == withdrawNo.Trim());
            if (withdraw != null && IsExit.Count != 0)
            {
                return Json(withdraw, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public List<CashOperationVModel> GetAllCashReceiptAndWithdraw(DateTime dateTime, string branchCode)
        {
            var cash = new List<CashOperationVModel>();
            int i = 0;
            _cashReceiptAppService.All().ToList().Where(x => x.BranchCode == branchCode && x.ReceiptDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                 new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.ReceiptNo, "CR", x.VoucherNo)
                ));
            _withdrawService.All().ToList().Where(x => x.BranchCode == branchCode && x.WithdrawDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.WithdrawNo, "BW", x.VoucherNo)));
            return cash;
        }


        public List<BankOperationVModel> GetAllPayment(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            // bankAccode = Session["BankAccode"].ToString();
            var bank = new List<BankOperationVModel>();
            int i = 0;
            //x.NewChartPur.AcName
            _paymentService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.PayDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                new BankOperationVModel(++i, "Payment From Bank", x.Amount, x.PayCode, "BP", x.VoucherNo)));

            _withdrawService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
               x.WithdrawDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                new BankOperationVModel(++i, "Withdraw From Bank", x.Amount, x.WithdrawNo, "BW", x.VoucherNo)));
            return bank;
        }
        public List<BankOperationVModel> GetAllRemittances(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            // bankAccode = Session["BankAccode"].ToString();
            var bank = new List<BankOperationVModel>();
            int i = 0;

            _HORemitService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.RemitDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                    new BankOperationVModel(++i, "H/O Remittance", x.Amount, x.RemitNo, "RT", x.VoucherNo)));

            _BankReceiptAppService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.BReceiptDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                    new BankOperationVModel(++i, x.NewChart.AcName, x.Amount, x.BReceiptNo, "BR", x.VoucherNo)));

            _DepositToBankService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.DepositDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
               new BankOperationVModel(++i, "Deposit To Bank", x.Amount, x.DepositNo, "BD", x.VoucherNo)));
            return bank;
        }

        public double GetOpenBal(DateTime date, string bankAccode)
        {
            double OpenBal = 0;
            // string bankAccode = Session["BankAccode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC GetBankOpenBal '" + finYear + "', '" + bankAccode + "', '" + date.ToString("MM/dd/yyyy") + "'");
            OpenBal = Convert.ToDouble(_OpnBalService.SqlQueary(sql).ToList().FirstOrDefault().OpenBalance.ToString());
            return OpenBal;
        }
        public ActionResult WithdrawNoIncreement(Withdraw model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var expenseNo = _withdrawService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int exNo = Convert.ToInt32(expenseNo.WithdrawNo);
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
