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
    public class RptSalesPurchaseController : Controller
    {
        private readonly ILocationAppService _locationService;
        

        public RptSalesPurchaseController(ILocationAppService _locationService)
        {
            this._locationService = _locationService;
            
        }
        public AcclineERPContext db = new AcclineERPContext();
        public ActionResult CustomerWiseSaleRptSearch(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();

                ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
                
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        [HttpPost]
        public ActionResult GetCustomerWiseSaleSummaryRpt(CustWiseSaleRptSearchVModel saleRpt)
        {
            
            if (saleRpt.LocCode == null)
            {
                saleRpt.LocCode = "";
            }
            if (saleRpt.RptType == 1)
            {
                string finYear = Session["FinYear"].ToString();

                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "CustomerWiseSaleRpt?finYear=" + finYear + "&locCode=" + saleRpt.LocCode.ToString() + "&fdate=" + saleRpt.fDate.ToString("MM/dd/yyyy") + "&tdate=" + saleRpt.toDate.ToString("MM/dd/yyyy"), Session["token"].ToString());
                JavaScriptSerializer js = new JavaScriptSerializer();

                IEnumerable<CustWiseSummSaleRpt> itemList = js.Deserialize<IEnumerable<CustWiseSummSaleRpt>>(JsonResponse);

                if (saleRpt.LocCode != null && saleRpt.LocCode != "0" && saleRpt.LocCode != "")
                {
                    ViewBag.Location = _locationService.All().FirstOrDefault(x => x.LocCode == saleRpt.LocCode.Trim()).LocName.ToString();
                }
                else
                {
                    ViewBag.Location = "All";
                }

                ViewBag.fDate = saleRpt.fDate.ToString("dd-MMM-yyyy");
                ViewBag.tDate = saleRpt.toDate.ToString("dd-MMM-yyyy");
                if (saleRpt.LocCode != null && saleRpt.LocCode != "")
                {
                    Session["LocCode"] = saleRpt.LocCode;
                }
                Session["RptType"] = saleRpt.RptType;
                //ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");

                if (itemList.ToList().Count == 0)
                {
                    string errMsg = "There is no data in this combination. Please try again !!!";
                    return RedirectToAction("CustomerWiseSaleRptSearch", "RptSalesPurchase", new { errMsg });
                }
                return View(itemList);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetCustomerWiseSaleSummaryRptPdf(CustWiseSaleRptSearchVModel saleRpt)
        {
            if (saleRpt.LocCode != null && saleRpt.LocCode != "")
            {
                saleRpt.LocCode = Session["LocCode"].ToString();
            }
            saleRpt.RptType = Convert.ToInt32(Session["RptType"]);
            if (saleRpt.LocCode == null)
            {
                saleRpt.LocCode = "";
            }
            if(saleRpt.RptType == 1)
            {
                string finYear = Session["FinYear"].ToString();
                
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "CustomerWiseSaleRpt?finYear=" + finYear + "&locCode=" + saleRpt.LocCode.ToString() + "&fdate=" + saleRpt.fDate.ToString("MM/dd/yyyy") + "&tdate=" + saleRpt.toDate.ToString("MM/dd/yyyy"), Session["token"].ToString());
                JavaScriptSerializer js = new JavaScriptSerializer();

                IEnumerable<CustWiseSummSaleRpt> itemList = js.Deserialize<IEnumerable<CustWiseSummSaleRpt>>(JsonResponse);

                if (saleRpt.LocCode != null && saleRpt.LocCode != "0" && saleRpt.LocCode != "")
                {
                    ViewBag.Location = _locationService.All().FirstOrDefault(x => x.LocCode == saleRpt.LocCode.Trim()).LocName.ToString();
                }
                else
                {
                    ViewBag.Location = "All";
                }

                ViewBag.fDate = saleRpt.fDate.ToString("dd-MMM-yyyy");
                ViewBag.tDate = saleRpt.toDate.ToString("dd-MMM-yyyy");
                ViewBag.PrintDate = DateTime.Now.ToShortDateString();

                //ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");

                if (itemList.ToList().Count==0)
                {
                    string errMsg = "There is no data in this combination. Please try again !!!";
                    return RedirectToAction("CustomerWiseSaleRptSearch", "RptSalesPurchase", new { errMsg });
                }
                return new Rotativa.ViewAsPdf("GetCustomerWiseSaleSummaryRptPdf", itemList) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };

                //return View(itemList);
            }
            else
            {
                return View();
            }
            
        }
	}
}