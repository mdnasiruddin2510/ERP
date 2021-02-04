using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    public class CustomerWiseRateChartController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly ILocationAppService _locationService;
        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        public CustomerWiseRateChartController(IBranchAppService _BranchService, ISubsidiaryInfoAppService _SubsidiaryService,ILocationAppService _locationService)
        {
            this._BranchService = _BranchService;
            this._locationService = _locationService;
            this._SubsidiaryService = _SubsidiaryService;
        }
        public ActionResult CustomerWiseRateChartRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                //ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                //ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
                ViewBag.SubCode = new SelectList(_SubsidiaryService.All().ToList(), "SubCode", "SubName");
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
            
        }
        [HttpPost]
        public ActionResult CustomerWiseRateChartRptPdf(string BranchCode, string SubCode)
        {
            string sql = string.Format("Exec rpt_CustWiseRateChart '" + Session["BranchCode"].ToString() + "','" + SubCode + "'");
            IEnumerable<CustomerWiseRateChartVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<CustomerWiseRateChartVM>(sql).ToList();
            }
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();

            //ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            //ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            return new Rotativa.ViewAsPdf("rpt_CustomerWiseRateChartPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }
	}
}