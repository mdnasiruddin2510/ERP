﻿using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class JobWiseRecPayController : Controller
    {
        private readonly IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly IBalSheetRptAppService _BalSheetRptService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IrptJobWiseVMAppService _JobWiseReportService;
        public JobWiseRecPayController(IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService,
            IBalSheetRptAppService _BalSheetRptService, ISysSetAppService _sysSetService, IFYDDAppService _FYDDService,
            IrptJobWiseVMAppService _JobWiseReportService)
        {
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._BalSheetRptService = _BalSheetRptService;
            this._sysSetService = _sysSetService;
            this._FYDDService = _FYDDService;
            this._JobWiseReportService = _JobWiseReportService;
        }
        // GET: JobWiseRecPay
        public ActionResult JobWiseRecPay(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
                //ViewBag.FYDD = new SelectList(_FYDDService.All().ToList(), "FinYear", "FinYear");
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
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

        [HttpPost]
        public ActionResult JobWiseRecPayPdf(string ProjCode, string BranchCode, string JobNo, string fDate, string tDate)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptJobWiseRecPay_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("JobWiseRecPay", "JobWiseRecPay", new { errMsg });
            }
            if (BranchCode == null)
            {
                BranchCode = Session["BranchCode"].ToString();
            }
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(Convert.ToDateTime(fDate));
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(Convert.ToDateTime(tDate));
            string sql = string.Format("Exec rpt_JobRecPay '" + Session["FinYear"].ToString() + "','" + ProjCode + "','" + BranchCode + "','" + JobNo + "','" + Convert.ToDateTime(fDate).ToString("yyyy/MM/dd") + "','" + Convert.ToDateTime(tDate).ToString("yyyy/MM/dd") + "',''");

            List<rptJobWiseVM> JobWiseRecPay = _JobWiseReportService.SqlQueary(sql).ToList();

            Response.AppendHeader("Content-Disposition", "inline; filename= JobWiseIncExp" + DateTime.Now.ToShortDateString() + ".pdf");
            return new Rotativa.ViewAsPdf("JobWiseRecPayPdf", "", JobWiseRecPay)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                //FileName = RptName + "-" + DateTime.Now.ToShortDateString() + ".pdf" , contStatementReportPdf
                CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

            };
        }

    }
}