 

using App.Domain;
using App.Domain.ViewModel;
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
using System.Globalization;
using System.Threading;


namespace AcclineERP.Controllers
{
    //[Authorize(Roles = "Admin, User")]
    public class CustomerLedgerGDSController : Controller
    {
        private readonly ICustomerLedgerAppService _CustomerLedgerService;
        private readonly ISubsidiaryInfoAppService _ISubsidiaryInfoAppService;

        private readonly IMoneyReceiptAppService _IMoneyReceiptAppService;
        private readonly ISalesMainAppService _ISalesMainAppService;
        private readonly ISalesDetailAppService _ISalesDetailAppService;
        private readonly ISaleRetMainAppService _ISaleRetMainAppService;
        private readonly IFYDDAppService _IFYDDAppService;
        private readonly ICustAdjustmentAppService _ICustAdjustmentAppService;





        public CustomerLedgerGDSController(ICustAdjustmentAppService _ICustAdjustmentAppService, IFYDDAppService _IFYDDAppService, ISaleRetMainAppService _ISaleRetMainAppService, ISalesDetailAppService _ISalesDetailAppService, ISalesMainAppService _ISalesMainAppService, IMoneyReceiptAppService _IMoneyReceiptAppService, ICustomerLedgerAppService _CustomerLedgerService, ISubsidiaryInfoAppService _ISubsidiaryInfoAppService)
        {
            this._CustomerLedgerService = _CustomerLedgerService;
            this._ISubsidiaryInfoAppService = _ISubsidiaryInfoAppService;
            this._IMoneyReceiptAppService = _IMoneyReceiptAppService;
            this._ISalesMainAppService = _ISalesMainAppService;
            this._ISalesDetailAppService = _ISalesDetailAppService;
            this._ISaleRetMainAppService = _ISaleRetMainAppService;
            this._IFYDDAppService = _IFYDDAppService;
            this._ICustAdjustmentAppService = _ICustAdjustmentAppService;
        }
        public ActionResult Search(string errMsg)
        {
            var Fydd = _IFYDDAppService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
            ViewBag.FyddFDate = Fydd.FYDF;
            ViewBag.FyddTDate = Fydd.FYDT;
            ViewBag.SubCode = new SelectList(_ISubsidiaryInfoAppService.All().Where(x => x.SubType == "1").ToList(), "SubCode", "SubName");

            ViewBag.Message = errMsg;
            return View();

        }

        public ActionResult GetCustomerLedger(RptSearchVModel vmodel, string finyear)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CustomerLedgerGDS_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            finyear = Session["FinYear"].ToString();
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.tDate);


            ViewBag.CustCode = vmodel.SubCode;
            ViewBag.Customer = _ISubsidiaryInfoAppService.All().FirstOrDefault(x => x.SubCode == vmodel.SubCode.Trim()).SubName.ToString();
            ViewBag.SubCode = vmodel.SubCode;




            string sql = string.Format("exec rpt_CustLedger_GDS '" + vmodel.fDate.ToString("yyyy/MM/dd") + "','" + vmodel.tDate.ToString("yyyy/MM/dd") + "','" + vmodel.SubCode + "','" + Session["FinYear"].ToString() + "','" + Session["UserName"] + "'  ");


            IEnumerable<CustomerLedgerVM> VchrLst;

            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<CustomerLedgerVM>(sql).ToList();
            }

            return new Rotativa.ViewAsPdf("CustomerLedgerPdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };



        }


    }
}




