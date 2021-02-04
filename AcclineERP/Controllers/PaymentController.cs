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
    public class PaymentController : Controller
    {
        private readonly IEmployeeAppService _employeeService;
        private readonly IHORemitAppService _hoRemitService;
        private readonly IBankOperationAppService _bankOperationService;
        private readonly IDepositToBankAppService _depositToBankService;
        private readonly IPaymentAppService _paymentService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IOpnBalAppService _OpnBalService;
        private readonly IHORemitAppService _HORemitService;
        private readonly IDepositToBankAppService _DepositToBankService;
        private readonly IBankReceiptAppService _BankReceiptAppService;
        private readonly IGsetAppService _gsetService;
        public PaymentController(IEmployeeAppService _employeeService, IHORemitAppService _hoRemitService,
            IBankOperationAppService _bankOperationService, ITransactionLogAppService _transactionLogService,
            IDepositToBankAppService _depositToBankService, IPaymentAppService _paymentService,
            IWithdrawAppService _withdrawService, IJarnalVoucherAppService _jarnalVoucherService,
            ISysSetAppService _sysSetService, IOpnBalAppService _OpnBalService, IHORemitAppService _HORemitService,
            IDepositToBankAppService _DepositToBankService, IBankReceiptAppService _BankReceiptAppService, IGsetAppService _gsetService)
        {
            this._bankOperationService = _bankOperationService;
            this._employeeService = _employeeService;
            this._hoRemitService = _hoRemitService;
            this._transactionLogService = _transactionLogService;
            this._depositToBankService = _depositToBankService;
            this._paymentService = _paymentService;
            this._withdrawService = _withdrawService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._HORemitService = _HORemitService;
            this._DepositToBankService = _DepositToBankService;
            this._BankReceiptAppService = _BankReceiptAppService;
            this._OpnBalService = _OpnBalService;
            this._gsetService = _gsetService;
        }

        public ActionResult SavePayment(Payment payment)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("BankPayment_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            var gset = _gsetService.All().LastOrDefault();
            try
            {
                var IfExit = _paymentService.All().Where(x => x.PayCode == payment.PayCode).FirstOrDefault();
                if (IfExit == null)
                {
                    payment.BranchCode = Session["BranchCode"].ToString();
                    payment.FinYear = Session["FinYear"].ToString();
                    payment.bankAccode = Session["BankAccode"].ToString();


                    var openBal = GetOpenBal(payment.PayDate);
                    var rcecBal = GetAllRemittances(payment.PayDate, payment.BranchCode, payment.bankAccode).Sum(x => x.Amount);
                    var payTotal = GetAllPayment(payment.PayDate, payment.BranchCode, payment.bankAccode).Sum(x => x.Amount);
                    var closBal = openBal + rcecBal - payTotal;

                    BankOperation bankOperation = new BankOperation(payment.PayDate, openBal, 0.0, payment.Amount, closBal, payment.BranchCode, payment.FinYear, false, payment.bankAccode);
                    // BankOperation bankOperation = new BankOperation(payment.PayDate, 0.0, 0.0, 0.0, 0.0, payment.BranchCode, payment.FinYear, false, payment.bankAccode);
                    var isAlreadySaved = _bankOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == payment.PayDate.ToString("MM-dd-yyyy") && x.BranchCode == payment.BranchCode && x.bankAccode == payment.bankAccode);

                    if (isAlreadySaved == null)
                    {
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _bankOperationService.Add(bankOperation);
                                _bankOperationService.Save();
                                _paymentService.Add(payment);
                                _paymentService.Save();
                                //Insert to provision 
                                LoadDropDown.journalVoucherSave("BP", "I", payment.PayCode, payment.VoucherNo, payment.FinYear, "01", payment.BranchCode, payment.PayDate, payment.bankAccode, Session["UserName"].ToString());
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Payment", "Save", payment.PayCode, User.Identity.Name);
                                CashOPVM = GetAllPayment(payment.PayDate, payment.BranchCode, payment.bankAccode);
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
                        var payTotals = GetAllPayment(payment.PayDate, payment.BranchCode, payment.bankAccode).Sum(x => x.Amount);
                        isAlreadySaved.PayTotal = payTotals + payment.Amount;
                        isAlreadySaved.CloseBal = isAlreadySaved.OpenBal + isAlreadySaved.RecTotal - isAlreadySaved.PayTotal;
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _bankOperationService.Update(isAlreadySaved);
                                _bankOperationService.Save();
                                _paymentService.Add(payment);
                                _paymentService.Save();
                                //Insert to provision 
                                LoadDropDown.journalVoucherSave("BP", "I", payment.PayCode, payment.VoucherNo, payment.FinYear, "01", payment.BranchCode, payment.PayDate, payment.bankAccode, Session["UserName"].ToString());
                                var sysSet = _sysSetService.All().FirstOrDefault();
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.BankRecalculate(payment.PayDate, Convert.ToDecimal(payment.Amount), Session["ProjCode"].ToString(), payment.BranchCode, payment.FinYear, payment.bankAccode);
                                }
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Payment", "Save", payment.PayCode, User.Identity.Name);
                                CashOPVM = GetAllPayment(payment.PayDate, payment.BranchCode, payment.bankAccode);
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
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdatePayment(Payment payment)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("BankPayment_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
                    var gset = _gsetService.All().LastOrDefault();

                    payment.BranchCode = Session["BranchCode"].ToString();
                    payment.FinYear = Session["FinYear"].ToString();
                    payment.bankAccode = Session["BankAccode"].ToString();



                    string branchCode = Session["BranchCode"].ToString();
                    string finYear = Session["FinYear"].ToString();
                    string bankAccode = Session["BankAccode"].ToString();
                    var bankOperation = _bankOperationService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode && x.bankAccode == bankAccode && x.TrDate == payment.PayDate);
                    var Payment = _paymentService.All().ToList().Where(x => x.PayCode == payment.PayCode.Trim()).FirstOrDefault();
                    if (bankOperation != null && Payment != null)
                    {
                        bankOperation.PayTotal = bankOperation.PayTotal + Payment.Amount - payment.Amount;
                        bankOperation.CloseBal = bankOperation.OpenBal + bankOperation.RecTotal - bankOperation.PayTotal;

                        _bankOperationService.Update(bankOperation);
                        _bankOperationService.Save();
                        Payment = payment;
                        _paymentService.All().ToList().Where(y => y.PayCode == payment.PayCode).ToList().ForEach(x => _paymentService.Delete(x));
                        _paymentService.Save();

                        _paymentService.Add(Payment);
                        _paymentService.Save();
                        // Delete Provision
                        var entryNo = payment.PayCode;
                        LoadDropDown.journalVoucherRemoval("BP", entryNo, payment.VoucherNo, payment.FinYear);

                        //Insert to provision 
                        LoadDropDown.journalVoucherSave("BP", "I", payment.PayCode, payment.VoucherNo, payment.FinYear, "01", payment.BranchCode, payment.PayDate, payment.bankAccode, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Payment", "Update", payment.PayCode, User.Identity.Name);
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        if (sysSet.CashRule == "O")
                        {
                            LoadDropDown.BankRecalculate(payment.PayDate, Convert.ToDecimal(payment.Amount), Session["ProjCode"].ToString(), payment.BranchCode, payment.FinYear, payment.bankAccode);
                        }
                        CashOPVM = GetAllPayment(payment.PayDate, payment.BranchCode, payment.bankAccode);
                        transaction.Complete();
                        return Json(new { CashOPVM, data = 1 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }

        }
        

        public ActionResult GetPaymentByPayNo(string payNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + payNo + "' and EntrySource='BP'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            var payment = _paymentService.All().FirstOrDefault(x => x.PayCode == payNo.Trim());
            if (payment != null && IsExit.Count != 0)
            {
                return Json(payment, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetWihdrawBYWithdrawNo()
        {
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPayNo(string branchCode)
        {
            return Json(GeneratePayNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GeneratePayNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string payNo = "";
            var payment = _paymentService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var pay = _paymentService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (pay != null)
                {

                    payNo = "00" + (Convert.ToInt64(pay.PayCode.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    payNo = "00000001";
                }
            }
            else
            {
                if (payment != null)
                {
                    payNo = branchCode + (Convert.ToInt64(payment.PayCode.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    payNo = branchCode + "000001";
                }

            }

            return payNo;

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

        //public List<BankOperationVModel> GetAllPayment(DateTime dateTime, string branchCode, string bankAccode)
        //{
        //    branchCode = Session["BranchCode"].ToString();
        //    bankAccode = Session["BankAccode"].ToString();
        //    var bank = new List<BankOperationVModel>();
        //    int i = 0;

        //    _paymentService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
        //        Convert.ToDateTime(x.PayDate.ToString("MM-dd-yyyy")) == Convert.ToDateTime(dateTime.ToString("MM-dd-yyyy"))).ToList().ForEach(x => bank.Add(
        //        new BankOperationVModel(++i, x.NewChartPur.AcName, x.Amount, x.PayCode,x.VoucherNo ,"BP")));



        //    _withdrawService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
        //        Convert.ToDateTime(x.WithdrawDate.ToString("MM-dd-yyyy")) == Convert.ToDateTime(dateTime.ToString("MM-dd-yyyy"))).ToList().ForEach(x => bank.Add(
        //        new BankOperationVModel(++i, "Withdraw From Bank", x.Amount, x.WithdrawNo,  x.VoucherNo,"BW")));
        //    return bank;
        //}


        public List<BankOperationVModel> GetAllPayment(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            bankAccode = Session["BankAccode"].ToString();
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

        public ActionResult PaymentNoIncreement(Payment model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var paymentNo = _paymentService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int pNo = Convert.ToInt32(paymentNo.PayCode);
                string PaymentNo = (pNo + 1).ToString();
                string Paymentno = "";
                if (PaymentNo.Length > 7)
                {
                    Paymentno = PaymentNo.ToString();
                }
                else
                {
                    Paymentno = "0" + PaymentNo.ToString();
                }

                return Json(Paymentno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

    }
}
