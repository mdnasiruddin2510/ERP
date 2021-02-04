using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportLedgerAppService _ReportLedgerService;
        private readonly ILedgerTypeAppService _LedgerTypeService;
        private readonly ILedgerCaptionAppService _LedgerCapService;
        private readonly IItemInfoAppService _ItemService;
        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        private readonly INewChartAppService _NewChartService;
        private readonly IBranchAppService _BranchService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IJarnalVoucherAppService _jarnalVoucher;
        private readonly IVchrPreviewVMAppService _VchrPreviewVMService;
        private readonly IUserBranchAppService _userbranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ISysSetAppService _sysSetService;

        public ReportController(IReportLedgerAppService _ReportLedgerService, ILedgerTypeAppService _LedgerTypeService,
            ILedgerCaptionAppService _LedgerCapService, IItemInfoAppService _ItemService, ISubsidiaryInfoAppService _SubsidiaryService,
            INewChartAppService _NewChartService, IBranchAppService _BranchService, IEmployeeAppService _employeeService, IJarnalVoucherAppService _jarnalVoucher,
            IVchrPreviewVMAppService _VchrPreviewVMService, IUserBranchAppService _userbranchService, IFYDDAppService _FYDDService, ISysSetAppService _sysSetService)
        {
            this._ReportLedgerService = _ReportLedgerService;
            this._LedgerTypeService = _LedgerTypeService;
            this._LedgerCapService = _LedgerCapService;
            this._ItemService = _ItemService;
            this._SubsidiaryService = _SubsidiaryService;
            this._NewChartService = _NewChartService;
            this._BranchService = _BranchService;
            this._employeeService = _employeeService;
            this._jarnalVoucher = _jarnalVoucher;
            this._VchrPreviewVMService = _VchrPreviewVMService;
            this._userbranchService = _userbranchService;
            this._FYDDService = _FYDDService;
            this._sysSetService = _sysSetService;
        }
        public ActionResult Search(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.LedgerTypeCode = new SelectList(_LedgerTypeService.All().ToList(), "LedgerTypeCode", "LedgerTypeName");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName"); //GardenSelection(); 
                ViewBag.AccountCode = LoadDropDown.LoadAccount("", _ItemService, _SubsidiaryService, _NewChartService);
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                //var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public SelectList GardenSelection()
        {

            var UserName = User.Identity.Name;
            var brans = _employeeService.All().Where(x => x.Email == UserName).FirstOrDefault();

            //shahin

            List<Branch> branchs = _BranchService.All().ToList();
            List<UserBranch> userbranch = _userbranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == brans.Id.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();

            if (branchInfo.Count == 1)
            {
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else if (branchInfo.Count > 1)
            {
                //List<Branch> branch = _BranchService.All().ToList();
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else
            {
                return null;
            }


        }

        public ActionResult GetAccountByLedgType(string ledgTypeCode)
        {
            return Json(LoadDropDown.LoadAccount(ledgTypeCode, _ItemService, _SubsidiaryService, _NewChartService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetAccountByControlType(string Transection)
        {
            return Json(LoadDropDown.LoadContlAccount(Transection, _ItemService, _SubsidiaryService, _NewChartService), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReportLedger(RptSearchVModel vmodel, string finyear)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("Search", "Report", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptGeneralLedger_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("Search", "Report", new { errMsg });
            }

            Session["Branch"] = vmodel.BranchCode;
            Session["LedgerType"] = vmodel.LedgerTypeCode;
            Session["AccountCode"] = vmodel.AccountCode;
            Session["fDate"] = vmodel.fDate;
            Session["tDate"] = vmodel.toDate;


            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.LedgerTypeCode == vmodel.LedgerTypeCode);


            ViewBag.LedgerCap = ledger.LedgerCap;
            ViewBag.RptCap = ledger.RptCap;
            ViewBag.OpeningCap = ledger.OpeningCap;
            ViewBag.ClosingCap = ledger.ClosingCap;
            ViewBag.Col1Cap = ledger.Col1Cap;
            ViewBag.Col2Cap = ledger.Col2Cap;
            ViewBag.Col3Cap = ledger.Col3Cap;
            ViewBag.Col4Cap = ledger.Col4Cap;
            ViewBag.Col5Cap = ledger.Col5Cap;
            ViewBag.Col6Cap = ledger.Col6Cap;
            ViewBag.Col7Cap = ledger.Col7Cap;
            ViewBag.Col8Cap = ledger.Col8Cap;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.AccountCode = vmodel.AccountCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;

            if (vmodel.BranchCode != null && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }


            if (vmodel.LedgerTypeCode == "01")
            {
                ViewBag.Account = _NewChartService.All().FirstOrDefault(x => x.Accode == vmodel.AccountCode.Trim()).AcName.ToString();
            }
            else if (vmodel.LedgerTypeCode == "02")
            {
                ViewBag.Account = _SubsidiaryService.All().FirstOrDefault(x => x.SubCode == vmodel.AccountCode.Trim()).SubName.ToString();
            }
            else
            {
                ViewBag.Account = _ItemService.All().FirstOrDefault(x => x.ItemCode == vmodel.AccountCode.Trim()).ItemName.ToString();
            }

            finyear = Session["FinYear"].ToString();


            string sql = string.Format(" EXEC " + ledger.SP_Name + " '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.AccountCode + "','" + vmodel.fDate.ToString("MM-dd-yyyy") + "','" + vmodel.toDate.ToString("MM-dd-yyyy") + "'");
            List<ReportLedger> rptLedger = _ReportLedgerService.SqlQueary(sql).ToList();
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View(rptLedger);


        }


        [HttpPost]
        public ActionResult ReportLedgerPdf(RptSearchVModel vmodel, string finyear)
        {
            vmodel.BranchCode = Session["Branch"].ToString();
            vmodel.LedgerTypeCode = Session["LedgerType"].ToString();
            vmodel.AccountCode = Session["AccountCode"].ToString();
            //vmodel.fDate = Session["fDate"].;
            //vmodel.tDate = Session["tDate"].ToString();
            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.LedgerTypeCode == vmodel.LedgerTypeCode);

            ViewBag.LedgerCap = ledger.LedgerCap;
            ViewBag.RptCap = ledger.RptCap;
            ViewBag.OpeningCap = ledger.OpeningCap;
            ViewBag.ClosingCap = ledger.ClosingCap;
            ViewBag.Col1Cap = ledger.Col1Cap;
            ViewBag.Col2Cap = ledger.Col2Cap;
            ViewBag.Col3Cap = ledger.Col3Cap;
            ViewBag.Col4Cap = ledger.Col4Cap;
            ViewBag.Col5Cap = ledger.Col5Cap;
            ViewBag.Col6Cap = ledger.Col6Cap;
            ViewBag.Col7Cap = ledger.Col7Cap;
            ViewBag.Col8Cap = ledger.Col8Cap;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.AccountCode = vmodel.AccountCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            if (vmodel.BranchCode != null)
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }

            if (vmodel.LedgerTypeCode == "01")
            {
                ViewBag.Account = _NewChartService.All().FirstOrDefault(x => x.Accode == vmodel.AccountCode.Trim()).AcName.ToString();
            }
            else if (vmodel.LedgerTypeCode == "02")
            {
                ViewBag.Account = _SubsidiaryService.All().FirstOrDefault(x => x.SubCode == vmodel.AccountCode.Trim()).SubName.ToString();
            }
            else
            {
                ViewBag.Account = _ItemService.All().FirstOrDefault(x => x.ItemCode == vmodel.AccountCode.Trim()).ItemName.ToString();
            }

            ViewBag.PrintDate = DateTime.Now.ToShortDateString();

            finyear = Session["FinYear"].ToString();

            string sql = string.Format(" EXEC " + ledger.SP_Name + " '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.AccountCode + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.toDate.ToString("MM/dd/yyyy") + "'");
            List<ReportLedger> rptLedger = _ReportLedgerService.SqlQueary(sql).ToList();
            //if (rptLedger.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("Search", "Report", new { errMsg });
            //}
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return new Rotativa.ViewAsPdf("ReportLedgerPdf", rptLedger) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }


        public ActionResult GetVoucherPreview(string VchrNo, string FinYear)
        {
            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == "rptVoucher");
            ViewBag.RptCap = ledger.RptCap;
            ViewBag.Col1Cap = ledger.Col1Cap;
            ViewBag.Col2Cap = ledger.Col2Cap;
            ViewBag.Col3Cap = ledger.Col3Cap;
            ViewBag.Col4Cap = ledger.Col4Cap;
            ViewBag.Col5Cap = ledger.Col5Cap;
            ViewBag.Col6Cap = ledger.Col6Cap;
            ViewBag.Col7Cap = ledger.Col7Cap;
            string BranchCode = Session["BranchCode"].ToString();
            ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode.Trim()).BranchName.ToString();

            FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC rptVoucher '" + FinYear + "','" + VchrNo + "'");
            List<VchrPreviewVM> rptVchr = _VchrPreviewVMService.SqlQueary(sql).ToList();
            //if (rptVchr.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("SearchVoucher", "VchrPreview", new { errMsg });
            //}
            //else
            //{
            double amt = 0;
            foreach (var item in rptVchr)
            {
                if (item.cramount != 0)
                {
                    amt += item.cramount;
                }
            }
            string InWordsamt = InWord.ConvertToWords(amt.ToString());
            ViewBag.InWordsAmt = InWordsamt;
            return View("~/Views/VchrPreview/GetVoucherPreview.cshtml", rptVchr);
            //}
        }
    }

}

