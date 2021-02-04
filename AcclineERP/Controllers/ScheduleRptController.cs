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
    public class ScheduleRptController : Controller
    {

        private readonly IItemInfoAppService _ItemService;
        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        private readonly INewChartAppService _NewChartService;
        private readonly IBranchAppService _BranchService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IUserBranchAppService _userbranchService;
        private readonly IDynaCapAppService _dynaCapService;
        private readonly ISysSetAppService _sysSetService;
        private readonly ISummaryReportAppService _summaryReportService;
        private readonly IFYDDAppService _FYDDService;
        public ScheduleRptController(IItemInfoAppService _ItemService, ISubsidiaryInfoAppService _SubsidiaryService,
            INewChartAppService _NewChartService, IBranchAppService _BranchService, IEmployeeAppService _employeeService,
            ISummaryReportAppService _summaryReportService, IUserBranchAppService _userbranchService, IDynaCapAppService _dynaCapService,
            ISysSetAppService _sysSetService, IFYDDAppService _FYDDService)
        {
            this._ItemService = _ItemService;
            this._SubsidiaryService = _SubsidiaryService;
            this._NewChartService = _NewChartService;
            this._BranchService = _BranchService;
            this._employeeService = _employeeService;
            this._summaryReportService = _summaryReportService;
            this._userbranchService = _userbranchService;
            this._sysSetService = _sysSetService;
            this._dynaCapService = _dynaCapService;
            this._FYDDService = _FYDDService;
        }
        public ActionResult ScheduleRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                ViewBag.AccountCode = LoadDropDown.LoadContlAccount("2", _ItemService, _SubsidiaryService, _NewChartService);
                ViewBag.UnitCode = LoadDropDown.LoadUnit();
                ViewBag.DeptCode = LoadDropDown.LoadDept();
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



        public SelectList GardenSelection()
        {
            var UserName = Session["UserName"].ToString();
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
                branchInfo.Insert(0, new { BranchCode = "", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else if (branchInfo.Count > 1)
            {
                branchInfo.Insert(0, new { BranchCode = "", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetDynaSysSet()
        {
            VMDynSysSet DsSet = new VMDynSysSet();

            DsSet.SysSet = _sysSetService.All().ToList().FirstOrDefault();
            DsSet.DynaCap = _dynaCapService.All().ToList().FirstOrDefault();

            if (DsSet.SysSet != null && DsSet.DynaCap != null)
            {
                return Json(DsSet, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ScheduleRptPdf(string AccountCode, string BranchCode, string UnitCode, string DeptCode, string fDate, string toDate)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(fDate, toDate, Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("ScheduleRpt", "ScheduleRpt", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptSchedule_Preview"))
            {
                string errMsg = "No View Permission for this User !!";
                return RedirectToAction("ScheduleRpt", "ScheduleRpt", new { errMsg });
            }
            string ProjCode = "01";
            VMDynSysSet DsSet = new VMDynSysSet();
            DsSet.DynaCap = _dynaCapService.All().ToList().FirstOrDefault();
            if (AccountCode != "")
            {
                ViewBag.Account = _NewChartService.All().FirstOrDefault(x => x.Accode == AccountCode.Trim()).AcName.ToString();
            }
            else
            {
                ViewBag.Account = "All";
            }

            ViewBag.RptCap = "Schedule Report of " + ViewBag.Account;
            ViewBag.Col1Cap = "Code";
            ViewBag.Col2Cap = "Name";
            ViewBag.Col3Cap = "Opening";
            ViewBag.Col4Cap = "Dr. Amount";
            ViewBag.Col5Cap = "Cr. Amount";
            ViewBag.Col6Cap = "Balance";
            ViewBag.fDate = fDate;
            ViewBag.tDate = toDate;

            var sysSet = _sysSetService.All().FirstOrDefault();
            string CriteriaBranch = ""; string CriteriaUnit = "";
            if (sysSet.HasBranch == true)
            {
                if (BranchCode != "")
                {
                    ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode.Trim()).BranchName.ToString();
                }
                else
                {
                    ViewBag.Branch = "All";
                }
                CriteriaBranch = DsSet.DynaCap.Branch + ": " + ViewBag.Branch + ", ";

            }
            if (sysSet.HasUnit == true)
            {
                CriteriaUnit = DsSet.DynaCap.Unit + ": " + LoadDropDown.LoadUnitInfo(UnitCode) + ", ";
            }
            ViewBag.Criteria = CriteriaBranch + CriteriaUnit + DsSet.DynaCap.Dept + ": " + LoadDropDown.LoadDeptInfo(DeptCode);

            string sql = string.Format("EXEC rptSchedule '" + Session["FinYear"] + "','" + ProjCode + "','" + BranchCode + "','" + UnitCode + "','" + DeptCode + "','" + Convert.ToDateTime(fDate).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(toDate).ToString("yyyy-MM-dd") + "','" + AccountCode + "'");
            List<SummaryReport> rptSchedule = _summaryReportService.SqlQueary(sql).ToList();
            if (rptSchedule.Count == 0)
            {
                string errMsg = "There is no data in this combination. Please try again !!!";
                return RedirectToAction("Search", "Report", new { errMsg });
            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return new Rotativa.ViewAsPdf("rptSchedulePdf", rptSchedule) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }
    }
}