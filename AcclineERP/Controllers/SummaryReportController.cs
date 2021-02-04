using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class SummaryReportController : Controller
    {
        private readonly ILocationAppService _locationService;
        private readonly ISummaryReportAppService _SumRptService;
        private readonly ISummaryReportTypeAppService _SumRptTypeService;
        private readonly ILedgerCaptionAppService _LedgerCapService;
        private readonly IBranchAppService _BranchService;
        private readonly IEmployeeAppService _employeeService;
        private readonly INewChartAppService _newchartAppService;
        private readonly ISubsidiaryInfoAppService _SubsidiaryService;
        private readonly IItemInfoAppService _ItemService;
        private readonly IReportLedgerAppService _ReportLedgerService;
        private readonly IVchrPreviewVMAppService _VchrPreviewVMService;
        private readonly IUserBranchAppService _userbranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ISysSetAppService _sysSetService;
        public SummaryReportController(ILocationAppService _locationService, ISummaryReportAppService _SumRptService, ISummaryReportTypeAppService _SumRptTypeService,
            ILedgerCaptionAppService _LedgerCapService, IBranchAppService _BranchService, IEmployeeAppService _employeeService, INewChartAppService _newchartAppService,
            ISubsidiaryInfoAppService _SubsidiaryService, IItemInfoAppService _ItemService, IReportLedgerAppService _ReportLedgerService,
            IVchrPreviewVMAppService _VchrPreviewVMService, IUserBranchAppService _userbranchService, IFYDDAppService _FYDDService, ISysSetAppService _sysSetService)
        {
            this._locationService = _locationService;
            this._SumRptService = _SumRptService;
            this._SumRptTypeService = _SumRptTypeService;
            this._LedgerCapService = _LedgerCapService;
            this._BranchService = _BranchService;
            this._employeeService = _employeeService;
            this._newchartAppService = _newchartAppService;
            this._SubsidiaryService = _SubsidiaryService;
            this._ItemService = _ItemService;
            this._ReportLedgerService = _ReportLedgerService;
            this._VchrPreviewVMService = _VchrPreviewVMService;
            this._userbranchService = _userbranchService;
            this._FYDDService = _FYDDService;
            this._sysSetService = _sysSetService;
        }
        public ActionResult Search(string errMsg, string RptCaption)
        {
            if (Session["UserID"] != null)
            {
                //ViewBag.LedgerTypeCode = new SelectList(_SumRptTypeService.All().ToList(), "ReportTypeCode", "ReportTypeName");
                ViewBag.LedgerTypeCode = new SelectList(_LedgerCapService.All().ToList(), "SP_Name", "RptName");
                //var result = EFContext.TestAddresses.Select(m => m.Name).Distinct();
                //callList.Select(c => c.ApplicationID).Distinct()
                // var query = prodDtcx.PersonAddress.Select(adr => adr.City).OrderBy(adr.City) .Distinct();
                //same data avoid (shahin)
                ViewBag.LevelNo = new SelectList(_newchartAppService.All().Select(x => new { LevelNo = x.LevelNo })
                                               .Distinct(), "LevelNo", "LevelNo");

                ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");//GardenSelection();
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT.ToString("dd/MM/yyyy");
                ViewBag.Message = errMsg;
                if (RptCaption == "rptTBProcess")
                {
                    ViewBag.RptCaption = "Trial Balance";
                }
                else if (RptCaption == "rptStockSumm")
                {

                    ViewBag.RptCaption = "Stock Summary";
                }
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }


        }

        public SelectList GardenSelection()
        {

            var UserName = Session["UserName"].ToString();
            var brans = _employeeService.All().Where(x => x.Email == UserName).FirstOrDefault();

            //shahin

            List<Branch> branchs = _BranchService.All().ToList();
            List<UserBranch> userbranch = _userbranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == brans.Id.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();


            if (branchInfo.Count == 1)
            {
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else if (branchInfo.Count > 1)
            {
                //List<Branch> branch = _BranchService.All().ToList();
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else
            {
                return null;
            }

            //return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");

        }

        //public SelectList GardenSelection()
        //{

        //    var UserName = User.Identity.Name;
        //    var brans = _employeeService.All().Where(x => x.Email == UserName).ToList().FirstOrDefault();
        //    var UserId = brans.Id;
        //    var items = _userbranchService.All().ToList().Where(x => x.Userid == UserId.ToString()).ToList();
        //    List<Branch> branchList = new List<Branch>();
        //    foreach (var item in items)
        //    {
        //        var branch = _BranchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
        //        branchList.Add(branch);
        //    }
        //    branchList.Insert(0, new Branch() { BranchCode = "0", BranchName = "---- Select ----" });
        //    //List<Branch> BranchCode = new SelectList(branchList, "BranchCode", "BranchName");
        //    return new SelectList(branchList.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");

        //}
        public ActionResult GetSummaryReport(RptSearchVModel vmodel, string finyear)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("Search", "SummaryReport", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptTrialBalance_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("Search", "SummaryReport", new { errMsg });
            }

            Session["Branch"] = (vmodel.BranchCode == null) ? "" : vmodel.BranchCode;
            Session["LedgerType"] = vmodel.LedgerTypeCode;
            Session["fDate"] = vmodel.fDate;
            Session["tDate"] = vmodel.toDate;
            if (vmodel.LevelNo == null)
            {
                vmodel.LevelNo = "0";
            }

            Session["LevelNo"] = vmodel.LevelNo;
            //  Session["LevelNo"] = vmodel.LevelNo;





            var rptCap = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == vmodel.LedgerTypeCode);


            ViewBag.LedgerCap = rptCap.LedgerCap;
            ViewBag.RptCap = rptCap.RptCap;
            ViewBag.Col1Cap = rptCap.Col1Cap;
            ViewBag.Col2Cap = rptCap.Col2Cap;
            ViewBag.Col3Cap = rptCap.Col3Cap;
            ViewBag.Col4Cap = rptCap.Col4Cap;
            ViewBag.Col5Cap = rptCap.Col5Cap;
            ViewBag.Col6Cap = rptCap.Col6Cap;
            ViewBag.Col7Cap = rptCap.Col7Cap;
            ViewBag.Col8Cap = rptCap.Col8Cap;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.foDate = vmodel.fDate.ToString("MM/dd/yyyy");
            ViewBag.toDate = vmodel.toDate.ToString("MM/dd/yyyy");
            ViewBag.LocCode = vmodel.LocCode;//----location---//
            ViewBag.BranchCode = vmodel.BranchCode;
            if (vmodel.BranchCode != null && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            //---------------Location---------------------------//

            if (vmodel.LocCode != null && vmodel.LocCode != "0")
            {
                ViewBag.Location = _locationService.All().FirstOrDefault(x => x.LocCode == vmodel.LocCode.Trim()).LocName.ToString();
            }
            else
            {
                ViewBag.Location = "All";
            }

            finyear = Session["FinYear"].ToString();
            if (vmodel.LevelNo == null)
            {
                vmodel.LevelNo = "0";
            }

            string sql = string.Format(" EXEC " + rptCap.SP_Name + " '" + finyear + "','01','" + vmodel.LocCode + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.toDate.ToString("MM/dd/yyyy") + "','" + vmodel.DisControl + "','" + vmodel.LevelNo + "'");
            List<SummaryReport> summaryReport = _SumRptService.SqlQueary(sql).ToList();
            Session["Discardcntl"] = vmodel.DisControl;
            //if (summaryReport.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("Search", "SummaryReport", new { errMsg });
            //}
            //else
            //{
            if (rptCap.RptCap == "Stock Summary")
            {
                return View("~/Views/SummaryReport/GetSummaryStockReport.cshtml", summaryReport);

            }
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View(summaryReport);
            //}

        }


        [HttpPost]
        public ActionResult SummaryReportPdf(RptSearchVModel vmodel, string finyear)
        {
            vmodel.BranchCode = Session["Branch"].ToString();
            vmodel.LedgerTypeCode = Session["LedgerType"].ToString();
            if (Session["LevelNo"] == null)
            {
                vmodel.LevelNo = "0";
            }
            else
            {
                vmodel.LevelNo = Session["LevelNo"].ToString();

            }
            // vmodel.LevelNo =Session["LevelNo"].ToString();
            var rptCap = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == vmodel.LedgerTypeCode);


            ViewBag.LedgerCap = rptCap.LedgerCap;
            ViewBag.RptCap = rptCap.RptCap;
            ViewBag.Col1Cap = rptCap.Col1Cap;
            ViewBag.Col2Cap = rptCap.Col2Cap;
            ViewBag.Col3Cap = rptCap.Col3Cap;
            ViewBag.Col4Cap = rptCap.Col4Cap;
            ViewBag.Col5Cap = rptCap.Col5Cap;
            ViewBag.Col6Cap = rptCap.Col6Cap;
            ViewBag.Col7Cap = rptCap.Col7Cap;
            ViewBag.Col8Cap = rptCap.Col8Cap;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.tDate);
            ViewBag.BranchCode = vmodel.BranchCode;
            if (vmodel.BranchCode != "" && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
            ViewBag.PrintDate = DateTime.Now.ToShortDateString();

            finyear = Session["FinYear"].ToString();
            if (Session["LevelNo"] == null)
            {
                vmodel.LevelNo = "0";
            }
            else
            {
                vmodel.LevelNo = Session["LevelNo"].ToString();

            }
            string sql = string.Format(" EXEC " + rptCap.SP_Name + " '" + finyear + "','','" + vmodel.BranchCode + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.tDate.ToString("MM/dd/yyyy") + "','" + Session["Discardcntl"] + "','" + vmodel.LevelNo + "'");
            List<SummaryReport> summaryReport = _SumRptService.SqlQueary(sql).ToList();
            Session["Discardcntl"] = "";
            Session["LevelNo"] = "";
            //if (summaryReport.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("GetSummaryReport2", "SummaryReport", new { errMsg });
            //}
            //else
            //{
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            if (rptCap.RptCap == "Stock Summary")
            {
                return new Rotativa.ViewAsPdf("SummaryStockReportPdf", summaryReport) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
            }
            return new Rotativa.ViewAsPdf("SummaryReportPdf", summaryReport) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
            //}

        }



        public ActionResult GetReportLedger(string LedgerTypeCode, DateTime fDate, DateTime toDate, string Ctrl_SubCode, string BranchCode)
        {

            Session["LedgerType"] = "01";
            Session["AccountCode"] = Ctrl_SubCode;
            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.RptCap == LedgerTypeCode);


            ViewBag.LedgerCap = ledger.LedgerCap;
            ViewBag.RptCap = ledger.RptCap;
            ViewBag.OpeningCap = ledger.OpeningCap;
            ViewBag.ClosingCap = ledger.ClosingCap;
            ViewBag.Col1Cap = ledger.Col1Cap;
            ViewBag.Col2Cap = ledger.Col2Cap;
            ViewBag.Col3Cap = ledger.Col3Cap;
            ViewBag.Col4Cap = ledger.Col4Cap;
            ViewBag.Col5Cap = ledger.Col5Cap;
            ViewBag.Col6Cap = ledger.Col6Cap;
            ViewBag.Col7Cap = ledger.Col7Cap;
            ViewBag.Col8Cap = ledger.Col8Cap;
            ViewBag.fDate = fDate;
            ViewBag.tDate = toDate;
            ViewBag.AccountCode = Ctrl_SubCode;
            ViewBag.BranchCode = BranchCode;
            //ViewBag.OB = _ReportLedgerService.All().ToList().FirstOrDefault().OpenBal;

            if (BranchCode != null)
            {
                ViewBag.Branch = BranchCode;//_BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }

            BranchCode = _BranchService.All().FirstOrDefault(x => x.BranchName == BranchCode).BranchCode.ToString();
            Session["Branch"] = BranchCode;
            ViewBag.Account = LedgerTypeCode;
            string finYear = Session["FinYear"].ToString();

            string sql = string.Format("EXEC " + ledger.SP_Name + " '" + finYear + "','','" + BranchCode + "','" + Ctrl_SubCode + "','" + fDate.ToString("MM/dd/yy") + "','" + toDate.ToString("MM/dd/yy") + "'");
            List<ReportLedger> rptLedger = _ReportLedgerService.SqlQueary(sql).ToList();
            //if (rptLedger.Count == 0)
            //{
            //    //, new { errMsg });//
            //    //string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("GetSummaryReport2", "SummaryReport", new { fDate = @ViewBag.fDate, tDate = @ViewBag.tDate, LedgerTypeCode = "011" });
            //}
            //else
            //{
            return View("~/Views/Report/GetReportLedger.cshtml", rptLedger);
            //}

        }
        public ActionResult GetVoucherPreview(string VchrNo, string FinYear)
        {
            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == "rptVoucher");
            ViewBag.RptCap = ledger.RptCap;
            ViewBag.Col1Cap = ledger.Col1Cap;
            ViewBag.Col2Cap = ledger.Col2Cap;
            ViewBag.Col3Cap = ledger.Col3Cap;
            ViewBag.Col4Cap = ledger.Col4Cap;
            ViewBag.Col5Cap = ledger.Col5Cap;
            ViewBag.Col6Cap = ledger.Col6Cap;
            ViewBag.Col7Cap = ledger.Col7Cap;
            string BranchCode = Session["BranchCode"].ToString();
            ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode.Trim()).BranchName.ToString();

            FinYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC rptVoucher '" + FinYear + "','" + VchrNo + "'");
            List<VchrPreviewVM> rptVchr = _VchrPreviewVMService.SqlQueary(sql).ToList();
            //if (rptVchr.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("SearchVoucher", "VchrPreview", new { errMsg });
            //}
            //else
            //{
            double amt = 0;
            foreach (var item in rptVchr)
            {
                if (item.cramount != 0)
                {
                    amt += item.cramount;
                }
            }
            string InWordsamt = InWord.ConvertToWords(amt.ToString());
            ViewBag.InWordsAmt = InWordsamt;
            return View("~/Views/VchrPreview/GetVoucherPreview.cshtml", rptVchr);
            //}
        }




        public ActionResult GetSummaryReport2(string LedgerTypeCode, DateTime fDate, DateTime tDate)
        {
            //Session["Branch"] = vmodel.BranchCode;
            //Session["LedgerType"] = vmodel.LedgerTypeCode;
            //Session["fDate"] = vmodel.fDate;
            //Session["tDate"] = vmodel.tDate;
            //Session["LevelNo"] = vmodel.LevelNo;
            //Session["Branch"] = vmodel.BranchCode;

            string BranchCode = Session["Branch"].ToString();
            var rptCap = _LedgerCapService.All().ToList().FirstOrDefault(x => x.LedgerTypeCode == "011");


            ViewBag.LedgerCap = rptCap.LedgerCap;
            ViewBag.RptCap = rptCap.RptCap;
            ViewBag.Col1Cap = rptCap.Col1Cap;
            ViewBag.Col2Cap = rptCap.Col2Cap;
            ViewBag.Col3Cap = rptCap.Col3Cap;
            ViewBag.Col4Cap = rptCap.Col4Cap;
            ViewBag.Col5Cap = rptCap.Col5Cap;
            ViewBag.Col6Cap = rptCap.Col6Cap;
            ViewBag.Col7Cap = rptCap.Col7Cap;
            ViewBag.Col8Cap = rptCap.Col8Cap;
            if (BranchCode != null)
            {
                ViewBag.Branch = BranchCode;
            }
            else
            {
                ViewBag.Branch = "All";
            }
            Session["Branch"] = BranchCode;
            ViewBag.Account = LedgerTypeCode;
            string finYear = Session["FinYear"].ToString();

            string sql = string.Format(" EXEC " + rptCap.SP_Name + " '" + finYear + "','','" + BranchCode + "','" + Convert.ToDateTime(fDate) + "','" + Convert.ToDateTime(tDate) + "','0','0'");
            List<SummaryReport> summaryReport = _SumRptService.SqlQueary(sql).ToList();
            // Session["Discardcntl"] = vmodel.DisControl;
            //if (summaryReport.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("Search", "SummaryReport", new { errMsg });
            //}
            //else
            //{
            if (rptCap.RptCap == "Stock Summary")
            {
                return View("~/Views/SummaryReport/GetSummaryStockReport.cshtml", summaryReport);

            }
            return View(summaryReport);
            // return View("~/Views/SummaryReport/GetSummaryStockReport.cshtml", summaryReport);
            //}

        }



        public ActionResult ExportExcel()
        {
            #region Excel


            string branch = Session["Branch"].ToString();
            string fDate = Session["fDate"].ToString();
            string tDate = Session["tDate"].ToString();
            string lt = Session["LedgerType"].ToString();
            string LevelNo = Session["LevelNo"].ToString();
            string finyear = Session["FinYear"].ToString();
            string Discardcntl = Session["Discardcntl"].ToString();
            string LedgerTypeCode = Session["LedgerType"].ToString();

            var rptCap = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == LedgerTypeCode);


            string LedgerCap = rptCap.LedgerCap;
            string RptCap = rptCap.RptCap;
            string Col1Cap = rptCap.Col1Cap;
            string Col2Cap = rptCap.Col2Cap;
            string Col3Cap = rptCap.Col3Cap;
            string Col4Cap = rptCap.Col4Cap;
            string Col5Cap = rptCap.Col5Cap;
            string Col6Cap = rptCap.Col6Cap;
            string Col7Cap = rptCap.Col7Cap;
            string Col8Cap = rptCap.Col8Cap;
            // string fDate = fDate.ToString("dd-MMM-yyyy");
            //string tDate = vmodel.tDate.ToString("dd-MMM-yyyy");

            // ViewBag.BranchCode = vmodel.BranchCode;
            //if (branch != null && branch != "0")
            //{
            //    ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            //}
            //else
            //{
            //    ViewBag.Branch = "All";
            //}

            //finyear = Session["FinYear"].ToString();
            if (LevelNo == null)
            {
                LevelNo = "0";
            }

            string sql = string.Format(" EXEC " + rptCap.SP_Name + " '" + finyear + "','','" + branch + "','" + Convert.ToDateTime(fDate).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(tDate).ToString("MM/dd/yyyy") + "','" + Discardcntl + "','" + LevelNo + "'");
            List<SummaryReport> summaryReport = _SumRptService.SqlQueary(sql).ToList();
            //  Session["Discardcntl"] = DisControl;
            var sb = new StringBuilder();
            //string sql = string.Format("Select * from rptMemClosingBal");
            // List<MemClosingBalVM> contStatement = _memClosingBalService.SqlQueary(sql).ToList();
            //if (summaryReport.Count == 0)
            //{
            //    string errMsg = "There is no data in this combination. Please try again !!!";
            //    return RedirectToAction("Search", "SummaryReport",
            //        new { errMsg });
            //}
            //else
            //{
            var list = summaryReport.ToList();
            var DTableRpt = ToDataTable(list, LedgerTypeCode);

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = DTableRpt;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename= Trial_Balance.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            //}
            return null;
            #endregion

        }


        private DataTable ToDataTable(List<SummaryReport> list, string LedgerTypeCode)
        {

            var rptCap = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == LedgerTypeCode);


            string LedgerCap = rptCap.LedgerCap;
            string RptCap = rptCap.RptCap;
            string AC = rptCap.Col1Cap;
            string AN = rptCap.Col2Cap;
            string OD = rptCap.Col3Cap;
            string OC = rptCap.Col4Cap;
            string TD = rptCap.Col5Cap;
            string TC = rptCap.Col6Cap;
            string D = rptCap.Col7Cap;
            string C = rptCap.Col8Cap;
            string fDate = Session["fDate"].ToString();
            string tDate = Session["tDate"].ToString();
            // string fDate = vmodel.fDate.ToString("dd-MMM-yyyy");
            // string tDate = vmodel.tDate.ToString("dd-MMM-yyyy");


            Type type = typeof(SummaryReport);

            var props = TypeDescriptor.GetProperties(type)
                                      .Cast<PropertyDescriptor>()
                                      .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                      .Where(propertyInfo => propertyInfo.IsReadOnly == false)
                                      .ToArray();

            var table = new DataTable();


            foreach (var propertyInfo in props)
            {
                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }
            table.Columns[1].ColumnName = AC;
            table.Columns[2].ColumnName = AN;
            table.Columns[3].ColumnName = OD;
            table.Columns[4].ColumnName = C;
            table.Columns[5].ColumnName = OC;
            table.Columns[6].ColumnName = TD;
            table.Columns[7].ColumnName = TC;
            table.Columns[8].ColumnName = D;
            //table.Columns[8].ColumnName = "Forfiture1";
            //table.Columns[9].ColumnName = "Sub-Total";
            //table.Columns[10].ColumnName = "Divident For The Period";
            // table.Columns[11].ColumnName = "Total Fund Balance As On";
            table.AcceptChanges();

            foreach (var item in list)
            {
                table.Rows.Add(props.Select(property => property.GetValue(item)).ToArray());
            }

            return table;
        }

    }
}
