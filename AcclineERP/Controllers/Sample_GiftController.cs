using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using AcclineERP.Models;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using App.Domain.Interface.Service;
using System.Globalization;
using System.Threading;


namespace AcclineERP.Controllers
{
    public class Sample_GiftController : Controller
    {
        // GET: Sample_Gift
        // private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IIssueMainService _IIssueMainService;
        private readonly IIssueDetailsService _IssueDetailsAppService;
        private readonly ILocationAppService _ILocationAppService;
        private readonly INewChartAppService _INewChartAppService;
        private readonly IItemInfoAppService _IItemInfoAppService;
        private readonly ISubsidiaryInfoService _ISubsidiaryInfoService;
        public Sample_GiftController(ISubsidiaryInfoService _ISubsidiaryInfoService, IItemInfoAppService _IItemInfoAppService, INewChartAppService _INewChartAppService, ILocationAppService _ILocationAppService, IIssueDetailsService _IssueDetailsAppService, IIssueMainService _IIssueMainService, IBranchAppService _BranchService, IFYDDAppService _FYDDService)
        {
            this._ISubsidiaryInfoService = _ISubsidiaryInfoService;
            this._INewChartAppService = _INewChartAppService;
            this._ILocationAppService = _ILocationAppService;
            this._FYDDService = _FYDDService;
            this._IIssueMainService = _IIssueMainService;
            this._IssueDetailsAppService = _IssueDetailsAppService;
            this._IItemInfoAppService = _IItemInfoAppService;


        }

        public ActionResult Sample_GiftRptSearch()
        {

            ViewBag.LocCode = new SelectList(_ILocationAppService.All().ToList(), "LocCode", "LocName");
            var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
            ViewBag.FyddFDate = Fydd.FYDF;
            ViewBag.FyddTDate = Fydd.FYDT;
            return View();

        }




        public ActionResult Sample_GiftRptPdf(string LocCode, DateTime fDate, DateTime tDate)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(fDate), Convert.ToString(tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Sample_Gift_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }



            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

         
            ViewBag.Location = "All";


            if (LocCode != "")
            {
                LocCode = _ILocationAppService.All().ToList().Where(s => s.LocCode == LocCode).Select(x => x.LocName).FirstOrDefault();
                ViewBag.Location = LocCode;
            } 
           

            List<Sample_giftRptVM> finalList = new List<Sample_giftRptVM>(); List<String> IssueIdList = new List<String>();
            if (LocCode=="")
            {
                IssueIdList = _IIssueMainService.All().Where(x => x.IssueDate > fDate && x.IssueDate < tDate).Select(s => s.IssueNo).ToList();
            
            }
            else
            {
                IssueIdList = _IIssueMainService.All().Where(x => x.IssueDate > fDate && x.IssueDate < tDate && x.FromLocCode == LocCode).Select(s => s.IssueNo).ToList();
            
            }
            foreach (var IssueId in IssueIdList)
            {
                Sample_giftRptVM item = new Sample_giftRptVM();
                var DetailList = _IssueDetailsAppService.All().FirstOrDefault(x => x.IssueNo == IssueId);
                var Mainlist = _IIssueMainService.All().FirstOrDefault(x => x.IssueNo == IssueId);
                item.No = IssueId;
                item.date = Mainlist.IssueDate.ToShortDateString();
                item.Type = _INewChartAppService.All().Where(x => x.Accode == Mainlist.DesLocCode).Select(x => x.AcName).FirstOrDefault();
                item.Lot = DetailList.LotNo;
                item.Quantity = DetailList.Qty;
                item.U_Price = DetailList.Price;
                item.Amount = Mainlist.Amount;
                item.Given_To = _ISubsidiaryInfoService.All().Where(x => x.SubCode == Mainlist.ToLocCode).Select(x => x.SubName).FirstOrDefault();
                finalList.Add(item);

            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            return new Rotativa.ViewAsPdf("~/Views/Sample_Gift/Sample_GiftRptPdf.cshtml", finalList)
            {
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            };

            //return View(finalList);
        }
    }
}