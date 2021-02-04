using AcclineERP.Models;
using App.Domain;
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

namespace AcclineERP.Controllers
{
    public class ProductionRptController : Controller
    {
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IBranchAppService _branchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ILocationAppService _locationService;
        public ProductionRptController(IItemTypeAppService _itemTypeService, IBranchAppService _branchService, IFYDDAppService _FYDDService,
            ILocationAppService _locationService, ICommonPVVMAppService _CommonVmService)
        {
            this._CommonVmService = _CommonVmService;
            this._itemTypeService = _itemTypeService;
            this._branchService = _branchService;
            this._FYDDService = _FYDDService;
            this._locationService = _locationService;
        }
        // GET: ProductionRpt
        public ActionResult ProductionRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.ItemCode = LoadDropDown.LoadItemByItemType("", _CommonVmService);
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
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
        public ActionResult LoadItemByItemType(string ItemTypeCode)
        {
            return Json(LoadDropDown.LoadItemByItemType(ItemTypeCode, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ProductionRptPdf(DateTime fDate, DateTime tDate, string BranchCode, string ItemCode, string FinYear, string ItemType)
        {
            string sql = string.Format("Exec rpt_ProductionStat '" + BranchCode + "','" + ItemType + "', '" + ItemCode + "','" + fDate.ToString("yyyy/MM/dd") + "', '" + tDate.ToString("yyyy/MM/dd") + "'");
            IEnumerable<ProductionRptVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<ProductionRptVM>(sql).ToList();
            }
            ViewBag.BranchCode = _branchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            return new Rotativa.ViewAsPdf("rpt_ProductionPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }
    }
}