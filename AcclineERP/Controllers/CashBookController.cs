using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class CashBookController : Controller
    {
        private readonly ISummaryReportAppService _SumRptService;
        private readonly ISummaryReportTypeAppService _SumRptTypeService;
        private readonly ILedgerCaptionAppService _LedgerCapService;
        private readonly IBranchAppService _BranchService;
        private readonly ICashBookAppService _CashBookService;
        private readonly ICashOperationAppService _CashOperationService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IUserBranchAppService _userbranchService;
        private readonly ISecUserInfoAppService _secUserInfoService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ISysSetAppService _sysSetService;

        public CashBookController(ISummaryReportAppService _SumRptService, ISummaryReportTypeAppService _SumRptTypeService,
            ILedgerCaptionAppService _LedgerCapService, IUserBranchAppService _userbranchService, IEmployeeAppService _employeeService,
            IBranchAppService _BranchService, ICashBookAppService _CashBookService, ICashOperationAppService _CashOperationService,
            ISecUserInfoAppService _secUserInfoService, IFYDDAppService _FYDDService, ISysSetAppService _sysSetService)
        {
            this._SumRptService = _SumRptService;
            this._SumRptTypeService = _SumRptTypeService;
            this._LedgerCapService = _LedgerCapService;
            this._BranchService = _BranchService;
            this._CashBookService = _CashBookService;
            this._CashOperationService = _CashOperationService;
            this._employeeService = _employeeService;
            this._userbranchService = _userbranchService;
            this._secUserInfoService = _secUserInfoService;
            this._FYDDService = _FYDDService;
            this._sysSetService = _sysSetService;
        }
        public ActionResult Search(string errMsg)
        {

            if (Session["UserID"] != null)
            {
                ViewBag.LedgerTypeCode = new SelectList(_SumRptTypeService.All().ToList(), "ReportTypeCode", "ReportTypeName");
                ViewBag.BranchCode = PermittedBranch();//new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.BookType = LoadDropDown.LoadBookType();
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                return View();

            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        public SelectList PermittedBranch()
        {

            var UserId = _secUserInfoService.All().Where(x => x.UserName == Session["UserName"].ToString()).FirstOrDefault().UserID;

            List<Branch> branchs = _BranchService.All().ToList();
            List<UserBranch> userbranch = _userbranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == UserId.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();

            if (branchInfo.Count == 1)
            {
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else if (branchInfo.Count > 1)
            {
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else
            {
                return null;
            }
        }


        public ActionResult GetCashBook(RptSearchVModel vmodel)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("Search", "CashBook", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptCashBook_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("Search", "CashBook", new { errMsg });
            }
            Session["fDate"] = vmodel.fDate;
            Session["tDate"] = vmodel.toDate;
            string finyear = Session["FinYear"].ToString();
            vmodel.BranchCode = (vmodel.BranchCode == "0") ? "" : vmodel.BranchCode;
            Session["Branch"] =  vmodel.BranchCode;

            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
            if (vmodel.BranchCode != "" && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            string sql = string.Format("EXEC rptCashBook '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.fDate.ToString("yyyy/MM/dd") + "','" + vmodel.toDate.ToString("yyyy/MM/dd") + "'");

            List<CashBook> cashBook = _CashBookService.SqlQueary(sql).ToList();
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (cashBook.Count == 0)
            {
                string errMsg = "There is no data on this date. Please try another !!!";
                return RedirectToAction("Search", "CashBook", new { errMsg });
            }
            else
            {
                ViewBag.Datef = InWord.GetAbbrMonthNameDate(vmodel.fDate);
                ViewBag.Datet = InWord.GetAbbrMonthNameDate(vmodel.toDate);                
                return View(cashBook);
            }


        }
        [HttpPost]
        public ActionResult CashBookPdf(RptSearchVModel vmodel)
        {
            vmodel.fDate = Convert.ToDateTime(Session["fDate"]);
            vmodel.toDate = Convert.ToDateTime(Session["tDate"]);
            string finyear = Session["FinYear"].ToString();
            vmodel.BranchCode = Session["Branch"].ToString();
            string sql = string.Format("EXEC rptCashBook '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.toDate.ToString("MM/dd/yyyy") + "'");
            List<CashBook> cashBook = _CashBookService.SqlQueary(sql).ToList();
            ViewBag.Datef = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.Datet = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
            if (vmodel.BranchCode != "" && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return new Rotativa.ViewAsPdf("CashBookPdf", cashBook) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }
    }
}
