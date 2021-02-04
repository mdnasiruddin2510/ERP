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
    public class HORemittanceController : Controller
    {
        private readonly IEmployeeAppService _employeeService;
        private readonly IHORemitAppService _hoRemitService;
        private readonly IBankOperationAppService _bankOperationService;
        private readonly IDepositToBankAppService _depositToBankService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IBankReceiptAppService _BankReceiptAppService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IOpnBalAppService _OpnBalService;
        private readonly IPaymentAppService _paymentService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly IGsetAppService _gsetService;
        public HORemittanceController(IEmployeeAppService _employeeService, IHORemitAppService _hoRemitService,
            IBankOperationAppService _bankOperationService, ITransactionLogAppService _transactionLogService,
            IDepositToBankAppService _depositToBankService, IBankReceiptAppService _BankReceiptAppService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IOpnBalAppService _OpnBalService, IPaymentAppService _paymentService, IWithdrawAppService _withdrawService, IGsetAppService _gsetService)
        {
            this._bankOperationService = _bankOperationService;
            this._employeeService = _employeeService;
            this._hoRemitService = _hoRemitService;
            this._transactionLogService = _transactionLogService;
            this._depositToBankService = _depositToBankService;
            this._BankReceiptAppService = _BankReceiptAppService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._OpnBalService = _OpnBalService;
            this._paymentService = _paymentService;
            this._withdrawService = _withdrawService;
            this._gsetService = _gsetService;
        }
        public ActionResult SaveHOR(HORemit hoRemit)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Remittance_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            var gset = _gsetService.All().LastOrDefault();
            try
            {
                var IfExit = _hoRemitService.All().Where(x => x.RemitNo == hoRemit.RemitNo).FirstOrDefault();
                if (IfExit == null)
                {
                    hoRemit.BranchCode = Session["BranchCode"].ToString();
                    hoRemit.FinYear = Session["FinYear"].ToString();
                    hoRemit.bankAccode = Session["BankAccode"].ToString();


                    var openBal = GetOpenBal(hoRemit.RemitDate);
                    var rcecBal = GetAllRemittances(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode).Sum(x => x.Amount);
                    var payTotal = GetAllPayment(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode).Sum(x => x.Amount);
                    var closBal = openBal + rcecBal - payTotal;


                    BankOperation bankOperation = new BankOperation(hoRemit.RemitDate, openBal, hoRemit.Amount, 0.0, closBal, hoRemit.BranchCode, hoRemit.FinYear, false, hoRemit.bankAccode);
                    var isAlreadySaved = _bankOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == hoRemit.RemitDate.ToString("MM-dd-yyyy") && x.BranchCode == hoRemit.BranchCode && x.bankAccode == hoRemit.bankAccode);

                    if (isAlreadySaved == null)
                    {
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _bankOperationService.Add(bankOperation);
                                _bankOperationService.Save();
                                _hoRemitService.Add(hoRemit);
                                _hoRemitService.Save();
                                LoadDropDown.journalVoucherSave("RT", "I", hoRemit.RemitNo, hoRemit.VoucherNo, hoRemit.FinYear, "01", hoRemit.BranchCode, hoRemit.RemitDate, gset.GCA_HO, Session["UserName"].ToString());
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "HORemittance", "Save", hoRemit.RemitNo, User.Identity.Name);
                                CashOPVM = GetAllRemittances(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode);
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
                        var rcecBals = GetAllRemittances(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode).Sum(x => x.Amount);
                        isAlreadySaved.RecTotal = rcecBals + hoRemit.Amount;
                        isAlreadySaved.CloseBal = isAlreadySaved.OpenBal + isAlreadySaved.RecTotal - isAlreadySaved.PayTotal;
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _bankOperationService.Update(isAlreadySaved);
                                _bankOperationService.Save();
                                _hoRemitService.Add(hoRemit);
                                _hoRemitService.Save();
                                LoadDropDown.journalVoucherSave("RT", "I", hoRemit.RemitNo, hoRemit.VoucherNo, hoRemit.FinYear, "01", hoRemit.BranchCode, hoRemit.RemitDate, gset.GCA_HO, Session["UserName"].ToString());
                               var sysSet = _sysSetService.All().FirstOrDefault();
                               if (sysSet.CashRule == "O")
                               {
                                   LoadDropDown.BankRecalculate(hoRemit.RemitDate, Convert.ToDecimal(hoRemit.Amount), Session["ProjCode"].ToString(), hoRemit.BranchCode, hoRemit.FinYear, hoRemit.bankAccode);
                               }
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "HORemittance", "Save", hoRemit.RemitNo, User.Identity.Name);
                                CashOPVM = GetAllRemittances(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode);
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
        public double GetOpenBal(DateTime date)
        {
            double OpenBal = 0;
            string bankAccode = Session["BankAccode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC GetBankOpenBal '" + finYear + "', '" + bankAccode + "', '" + date.ToString("MM-dd-yyyy") + "'");
            OpenBal = Convert.ToDouble(_OpnBalService.SqlQueary(sql).ToList().FirstOrDefault().OpenBalance.ToString());
            return OpenBal;
        }
        public ActionResult UpdateHOR(HORemit hoRemit)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Remittance_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            var gset = _gsetService.All().LastOrDefault();
            try
            {
                hoRemit.BranchCode = Session["BranchCode"].ToString();
                hoRemit.FinYear = Session["FinYear"].ToString();
                hoRemit.bankAccode = Session["BankAccode"].ToString();


                using (var transaction = new TransactionScope())
                {
                    try
                    {

                        _hoRemitService.Update(hoRemit);
                        _hoRemitService.Save();

                        //Delete to provition 
                        // var entryNo = Convert.ToInt64(hoRemit.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                        var entryNo = hoRemit.RemitNo;
                        LoadDropDown.journalVoucherRemoval("RT", entryNo, hoRemit.VoucherNo, hoRemit.FinYear);

                        //Insert to provision 
                        LoadDropDown.journalVoucherSave("RT", "I", hoRemit.RemitNo, hoRemit.VoucherNo, hoRemit.FinYear, "01", hoRemit.BranchCode, hoRemit.RemitDate, gset.GCA_HO, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "HORemittance", "Update", hoRemit.RemitNo, User.Identity.Name);
                        CashOPVM = GetAllRemittances(hoRemit.RemitDate, hoRemit.BranchCode, hoRemit.bankAccode);
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


        public ActionResult GetHORBYRemitNo(string remitNo)
        {

            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + remitNo + "' and EntrySource='RT'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            var hoRemit = _hoRemitService.All().FirstOrDefault(x => x.RemitNo == remitNo.Trim());
            if (hoRemit != null && IsExit.Count != 0)
            {
                return Json(hoRemit, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetDtbBYDepositNo()
        {
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRemitNo(string branchCode)
        {
            return Json(GenerateRemitNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateRemitNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string remitNo = "";
            var hoRemittance = _hoRemitService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var hoRemit = _hoRemitService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (hoRemit != null)
                {

                    remitNo = "00" + (Convert.ToInt64(hoRemit.RemitNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    remitNo = "00000001";
                }
            }
            else
            {
                if (hoRemittance != null)
                {
                    remitNo = branchCode + (Convert.ToInt64(hoRemittance.RemitNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    remitNo = branchCode + "000001";
                }

            }

            return remitNo;

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

        public List<BankOperationVModel> GetAllRemittances(DateTime dateTime, string branchCode, string bankAccode)
        {
            branchCode = Session["BranchCode"].ToString();
            bankAccode = Session["BankAccode"].ToString();
            var bank = new List<BankOperationVModel>();
            int i = 0;

            _hoRemitService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.RemitDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                    new BankOperationVModel(++i, "H/O Remittance", x.Amount, x.RemitNo, "RT", x.VoucherNo)));

            _BankReceiptAppService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.BReceiptDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
                    new BankOperationVModel(++i, x.NewChart.AcName, x.Amount, x.BReceiptNo, "BR", x.VoucherNo)));

            _depositToBankService.All().ToList().Where(x => x.BranchCode == branchCode && x.bankAccode == bankAccode &&
                x.DepositDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => bank.Add(
               new BankOperationVModel(++i, "Deposit To Bank", x.Amount, x.DepositNo, "BD", x.VoucherNo)));
            return bank;
        }

        public ActionResult RemittanceNoIncreement(HORemit model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var remitNo = _hoRemitService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int rtNo = Convert.ToInt32(remitNo.RemitNo);
                string RemitNoNo = (rtNo + 1).ToString();
                string HORemitNoNo = "";
                if (RemitNoNo.Length > 7)
                {
                    HORemitNoNo = RemitNoNo.ToString();
                }
                else
                {
                    HORemitNoNo = "0" + RemitNoNo.ToString();
                }

                return Json(HORemitNoNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

    }
}
