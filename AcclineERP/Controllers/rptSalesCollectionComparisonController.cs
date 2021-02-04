

using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AcclineERP.Models;


namespace AcclineERP.Controllers
{
    public class rptSalesCollectionComparisonController : Controller
    {
        // GET: rptSales_Collection

        
         private readonly ISubsidiaryInfoAppService _SubsidiaryService;
         private readonly ILocationAppService _locationService;
         private readonly IBranchAppService _BranchService;
         private readonly IFYDDAppService _FYDDService;
         private readonly IProjInfoAppService _ProjInfoService;
         public rptSalesCollectionComparisonController(IFYDDAppService _FYDDService,ISubsidiaryInfoAppService _SubsidiaryService, ILocationAppService _locationService, IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService)
        {
            
            this._locationService = _locationService;
            this._BranchService= _BranchService;
            this._ProjInfoService=_ProjInfoService;
             this._SubsidiaryService=_SubsidiaryService;
             this._FYDDService = _FYDDService;
            
        }
         public ActionResult SalesCollectionComparisonSearch(string errMsg)
        {



            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                string branchCode = Session["BranchCode"].ToString();

                ViewBag.Location = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
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





         public ActionResult rptSalesCollectionComparisonPdf(RptSearchVModel vmodel, string finyear)
        {

            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SalesCollectionComparison_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }

            finyear = Session["FinYear"].ToString();
            ViewBag.SubCode = vmodel.SubCode;
            ViewBag.ProjCode = vmodel.projCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.tDate);
            String BranchCode = vmodel.BranchCode;
            ViewBag.BranchName = "All";
            if (BranchCode != "")
            {
                ViewBag.BranchName = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
                
            }
			
			
          
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

        
            string sql_1 = string.Format("exec rpt_salescolcomparison_det  '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.projCode + "','" + vmodel.BranchCode + "' ,'" + Session["UserName"] + "'  ");
            string sql_2 = string.Format("exec rpt_salescolcomparison_foot   '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.projCode + "','" + vmodel.BranchCode + "','" + Session["UserName"] + "'  ");
           
           // string sql_1 = string.Format("exec rpt_salescolcomparison_det  '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.projCode + "','" + vmodel.BranchCode + "' ");
            //string sql_2 = string.Format("exec rpt_salescolcomparison_foot   '" + vmodel.fDate.ToString("yyyy-MM-dd") + "','" + vmodel.tDate.ToString("yyyy-MM-dd") + "','" + vmodel.projCode + "','" + vmodel.BranchCode + "' ");
            IEnumerable<SalesCollectionComparisionVM> VchrLst;
            SalesCollectionComparisionVM yearlyData=new SalesCollectionComparisionVM();
            //IEnumerable<SalesCollectionComparisionVM > yearlyData;

            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<SalesCollectionComparisionVM>(sql_1).ToList();
                //yearlyData = dbContext.Database.SqlQuery<SalesCollectionComparisionVM>(sql_2).FirstOrDefault();
                yearlyData = dbContext.Database.SqlQuery<SalesCollectionComparisionVM>(sql_2).FirstOrDefault();


       
           }
            ViewBag.yearlyDataV = yearlyData;
            return new Rotativa.ViewAsPdf("rptSalesCollectionComparisonPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };

        }

    }
}