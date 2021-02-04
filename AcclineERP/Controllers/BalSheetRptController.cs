using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using Data.Context;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class BalSheetRptController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly IBalSheetRptAppService _BalSheetRptService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IFYDDAppService _FYDDService;

        public BalSheetRptController(IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService,
            IBalSheetRptAppService _BalSheetRptService, ISysSetAppService _sysSetService, IFYDDAppService _FYDDService)
        {
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._BalSheetRptService = _BalSheetRptService;
            this._sysSetService = _sysSetService;
            this._FYDDService = _FYDDService;
        }
        public ActionResult BalSheetRpt(string errMsg, string RptCaption)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ProjName = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.BrName = new SelectList(_BranchService.All().ToList(), "BrCode", "BrName");
                ViewBag.FYDD = new SelectList(_FYDDService.All().ToList(), "FinYear", "FinYear");
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddTDate = Fydd.FYDT;

                ViewBag.Message = errMsg;
                if (RptCaption == "rptBalanceSheet")
                {
                    ViewBag.RptCaption = "Balance Sheet Report";
                }
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public class retvalpro
        {
            [DefaultValue(0)]
            public decimal PLAmount { get; set; }
        }

        [HttpPost]
        public ActionResult BalSheetRptPdf(string ProjName, string RptName, string tDate)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptBalanceSheet_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("BalSheetRpt", "BalSheetRpt", new { errMsg });
            }
            retvalpro PLAmountPro;
            decimal plAmt = 0;
            string plAmts = "0";
            string FinYear = Session["FinYear"].ToString();
            DateTime FYDF = _FYDDService.All().FirstOrDefault(s => s.FinYear == FinYear).FYDF;

            string sqlp = string.Format("EXEC PLAmount '" + FinYear + "','" + ProjName + "', '" + Session["BranchCode"] + "', '" + FYDF.ToString("MM/dd/yyyy") + "', '" + Convert.ToDateTime(tDate).ToString("MM/dd/yyyy") + "','" + FinYear + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                PLAmountPro = dbContext.Database.SqlQuery<retvalpro>(sqlp).FirstOrDefault();
                plAmt = PLAmountPro.PLAmount;
                plAmts = Convert.ToString(plAmt).Replace(",", ".");
            }

            string sql = string.Format("Exec rptIncExpAC2 '" + ProjName + "', '', '" + Convert.ToDateTime(tDate).ToString("MM/dd/yyyy") + "', '" + FinYear + "', '" + plAmts + "'");
         


            //string sql = string.Format("Exec rptIncExpAC2 '" + ProjName + "','" + Session["BranchCode"].ToString() + "','" + Convert.ToDateTime(tDate).ToString("MM/dd/yyyy") + "','" + Session["FinYear"].ToString() + "'");

            List<BalSheetRptVM> balSheet = _BalSheetRptService.SqlQueary(sql).ToList();

            ViewBag.PlAmount = plAmt;
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci; 
            Response.AppendHeader("Content-Disposition", "inline; filename=" + RptName + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("BalSheetRptPdf", "", balSheet)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                //FileName = RptName + "-" + DateTime.Now.ToShortDateString() + ".pdf" , contStatementReportPdf
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };

            //}
        }


    }
}