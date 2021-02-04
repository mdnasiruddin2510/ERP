using App.Domain.Interface.Service;
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
    public class StoreRptController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IItemInfoAppService _ItemInfoService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        public StoreRptController(IBranchAppService _BranchService, 
                                       IIssRecSrcDestAppService _issRecvSrcDestService, IFYDDAppService _FYDDService,
                                        IItemInfoAppService _ItemInfoService)
        {
            this._BranchService = _BranchService;
            this._FYDDService = _FYDDService;
            this._ItemInfoService = _ItemInfoService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
        }
        // GET: Store
        public ActionResult StoreRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());

                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.ItemCode = new SelectList(_ItemInfoService.All().ToList(), "ItemCode", "ItemName");
                ViewBag.Source = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.Type == "S"), "SrcDestId", "SrcDestName");
                ViewBag.DesLocCode = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.Type == "D"), "SrcDestId", "SrcDestName");
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
        public ActionResult StoreRptPdf(DateTime fDate, DateTime tDate, string BranchCode, string ItemCode, string FinYear)
        {
            //string sql = string.Format("EXEC rpt_SP_CollectionStat_1'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "', '" + BranchCode + "', '" + FinYear + "',''");
            //IEnumerable<MovementItemVM> VchrLst;
            //using (AcclineERPContext dbContext = new AcclineERPContext())
            //{
            //    VchrLst = dbContext.Database.SqlQuery<MovementItemVM>(sql).ToList();
            //}
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
            ViewBag.ItemCode = _ItemInfoService.All().Where(s => s.ItemCode == ItemCode).Select(x => x.ItemCode).FirstOrDefault();
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("StoreRptPdf", "", "")
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }
    }
}