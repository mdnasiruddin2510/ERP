

using AcclineERP.Models;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace AcclineERP.Controllers
{
    public class rptSales_CollectionController : Controller
    {
        // GET: rptSales_Collection


        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        private readonly ILocationAppService _ILocationAppService;
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IProjInfoAppService _ProjInfoService;
        public rptSales_CollectionController(IFYDDAppService _FYDDService, ISubsidiaryInfoAppService _SubsidiaryService, ILocationAppService _ILocationAppService, IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService)
        {

            this._FYDDService = _FYDDService;
            this._ILocationAppService = _ILocationAppService;
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._SubsidiaryService = _SubsidiaryService;

        }


        public ActionResult Sales_CollectionSearch(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                string branchCode = Session["BranchCode"].ToString();

                ViewBag.LocCode = new SelectList(_ILocationAppService.All().ToList(), "LocCode", "LocName");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.SubCode = new SelectList(_SubsidiaryService.All().ToList(), "SubCode", "SubName");
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }





        public ActionResult GetSale_collection(RptSearchVModel vmodel, string finyear)
        {
            finyear = Session["FinYear"].ToString();
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate); 
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.tDate);
            ViewBag.SubCode = vmodel.SubCode;
            ViewBag.ProjCode = vmodel.projCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            ViewBag.LocCode = vmodel.LocCode;
            String Location = vmodel.LocCode;
           
            ViewBag.Location = "All";
            if (Location != null)
            {
                ViewBag.Location = _ILocationAppService.All().ToList().Where(s => s.LocCode == Location).Select(x => x.LocName).FirstOrDefault();
                

            }
			

            // For checked current finyear 
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {


                // return RedirectToAction("Sales_CollectionSearch", "rptSales_Collection", new { errMsg = ChkFYR });

            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());

            if (!rUser.HasPermission("AgeingReport(Marketing)_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("Sales_CollectionSearch", "rptSales_Collection", new { errMsg });
            }


            string sql = string.Format("exec sp_fageCalcu_L_5Col_New  '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.LocCode + "','" + Session["FinYear"].ToString() + "','" + Session["UserName"] + "'  ");




            //string sql = string.Format("exec sp_fageCalcu_L_5Col_New  '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.LocCode + "','" + Session["FinYear"].ToString() + "'");

            IEnumerable<Sale_CollectionVM> VchrLst;

            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<Sale_CollectionVM>(sql).ToList();
            }

            return new Rotativa.ViewAsPdf("rptSales_CollectionPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };

        }
















    }
}