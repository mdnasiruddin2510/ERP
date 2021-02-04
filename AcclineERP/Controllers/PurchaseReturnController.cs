using AcclineERP.Models;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Configuration;
using System.Web.Script.Serialization;

namespace AcclineERP.Controllers
{
    public class PurchaseReturnController : Controller
    {
        private readonly ILocationAppService _locationService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly IPurRetMainAppService _PurRetService;
        private readonly ISalesDetailAppService _SaleDetailService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IVchrSetAppService _vchrSetService;
        private readonly ILotDTAppService _lotDtService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly IDefACAppService _defACService;
        private readonly ISaleRetMainAppService _saleRetMainService;
        private readonly ISaleRetDetailAppService _saleRetDetailService;
        private readonly IBranchAppService _branchService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailService;
        private readonly IPurRetMainAppService _PurRetMainService;
        private readonly IPurRetDetailAppService _PurRetDetailService;
        private readonly IPurchaseMainAppService _PurchaseMianService;
        private readonly IPurchaseDetailAppService _PurchaseDetailService;
        private readonly IIssueDetailsAppService _issueDetailService;
        private readonly IUnitAppService _unitService;

        public PurchaseReturnController(ILocationAppService _locationService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IItemInfoAppService _itemInfoService,
            ICurrentStockAppService _currentStockService, IPurRetMainAppService _PurRetService,
            ISalesDetailAppService _SaleDetailService,
            IEmployeeAppService _EmployeeService, IBranchAppService _branchService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IVchrSetAppService _vchrSetService, ILotDTAppService _lotDtService, ICostLedgerAppService CostLedgerAppService,
            IDefACAppService _defACService, IUnitAppService _unitService,
            ISaleRetMainAppService _saleRetMainService, IIssueMainAppService _issueMainService,
            ITransactionLogAppService _transactionLogService, ISaleRetDetailAppService _saleRetDetailService,
            IReceiveAppService _receiveMainService, IReceiveDetailsAppService _receiveDetailService,
            IPurRetMainAppService _PurRetMainService, IPurRetDetailAppService _PurRetDetailService,
            IPurchaseMainAppService _PurchaseMianService, IPurchaseDetailAppService _PurchaseDetailService,
            IIssueDetailsAppService _issueDetailService)
        {
            this._locationService = _locationService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._itemInfoService = _itemInfoService;
            this._currentStockService = _currentStockService;
            this._PurRetService = _PurRetService;
            this._SaleDetailService = _SaleDetailService;
            this._EmployeeService = _EmployeeService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._vchrSetService = _vchrSetService;
            this._lotDtService = _lotDtService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._defACService = _defACService;
            this._saleRetMainService = _saleRetMainService;
            this._branchService = _branchService;
            this._issueMainService = _issueMainService;
            this._transactionLogService = _transactionLogService;
            this._saleRetDetailService = _saleRetDetailService;
            this._receiveMainService = _receiveMainService;
            this._receiveDetailService = _receiveDetailService;
            this._PurRetMainService = _PurRetMainService;
            this._PurRetDetailService = _PurRetDetailService;
            this._PurchaseMianService = _PurchaseMianService;
            this._PurchaseDetailService = _PurchaseDetailService;
            this._issueDetailService = _issueDetailService;
            this._unitService = _unitService;
        }
        // GET: PurchaseReturn
        public ActionResult PurchaseReturn(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("PR");
                ViewBag.GLEntries = CountEntries("PR", datetime);
                ViewBag.LocCode = new SelectList(_locationService.All().Where(s => s.LocCode != "900").ToList(), "LocCode", "LocName");
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "2").OrderBy(x => x.SubName), "SubCode", "SubName");
                ViewBag.ItemCode = LoadDropDown.LoadEmpDlList();
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintVAT = sysSet.MaintVAT;
                Session["MaintVAT"] = sysSet.MaintVAT;
                ViewBag.Reason = LoadDropDown.LoadAdjReason();
                ViewBag.ApprBy = new SelectList(_EmployeeService.All().ToList(), "Id", "UserName");
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                ViewBag.PurRetNo = LoadDropDown.GeneratePurRetNo(_PurRetMainService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString());
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult GetDataByChallanNo(string ChallanNo)
        {
            var Purchase = _PurchaseMianService.All().Where(s => s.B_C_No == ChallanNo).FirstOrDefault();
            var PurDetail = new SelectList(_PurchaseDetailService.All().Where(s => s.PurMainID == Purchase.PurMainID).ToList(), "ItemCode", "ItemCode"); ;
            //var RcvMain = _receiveMainService.All().Where(s => s.ChallanNo == ChallanNo).Select(x => new { x.RcvDate, x.RcvNo }).FirstOrDefault();
            return Json(new { Purchase = Purchase, PurDetail = PurDetail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByQtyPrice(string ItemCode, string ChallanNo)
        {
            var Purchase = _PurchaseMianService.All().Where(s => s.B_C_No == ChallanNo).FirstOrDefault();
            var PurDetail = _PurchaseDetailService.All().Where(s => s.PurMainID == Purchase.PurMainID && s.ItemCode == ItemCode).FirstOrDefault();
            return Json(PurDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByPurRetNo(string PurRetNo)
        {
            var PurRetM = _PurRetMainService.All().Where(s => s.PurRetNo == PurRetNo).FirstOrDefault();
            var PurRetD = _PurRetDetailService.All().Where(s => s.PurRetId == PurRetM.PurRetId).ToList();

            return Json(new { PurRetM = PurRetM, PurRetD = PurRetD }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUpdatePurRet(PurRetMain PurRet, List<PurRetDetail> PurRetDetail, string IsSave)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());

                    #region for Purs Return transection
                    string trnType = "Save";
                    string Msg = "1";


                    //Firstly deleted the saved For Update
                    if (IsSave == "1")
                    {
                        if (!rUser.HasPermission("PurchaseReturn_Update"))
                        {
                            return Json("U", JsonRequestBehavior.AllowGet);
                        }
                        trnType = "Update";
                        Msg = "2";

                        #region For AVP OR LOT

                        if (Session["MaintLot"].ToString() == "True")
                        {
                            var sRetMain = _PurRetMainService.All().Where(s => s.PurRetNo == PurRet.PurRetNo).FirstOrDefault();
                            var sRetDetail = _PurRetDetailService.All().Where(s => s.PurRetId == sRetMain.PurRetId).ToList();
                            foreach (var currentItem in sRetDetail)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == PurRet.LocNo &&
                                    m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentStocks.CurrQty + currentItem.ReturnQty;
                                    currentStocks.UnitPrice = Convert.ToDouble(currentItem.UnitPrice);
                                    _currentStockService.Update(currentStocks);
                                }
                            }
                        }
                        else
                        {
                            // For Costledger Code
                        }
                        #endregion

                        var saleRetDel = _PurRetMainService.All().ToList().Where(y => y.PurRetNo == PurRet.PurRetNo).FirstOrDefault();
                        _PurRetDetailService.All().ToList().Where(y => y.PurRetId == saleRetDel.PurRetId).ToList().ForEach(x => _PurRetDetailService.Delete(x));
                        _PurRetDetailService.Save();
                        _PurRetMainService.Delete(saleRetDel);
                        _PurRetMainService.Save();

                        var rcvDel = _issueMainService.All().ToList().Where(y => y.RefNo == PurRet.PurRetNo && y.VchrNo == PurRet.VchrNo).FirstOrDefault();
                        _issueDetailService.All().ToList().Where(y => y.IssueNo == PurRet.ChallanNo).ToList().ForEach(x => _issueDetailService.Delete(x));
                        _receiveDetailService.Save();
                        _issueMainService.Delete(rcvDel);
                        _issueMainService.Save();
                    }

                    if (!rUser.HasPermission("PurchaseReturn_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    var IfExist = _PurRetMainService.All().Where(x => x.PurRetNo == PurRet.PurRetNo).FirstOrDefault();
                    if (IfExist == null)
                    {
                        PurRet.FinYear = Session["FinYear"].ToString();
                        PurRet.BranchCode = Session["BranchCode"].ToString();
                        PurRet.ProjCode = Session["ProjCode"].ToString();
                        PurRet.EntryDateTime = DateTime.Now;
                        _PurRetMainService.Add(PurRet);
                        _PurRetMainService.Save();

                        if (PurRetDetail != null)
                        {
                            foreach (var sRetDExtItem in PurRetDetail)
                            {

                                PurRetDetail sRetDetail = new PurRetDetail();
                                var srID = _PurRetMainService.All().Select(s => s.PurRetId).LastOrDefault();
                                if (srID == 0)
                                {
                                    sRetDExtItem.PurRetId = 1;
                                }
                                else
                                {
                                    sRetDExtItem.PurRetId = srID;
                                }
                                sRetDetail = sRetDExtItem;
                                _PurRetDetailService.Add(sRetDetail);
                            }
                        }
                        _PurRetDetailService.Save();

                        #region  For Issue
                        var IssueNo = _issueMainService.All().Select(s => Convert.ToInt32(s.IssueNo)).LastOrDefault() + 1;
                        if (IssueNo != 0)
                        {
                            var todayDate = DateTime.Now;
                            IssueMain issueInfo = new IssueMain();
                            issueInfo.IssueNo = "0" + IssueNo.ToString(); //PurRet.PurRetNo;
                            issueInfo.BranchCode = Session["BranchCode"].ToString();
                            issueInfo.IssueDate = PurRet.PurRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.FromLocCode = PurRet.LocNo;
                            //issueInfo.StoreLocCode = PurRet.StoreLocCode;
                            issueInfo.IssueToSubCode = PurRet.CustCode;
                            //issueInfo.DesLocCode = PurRet.DesLocCode;
                            //issueInfo.Accode = PurRet.Accode;
                            issueInfo.RefNo = PurRet.PurRetNo;
                            issueInfo.RefDate = PurRet.PurRetDate;
                            issueInfo.Remarks = PurRet.Remarks;
                            //issueInfo.IssueByCode = PurRet.IssueByCode;
                            issueInfo.AppByCode = PurRet.ApprBy;
                            issueInfo.IssTime = DateTime.Now;
                            issueInfo.FinYear = Session["FinYear"].ToString();
                            issueInfo.GLPost = false;
                            issueInfo.IssDate = PurRet.PurRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.cashReceiptStatus = false;
                            issueInfo.IsReceived = false;
                            issueInfo.VchrNo = PurRet.VchrNo;
                            double amount = 0.0;
                            issueInfo.EntrySrc = "PR";
                            issueInfo.EntrySrcNo = issueInfo.IssueNo;
                            List<IssueDetails> issuDetailsList = new List<IssueDetails>();
                            #region For LOT OR AVP
                            if (Session["MaintLot"].ToString() == "True")
                            {
                                #region Not AutoLot by issueDetail

                                foreach (var currentItem in PurRetDetail)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                        m.LocCode == PurRet.LocNo &&
                                        m.LotNo == currentItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty - currentItem.ReturnQty;
                                        currentStocks.UnitPrice = (double)currentItem.UnitPrice;
                                        _currentStockService.Update(currentStocks);
                                    }
                                    else
                                    {
                                        CurrentStock currStock = new CurrentStock();
                                        currStock.LocCode = PurRet.LocNo;
                                        currStock.LotNo = currentItem.LotNo;
                                        currStock.ItemCode = currentItem.ItemCode;
                                        currStock.CurrQty = 0 - currentItem.ReturnQty;
                                        currStock.UnitPrice = (double)currentItem.UnitPrice;
                                        _currentStockService.Add(currStock);
                                    }
                                }
                                #endregion

                            }
                            else
                            {
                                #region for Cost ledger
                                foreach (var costLedger in PurRetDetail)
                                {
                                    CostLedger cLedger = new CostLedger();
                                    cLedger.IssQty = costLedger.ReturnQty;
                                    cLedger.IssRate = (double)costLedger.UnitPrice;
                                    cLedger.IssTotal = cLedger.IssQty * cLedger.IssRate;

                                    cLedger.RecQty = 0;
                                    cLedger.RecRate = 0;
                                    cLedger.RecTotal = 0;

                                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == PurRet.LocNo.Trim()).ToList();
                                    if (existCurrStoc.Count != 0)
                                    {
                                        var date = existCurrStoc.Max(x => x.TrDate);
                                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == PurRet.LocNo.Trim() && x.TrDate == date).FirstOrDefault();

                                        cLedger.BalQty = existCurrStock.BalQty - cLedger.IssQty;
                                        cLedger.BalRate = System.Math.Round(existCurrStock.BalRate, 2);
                                        cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                        cLedger.LocNo = PurRet.LocNo;
                                        cLedger.ItemCode = costLedger.ItemCode;
                                        cLedger.TrDate = PurRet.PurRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                        cLedger.UpdSrcNo = PurRet.PurRetNo;
                                        cLedger.UpdSrc = "PR";
                                        CostLedgerAppService.Add(cLedger);
                                    }
                                    else if (existCurrStoc.Count == 0)
                                    {
                                        cLedger.CurrQty = 0;
                                        cLedger.UnitPrice = 0;
                                        cLedger.BalTotal = 0;

                                        cLedger.BalQty = cLedger.BalQty - cLedger.IssQty;
                                        cLedger.BalRate = cLedger.IssRate;
                                        cLedger.BalRate = System.Math.Round(cLedger.BalRate, 2);
                                        cLedger.BalTotal =System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                        cLedger.LocNo = PurRet.LocNo;
                                        cLedger.ItemCode = costLedger.ItemCode;
                                        cLedger.TrDate = PurRet.PurRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                        cLedger.UpdSrcNo = PurRet.PurRetNo;
                                        cLedger.UpdSrc = "PR";
                                        CostLedgerAppService.Add(cLedger);
                                    }
                                    CostLedgerAppService.Save();
                                }
                                #endregion
                            }
                            #endregion


                            foreach (var issuDetailsItem in PurRetDetail)
                            {
                                IssueDetails issueDetailsInfo = new IssueDetails();
                                issueDetailsInfo.IssueNo = "0" + IssueNo.ToString();
                                issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                issueDetailsInfo.SubCode = PurRet.CustCode;
                                issueDetailsInfo.Description = issuDetailsItem.Description;
                                issueDetailsInfo.Qty = issuDetailsItem.ReturnQty;
                                issueDetailsInfo.Price = (double)issuDetailsItem.UnitPrice;
                                //issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                issueDetailsInfo.LotNo = "01";
                                amount = amount + (issuDetailsItem.ReturnQty * (double)issuDetailsItem.UnitPrice);
                                issuDetailsList.Add(issueDetailsInfo);



                            }

                            issueInfo.Amount = amount;
                            issueInfo.IssueDetails = issuDetailsList;
                            _issueMainService.Add(issueInfo);
                            _issueMainService.Save();
                        }
                        #endregion
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Purchase Return", trnType, PurRet.VchrNo, Session["UserName"].ToString());
                        //LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                        transaction.Complete();
                        return Json(new { Msg = Msg }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GeneratePurRetNo()
        {
            return Json(LoadDropDown.GeneratePurRetNo(_PurRetMainService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString()), JsonRequestBehavior.AllowGet);

        }

        #region Generate Voucher No
        public ActionResult GetVoucherNo(string VType)
        {
            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            //string VLen = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sql = string.Format("exec [GetNewVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "'  ,'" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sql).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region For Intregration

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("PurchaseReturn_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("PurchaseReturn", "PurchaseReturn", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("PurchaseReturn", "PurchaseReturn", new { errMsg });
            }
            else
            {
                //string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntriesPdf(string vchrNo)
        {
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary,pvd.Narration,"
                + " pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from VchrMain as pvm "
                + "join VchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear  where pvm.VchrNo ='" + vchrNo + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("PurchaseReturn", "PurchaseReturn", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }


        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("PurchaseReturn_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("PurchaseReturn", "PurchaseReturn", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear left outer join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "' and pvm.BranchCode ='" + Session["BranchCode"].ToString() + "'"
                 + "and pvm.BranchCode= '" + branchCode + "'"
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.slno,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac order by pvd.slno");


            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("PurchaseReturn", "PurchaseReturn", new { errMsg });

            }
            else
            {
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                // ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGlPCountN(string VType)
        {
            return Json(Counter(VType), JsonRequestBehavior.AllowGet);
        }
        public string Counter(string VType)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain where VType='" + VType + "' and BranchCode='" + branchCode + "'");
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }

        public ActionResult GetGlECountN(string VType, DateTime datetime)
        {
            return Json(CountEntries(VType, datetime), JsonRequestBehavior.AllowGet);
        }
        public string CountEntries(string VType, DateTime datetime)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain where VType='" + VType + "' and vDate ='" + datetime.ToString("yyyy/MM/dd") + "'and BranchCode='" + branchCode + "'");
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }

        #endregion

    }
}