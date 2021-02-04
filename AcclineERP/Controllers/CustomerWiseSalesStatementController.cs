using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
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
    public class CustomerWiseSalesStatementController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        public CustomerWiseSalesStatementController(IBranchAppService _BranchService, IFYDDAppService _FYDDService,
            IProjInfoAppService _ProjInfoService, ISubsidiaryInfoAppService _SubsidiaryService)
        {
            this._BranchService = _BranchService;
            this._FYDDService = _FYDDService;
            this._ProjInfoService = _ProjInfoService;
            this._SubsidiaryService = _SubsidiaryService;
        }
        public ActionResult CustomerWiseSalesStatementRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.SubCode = new SelectList(_SubsidiaryService.All().ToList(), "SubCode", "SubName");
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
        [HttpPost]
        public ActionResult CustomerWiseSalesStatementRptPdf(DateTime fDate, DateTime tDate, string ProjName, string BranchCode, string FinYear, string SubCode)
        {
            string customerGroup = "";
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(fDate), Convert.ToString(tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CustomerWiseSalesStatement_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }


            string sql = string.Format("EXEC rpt_SP_CustomerWiseDetails '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + ProjName + "', '" + BranchCode + "', '" + FinYear + "','" + SubCode + "'  "); //,'" + Session["UserName"] + "'
            IEnumerable<SalesStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<SalesStatementVM>(sql).ToList();
            }
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
            ViewBag.SubName = _SubsidiaryService.All().Where(s => s.SubCode == SubCode).Select(x => x.SubName).FirstOrDefault();
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            ViewBag.BranchName = "All";
            if (BranchCode != "")
            {
                String BranchName = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
                ViewBag.BranchName = BranchName;
            }
			
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("rptCustomerWiseSalesStatementPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };

        }

    }
}