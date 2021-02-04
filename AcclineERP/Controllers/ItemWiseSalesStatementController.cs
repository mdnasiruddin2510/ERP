using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
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
    public class ItemWiseSalesStatementController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly ILocationAppService _locationService;
        private readonly IItemInfoAppService _ItemService;
        private readonly IFYDDAppService _FYDDService;
        public ItemWiseSalesStatementController(IBranchAppService _BranchService, ILocationAppService _locationService, IItemInfoAppService _ItemService, IFYDDAppService _FYDDService)
        {
            this._BranchService = _BranchService;
            this._locationService = _locationService;
            this._ItemService = _ItemService;
            this._FYDDService = _FYDDService;
        }
        public ActionResult ItemWiseSalesStatementRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
                ViewBag.ItemCode = LoadDropDown.LoadItems(_ItemService);
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
        public ActionResult ItemWiseSalesStatementRptPdf(DateTime fDate,DateTime tDate, string BranchCode,string LocCode,string FinYear,string ItemCode)
        {
            string sql = string.Format("Exec rpt_ItemWiseSalesStat '" + BranchCode + "','" + LocCode + "','" +ItemCode +"', '" + fDate.ToString("yyyy/MM/dd") + "', '" + tDate.ToString("yyyy/MM/dd") + "'");
            IEnumerable<ItemWiseSaleVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<ItemWiseSaleVM>(sql).ToList();
            }
            ViewBag.LocName = _locationService.All().Where(s => s.LocCode == LocCode).Select(x => x.LocName).FirstOrDefault();

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            return new Rotativa.ViewAsPdf("rpt_ItemWiseSalesStatementPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }
	}
}