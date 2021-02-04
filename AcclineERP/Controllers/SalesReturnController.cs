using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using App.Domain;
using System.Transactions;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;

namespace AcclineERP.Controllers
{
    public class SalesReturnController : Controller
    {
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly ILocationAppService _locationService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ISalesMainAppService _SalesMainService;
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
        private readonly IUnitAppService _unitService;

        public SalesReturnController(ICommonPVVMAppService _CommonVmService, ILocationAppService _locationService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IItemInfoAppService _itemInfoService,
            ICurrentStockAppService _currentStockService, ISalesMainAppService _SalesMainService,
            ISalesDetailAppService _SaleDetailService,
            IEmployeeAppService _EmployeeService, IBranchAppService _branchService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IVchrSetAppService _vchrSetService, ILotDTAppService _lotDtService, ICostLedgerAppService CostLedgerAppService,
            IDefACAppService _defACService, IUnitAppService _unitService,
            ISaleRetMainAppService _saleRetMainService, IIssueMainAppService _issueMainService,
            ITransactionLogAppService _transactionLogService, ISaleRetDetailAppService _saleRetDetailService,
            IReceiveAppService _receiveMainService, IReceiveDetailsAppService _receiveDetailService)
        {
            this._CommonVmService = _CommonVmService;
            this._locationService = _locationService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._itemInfoService = _itemInfoService;
            this._currentStockService = _currentStockService;
            this._SalesMainService = _SalesMainService;
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
            this._unitService = _unitService;
        }
        // GET: SalesReturn
        public ActionResult SalesReturn(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("SR");
                ViewBag.GLEntries = CountEntries("SR", datetime);
                ViewBag.LocCode = new SelectList(_locationService.All().Where(s => s.LocCode != "900").ToList(), "LocCode", "LocName");
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "1").OrderBy(x => x.SubName), "SubCode", "SubName");
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
                ViewBag.SaleRetNo = LoadDropDown.GenerateSaleRetNo(_saleRetMainService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString());
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
            var Sales = _SalesMainService.All().Where(s => s.IssueNo == ChallanNo).FirstOrDefault();
            var SalesDetail = LoadDropDown.LoadItemByChallanNo(ChallanNo, _CommonVmService); //new SelectList(_SaleDetailService.All().Where(s => s.SalesMainID == Sales.SalesMainID).ToList(), "ItemCode", "ItemCode"); ;
            var issueMain = _issueMainService.All().Where(s => s.IssueNo == ChallanNo).Select(x => new { x.IssDate, x.IssueNo }).FirstOrDefault();
            return Json(new { Sales = Sales, SalesDetail = SalesDetail, issueMain = issueMain }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByQtyPrice(string ItemCode, string ChallanNo)
        {
            var Sales = _SalesMainService.All().Where(s => s.IssueNo == ChallanNo).FirstOrDefault();
            var SalesDetail = _SaleDetailService.All().Where(s => s.SalesMainID == Sales.SalesMainID && s.ItemCode == ItemCode).FirstOrDefault();
            return Json(SalesDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBySaleRetNo(string SaleRetNo)
        {
            var SaleRetM = _saleRetMainService.All().Where(s => s.SaleRetNo == SaleRetNo).FirstOrDefault();
            var SaleRetD = _saleRetDetailService.All().Where(s => s.SaleRetId == SaleRetM.SaleRetId).ToList();

            return Json(new { SaleRetM = SaleRetM, SaleRetD = SaleRetD }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUpdateSaleRet(SaleRetMain SaleRet, List<SaleRetDetail> SaleRetDetail, string IsSave)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());

                    #region for Sales Return transection
                    string trnType = "Save";
                    string Msg = "1";

                    //Firstly deleted the saved For Update
                    if (IsSave == "1")
                    {
                        if (!rUser.HasPermission("SalesReturn_Update"))
                        {
                            return Json("U", JsonRequestBehavior.AllowGet);
                        }
                        trnType = "Update";
                        Msg = "2";

                        #region For AVP OR LOT

                        if (Session["MaintLot"].ToString() == "True")
                        {
                            var sRetMain = _saleRetMainService.All().Where(s => s.SaleRetNo == SaleRet.SaleRetNo).FirstOrDefault();
                            var sRetDetail = _saleRetDetailService.All().Where(s => s.SaleRetId == sRetMain.SaleRetId).ToList();
                            foreach (var currentItem in sRetDetail)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == SaleRet.LocNo &&
                                    m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentStocks.CurrQty - currentItem.ReturnQty;
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

                        var saleRetDel = _saleRetMainService.All().ToList().Where(y => y.SaleRetNo == SaleRet.SaleRetNo).FirstOrDefault();
                        _saleRetDetailService.All().ToList().Where(y => y.SaleRetId == saleRetDel.SaleRetId).ToList().ForEach(x => _saleRetDetailService.Delete(x));
                        _saleRetDetailService.Save();
                        _saleRetMainService.Delete(saleRetDel);
                        _saleRetMainService.Save();

                        var rcvDel = _receiveMainService.All().ToList().Where(y => y.ChallanNo == SaleRet.ChallanNo && y.RefNo == SaleRet.SaleRetNo && y.VchrNo == SaleRet.VchrNo).FirstOrDefault();
                        _receiveDetailService.All().ToList().Where(y => y.RcvNo == rcvDel.RcvNo).ToList().ForEach(x => _receiveDetailService.Delete(x));
                        _receiveDetailService.Save();
                        _receiveMainService.Delete(rcvDel);
                        _receiveMainService.Save();


                    }

                    if (!rUser.HasPermission("SalesReturn_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    var IfExist = _saleRetMainService.All().Where(x => x.SaleRetNo == SaleRet.SaleRetNo).FirstOrDefault();
                    if (IfExist == null)
                    {
                        SaleRet.FinYear = Session["FinYear"].ToString();
                        SaleRet.BranchCode = Session["BranchCode"].ToString();
                        SaleRet.ProjCode = Session["ProjCode"].ToString();
                        SaleRet.EntryDateTime = DateTime.Now;
                        _saleRetMainService.Add(SaleRet);
                        _saleRetMainService.Save();

                        if (SaleRetDetail != null)
                        {
                            foreach (var sRetDExtItem in SaleRetDetail)
                            {
                                SaleRetDetail sRetDetail = new SaleRetDetail();
                                var srID = _saleRetMainService.All().Select(s => s.SaleRetId).LastOrDefault();
                                if (srID == null)
                                {
                                    sRetDExtItem.SaleRetId = 1;
                                }
                                else
                                {
                                    sRetDExtItem.SaleRetId = srID;
                                }
                                sRetDetail = sRetDExtItem;
                                _saleRetDetailService.Add(sRetDetail);



                            }
                        }
                        _saleRetDetailService.Save();

                        #region for Receive transection

                        //var ExistRcv = _receiveMainService.All().Where(x => x.RcvNo == SaleRet.SaleRetNo).FirstOrDefault();
                        var rcvNo = _receiveMainService.All().Select(s => Convert.ToInt32(s.RcvNo)).LastOrDefault() + 1;
                        //if (ExistRcv == null)
                        //{
                        var todayDate = DateTime.Now;
                        ReceiveMain recvMain = new ReceiveMain();
                        recvMain.RcvNo = "0" + rcvNo.ToString();
                        recvMain.BranchCode = Session["BranchCode"].ToString();
                        recvMain.RcvDate = SaleRet.SaleRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.Source = SaleRet.CustCode;
                        recvMain.ChallanNo = SaleRet.ChallanNo;
                        recvMain.RefNo = SaleRet.SaleRetNo;
                        recvMain.RefDate = (DateTime)SaleRet.SaleRetDate;
                        recvMain.Remarks = SaleRet.Remarks;
                        recvMain.AppByCode = SaleRet.ApprBy;
                        recvMain.RcvdTime = DateTime.Now;
                        recvMain.StoreLocCode = SaleRet.LocNo;
                        recvMain.RecvFromSubCode = SaleRet.CustCode;
                        recvMain.VchrNo = SaleRet.VchrNo;
                        recvMain.FinYear = Session["FinYear"].ToString();
                        recvMain.GLPost = false;
                        recvMain.RcvdDate = SaleRet.SaleRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.expenseStatus = false;
                        recvMain.CreditPur = false;

                        double amount = 0.0;

                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                        foreach (var recvDetailsItem in SaleRetDetail)
                        {
                            ReceiveDetails receivDetails = new ReceiveDetails();
                            receivDetails.RcvNo = "0" + rcvNo.ToString();
                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                            receivDetails.SubCode = SaleRet.CustCode;
                            receivDetails.Description = recvDetailsItem.Description;
                            receivDetails.Qty = recvDetailsItem.ReturnQty;
                            receivDetails.Price = Convert.ToDouble(recvDetailsItem.UnitPrice);
                            receivDetails.ExQty = recvDetailsItem.ReturnQty;
                            receivDetails.LotNo = recvDetailsItem.LotNo;

                            amount = amount + Convert.ToDouble((recvDetailsItem.ReturnQty * recvDetailsItem.UnitPrice));
                            receveDetailsList.Add(receivDetails);
                        }

                        recvMain.Amount = amount;

                        #region For AVP OR LOT

                        if (Session["MaintLot"].ToString() == "True")
                        {
                            foreach (var currentItem in SaleRetDetail)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == recvMain.StoreLocCode &&
                                    m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentItem.ReturnQty + currentStocks.CurrQty;
                                    currentStocks.UnitPrice = Convert.ToDouble(currentItem.UnitPrice);
                                    _currentStockService.Update(currentStocks);
                                }
                                else
                                {
                                    CurrentStock currStock = new CurrentStock();
                                    currStock.LocCode = SaleRet.LocNo;
                                    currStock.LotNo = currentItem.LotNo;
                                    currStock.ItemCode = currentItem.ItemCode;
                                    currStock.CurrQty = currentItem.ReturnQty;
                                    currStock.UnitPrice = Convert.ToDouble(currentItem.UnitPrice);
                                    _currentStockService.Add(currStock);
                                }

                                var lotNO = _lotDtService.All().ToList().Where(m => m.LotNo == currentItem.LotNo).FirstOrDefault();
                                if (lotNO == null)
                                {
                                    LotDT lotDt = new LotDT();
                                    lotDt.LotNo = currentItem.LotNo;
                                    lotDt.LotDate = SaleRet.SaleRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                    lotDt.RefNo = SaleRet.SaleRetNo;
                                    lotDt.RefSource = "Sales Return Receive";
                                    _lotDtService.Add(lotDt);
                                }
                            }
                        }
                        else
                        {
                            #region AVP
                            foreach (var cItem in SaleRetDetail)
                            {
                                CostLedger cLedger = new CostLedger();
                                cLedger.RecQty = cItem.ReturnQty;
                                cLedger.RecRate = Convert.ToDouble(cItem.UnitPrice);
                                cLedger.RecTotal = System.Math.Round(cLedger.RecQty * cLedger.RecRate, 2);

                                double CurrQty = 0, BalTotal = 0;
                                var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == cItem.ItemCode && x.LocNo == recvMain.StoreLocCode.Trim()).ToList();
                                if (existCurrStoc.Count != 0)
                                {
                                    var date = existCurrStoc.Max(x => x.TrDate);
                                    var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == cItem.ItemCode && x.LocNo == recvMain.StoreLocCode.Trim() && x.TrDate == date).ToList();
                                    foreach (var item in existCurrStock)
                                    {
                                        CurrQty = item.BalQty;
                                        BalTotal = item.BalTotal;
                                    }
                                }
                                cLedger.IssQty = 0;
                                cLedger.IssRate = 0;
                                cLedger.IssTotal = 0;
                                cLedger.BalQty = CurrQty + cLedger.RecQty;

                                if (cLedger.BalQty != 0)
                                { cLedger.BalRate = (BalTotal + cLedger.RecTotal) / cLedger.BalQty; }
                                else { cLedger.BalRate = 0; }
                                cLedger.BalRate = System.Math.Round(cLedger.BalRate, 2);

                                cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                cLedger.LocNo = recvMain.StoreLocCode;
                                cLedger.ItemCode = cItem.ItemCode;
                                cLedger.TrDate = (DateTime)SaleRet.SaleRetDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                cLedger.UpdSrcNo = SaleRet.SaleRetNo;
                                cLedger.UpdSrc = "SR";
                                CostLedgerAppService.Add(cLedger);
                            }
                            CostLedgerAppService.Save();

                            #endregion
                        }
                        #endregion

                        recvMain.RcvDetails = receveDetailsList;
                        _receiveMainService.Add(recvMain);
                        _currentStockService.Save();
                        _receiveMainService.Save();
                        //}

                        #endregion


                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Sales Return", trnType, rcvNo.ToString(), Session["UserName"].ToString());
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

        public ActionResult GetItemByitemType(string ItemType)
        {
            return Json(LoadDropDown.ByItmeType(ItemType, _itemInfoService), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateSaleRetNo()
        {
            return Json(LoadDropDown.GenerateSaleRetNo(_saleRetMainService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString()), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SalesReturn_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("SalesReturn", "SalesReturn", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("SalesReturn", "SalesReturn", new { errMsg });
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
                return RedirectToAction("SalesReturn", "SalesReturn", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }


        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SalesReturn_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("SalesReturn", "SalesReturn", new { errMsg });
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
                return RedirectToAction("SalesReturn", "SalesReturn", new { errMsg });

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

    }
}