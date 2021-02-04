using AcclineERP.Models;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    public class StockLedgerRptController : Controller
    {
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IReportLedgerAppService _ReportLedgerService;
        private readonly ILedgerCaptionAppService _LedgerCapService;
        private readonly IItemInfoAppService _ItemService;
        private readonly IBranchAppService _BranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ISysSetAppService _sysSetService;
        private readonly ICommonPVVMAppService _CommonVmService;

        public StockLedgerRptController(IReportLedgerAppService _ReportLedgerService, ILedgerCaptionAppService _LedgerCapService,
            IItemInfoAppService _ItemService, IBranchAppService _BranchService, IFYDDAppService _FYDDService, ISysSetAppService _sysSetService,
            IItemTypeAppService _itemTypeService, ICommonPVVMAppService _CommonVmService)
        {
            this._ReportLedgerService = _ReportLedgerService;
            this._LedgerCapService = _LedgerCapService;
            this._ItemService = _ItemService;
            this._BranchService = _BranchService;
            this._FYDDService = _FYDDService;
            this._sysSetService = _sysSetService;
            this._itemTypeService = _itemTypeService;
            this._CommonVmService = _CommonVmService;
        }
        // GET: StockLedgerRpt
        public ActionResult StockLedgerRpt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName"); //GardenSelection(); 
                ViewBag.AccountCode = new SelectList(_ItemService.All().ToList(), "ItemCode", "ItemName");
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();

                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion

                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }


        public ActionResult GetReportLedger(RptSearchVModel vmodel, string finyear)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("StockLedgerRpt", "StockLedgerRpt", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptGeneralLedger_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("StockLedgerRpt", "StockLedgerRpt", new { errMsg });
            }
            vmodel.BranchCode = (vmodel.BranchCode == null) ? "" : vmodel.BranchCode;
            Session["Branch"] = vmodel.BranchCode;
            vmodel.AccountCode = (vmodel.AccountCode == null) ? "" : vmodel.AccountCode;
            Session["AccountCode"] = vmodel.AccountCode;
            Session["fDate"] = vmodel.fDate;
            Session["tDate"] = vmodel.toDate;


            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == "rptStoreLedger");


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
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.AccountCode = vmodel.AccountCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;

            if (vmodel.BranchCode != "" && vmodel.BranchCode != "0")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            if (vmodel.AccountCode != "")
            {
                ViewBag.Account = _ItemService.All().FirstOrDefault(x => x.ItemCode == vmodel.AccountCode.Trim()).ItemName.ToString();
            }
            else
            {
                ViewBag.Account = "All";
            }

            finyear = Session["FinYear"].ToString();

            string sql = string.Format(" EXEC " + ledger.SP_Name + " '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.AccountCode + "','" + vmodel.fDate.ToString("MM-dd-yyyy") + "','" + vmodel.toDate.ToString("MM-dd-yyyy") + "'");
            List<ReportLedger> rptLedger = _ReportLedgerService.SqlQueary(sql).ToList();
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View(rptLedger);

        }


        [HttpPost]
        public ActionResult StockLedgerPdf(RptSearchVModel vmodel, string finyear)
        {
            vmodel.BranchCode = Session["Branch"].ToString();
            vmodel.AccountCode = Session["AccountCode"].ToString();
            var ledger = _LedgerCapService.All().ToList().FirstOrDefault(x => x.SP_Name == "rptStoreLedger");

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
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.AccountCode = vmodel.AccountCode;
            ViewBag.BranchCode = vmodel.BranchCode;
            if (vmodel.BranchCode != "")
            {
                ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
            }
            else
            {
                ViewBag.Branch = "All";
            }
            if (vmodel.AccountCode != "")
            {
                ViewBag.Account = _ItemService.All().FirstOrDefault(x => x.ItemCode == vmodel.AccountCode.Trim()).ItemName.ToString();

            }
            else
            {
                ViewBag.Account = "All";
            }

            ViewBag.PrintDate = DateTime.Now.ToShortDateString();

            finyear = Session["FinYear"].ToString();

            string sql = string.Format(" EXEC " + ledger.SP_Name + " '" + finyear + "','" + Session["ProjCode"].ToString() + "','" + vmodel.BranchCode + "','" + vmodel.AccountCode + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.toDate.ToString("MM/dd/yyyy") + "'");
            List<ReportLedger> rptLedger = _ReportLedgerService.SqlQueary(sql).ToList();
            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return new Rotativa.ViewAsPdf("StockLedgerPdf", rptLedger) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }



    }
}