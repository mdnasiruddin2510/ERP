using App.Domain;
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
using App.Domain.ViewModel;

namespace AcclineERP.Controllers
{
    public class MontlyQuantityController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly ISubsidiaryInfoAppService _ISubsidiaryInfoService;
        private readonly ILocationAppService _ILocationAppService;
        public MontlyQuantityController(IFYDDAppService _FYDDService, ILocationAppService _ILocationAppService, ISubsidiaryInfoAppService _ISubsidiaryInfoService, IBranchAppService _BranchService,
            IProjInfoAppService _ProjInfoService)
        {
            this._BranchService = _BranchService;
            this._ISubsidiaryInfoService = _ISubsidiaryInfoService;
            this._ProjInfoService = _ProjInfoService;
            this._ILocationAppService = _ILocationAppService;
            this._ProjInfoService = _ProjInfoService;
            this._FYDDService = _FYDDService;
        }

        public ActionResult Montly_Amount_Quantity_Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.Location = new SelectList(_ILocationAppService.All().ToList(), "LocCode", "LocName").ToList();//GardenSelection();
                ViewBag.CustomerName = new SelectList(_ISubsidiaryInfoService.All().ToList().Where(x => x.SubType == "1"), "SubCode", "SubName");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");

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

        public ActionResult MontlyQuantityRptPdf2(string Location, string CustomerName, DateTime fDate, DateTime tDate, string ProjCode, string BranchCode, string FinYear, string QtyAmt)
        {



            //#### AMOUNT

            string ItemGroup = "";

            string sp = "";
            string customerGroup = "";
            //string Location = "";

            if (QtyAmt != "Amount")
            {
                sp = "rpt_SP_ProdSales_Qty";
            }
            else
            {
                sp = "rpt_SP_ProdSales_Amt";
            }

            customerGroup = CustomerName;
            //rpt_SP_ProdSales_Qty @fdate smalldatetime, @tdate smalldatetime, @ProjCode varchar(3), @BranchCode varchar(3),@FinYear varchar(7), @customerGroup varchar(7),@ItemGroup varchar(6)

            string sql = string.Format("EXEC " + sp + "'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + ProjCode + "', '" + BranchCode + "','" + FinYear + "','" + customerGroup + "','" + ItemGroup + "','" + Session["UserName"] + "'  ");
            IEnumerable<MontlyQuantity> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<MontlyQuantity>(sql).ToList();
            }
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
            //ViewBag.fDate = fDate.ToString("dd-MMM-yyyy");
            //ViewBag.tDate = tDate.ToString("dd-MMM-yyyy");
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");



            //#### AMOUNT  #######


            ViewBag.CustomerName = "All";
            ViewBag.Location = "All";

            if (CustomerName != "")
            {

                CustomerName = _ISubsidiaryInfoService.All().Where(s => s.SubCode == CustomerName).Select(x => x.SubName).FirstOrDefault();
                ViewBag.CustomerName = CustomerName;
            }
            if (Location != "")
            {
                Location = _ILocationAppService.All().ToList().Where(s => s.LocCode == Location).Select(x => x.LocName).FirstOrDefault();
                ViewBag.Location = Location;
            }




            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;



            if (QtyAmt != "Amount")
            {
                return new Rotativa.ViewAsPdf("~/Views/MontlyQuantity/QuantityPdf.cshtml", VchrLst)
                {
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4,
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }

            else
            {

                return new Rotativa.ViewAsPdf("~/Views/MontlyQuantity/AmountPdf.cshtml", VchrLst)
                {
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4,
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }


        }
        [HttpPost]
        public ActionResult contStatementReportPdf(string ProjName, string BrName, string RptName)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Montly_Amount_Quantity_Rpt_Preview"))
            {
                string errMsg = "No View Permission for this User !!";
                //return RedirectToAction("ContStatementRpt", "ContStatementRpt", new { errMsg, RptCaption = CaptionName });
                return RedirectToAction("ContStatementRpt", "ContStatementRpt", new { errMsg });
            }


            string spName = "";
            string CaptionName = "";
            string FinYear = Session["FinYear"].ToString();
            DateTime fSMonth = _FYDDService.All().FirstOrDefault(s => s.FinYear == FinYear).FYDF;
            var sMonth = fSMonth.Month;
            if (RptName == "rptContStatement")
            {

                CaptionName = "rptContStatement";


                spName = "Exec rptContStatement '" + ProjName + "','" + BrName + "','" + FinYear + "','" + sMonth + "'";
            }
            else
                if (RptName == "rptCumlBal")
                {
                    CaptionName = "rptCumlBal";
                    spName = "Exec rptCumlBalPrepare '" + ProjName + "','" + BrName + "','" + FinYear + "','" + sMonth + "'";  //"rptCumlBal";
                }
                else
                {

                    CaptionName = "rptProfitDistribution";
                    spName = "Exec rptProfitDistribution '" + ProjName + "','" + BrName + "','" + FinYear + "','" + sMonth + "'";

                }

            ViewBag.PrintDate = DateTime.Now.ToShortDateString();
            AcclineERPContext dbContext = new AcclineERPContext();
            string sql = string.Format(spName);
            List<MontlyQuantity> contStatement = dbContext.Database.SqlQuery<MontlyQuantity>(sql).ToList(); //_rptContStatementService.SqlQueary(sql).ToList();

            if (contStatement.Count == 0)
            {
                string errMsg = "There is no data in this combination. Please try again !!!";
                return RedirectToAction("ContStatementRpt", "ContStatementRpt", new { errMsg, RptCaption = CaptionName });
            }
            else
            {
                var FY = FinYear;
                var fydd = _FYDDService.All().ToList().FirstOrDefault(x => x.FinYear == FinYear);
                ViewBag.Fdate = fydd.FYDF;
                ViewBag.Tdate = fydd.FYDT;
                ViewBag.EndedYear = _FYDDService.All().ToList().FirstOrDefault(x => x.FinYear == FinYear).FYDT;
                ViewBag.SCriteria = GetCompanyInfo.GetReportCriteria(ProjName, BrName, null, null, FinYear);
                ViewBag.rptName = RptName;
                Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
                return new Rotativa.ViewAsPdf("contStatementReportPdf", contStatement)
                {
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4,
                    //FileName = RptName + "-" + DateTime.Now.ToShortDateString() + ".pdf" , contStatementReportPdf
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

                };
            }

        }




        [HttpPost]
        public ActionResult MontlyQuantityRptPdf(DateTime fDate, DateTime tDate, string ProjCode, string BranchCode, string FinYear)
        {
            string customerGroup = "";
            //rpt_SP_ProdSales_Qty @fdate smalldatetime, @tdate smalldatetime, @ProjCode varchar(3), @BranchCode
            string sql = string.Format("EXEC rpt_SP_ProdSales_Qty '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + ProjCode + "', '" + BranchCode + "'");
            IEnumerable<MontlyQuantity> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<MontlyQuantity>(sql).ToList();
            }
            var FY = FinYear;
            ViewBag.BranchCode = _BranchService.All().Where(s => s.BranchCode == BranchCode).Select(x => x.BranchName).FirstOrDefault();
            //ViewBag.fDate = fDate.ToString("dd-MMM-yyyy");
            //ViewBag.tDate = tDate.ToString("dd-MMM-yyyy");
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("rptSalesCollectionStatePdf", "", VchrLst)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            };

        }
    }
}