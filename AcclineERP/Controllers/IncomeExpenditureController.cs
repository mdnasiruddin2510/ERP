using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class IncomeExpenditureController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly IBalSheetRptAppService _BalSheetRptService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IFYDDAppService _FYDDService;
        public IncomeExpenditureController(IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService,
            IBalSheetRptAppService _BalSheetRptService, ISysSetAppService _sysSetService, IFYDDAppService _FYDDService)
        {
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._BalSheetRptService = _BalSheetRptService;
            this._sysSetService = _sysSetService;
            this._FYDDService = _FYDDService;
        }


        // GET: IncomeExpenditure
        public ActionResult IncomeExpenditure()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ProjName = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.BrName = new SelectList(_BranchService.All().ToList(), "BrCode", "BrName");
                ViewBag.FYDD = new SelectList(_FYDDService.All().ToList(), "FinYear", "FinYear");
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;

                //ViewBag.RptCaption = "Income and Expenditure A/C Report";

                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        [HttpPost]
        public ActionResult IncomeExpenditurePdf(string ProjName, string fDate, string tDate)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptBalanceSheet_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("IncExpAcRpt", "IncExpAcRpt", new { errMsg });
            }
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(Convert.ToDateTime(fDate));
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(Convert.ToDateTime(tDate));
            string sql = string.Format("Exec rptIncExpAC1 '" + ProjName + "','" + Session["BranchCode"].ToString() + "','" + Convert.ToDateTime(fDate).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(tDate).ToString("MM/dd/yyyy") + "','" + Session["FinYear"].ToString() + "'");

            List<BalSheetRptVM> balSheet = _BalSheetRptService.SqlQueary(sql).ToList();

            Response.AppendHeader("Content-Disposition", "inline; filename= IncomeExpenditure_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("IncomeExpenditurePdf", "", balSheet)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                //FileName = RptName + "-" + DateTime.Now.ToShortDateString() + ".pdf" , contStatementReportPdf
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }


    }
}