using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class VchrPreviewController : Controller
    {
        private readonly IVchrMainAppService _vchrMainService;
        private readonly IJTrGrpAppService _jTrGrpService;
        private readonly IVchrPreviewVMAppService _VchrPreviewVMService;
        private readonly ILedgerCaptionAppService _LedgerCapService;
        private readonly IBranchAppService _BranchService;
        private readonly ISysSetAppService _sysSetService;
        public VchrPreviewController(IVchrMainAppService _vchrMainService, IJTrGrpAppService _jTrGrpService,
            IVchrPreviewVMAppService _VchrPreviewVMService, ILedgerCaptionAppService _LedgerCapService,
            IBranchAppService _BranchService, ISysSetAppService _sysSetService)
        {
            this._VchrPreviewVMService = _VchrPreviewVMService;
            this._vchrMainService = _vchrMainService;
            this._jTrGrpService = _jTrGrpService;
            this._LedgerCapService = _LedgerCapService;
            this._BranchService = _BranchService;
            this._sysSetService = _sysSetService;
        }

        public ActionResult SearchVoucher(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.JTrGrpId = new SelectList(_jTrGrpService.All().ToList(), "VType", "TypeDesc");
                ViewBag.VchrNo = LoadDropDown.LoadVchrNoByVchrType("", _vchrMainService);
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult GetVchrNoByVchrType(string VchrType)
        {
            return Json(LoadDropDown.LoadVchrNoByVchrType(VchrType, _vchrMainService), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVoucherPreview(string VchrNo, string FinYear)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptVoucherPreview_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("SearchVoucher", "VchrPreview", new { errMsg });
            }
            if (VchrNo != null)
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
                ViewBag.Branch = _BranchService.All().Where(x => x.BranchCode == BranchCode.Trim()).Select(s => s.BranchName).ToString();

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
                return View(rptVchr);
                //}

            }
            else
            {
                string errMsg = "Voucher no is required!";
                return RedirectToAction("SearchVoucher", "VchrPreview", new { errMsg });
            }
        }
        public ActionResult GetVoucherPreviewPdf(string FinYear, string vchrNo, string pType)
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
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
            if (ViewBag.HasBranch == true)
            {
                var BranchCode = Session["BranchCode"].ToString();
                if (BranchCode != null)
                {
                    ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode).BranchName.ToString();
                }
                else
                {
                    ViewBag.Branch = "All";
                }
            }
            
            string sql = string.Format("EXEC rptVoucher '" + FinYear + "','" + vchrNo + "'");
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
            if (pType == "A4") { return new Rotativa.ViewAsPdf("~/Views/VchrPreview/VchrPreviewPdf.cshtml", rptVchr) { PageSize = Rotativa.Options.Size.A4 }; }
            else if (pType == "A3") { return new Rotativa.ViewAsPdf("~/Views/VchrPreview/VchrPreviewPdf.cshtml", rptVchr) { PageSize = Rotativa.Options.Size.A3 }; }
            else { return new Rotativa.ViewAsPdf("~/Views/VchrPreview/VchrPreviewPdf.cshtml", rptVchr) { PageSize = Rotativa.Options.Size.A5 }; }
            //}
        }
    }
}
