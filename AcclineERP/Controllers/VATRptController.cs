using AcclineERP.Models;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;
using System.Globalization;
using System.Threading;
using System.Drawing.Printing;
using System.IO;
using Neodynamic.SDK.Web;
using System.Diagnostics;
using System.Security;
using Spire.Pdf;
using Rotativa;

namespace AcclineERP.Controllers
{
    public class VATRptController : Controller
    {
        private readonly IFYDDAppService _FYDDService;
        private readonly IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        public VATRptController(IFYDDAppService _FYDDService, IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService, IEmployeeAppService _employeeInfoService)
        {
            this._FYDDService = _FYDDService;
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._employeeInfoService = _employeeInfoService;
        }
        public SelectList LoadAppBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Vat 9.1";
            string functionName = "Approved By";
            string LogName = Session["UserName"].ToString();
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "','" + form + "', '" + LogName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }
        public ActionResult VatRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
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
        public ActionResult Vat6P2Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
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
        public ActionResult PrintView(string errMsg)
        {
            return View();
        }
        public ActionResult Vat6P3Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                //var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                //ViewBag.FyddFDate = Fydd.FYDF;
                //ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat6P2P1Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
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
        public ActionResult Vat6P10Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
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
        public IEnumerable GetMonths(DateTime currentDate, IFormatProvider provider)
        {
            return from i in Enumerable.Range(0, 12)
                   let now = currentDate.AddMonths(i)
                   select new
                   {
                       MonthLabel = now.ToString("MMMM", provider),
                       Month = now.Month,
                       Year = now.Year
                   };
        }
        public ActionResult Vat6P9P1_1Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                AcclineERPContext dbContext = new AcclineERPContext();
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.DesignationCode = new SelectList(dbContext.Designation, "DesigCode", "DesigDesc");

                //ViewBag.DesignationCode = LoadDropDown.LoadAllDesignation(_designationService);
                //ViewBag.des = new SelectList(_designationService.All().ToList(), "DesigCode", "DesigDesc");
                ViewBag.ApprBy = LoadAppBy(_employeeInfoService);

                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat6P5Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat4P3Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }



        public ActionResult TransWiseVat6P5RptPdf(string TransNo, string command)
        {
            string FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC VM_rpt6P5 '" + TransNo + "', '" + FinYear + "'");
            IEnumerable<Vat6_5> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<Vat6_5>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P5Pdf", VchrLst);
            else
                return View("rptVat6P5Pdf - Copy", VchrLst);
        }
        public ActionResult TransWiseVatRptPdf(DateTime fDate, DateTime tDate, string TransNo, string command)
        {
            string FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC VM_rpt6P1  '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + TransNo + "', '" + FinYear + "','" + Session["ProjCode"].ToString() + "','" + Session["BranchCode"].ToString() + "'");
            IEnumerable<VatStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VatStatementVM>(sql).ToList();
            }
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;


            if (command == "Preview")
            {
                return View("rptVatPdf", VchrLst);
            }
            else
            {
                return View("~/Views/VATRpt/rptVatPdf - Copy.cshtml", VchrLst);
            }

        }





        public ActionResult TransWiseVat6P2RptPdf(DateTime fDate, DateTime tDate, string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P2  '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VatStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VatStatementVM>(sql).ToList();
            }

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P2Pdf", VchrLst);
            else
                return View("rptVat6P2Pdf", VchrLst);

            //return new Rotativa.ViewAsPdf("rptVat6P2Pdf", "", VchrLst)
            //{
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    PageSize = Rotativa.Options.Size.Legal,
            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }
        public ActionResult TransWiseVat6P3RptPdf(string TransNo, string FinYear, string command)
        {//VM_rpt6P3 '','','','2019-20'
            string sql = string.Format("EXEC VM_rpt6P3 '" + TransNo + "','','', '" + FinYear + "'");
            IEnumerable<Vat6_3> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<Vat6_3>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P3Pdf", VchrLst);
            else
                return View("rptVat6P3Pdf - Copy", VchrLst);

            //return new Rotativa.ViewAsPdf("rptVat6P3Pdf", "", VchrLst)
            //{
            //    //PageMargins= new Rotativa.Options.Margins(1,1,1,1),
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    //PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    //PageSize = Rotativa.Options.Size.Legal,
            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }
        public ActionResult TransWiseVat6P2P1RptPdf(DateTime fDate, DateTime tDate, string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P2P1 '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VatStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VatStatementVM>(sql).ToList();
            }

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P2P1Pdf", VchrLst);
            else
                return View("rptVat6P2P1Pdf", VchrLst);

            //return new Rotativa.ViewAsPdf("rptVat6P2P1Pdf", "", VchrLst)
            //{
            //    //PageMargins= new Rotativa.Options.Margins(2,2,0,1),
            //    PageSize = Rotativa.Options.Size.A4, // (14f, 8.5f), 
            //    // PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,

            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }
        public ActionResult TransWiseVat6P10khaRptPdf(DateTime fDate, DateTime tDate, string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P10  '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VatStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VatStatementVM>(sql).ToList();
            }

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View(VchrLst);
            else
                return View("TransWiseVat6P10RptPdf - Copy", VchrLst);

            //return new Rotativa.ViewAsPdf("rptVat6P10khaPdf", "", VchrLst)
            //{
            //    //PageMargins= new Rotativa.Options.Margins(2,2,0,1),
            //    PageSize = Rotativa.Options.Size.A4, // (14f, 8.5f), 
            //    // PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,

            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }
        public ActionResult TransWiseVat6P10khoRptPdf(DateTime fDate, DateTime tDate, string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P2  '" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VatStatementVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VatStatementVM>(sql).ToList();
            }

            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P10khoPdf", VchrLst);
            else
                return View("rptVat6P10khoPdf", VchrLst);

            //return new Rotativa.ViewAsPdf("rptVat6P10khoPdf", "", VchrLst)
            //{
            //    PageSize = Rotativa.Options.Size.A4, // (14f, 8.5f), 
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }
        public ActionResult TransWiseVat6P10RptPdf(DateTime fDate, DateTime tDate, string TransNo, string command)
        {
            List<Vat6P10> VchrLst_Ka = new List<Vat6P10>();
            List<Vat6P10> VchrLst_Kha = new List<Vat6P10>();

            string sql = string.Format("EXEC VM_rpt6P10 '" + TransNo + "','','','','" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','Ka' ");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst_Ka = dbContext.Database.SqlQuery<Vat6P10>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt6P10 '" + TransNo + "','','','','" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','Kha' ");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst_Kha = dbContext.Database.SqlQuery<Vat6P10>(sql).ToList();
            }
            Ka_KhaModel finalItem = new Ka_KhaModel();
            finalItem.VchrLst_Ka1 = VchrLst_Ka;
            finalItem.VchrLst_Kha1 = VchrLst_Kha;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View(finalItem);
            else
                return View("TransWiseVat6P10RptPdf - Copy", finalItem);

            //return new Rotativa.ViewAsPdf("TransWiseVat6P10RptPdf", "", finalItem)
            //{
            //    //PageMargins= new Rotativa.Options.Margins(2,2,0,1),
            //    PageSize = Rotativa.Options.Size.A4, // (14f, 8.5f), 
            //    // PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,

            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }


        public ActionResult Vat6P4Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                ViewBag.Message = errMsg;

                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult Vat6P6Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat6P7Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat6P8Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat9P1_SF_KaRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.projCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult Vat9P1_SF_Ka_2_Rpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.projCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }


        public ActionResult TransWiseVat6P4RptPdf(string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P4 '" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VM_6P4> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VM_6P4>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P4Pdf", VchrLst);
            else
                return View("rptVat6P4PdfPrint", VchrLst);
        }

        public ActionResult TransWiseVat6P6RptPdf(string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P6 '" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VM_6P6> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VM_6P6>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P6Pdf", VchrLst);
            else
                return View("rptVat6P6PdfPrint", VchrLst);
        }
        public ActionResult TransWiseVat6P7RptPdf(string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P7 '" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VM_6P7> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VM_6P7>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P7Pdf", VchrLst);
            else
                return View("rptVat6P7PdfPrint", VchrLst);
        }
        public ActionResult TransWiseVat6P8RptPdf(string TransNo, string FinYear, string command)
        {
            string sql = string.Format("EXEC VM_rpt6P8 '" + TransNo + "', '" + FinYear + "'");
            IEnumerable<VM_6P8> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VM_6P8>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat6P8Pdf", VchrLst);
            else
                return View("rptVat6P8PdfPrint", VchrLst);
        }
        public ActionResult TransWiseVat9P1_SF_KaRptPdf(string Project, string Branch, DateTime fDate, DateTime tDate, string Year, string Month, string FinYear, string command)
        {
            FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC VM_rpt9P1_SF_Ka_1 '" + Project + "', '" + Branch + "', '" + FinYear + "', '" + Year + "', '" + Month + "','" + fDate.ToString("yyyy/MM/dd") + "', '" + tDate.ToString("yyyy/MM/dd") + "',''");
            IEnumerable<Vat9P1_SF_Ka> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<Vat9P1_SF_Ka>(sql).ToList();
            }

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View("rptVat9P1_SF_KaRptPdf", VchrLst);
            else
                return View("rptVat9P1_SF_KaRptPdfPrint", VchrLst);
        }
        public ActionResult TransWiseVat9P1_SF_Ka_2_RptPdf(string Project, string Branch, DateTime fDate, DateTime tDate, string Year, string Month, string FinYear, string command)
        {
            FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC VM_rpt9P1_SF_Ka_2 '" + Project + "', '" + Branch + "', '" + FinYear + "', '" + Year + "', '" + Month + "','" + fDate.ToString("yyyy/MM/dd") + "', '" + tDate.ToString("yyyy/MM/dd") + "',''");
            IEnumerable<Vat9P1_SF_Ka> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<Vat9P1_SF_Ka>(sql).ToList();
            }
            if (command == "Preview")
                return View("rptVat9P1_SF_Ka_2_RptPdf", VchrLst);
            else
                return View("rptVat9P1_SF_Ka_2_RptPdfPrint", VchrLst);

        }


        public ActionResult vat9P1P1Process()
        {
            return View();
        }

        public ActionResult TransWiseVat6P9P1_1RptPdf(string finYear, DateTime fDate, DateTime tDate, string command)
        {
            List<Vat6P9P1_1> Part1 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part2 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part3 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part4 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part5 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part6 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part7 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part8 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part9 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part10 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part11 = new List<Vat6P9P1_1>();
            string today = DateTime.Today.ToString("yyyy/MM/dd");
            int month = fDate.Day;
            int year = fDate.Year;
            //@ProjCode, @BranchCode, @FinYear,@Year, @Month, @fdate,@tdate
            //VM_rpt9P1_P1 '','','2019-20',2019,7,'2019-7-1','2019-7-31'
            string sql = string.Format("EXEC VM_rpt9P1_P1 '','',''," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part1 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P2 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + today + "','64',1");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part2 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }

            sql = string.Format("EXEC VM_rpt9P1_P3 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part3 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P4 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part4 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P5  '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part5 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P6 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part6 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P7 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part7 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P8 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part8 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P9 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part9 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P10 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part10 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P11 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part11 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            Part1Part2Model finalItem = new Part1Part2Model();
            finalItem.P1_1PArt1 = Part1;
            finalItem.P1_1PArt2 = Part2;
            finalItem.P1_1PArt3 = Part3;
            finalItem.P1_1PArt4 = Part4;
            finalItem.P1_1PArt5 = Part5;
            finalItem.P1_1PArt6 = Part6;
            finalItem.P1_1PArt7 = Part7;
            finalItem.P1_1PArt8 = Part8;
            finalItem.P1_1PArt9 = Part9;
            finalItem.P1_1PArt10 = Part10;
            finalItem.P1_1PArt11 = Part11;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);
            //bool process = false;            

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (command == "Preview")
                return View(finalItem);

                //return Json(process, JsonRequestBehavior.AllowGet);
            else
                sql = string.Format("exec Count_MRPrint " + year + "," + month + "");
            IEnumerable<MonthlyReturn> MontlyReturnLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                MontlyReturnLst = dbContext.Database.SqlQuery<MonthlyReturn>(sql).ToList();
            }

            return View("TransWiseVat6P9P1_1RptPdf - Copy", finalItem);
            //return new Rotativa.ViewAsPdf("TransWiseVat6P9P1_1RptPdf", "", finalItem)
            //{
            //    //PageMargins= new Rotativa.Options.Margins(2,2,0,1),
            //    PageSize = Rotativa.Options.Size.A4, // (14f, 8.5f), 
            //    // PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
            //};
        }

        [HttpPost]
        public JsonResult UpdateData(string finYear, DateTime fDate, DateTime tDate, string command)
        {
            List<Vat6P9P1_1> Part1 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part2 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part3 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part4 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part5 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part6 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part7 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part8 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part9 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part10 = new List<Vat6P9P1_1>();
            List<Vat6P9P1_1> Part11 = new List<Vat6P9P1_1>();
            string today = DateTime.Today.ToString("yyyy/MM/dd");
            int month = fDate.Day;
            int year = fDate.Year;
            //@ProjCode, @BranchCode, @FinYear,@Year, @Month, @fdate,@tdate
            //VM_rpt9P1_P1 '','','2019-20',2019,7,'2019-7-1','2019-7-31'
            string sql = string.Format("EXEC VM_rpt9P1_P1 '','',''," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part1 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P2 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "','" + today + "','64',1");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part2 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }

            sql = string.Format("EXEC VM_rpt9P1_P3 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part3 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P4 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part4 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P5  '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part5 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P6 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part6 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P7 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part7 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P8 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part8 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P9 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part9 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P10 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part10 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            sql = string.Format("EXEC VM_rpt9P1_P11 '','','" + finYear + "'," + year + "," + month + ",'" + fDate.ToString("yyyy/MM/dd") + "','" + tDate.ToString("yyyy/MM/dd") + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                Part11 = dbContext.Database.SqlQuery<Vat6P9P1_1>(sql).ToList();
            }
            Part1Part2Model finalItem = new Part1Part2Model();
            finalItem.P1_1PArt1 = Part1;
            finalItem.P1_1PArt2 = Part2;
            finalItem.P1_1PArt3 = Part3;
            finalItem.P1_1PArt4 = Part4;
            finalItem.P1_1PArt5 = Part5;
            finalItem.P1_1PArt6 = Part6;
            finalItem.P1_1PArt7 = Part7;
            finalItem.P1_1PArt8 = Part8;
            finalItem.P1_1PArt9 = Part9;
            finalItem.P1_1PArt10 = Part10;
            finalItem.P1_1PArt11 = Part11;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(tDate);
            return Json(new
            {
                redirectUrl = Url.Action("TransWiseVat6P9P1_1RptPdf", "VATRpt"),
                finalItem,
                isRedirect = true
            });
        }
        public JsonResult GetMonthlyReturn(int year, int month)
        {
            AcclineERPContext dbContext = new AcclineERPContext();
            var jsonData = new List<MonthlyReturn>();
            jsonData = dbContext.Database.SqlQuery<MonthlyReturn>("select * from VM_MonthlyReturn where P2_TD_Yr=" + year + " and P2_TD_Mon= " + month + "").ToList();
            return Json(new { data = jsonData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastMonthReturn(int year, int month)
        {
            DateTime now = DateTime.Now;
            int LastMonth = now.AddMonths(-1).Month;
            int Currentyear = now.AddMonths(-1).Year;

            AcclineERPContext dbContext = new AcclineERPContext();
            var jsonData = new List<MonthlyReturn>();
            jsonData = dbContext.Database.SqlQuery<MonthlyReturn>("select P2_TD_Mon from VM_MonthlyReturn where P2_TD_Yr=" + Currentyear + " and P2_TD_Mon= " + LastMonth + "").ToList();
            return Json(new { data = jsonData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Count_MRPrint(int year, int month)
        {
            try
            {
                string sql = string.Format("exec Count_MRPrint " + year + "," + month + "");
                IEnumerable<MonthlyReturn> MontlyReturnLst;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    MontlyReturnLst = dbContext.Database.SqlQuery<MonthlyReturn>(sql).ToList();
                }

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveMonthlyReturn(int year, int month, string projCode, string branchCode, string userName)
        {
            try
            {
                string sql = string.Format("exec VM_MonthlyReturn_Create '" + projCode + "','" + branchCode + "'," + year + "," + month + ",'" + userName + "'");
                IEnumerable<MonthlyReturn> MontlyReturnLst;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    MontlyReturnLst = dbContext.Database.SqlQuery<MonthlyReturn>(sql).ToList();
                }

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Vat4P3RptPdf(string DeclarationNo, string command, string ItemCode)
        {
            string sql = string.Format("EXEC VM_rpt4P3 '" + DeclarationNo + "','" + ItemCode + "'");
            IEnumerable<App.Domain.ViewModel.VM_4P3> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<VM_4P3>(sql).ToList();
            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            if (command == "Preview")
                return View("rptVat4P3Pdf", VchrLst);
            else
                return View("rptVat4P3Pdf", VchrLst);

        }
    }
}