using App.Domain;
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
using App.Domain.ViewModel;

namespace AcclineERP.Controllers
{
    public class SalesCollectionStatController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IProjInfoAppService _ProjInfoService;
        public SalesCollectionStatController(IBranchAppService _BranchService, IFYDDAppService _FYDDService,
            IProjInfoAppService _ProjInfoService)
        {
            this._BranchService = _BranchService;
            this._FYDDService = _FYDDService;
            this._ProjInfoService = _ProjInfoService;
        }

        public ActionResult SalesCollectionStatRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");

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
        public ActionResult SalesCollectionStatementRptPdf(DateTime fDate, DateTime tDate, string ProjCode, string BranchCode, string FinYear)
        {

            ViewBag.BranchName = "All";
            if (BranchCode != "")
            {
                String BranchName = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
                ViewBag.BranchName = BranchName;
            }

            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(fDate), Convert.ToString(tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SalesCollectionStatRpt_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }




            string customerGroup = "";
            //rpt_SP_SalesCollectionStat_1  @fdate smalldatetime, @tdate smalldatetime, @ProjCode varchar(3), @BranchCode varchar(3),@FinYear varchar(7), @customerGroup varchar(7)

            string sql = string.Format("EXEC rpt_SP_SalesCollectionStat_1 '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + ProjCode + "', '" + BranchCode + "', '" + FinYear + "','" + customerGroup + "' "); //,'" + Session["UserName"] + "' 

           
            //string sql = string.Format("EXEC rpt_SP_SalesCollectionStat_1 '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + ProjCode + "', '" + BranchCode + "', '" + FinYear + "','"+ customerGroup +"'");
            IEnumerable<SalesCollectionStat> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<SalesCollectionStat>(sql).ToList();
            }
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
            //ViewBag.fDate = fDate.ToString("dd-MMM-yyyy");
            //ViewBag.tDate = tDate.ToString("dd-MMM-yyyy");
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("rptSalesCollectionStatePdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            };

        }
	}
}