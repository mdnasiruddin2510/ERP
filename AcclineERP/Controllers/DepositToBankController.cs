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
    public class DepositToBankController : Controller
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
        private readonly IBankOperationAppService _BankOperationService;
        private readonly IGsetAppService _gsetService;

        public DepositToBankController(
            ICashOperationAppService _CashOperationService,
            ICashReceiptAppService _cashReceiptAppService,
            IWithdrawAppService _withdrawService,
            IExpenseAppService _ExpenseService,
            IDepositToBankAppService _DepositToBankService,
            IEmployeeAppService _EmployeeService,
            ISubsidiaryInfoAppService _subsidiaryService,
            IItemInfoAppService _ItemService,
            INewChartAppService _NewChartService,
            ILocationAppService _LocationService,
            IUnitAppService _UnitService,
            ITransactionLogAppService _transactionLogService,
            IBankOperationAppService _BankOperationService,
            IJarnalVoucherAppService _jarnalVoucherService,
            ISysSetAppService _sysSetService,
            IOpnBalAppService _OpnBalService,
            IPaymentAppService _paymentService,
            IHORemitAppService _HORemitService,IAcBRAppService _AcBrService,
            IBankReceiptAppService _BankReceiptAppService, IGsetAppService _gsetService)
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
            this._BankOperationService = _BankOperationService;
            this._gsetService = _gsetService;
            this._AcBrService = _AcBrService;
        }

        [HttpPost]
        public ActionResult SaveDepositToBank(Deposit deposit)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankDeposit_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            
            try
            {
                var IfExit = _DepositToBankService.All().Where(x => x.DepositNo == deposit.DepositNo).FirstOrDefault();
                if (IfExit == null)
                {
                    if (!string.IsNullOrEmpty(deposit.bankAccode) && deposit.bankAccode.Trim() != "Select Bank A/C")
                    {
                        deposit.BranchCode = Session["BranchCode"].ToString();
                        var GCa = _AcBrService.All().Where(s => s.BranchCode == deposit.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }

                        deposit.FinYear = Session["FinYear"].ToString();
                        deposit.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == deposit.bankAccode.Trim());
                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        CashOperation cashOperation = new CashOperation(deposit.DepositDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, deposit.FinYear, false, deposit.BranchCode);
                        var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == deposit.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == deposit.DepositDate.ToString("MM-dd-yyyy"));


                        var openBal = GetOpenBal(deposit.DepositDate, deposit.bankAccode);
                        var rcecBal = GetAllRemittances(deposit.DepositDate, deposit.BranchCode, deposit.bankAccode).Sum(x => x.Amount);
                        var payTotal = GetAllPayment(deposit.DepositDate, deposit.BranchCode, deposit.bankAccode).Sum(x => x.Amount);
                        var closBal = openBal + rcecBal - payTotal;
                        BankOperation bankOperation = new BankOperation(deposit.DepositDate, openBal, deposit.Amount, 0.0, closBal, deposit.BranchCode, deposit.FinYear, false, deposit.bankAccode);

                        var sysSet = _sysSetService.All().FirstOrDefault();


                        var isAlreadySaved = _BankOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == deposit.DepositDate.ToString("MM-dd-yyyy") && x.BranchCode == deposit.BranchCode && x.bankAccode == deposit.bankAccode);

                        if (isAlreadyClosed == null || isAlreadySaved == null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _CashOperationService.Add(cashOperation);
                                    _CashOperationService.Save();
                                    _BankOperationService.Add(bankOperation);
                                    _BankOperationService.Save();
                                    _DepositToBankService.Add(deposit);
                                    _DepositToBankService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Deposit To Bank", "Save", deposit.DepositNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("BD", "I", deposit.DepositNo, deposit.VoucherNo, deposit.FinYear, "01", deposit.BranchCode, deposit.DepositDate,GCa, Session["UserName"].ToString());
                                    CashOPVM = GetAllExpenseAndDeposit(deposit.DepositDate, deposit.BranchCode);
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
                            var rcecBals = GetAllRemittances(deposit.DepositDate, deposit.BranchCode, deposit.bankAccode).Sum(x => x.Amount);
                            isAlreadySaved.RecTotal = rcecBals + deposit.Amount;
                            isAlreadySaved.CloseBal = isAlreadySaved.OpenBal + isAlreadySaved.RecTotal - isAlreadySaved.PayTotal;

                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _BankOperationService.Update(isAlreadySaved);
                                    _BankReceiptAppService.Save();
                                    _DepositToBankService.Add(deposit);
                                    _DepositToBankService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Deposit To Bank", "Save", deposit.DepositNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("BD", "I", deposit.DepositNo, deposit.VoucherNo, deposit.FinYear, "01", deposit.BranchCode, deposit.DepositDate,GCa, Session["UserName"].ToString());
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), deposit.BranchCode, deposit.FinYear, Convert.ToDecimal(deposit.Amount), deposit.DepositDate);
                                    }  
                                    CashOPVM = GetAllExpenseAndDeposit(deposit.DepositDate, deposit.BranchCode);
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

        public ActionResult UpdateDepositToBank(Deposit deposit, DateTime dateTime)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankDeposit_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            if (ModelState.IsValid)
            {
                deposit.BranchCode = Session["BranchCode"].ToString();
                var GCa = _AcBrService.All().Where(s => s.BranchCode == deposit.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                if (GCa == null)
                {
                    var gset = _gsetService.All().LastOrDefault();
                    GCa = gset.GCa;
                }

                deposit.FinYear = Session["FinYear"].ToString();
                deposit.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == deposit.bankAccode.Trim());
                CashOperation cashOperation = new CashOperation();
                var isClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == deposit.BranchCode && Convert.ToDateTime(x.TrDate.ToShortDateString()) <= Convert.ToDateTime(dateTime.ToShortDateString()));
                if (isClosed.IsClosed == false)
                {
                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            _DepositToBankService.Update(deposit);
                            _DepositToBankService.Save();

                            //delete to provision
                            var entryNo = deposit.DepositNo;
                            //var entryNo = Convert.ToInt64(deposit.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                            LoadDropDown.journalVoucherRemoval("BD", entryNo, deposit.VoucherNo, deposit.FinYear);
                            //insert to provision
                            LoadDropDown.journalVoucherSave("BD", "I", deposit.DepositNo, deposit.VoucherNo, deposit.FinYear, "01", deposit.BranchCode, deposit.DepositDate, GCa, Session["UserName"].ToString());
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Deposit To Bank", "Update", deposit.DepositNo, Session["UserName"].ToString());

                            var sysSet = _sysSetService.All().FirstOrDefault(); 
                            if (sysSet.CashRule == "O")
                            {
                                LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), deposit.BranchCode, deposit.FinYear, Convert.ToDecimal(deposit.Amount), deposit.DepositDate);
                            } 
                            CashOPVM = GetAllExpenseAndDeposit(deposit.DepositDate, deposit.BranchCode);
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

        public ActionResult GetDepositNo(string branchCode)
        {
            return Json(GenerateDepositNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateDepositNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string depositNo = "";

            var deposits = _DepositToBankService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var deposit = _DepositToBankService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (deposit != null)
                {

                    depositNo = "00" + (Convert.ToInt64(deposit.DepositNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    depositNo = "00000001";
                }
            }
            else
            {
                if (deposits != null)
                {
                    depositNo = branchCode + (Convert.ToInt64(deposits.DepositNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    depositNo = branchCode + "000001";
                }

            }

            return depositNo;

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

       
        public ActionResult GetDepositBYDepositNo(string depositNo)
        {

            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + depositNo + "' and EntrySource='BD'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            var deposit = _DepositToBankService.All().FirstOrDefault(x => x.DepositNo == depositNo.Trim());
            if (deposit != null && IsExit.Count != 0)
            {
                return Json(deposit, JsonRequestBehavior.AllowGet);
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
            string finYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC GetBankOpenBal '" + finYear + "', '" + bankAccode + "', '" + date.ToString("MM-dd-yyyy") + "'");
            OpenBal = Convert.ToDouble(_OpnBalService.SqlQueary(sql).ToList().FirstOrDefault().OpenBalance.ToString());
            return OpenBal;
        }
        public List<BankOperationVModel> GetAllPayment(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            var bank = new List<BankOperationVModel>();
            int i = 0;

            _paymentService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.PayDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                new BankOperationVModel(++i, x.NewChartPur.AcName, x.Amount, x.PayCode, "BP", x.VoucherNo)));

            _withdrawService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.WithdrawDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                new BankOperationVModel(++i, "Withdraw From Bank", x.Amount, x.WithdrawNo, "BW", x.VoucherNo)));
            return bank;
        }


        public ActionResult DepositNoIncreement(Deposit model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var expenseNo = _DepositToBankService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int exNo = Convert.ToInt32(expenseNo.DepositNo);
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
