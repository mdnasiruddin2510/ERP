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
    public class BankReceiptController : Controller
    {
        private readonly IBankOperationAppService _BankOperationService;
        private readonly IBankReceiptAppService _BankReceiptAppService;
        private readonly INewChartAppService _NewChartService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IHORemitAppService _HORemitService;
        private readonly IDepositToBankAppService _DepositToBankService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IVchrMainAppService _VchrMainService;
        private readonly IVchrDetailAppService _VchrDetailService;
        private readonly IBranchAppService _BranchService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;

        private readonly IOpnBalAppService _OpnBalService;
        private readonly IPaymentAppService _paymentService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly IGsetAppService _gsetService;
        public BankReceiptController(IBankOperationAppService _BankOperationService, IBankReceiptAppService _BankReceiptAppService,
            INewChartAppService _NewChartService, IEmployeeAppService _EmployeeService, IHORemitAppService _HORemitService,
            IDepositToBankAppService _DepositToBankService, ITransactionLogAppService _transactionLogService,
            IVchrMainAppService _VchrMainService, IVchrDetailAppService _VchrDetailService,
            IBranchAppService _BranchService, IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IOpnBalAppService _OpnBalService, IPaymentAppService _paymentService, IWithdrawAppService _withdrawService, IGsetAppService _gsetService)
        {
            this._BankOperationService = _BankOperationService;
            this._BankReceiptAppService = _BankReceiptAppService;
            this._NewChartService = _NewChartService;
            this._EmployeeService = _EmployeeService;
            this._HORemitService = _HORemitService;
            this._DepositToBankService = _DepositToBankService;
            this._transactionLogService = _transactionLogService;
            this._VchrMainService = _VchrMainService;
            this._VchrDetailService = _VchrDetailService;
            this._BranchService = _BranchService;
            this._OpnBalService = _OpnBalService;
            this._paymentService = _paymentService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._withdrawService = _withdrawService;
            this._gsetService = _gsetService;
        }


        [HttpPost]
        public ActionResult SaveBankReceipt(BankReceipt bankReceipt)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankReceipt_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            var gset = _gsetService.All().LastOrDefault();
            try
            {
                var IfExit = _BankReceiptAppService.All().Where(x => x.BReceiptNo == bankReceipt.BReceiptNo).FirstOrDefault();
                if (IfExit == null)
                {
                    if (!string.IsNullOrEmpty(bankReceipt.purAccode) && bankReceipt.purAccode.Trim() != "Select Purpose")
                    {
                        bankReceipt.BranchCode = Session["BranchCode"].ToString();
                        bankReceipt.bankAccode = Session["BankAccode"].ToString();
                        bankReceipt.FinYear = Session["FinYear"].ToString();
                        bankReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.purAccode.Trim());
                        bankReceipt.bankNewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.bankAccode.Trim());


                        var openBal = GetOpenBal(bankReceipt.BReceiptDate);
                        var rcecBal = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode).Sum(x => x.Amount);
                        var payTotal = GetAllPayment(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode).Sum(x => x.Amount);
                        var closBal = openBal + rcecBal - payTotal;
                        BankOperation bankOperation = new BankOperation(bankReceipt.BReceiptDate, openBal, bankReceipt.Amount, 0.0, closBal, bankReceipt.BranchCode, bankReceipt.FinYear, false, bankReceipt.bankAccode);


                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        //BankOperation bankOperation = new BankOperation(bankReceipt.BReceiptDate, 0.0, 0.0, 0.0, 0.0, bankReceipt.BranchCode, bankReceipt.FinYear, false, bankReceipt.bankAccode);
                        var isAlreadySaved = _BankOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == bankReceipt.BReceiptDate.ToString("MM-dd-yyyy") && x.BranchCode == bankReceipt.BranchCode && x.bankAccode == bankReceipt.bankAccode);

                        if (isAlreadySaved == null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _BankOperationService.Add(bankOperation);
                                    _BankOperationService.Save();

                                    _BankReceiptAppService.Add(bankReceipt);
                                    _BankReceiptAppService.Save();

                                    //insert to provision
                                    LoadDropDown.journalVoucherSave("BR", "I", bankReceipt.BReceiptNo, bankReceipt.VoucherNo, bankReceipt.FinYear, "01", bankReceipt.BranchCode, bankReceipt.BReceiptDate, bankReceipt.bankAccode, Session["UserName"].ToString());

                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "BankReceipt", "Save", bankReceipt.BReceiptNo, User.Identity.Name);
                                    CashOPVM = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode);
                                    transaction.Complete();
                                }
                                catch (Exception)
                                {
                                    transaction.Dispose();
                                    return Json("0", JsonRequestBehavior.AllowGet);
                                }

                            }
                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            var rcecBals = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode).Sum(x => x.Amount);
                            isAlreadySaved.RecTotal = rcecBals + bankReceipt.Amount;
                            isAlreadySaved.CloseBal = isAlreadySaved.OpenBal + isAlreadySaved.RecTotal - isAlreadySaved.PayTotal;

                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _BankOperationService.Update(isAlreadySaved);
                                    _BankReceiptAppService.Save();

                                    _BankReceiptAppService.Add(bankReceipt);
                                    _BankReceiptAppService.Save();
                                    var sysSet = _sysSetService.All().FirstOrDefault();
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.journalVoucherSave("BR", "I", bankReceipt.BReceiptNo, bankReceipt.VoucherNo, bankReceipt.FinYear, "01", bankReceipt.BranchCode, bankReceipt.BReceiptDate, bankReceipt.bankAccode, Session["UserName"].ToString());
                                    } 
                                    //insert to provision
                                    LoadDropDown.BankRecalculate(bankReceipt.BReceiptDate, Convert.ToDecimal(bankReceipt.Amount), Session["ProjCode"].ToString(), bankReceipt.BranchCode, bankReceipt.FinYear, bankReceipt.bankAccode);
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "BankReceipt", "Save", bankReceipt.BReceiptNo, User.Identity.Name);
                                    CashOPVM = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode);
                                    transaction.Complete();
                                }
                                catch (Exception)
                                {
                                    transaction.Dispose();
                                    return Json("0", JsonRequestBehavior.AllowGet);
                                }

                            }

                            return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateBankReceipt(BankReceipt bankReceipt)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankReceipt_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            var gset = _gsetService.All().LastOrDefault();
            try
            {
                bankReceipt.BranchCode = Session["BranchCode"].ToString();
                bankReceipt.FinYear = Session["FinYear"].ToString();
                bankReceipt.bankAccode = Session["BankAccode"].ToString();
                bankReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.purAccode.Trim());
                bankReceipt.bankNewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.bankAccode.Trim());

                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        _BankReceiptAppService.Update(bankReceipt);
                        _BankReceiptAppService.Save();
                        //delete to provision 
                        //var entryNo = Convert.ToInt64(bankReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                        var entryNo = bankReceipt.BReceiptNo;
                        LoadDropDown.journalVoucherRemoval("BR", entryNo, bankReceipt.VoucherNo, bankReceipt.FinYear);
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        if (sysSet.CashRule == "O")
                        {
                            LoadDropDown.journalVoucherSave("BR", "I", bankReceipt.BReceiptNo, bankReceipt.VoucherNo, bankReceipt.FinYear, "01", bankReceipt.BranchCode, bankReceipt.BReceiptDate, bankReceipt.bankAccode, Session["UserName"].ToString());
                        } 
                        //insert to provision
                        LoadDropDown.journalVoucherSave("BR", "I", bankReceipt.BReceiptNo, bankReceipt.VoucherNo, bankReceipt.FinYear, "01", bankReceipt.BranchCode, bankReceipt.BReceiptDate, bankReceipt.bankAccode, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Bank Receipt", "Update", bankReceipt.BReceiptNo, User.Identity.Name);

                        CashOPVM = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankReceipt.bankAccode);
                        transaction.Complete();
                    }
                    catch (Exception)
                    {
                        transaction.Dispose();
                    }
                }

                return Json(CashOPVM, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetReceiptNo(string branchCode)
        {
            return Json(GenerateReceiptNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateReceiptNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string receiptNo = "";
            var bankReceipt = _BankReceiptAppService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var bankRcpt = _BankReceiptAppService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (bankRcpt != null)
                {

                    receiptNo = "00" + (Convert.ToInt64(bankRcpt.BReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = "00000001";
                }
            }
            else
            {
                if (bankReceipt != null)
                {
                    receiptNo = branchCode + (Convert.ToInt64(bankReceipt.BReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = branchCode + "000001";
                }

            }

            return receiptNo;

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
            string sqls = string.Format("exec [GetExistVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + TrDate + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sqls).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult GetBankReceiptBYReceiptNo(string receiptNo)
        {

            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + receiptNo + "' and EntrySource='BR'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();

            var bankReceipt = _BankReceiptAppService.All().FirstOrDefault(x => x.BReceiptNo == receiptNo.Trim());
            if (bankReceipt != null && IsExit.Count != 0)
            {
                return Json(bankReceipt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }


        public List<BankOperationVModel> GetAllRemittances(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            bankAccode = Session["BankAccode"].ToString();
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


        public double GetOpenBal(DateTime date)
        {
            double OpenBal = 0;
            string bankAccode = Session["BankAccode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC GetBankOpenBal '" + finYear + "', '" + bankAccode + "', '" + date.ToString("MM-dd-yyyy") + "'");
            OpenBal = Convert.ToDouble(_OpnBalService.SqlQueary(sql).ToList().FirstOrDefault().OpenBalance.ToString());
            return OpenBal;
        }

        public List<BankOperationVModel> GetAllPayment(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            bankAccode = Session["BankAccode"].ToString();
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

        public ActionResult BankReceiptNoIncreement(BankReceipt model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var brcptNo = _BankReceiptAppService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int brNo = Convert.ToInt32(brcptNo.BReceiptNo);
                string BankReceiptNo = (brNo + 1).ToString();
                string BankRecptNo = "";
                if (BankReceiptNo.Length > 7)
                {
                    BankRecptNo = BankReceiptNo.ToString();
                }
                else
                {
                    BankRecptNo = "0" + BankReceiptNo.ToString();
                }
                return Json(BankRecptNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
    }
}
