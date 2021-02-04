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
using System.Threading;
using System.Globalization;

namespace AcclineERP.Controllers
{
    public class PendingController : Controller
    {
        // GET: Pending
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IMoneyReceiptAppService _IMoneyReceiptAppService;
        private readonly IChequeReceiptAppService _IChequeReceiptAppService;

        public PendingController(IChequeReceiptAppService _IChequeReceiptAppService, IMoneyReceiptAppService _IMoneyReceiptAppService, IBranchAppService _BranchService, IFYDDAppService _FYDDService)
        {
            this._IMoneyReceiptAppService = _IMoneyReceiptAppService;
            this._BranchService = _BranchService;
            this._FYDDService = _FYDDService;
            this._IChequeReceiptAppService = _IChequeReceiptAppService;


        }
        public ActionResult PendingSearch()
        {

            var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
            ViewBag.FyddFDate = Fydd.FYDF;
            ViewBag.FyddTDate = Fydd.FYDT;
            return View();
        }


        public ActionResult PendingRptPdf(DateTime fDate, DateTime tDate)
        {

            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(fDate), Convert.ToString(tDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("PendingSearch_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SecUserLogin", "SecUserLogin", new { errMsg });
            }

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            List<PendingNotEncashRptVM> finalList = new List<PendingNotEncashRptVM>();
            List<MoneyReceipt> MRList = new List<MoneyReceipt>();
            List<ChequeReceipt> CRList = new List<ChequeReceipt>();

            
           
            //ChequeReceipt itemob2 = new ChequeReceipt();

            //MoneyReceipt MoneyReceipt = new MoneyReceipt();
            //List<String> MRIdList = new List<String>();


            MRList = _IMoneyReceiptAppService.All().Where(x => x.EncashDate == null).ToList();
            CRList = _IChequeReceiptAppService.All().Where(x => x.ChqStatus == null).ToList();


            foreach (var item in MRList)
            {
                PendingNotEncashRptVM itemob = new PendingNotEncashRptVM();

                itemob.MRSL = item.MRSL;
                itemob.MRNo = item.MRNo;
                itemob.MRDate = item.MRDate;
                itemob.CustCode = item.CustCode;
                //itemob.ChkAmount = "";
                itemob.MRAmount = item.MRAmount;
                itemob.Remarks = item.Remarks;

                finalList.Add(itemob);
            }


            foreach (var item in CRList)
            {

                PendingNotEncashRptVM itemob = new PendingNotEncashRptVM();
                itemob.MRSL = item.MRSL;
                itemob.MRNo = item.ChqReceiptNo;
                itemob.MRDate = item.ChqReceiptDate;
                itemob.CustCode = item.SubCode;
                itemob.ChkAmount = item.Amount;
               // itemob.MRAmount = item.MRAmount;
                itemob.Remarks = item.Remarks;

                finalList.Add(itemob);
            }



            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            return new Rotativa.ViewAsPdf("~/Views/Pending/PendingRptPdf.cshtml", finalList)
            {
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            };

            //return View(finalList);
        }
    }
}