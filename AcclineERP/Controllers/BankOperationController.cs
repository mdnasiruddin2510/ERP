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
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class BankOperationController : Controller
    {
        private readonly IBankOperationAppService _bankOperationService;
        private readonly IHORemitAppService _hoRemitService;
        private readonly IPaymentAppService _paymentService;
        private readonly INewChartAppService _newChartService;
        private readonly ISubsidiaryInfoAppService _subsidiaryService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IAcBRAppService _AcBRService;
        private readonly IDepositToBankAppService _depositToBankService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IOpnBalAppService _OpnBalService;
        private readonly IBankReceiptAppService _BankReceiptAppService;
        private readonly IBankInfoAppService _BankInfoService;
        private readonly ISubsidiaryCtrlAppService _subsidiaryCtrlService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly IBranchAppService _BranchService;
        private readonly ISysSetAppService _sysSetService;
        public BankOperationController(IBankOperationAppService _bankOperationService, IHORemitAppService _hoRemitService,
            IPaymentAppService _paymentService, INewChartAppService _newChartService, ITransactionLogAppService _transactionLogService,
            ISubsidiaryInfoAppService _subsidiaryService, IEmployeeAppService _employeeService, IAcBRAppService _AcBRService,
            IDepositToBankAppService _depositToBankService, IWithdrawAppService _withdrawService, IOpnBalAppService _OpnBalService,
            IBankReceiptAppService _BankReceiptAppService, IBankInfoAppService _BankInfoService, ISubsidiaryCtrlAppService _subsidiaryCtrlService,
            IJarnalVoucherAppService _jarnalVoucherService, IBranchAppService _BranchService, ISysSetAppService _sysSetService)
        {
            this._bankOperationService = _bankOperationService;
            this._hoRemitService = _hoRemitService;
            this._paymentService = _paymentService;
            this._newChartService = _newChartService;
            this._subsidiaryService = _subsidiaryService;
            this._employeeService = _employeeService;
            this._AcBRService = _AcBRService;
            this._depositToBankService = _depositToBankService;
            this._withdrawService = _withdrawService;
            this._transactionLogService = _transactionLogService;
            this._OpnBalService = _OpnBalService;
            this._BankReceiptAppService = _BankReceiptAppService;
            this._BankInfoService = _BankInfoService;
            this._subsidiaryCtrlService = _subsidiaryCtrlService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._BranchService = _BranchService;
            this._sysSetService = _sysSetService;
        }

        public ActionResult TakeDecision()
        {
            string branchCode = Session["BranchCode"].ToString();
            ViewBag.branchCode = _BranchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).ToString();
            var banks = _newChartService.All().Where(x => x.BranchCode == branchCode).ToList();
            if (banks.Count == 1)
            {
                var bank = _newChartService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode);
                Session["BankAccode"] = bank.Accode;
                return RedirectToAction("BankOperation", "BankOperation");
            }
            else
            {
                return RedirectToAction("BankSelection", "BankOperation");
            }
        }

        public ActionResult BankSelection()
        {
            string branchCode = Session["BranchCode"].ToString();
            var banks = _AcBRService.All().Where(x => x.BranchCode == branchCode && x.Ca_Ba == "Ba").ToList();
            List<NewChart> bankList = new List<NewChart>();
            foreach (var item in banks)
            {
                var bank = _newChartService.All().ToList().FirstOrDefault(x => x.Accode == item.Accode);
                bankList.Add(bank);
            }
            ViewBag.bankAccode = new SelectList(bankList, "Accode", "AcName");
            return View();
        }
        public ActionResult BankOperation(BankOperation bankOperation, string errMsg)
        {
            if (bankOperation.bankAccode == null)
            {
                bankOperation.bankAccode = Session["BankAccode"].ToString();
                var bankInfo = _newChartService.All().ToList().FirstOrDefault(x => x.Accode == bankOperation.bankAccode);
                ViewBag.bankAccode = bankInfo.AcName;
                Session["BankAccode"] = bankInfo.Accode;
            }
            else
            {
                var bankInfo = _newChartService.All().ToList().FirstOrDefault(x => x.Accode == bankOperation.bankAccode);
                ViewBag.bankAccode = bankInfo.AcName;
                Session["BankAccode"] = bankInfo.Accode;
            }
            List<NewChart> charts = _newChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "3" || x.Accode.Substring(0, 1) == "2").ToList();
            // List<NewChart> charts = _newChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "3").ToList();
            List<NewChart> finalList = new List<NewChart>();

            foreach (var chart in charts)
            {
                bool key = true;
                foreach (var temp in charts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    finalList.Add(chart);
                }
            }
            ViewBag.brPurAccode = new SelectList(finalList, "Accode", "AcName");

            List<NewChart> echarts = _newChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "4").ToList();
            List<NewChart> efinalList = new List<NewChart>();

            foreach (var chart in echarts)
            {
                bool key = true;
                foreach (var temp in echarts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    efinalList.Add(chart);
                }
            }
            ViewBag.purAccode = new SelectList(efinalList, "Accode", "AcName");
            ViewBag.SubCode = new SelectList(_subsidiaryService.All().ToList(), "SubCode", "SubName");
            ViewBag.bankCode = new SelectList(_BankInfoService.All().ToList(), "BankCode", "BankName");
            DateTime datetime = DateTime.Now;
            ViewBag.Date = datetime.ToString("dd/MM/yyyy");
            string branchCode = Session["BranchCode"].ToString();
            string bankAccode = Session["BankAccode"].ToString();
            //ViewBag.OpenBal = GetOpenBal(DateTime.Now, bankAccode);
            ViewBag.OpenBal = GetOpenBal(datetime);

            ViewBag.GLProvition = Counter("BOR", "PV");
            ViewBag.GLEntries = CountVoucherMain("BOR", "MV", datetime);
            ViewBag.GLProvitionB = Counter("BOP", "PV");
            ViewBag.GLEntriesB = CountVoucherMain("BOP", "MV", datetime);

            var bank = new List<BankOperationVModel>();
            bank = GetAllRemittances(datetime, branchCode, bankAccode);
            GetAllPayment(datetime, branchCode, bankAccode).ForEach(x => bank.Add(x));
            ViewBag.Items = bank;
            ViewBag.JobNo = LoadDropDown.LoadJobInfo();
            //By Farhad For Remittance button/popup will be show/hide.
            var sysSet = _sysSetService.All().ToList().FirstOrDefault();
            ViewBag.MrConv = sysSet.MRConv;
            ViewBag.MaintJob = sysSet.MaintJob;
            Session["MaintLot"] = sysSet.MaintLot;
            ViewBag.MaintLot = sysSet.MaintLot;
            ViewBag.HasRemittance = sysSet.HasRemittance;
            ViewBag.IsSubsidiaryCtrl = sysSet.IsSubsidiaryCtrl;
            if (sysSet.IsSubsidiaryCtrl == true)
            {
                ViewBag.brSubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
            }
            else
            {
                ViewBag.brSubCode = LoadDropDown.LoadSubsidiary(_subsidiaryService);
            }
            ViewBag.errMsg = errMsg;
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View();
        }


        [HttpPost]
        public ActionResult BankOperation(DateTime TrDate)
        {

            List<NewChart> charts = _newChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "3").ToList();
            List<NewChart> finalList = new List<NewChart>();

            foreach (var chart in charts)
            {
                bool key = true;
                foreach (var temp in charts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    finalList.Add(chart);
                }
            }
            ViewBag.brPurAccode = new SelectList(finalList, "Accode", "AcName");
            List<NewChart> echarts = _newChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "4").ToList();
            List<NewChart> efinalList = new List<NewChart>();

            foreach (var chart in echarts)
            {
                bool key = true;
                foreach (var temp in echarts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    efinalList.Add(chart);
                }
            }
            ViewBag.purAccode = new SelectList(efinalList, "Accode", "AcName");
            ViewBag.SubCode = new SelectList(_subsidiaryService.All().ToList(), "SubCode", "SubName");
            ViewBag.bankCode = new SelectList(_BankInfoService.All().ToList(), "BankCode", "BankName");
            DateTime datetime = TrDate;
            ViewBag.Date = datetime.ToString("dd/MM/yyyy");
            string branchCode = Session["BranchCode"].ToString();
            string bankAccode = Session["BankAccode"].ToString();
            // ViewBag.OpenBal = GetOpenBal(DateTime.Now, bankAccode);

            ViewBag.OpenBal = GetOpenBal(TrDate);
            ViewBag.GLProvition = Counter("BOR", "PV");
            ViewBag.GLEntries = CountVoucherMain("BOR", "MV", datetime);
            ViewBag.GLProvitionB = Counter("BOP", "PV");
            ViewBag.GLEntriesB = CountVoucherMain("BOP", "MV", datetime);

            //DateTime datetime = TrDate;
            var bank = new List<BankOperationVModel>();
            bank = GetAllRemittances(datetime, branchCode, bankAccode);
            GetAllPayment(datetime, branchCode, bankAccode).ForEach(x => bank.Add(x));
            ViewBag.Items = bank;
            ViewBag.JobNo = LoadDropDown.LoadJobInfo();

            //By Farhad For Remittance button/popup will be show/hide.
            var sysSet = _sysSetService.All().ToList().FirstOrDefault();
            ViewBag.MrConv = sysSet.MRConv;
            ViewBag.MaintJob = sysSet.MaintJob;
            Session["MaintLot"] = sysSet.MaintLot;
            ViewBag.MaintLot = sysSet.MaintLot;
            ViewBag.HasRemittance = sysSet.HasRemittance;
            ViewBag.IsSubsidiaryCtrl = sysSet.IsSubsidiaryCtrl;
            if (sysSet.IsSubsidiaryCtrl == true)
            {
                ViewBag.brSubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
            }
            else
            {
                ViewBag.brSubCode = LoadDropDown.LoadSubsidiary(_subsidiaryService);
            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View();
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

        public ActionResult GetSubsidiaryByPurpose(string purAccode)
        {
            return Json(LoadDropDown.LoadSubsidiaryByPurpuse(purAccode, _subsidiaryService, _subsidiaryCtrlService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult DelBankOperation(string tag, string transactionNo, string dateTime)
        {
            if (tag == "RT")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("Remittance_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            if (tag == "BR")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("BankReceipt_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            if (tag == "BP")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("BankPayment_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            List<BankOperationVModel> CashOPVM = new List<BankOperationVModel>();
            string branchCode = Session["BranchCode"].ToString();
            string bankAccode = Session["BankAccode"].ToString();
            switch (tag)
            {
                case "RT":
                    {


                        var hoRemit = _hoRemitService.All().ToList().FirstOrDefault(x => x.RemitNo == transactionNo.Trim());
                        if (hoRemit != null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {

                                    _hoRemitService.Delete(hoRemit);
                                    _hoRemitService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "H/O Remittance", "Delete", hoRemit.RemitNo, User.Identity.Name);
                                    var entryNo = hoRemit.RemitNo;
                                    // var entryNo = Convert.ToInt64(hoRemit.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                    LoadDropDown.journalVoucherRemoval("RT", entryNo, hoRemit.VoucherNo, hoRemit.FinYear);
                                    CashOPVM = GetAllRemittances(Convert.ToDateTime(dateTime), branchCode, bankAccode);
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
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                    }
                case "BR":
                    {


                        var bankReceipt = _BankReceiptAppService.All().ToList().FirstOrDefault(x => x.BReceiptNo == transactionNo.Trim());
                        if (bankReceipt != null)
                        {
                            //bankReceipt.BranchCode = Session["BranchCode"].ToString();
                            //bankReceipt.FinYear = Session["FinYear"].ToString();
                            //bankReceipt.bankAccode = Session["BankAccode"].ToString();
                            //bankReceipt.NewChart = _newChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.purAccode.Trim());
                            //bankReceipt.bankNewChart = _newChartService.All().ToList().FirstOrDefault(x => x.Accode == bankReceipt.bankAccode.Trim());
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _BankReceiptAppService.Delete(bankReceipt);
                                    _BankReceiptAppService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Bank Receipt", "Delete", bankReceipt.BReceiptNo, User.Identity.Name);
                                    var entryNo = bankReceipt.BReceiptNo;
                                    // var entryNo = Convert.ToInt64(bankReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                    LoadDropDown.journalVoucherRemoval("BR", entryNo, bankReceipt.VoucherNo, bankReceipt.FinYear);
                                    CashOPVM = GetAllRemittances(bankReceipt.BReceiptDate, bankReceipt.BranchCode, bankAccode);
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
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                    }
                case "BW":
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                case "BP":
                    {

                        var payment = _paymentService.All().ToList().FirstOrDefault(x => x.PayCode == transactionNo.Trim());
                        if (payment != null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _paymentService.Delete(payment);
                                    _paymentService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Expene", "Delete", payment.PayCode, User.Identity.Name);
                                    var entryNo = payment.PayCode;
                                    // var entryNo = Convert.ToInt64(payment.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                    LoadDropDown.journalVoucherRemoval("BP", entryNo, payment.VoucherNo, payment.FinYear);
                                    CashOPVM = GetAllPayment(payment.PayDate, payment.BranchCode, bankAccode);
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
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                    }
                case "BD":
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                default:
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }


            }


        }

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


        public ActionResult SaveBankOperation(DateTime date)
        {
            string branchCode = Session["BranchCode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string bankAccode = Session["BankAccode"].ToString();
            //BankOperation bankOperation = new App.Domain.BankOperation();

            var bankOperation = _bankOperationService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode && x.bankAccode == bankAccode && x.TrDate.ToString("MM-dd-yyyy") == date.ToString("MM-dd-yyyy"));

            if (bankOperation != null)
            {
                bankOperation.TrDate = Convert.ToDateTime(date.ToString("MM-dd-yyyy"));
                bankOperation.OpenBal = GetOpenBal(date);
                bankOperation.RecTotal = GetAllRemittances(date, branchCode, bankAccode).Sum(x => x.Amount);
                bankOperation.PayTotal = GetAllPayment(date, branchCode, bankAccode).Sum(x => x.Amount);
                bankOperation.CloseBal = bankOperation.OpenBal + bankOperation.RecTotal - bankOperation.PayTotal;
                bankOperation.FinYear = finYear;
                bankOperation.GLPost = false;
                bankOperation.BranchCode = branchCode;
                bankOperation.bankAccode = bankAccode;

                _bankOperationService.Update(bankOperation);
                _bankOperationService.Save();
                return Json("1", JsonRequestBehavior.AllowGet);
            }

            else if (bankOperation == null)
            {
                bankOperation.TrDate = Convert.ToDateTime(date.ToString("MM-dd-yyyy"));
                bankOperation.OpenBal = GetOpenBal(date);
                bankOperation.RecTotal = GetAllRemittances(date, branchCode, bankAccode).Sum(x => x.Amount);
                bankOperation.PayTotal = GetAllPayment(date, branchCode, bankAccode).Sum(x => x.Amount);
                bankOperation.CloseBal = bankOperation.OpenBal + bankOperation.RecTotal - bankOperation.PayTotal;
                bankOperation.FinYear = finYear;
                bankOperation.GLPost = false;
                bankOperation.BranchCode = branchCode;
                bankOperation.bankAccode = bankAccode;

                _bankOperationService.Add(bankOperation);
                _bankOperationService.Save();
                return Json("1", JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (pageType == "BOR")
            {
                if (!rUser.HasPermission("BankReceive_VchrWaitingForPosting"))
                {
                    string errMsg = "VWP";
                    return RedirectToAction("BankOperation", "BankOperation", new { errMsg });
                }
            }
            else if (pageType == "BOP")
            {
                if (!rUser.HasPermission("BankPayment_VchrWaitingForPosting"))
                {
                    string errMsg = "VWP";
                    return RedirectToAction("BankOperation", "BankOperation", new { errMsg });
                }
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("select pvd.PVchrDetailId, pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear left outer join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "'  and pvm.BranchCode ='" + Session["BranchCode"].ToString() + "'"
                 + "and pvm.BranchCode= '" + branchCode + "'"
                + "group by pvd.PVchrDetailId, pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac order by pvd.PVchrDetailId");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("BankOperation", "BankOperation", new { errMsg });

            }
            else
            {
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (pageType == "BOR")
            {
                if (!rUser.HasPermission("BankReceive_VchrList"))
                {
                    string errMsg = "VL";
                    return RedirectToAction("BankOperation", "BankOperation", new { errMsg });
                }
            }
            else if (pageType == "BOP")
            {
                if (!rUser.HasPermission("BankPayment_VchrList"))
                {
                    string errMsg = "VL";
                    return RedirectToAction("BankOperation", "BankOperation", new { errMsg });
                }
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("BankOperation", "BankOperation", new { errMsg });
            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntriesPdf(string vchrNo)
        {
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary,pvd.Narration,"
                + " pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from VchrMain as pvm "
                + "join VchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear  where pvm.VchrNo ='" + vchrNo + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }
        public ActionResult GetGlECount(string pageType, string IsPv)
        {
            return Json(Counter(pageType, IsPv), JsonRequestBehavior.AllowGet);
        }
        public string Counter(string pageType, string IsPv)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = "";
            if ((pageType == "BOR" || pageType == "BOP") && (IsPv == "PV"))
            {
                sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain "
               + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "' and BranchCode='" + branchCode + "'");
            }
            else
            {
                sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
               + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "' and BranchCode='" + branchCode + "'");
            }
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }


        public string CountVoucherMain(string pageType, string IsPv, DateTime TrDate)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = "";
            if ((pageType == "BOR" || pageType == "BOP") && (IsPv == "PV"))
            {
                sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain "
               + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "' and pvm.VDate='" + TrDate.ToString("MM-dd-yyyy") + "'and BranchCode='" + branchCode + "'");
            }
            else
            {
                sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
               + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'and pvm.VDate='" + TrDate.ToString("MM-dd-yyyy") + "'and BranchCode='" + branchCode + "'");
            }
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }

        public ActionResult DeleteVoucherPro(string remitNo, string tag)
        {

            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + remitNo + "' and EntrySource='" + tag + "'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            //var hoRemit = _hoRemitService.All().FirstOrDefault(x => x.RemitNo == remitNo.Trim());
            if (IsExit.Count == 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }

        }

    }
}
