using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Web.Script.Serialization;
using Data.Context;

namespace AcclineERP.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly ILocationAppService _locationService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly ISubsidiaryExtAppService _subSidiaryExtService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly ItmpPurItemAppService _tmpPurItemService;
        private readonly IPurchaseMainAppService _purchaseMainService;
        private readonly IPurchaseDetailAppService _purchaseDetailService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ILotDTAppService _lotDTService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly IDefACAppService _defACService;
        private readonly INewChartAppService _newChartService;
        public PurchaseController(ILocationAppService _locationService, IItemTypeAppService _itemTypeService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IItemInfoAppService _itemInfoService,
            ICurrentStockAppService _currentStockService, ICommonPVVMAppService _CommonVmService,
            IEmployeeAppService _EmployeeService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            ItmpPurItemAppService _tmpPurItemService, IPurchaseMainAppService _purchaseMainService,
            IPurchaseDetailAppService _purchaseDetailService, ITransactionLogAppService _transactionLogService,
            ILotDTAppService _lotDTService, ISubsidiaryExtAppService _subSidiaryExtService,
            IReceiveAppService _receiveMainService, IReceiveDetailsAppService _receiveDetailsService,
            ICostLedgerAppService CostLedgerAppService, IDefACAppService _defACService,
            INewChartAppService _newChartService)
        {
            this._locationService = _locationService;
            this._itemTypeService = _itemTypeService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._subSidiaryExtService = _subSidiaryExtService;
            this._itemInfoService = _itemInfoService;
            this._currentStockService = _currentStockService;
            this._CommonVmService = _CommonVmService;
            this._EmployeeService = _EmployeeService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._tmpPurItemService = _tmpPurItemService;
            this._purchaseMainService = _purchaseMainService;
            this._purchaseDetailService = _purchaseDetailService;
            this._transactionLogService = _transactionLogService;
            this._lotDTService = _lotDTService;
            this._receiveMainService = _receiveMainService;
            this._receiveDetailsService = _receiveDetailsService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._defACService = _defACService;
            this._newChartService = _newChartService;
        }
        // GET: Purchase
        public ActionResult Purchase()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.LocNo = new SelectList(_locationService.All().Where(s => s.LocCode != "900").ToList(), "LocCode", "LocName");
                ViewBag.PurNo = GeneratePurNo(Session["BranchCode"].ToString());
                ViewBag.PurCurrency = LoadDropDown.LoadCurrInfo();
                ViewBag.SupCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "2").OrderBy(x => x.SubName), "SubCode", "SubName");
                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.ItemName = LoadDropDown.LoadItemBySSGroupID("", "", "", "", _itemInfoService, _CommonVmService);
                ViewBag.EntryBy = LoadIssueBy(_EmployeeService);
                ViewBag.GLProvition = Count("AP");
                ViewBag.GLEntries = CountEntries("AP", DateTime.Now);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                ViewBag.MaintLot = sysSet.MaintLot;
                Session["MaintLot"] = sysSet.MaintLot;
                Session["MaintVAT"] = sysSet.MaintVAT;
                ViewBag.MaintVAT = sysSet.MaintVAT;
                ViewBag.OnlyVAT = sysSet.OnlyVAT;
                ViewBag.MakeRecvAuto = sysSet.MakeRecvAuto;
                ViewBag.VATType = LoadDropDown.LoadVATType();
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                var DefAc = _defACService.All().ToList().FirstOrDefault();
                ViewBag.Accode = LoadDropDown.LoadGLAc(DefAc.Purchase, _newChartService);
                ViewBag.Ca_Bk_Op = LoadDropDown.LoadGLAc(DefAc.PurAc1, _newChartService);
                ViewBag.CountryCode = LoadDropDown.LoadCountryDDL();
                ViewBag.CHCode = LoadDropDown.LoadCustomHouseDDL();
                ViewBag.AccountCode = LoadDropDown.LoadVM_TreasuryAc();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        class retvalpro
        {
            public string VATRate { get; set; }
            public string SDRate { get; set; }
            public Nullable<decimal> FixedRate { get; set; }
        }
        public ActionResult GetSuppTaxVATPrec(string TaxHeadingNo, string HSCode, decimal PurQty)
        {
            retvalpro taxChart;
            string fixedsql = string.Format("EXEC VM_GetFixedRate '" + HSCode + "','" + TaxHeadingNo + "', '" + PurQty + "'");
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                taxChart = dbContext.Database.SqlQuery<retvalpro>(fixedsql).FirstOrDefault();
            }

            if (taxChart == null || taxChart.FixedRate == 0)
            {
                string sql = string.Format("EXEC VM_GetVAT '" + HSCode + "','" + TaxHeadingNo + "', '" + PurQty + "'");
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    taxChart = dbContext.Database.SqlQuery<retvalpro>(sql).FirstOrDefault();
                }
            }

            if (taxChart == null || taxChart.VATRate == "0")
            {
                string sql = string.Format("EXEC VM_GetSD '" + HSCode + "','" + TaxHeadingNo + "', '" + PurQty + "'");
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    taxChart = dbContext.Database.SqlQuery<retvalpro>(sql).FirstOrDefault();
                }
            }
            if (taxChart == null)
            {
                return Json(new
                {
                    VATRate = "0",
                    SDRate = "0",
                    FixedRate = 0
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    VATRate = (taxChart.VATRate == null) ? "0" : taxChart.VATRate,
                    SDRate = (taxChart.SDRate == null) ? "0" : taxChart.SDRate,
                    FixedRate = (taxChart.FixedRate == null) ? 0 : taxChart.FixedRate
                }, JsonRequestBehavior.AllowGet);
            }


            //string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetSuppTaxVATPrec?HSCode=" + HSCode.ToString() + "&PurQty=" + PurQty + "&TaxHeadingNo=" + TaxHeadingNo, Session["token"].ToString());
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //VM_TaxChart itemList = js.Deserialize<VM_TaxChart>(JsonResponse);
            //return Json(new { data = itemList }, JsonRequestBehavior.AllowGet);
        }

        public List<tmpPurItem> GetAllTempDataList(string purNo)
        {
            var iData = new List<tmpPurItem>();
            _tmpPurItemService.All().ToList().Where(x => x.PurNo == purNo).ToList().ForEach(x => iData.Add(
                new tmpPurItem(x.tPurId, x.LocNo, x.PurNo, x.ItemName, x.LotNo, x.OBQty, x.OBAmt, x.PurQty, x.UPrice,
                    x.Amount, x.SupTaxAmt, x.Description, x.VATType, x.VATAmt, x.SubTotal, x.VATEx, x.TaxEx, x.VATRate, x.SDRate)
            ));
            return iData;
        }

        public ActionResult GetTmpPurDatatableOnly(string purNo)
        {
            List<tmpPurItem> TvchrLst = GetAllTempDataList(purNo);
            foreach (var item in TvchrLst)
            {
                item.ItemName = _itemInfoService.All().Where(s => s.ItemCode == item.ItemName).Select(s => s.ItemName).FirstOrDefault();
            }
            return Json(new { data = TvchrLst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByPurNo(string PurNo)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var pMain = default(dynamic);
                    var FinYear = Session["FinYear"].ToString();
                    var LoginName = Session["UserName"].ToString();
                    pMain = _purchaseMainService.All().ToList().FirstOrDefault(x => x.PurNo == PurNo);
                    if (pMain != null)
                    {
                        var IsExistPur = _tmpPurItemService.All().ToList().Where(x => x.PurNo == PurNo).ToList();
                        if (IsExistPur.Count != 0)
                        {
                            _tmpPurItemService.All().ToList().Where(y => y.PurNo == PurNo && y.FinYear == FinYear && y.LoginName == LoginName).ToList().ForEach(x => _tmpPurItemService.Delete(x));
                            _tmpPurItemService.Save();
                        }
                        var pDetail = _purchaseDetailService.All().ToList().Where(x => x.PurMainID == pMain.PurMainID).ToList();
                        foreach (var item in pDetail)
                        {
                            tmpPurItem tmpPur = new tmpPurItem();
                            tmpPur.LocNo = pMain.LocNo;
                            tmpPur.LotNo = item.LotNo;
                            tmpPur.PurNo = pMain.PurNo;
                            tmpPur.ItemName = item.ItemCode;
                            tmpPur.OBQty = item.OBQty;
                            tmpPur.OBAmt = item.OBAmt;
                            tmpPur.PurQty = item.PurQty;
                            tmpPur.Amount = item.Amount;
                            tmpPur.SubTotal = item.Amount + item.SupTaxAmt + item.VATAmt;
                            if (item.VATType == "Exempted")
                            {
                                tmpPur.VATEx = item.VATType;
                            }
                            tmpPur.SupTaxAmt = item.SupTaxAmt;
                            tmpPur.VATAmt = item.VATAmt;
                            tmpPur.VATType = item.VATType;
                            tmpPur.FinYear = FinYear;
                            tmpPur.LoginName = LoginName;
                            _tmpPurItemService.Add(tmpPur);
                            _tmpPurItemService.Save();
                        }
                        string returnValue = "";
                        if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                        {
                            string respse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetVM_PurRegister1_6P1?TransNo=" + PurNo.ToString(), Session["token"].ToString());
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            returnValue = js.Deserialize<string>(respse);
                        }
                        transaction.Complete();
                        return Json(new { pMain = pMain, returnValue = returnValue }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetSupCodeByAddReg(string SupCode)
        {
            var subExt = _subSidiaryExtService.All().ToList().Where(s => s.SubCode == SupCode).Select(x => new { SubAddress = x.SubAddress, BIN = x.BIN, SubCategory = x.SubCategory }).FirstOrDefault();
            return Json(subExt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUpdatePur(PurchaseMain purMain, string isSave, VM_VDS_Payment VDSPayment)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var BranchCode = Session["BranchCode"].ToString();
                    var finYear = Session["FinYear"].ToString();
                    var LogName = Session["UserName"].ToString();
                    string returnValue = "";
                    if (isSave == "0")
                    {
                        RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                        if (!rUser.HasPermission("Purchase_Update"))
                        {
                            return Json("U", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                        if (!rUser.HasPermission("Purchase_Insert"))
                        {
                            return Json("X", JsonRequestBehavior.AllowGet);
                        }
                    }

                    var nRcvNo = _receiveMainService.All().Select(s => Convert.ToInt32(s.RcvNo)).LastOrDefault() + 1;
                    string rcvNo = nRcvNo.ToString();
                    rcvNo = (rcvNo == "1") ? BranchCode + "000001" : "0" + rcvNo;

                    purMain.BranchCode = BranchCode;
                    purMain.ProjCode = Session["ProjCode"].ToString();
                    purMain.FinYear = finYear;
                    if (purMain.RefDate == DateTime.MinValue || purMain.RefDate == null)
                    {
                        purMain.RefDate = DateTime.Now; //null;
                    }
                    if (purMain.LCOpenDate == DateTime.MinValue)
                    {
                        purMain.LCOpenDate = null;
                    }
                    if (purMain.B_C_Date == DateTime.MinValue)
                    {
                        purMain.B_C_Date = null;
                    }
                    if (purMain.PayDueDate == DateTime.MinValue)
                    {
                        purMain.PayDueDate = null;
                    }
                    if (purMain.RebateDueDate == DateTime.MinValue)
                    {
                        purMain.RebateDueDate = null;
                    }
                    if (GetAllTempDataList(purMain.PurNo).Count != 0)
                    {

                        var subExt = _subSidiaryExtService.All().ToList().Where(s => s.SubCode == purMain.SupCode).FirstOrDefault();
                        subExt.BIN = purMain.RegNo;
                        subExt.RegNo = purMain.RegNo;
                        subExt.RegType = purMain.RegType;
                        _subSidiaryExtService.Update(subExt);
                        _subSidiaryExtService.Save();

                        var todayDate = DateTime.Now;
                        purMain.EntryDateTime = purMain.EntryDateTime.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        if (isSave == "0")
                        {
                            #region // For update Purchase
                            var ExistRcvNo = _receiveMainService.All().Where(x => x.RefNo == purMain.PurNo).Select(s => s.RcvNo).FirstOrDefault();
                            var existMain = _purchaseMainService.All().Where(s => s.PurNo == purMain.PurNo).FirstOrDefault();
                            var pMainId = existMain.PurMainID;
                            existMain = purMain;
                            existMain.PurMainID = pMainId;
                            _purchaseMainService.Add(existMain);
                            _purchaseMainService.All().ToList().Where(y => y.PurMainID == pMainId).ToList().ForEach(x => _purchaseMainService.Delete(x));


                            #region For LOT OR AVP
                            if (Session["MaintLot"].ToString() == "True")
                            {
                                foreach (var item in GetAllTempDataList(purMain.PurNo))
                                {
                                    var purDetail = _purchaseDetailService.All().ToList().Where(m => m.PurMainID == pMainId && m.ItemCode == item.ItemName
                                        && m.LotNo == item.LotNo && m.LocNo == item.LocNo).FirstOrDefault();

                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == item.ItemName &&
                                        m.LocCode == item.LocNo && m.LotNo == item.LotNo);
                                    if (purDetail != null && currentStocks != null)
                                    {
                                        currentStocks.CurrQty = ((double)purDetail.PurQty + currentStocks.CurrQty) - (double)item.PurQty;
                                        _currentStockService.Update(currentStocks);
                                        _currentStockService.Save();
                                    }
                                    else
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty - (double)item.PurQty;
                                        _currentStockService.Update(currentStocks);
                                        _currentStockService.Save();
                                    }
                                }
                            }
                            else
                            {
                                foreach (var cItem in GetAllTempDataList(purMain.PurNo))
                                {
                                    CostLedger cLedger = new CostLedger();
                                    cLedger.RecQty = (double)cItem.PurQty;
                                    cLedger.RecRate = (double)cItem.UPrice;
                                    cLedger.RecTotal = System.Math.Round(cLedger.RecQty * cLedger.RecRate, 2);

                                    double CurrQty = 0, BalTotal = 0;
                                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == cItem.ItemName && x.LocNo == cItem.LocNo.Trim()).ToList();
                                    if (existCurrStoc.Count != 0)
                                    {
                                        var date = existCurrStoc.Max(x => x.TrDate);
                                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == cItem.ItemName && x.LocNo == cItem.LocNo.Trim() && x.TrDate == date).ToList();
                                        foreach (var item in existCurrStock)
                                        {
                                            CurrQty = item.BalQty;
                                            BalTotal = item.BalTotal;
                                        }
                                    }
                                    cLedger.IssQty = 0;
                                    cLedger.IssRate = 0;
                                    cLedger.IssTotal = 0;

                                    if (cLedger.BalQty != 0)
                                    { cLedger.BalRate = (BalTotal + cLedger.RecTotal) / cLedger.BalQty; }
                                    else { cLedger.BalRate = 0; }
                                    cLedger.BalRate = System.Math.Round(cLedger.BalRate, 2);

                                    cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                    cLedger.LocNo = cItem.LocNo;
                                    cLedger.ItemCode = cItem.ItemName;
                                    cLedger.TrDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                    cLedger.UpdSrcNo = purMain.PurNo;
                                    cLedger.UpdSrc = "AP";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                CostLedgerAppService.Save();
                            }
                            #endregion

                            _purchaseDetailService.All().ToList().Where(y => y.PurMainID == pMainId).ToList().ForEach(x => _purchaseDetailService.Delete(x));
                            _purchaseDetailService.Save();

                            if (purMain.MakeRecvAuto == "MakeRecvAuto")
                            {
                                _receiveDetailsService.All().ToList().Where(y => y.RcvNo == ExistRcvNo).ToList().ForEach(x => _receiveDetailsService.Delete(x));
                                _receiveDetailsService.Save();
                                _receiveMainService.All().ToList().Where(y => y.RcvNo == ExistRcvNo).ToList().ForEach(x => _receiveMainService.Delete(x));
                                _receiveMainService.Save();
                            }
                            ReceiveMain recvMain = new ReceiveMain();
                            double amount = 0.0;

                            foreach (var item in GetAllTempDataList(purMain.PurNo))
                            {
                                PurchaseDetail deatil = new PurchaseDetail();
                                deatil.PurMainID = pMainId;
                                deatil.ItemCode = item.ItemName;
                                deatil.LotNo = item.LotNo;
                                deatil.LocNo = item.LocNo;
                                deatil.UPrice = item.UPrice;
                                deatil.PurQty = item.PurQty;
                                deatil.Amount = item.Amount;
                                deatil.OBQty = item.OBQty;
                                deatil.OBAmt = item.OBAmt;
                                deatil.SupTaxAmt = item.SupTaxAmt;
                                deatil.Description = item.Description;
                                deatil.VATAmt = item.VATAmt;
                                deatil.VATType = item.VATType;
                                _purchaseDetailService.Add(deatil);

                                if (purMain.MakeRecvAuto == "MakeRecvAuto")
                                {
                                    ReceiveDetails receivDetails = new ReceiveDetails();
                                    receivDetails.RcvNo = rcvNo;
                                    receivDetails.ItemCode = item.ItemName;
                                    receivDetails.SubCode = purMain.SupCode;
                                    receivDetails.Description = item.Description;
                                    receivDetails.Qty = (double)item.PurQty;
                                    receivDetails.Price = (double)item.UPrice;
                                    receivDetails.ExQty = (double)item.OBQty;
                                    receivDetails.LotNo = item.LotNo;

                                    amount = amount + Convert.ToDouble((item.PurQty * item.UPrice));
                                    _receiveDetailsService.Add(receivDetails);
                                }
                            }

                            if (purMain.MakeRecvAuto == "MakeRecvAuto")
                            {
                                recvMain.Amount = amount;
                                recvMain.RcvNo = rcvNo;
                                recvMain.BranchCode = Session["BranchCode"].ToString();
                                recvMain.RcvDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.Source = "";
                                recvMain.Accode = purMain.Accode;
                                recvMain.RefNo = purMain.PurNo;
                                recvMain.RefDate = purMain.PurDate;
                                recvMain.Remarks = purMain.Remarks;
                                recvMain.RcvdByCode = "";
                                recvMain.AppByCode = purMain.EntryBy.ToString();
                                recvMain.RcvdTime = purMain.EntryDateTime;
                                recvMain.StoreLocCode = purMain.LocNo;
                                recvMain.RecvFromSubCode = purMain.SupCode;
                                recvMain.VchrNo = purMain.VchrNo;
                                recvMain.FinYear = Session["FinYear"].ToString();
                                recvMain.GLPost = false;
                                recvMain.RcvdDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.expenseStatus = false;
                                recvMain.CreditPur = false;
                                recvMain.EntrySrc = "AP";
                                recvMain.EntrySrcNo = rcvNo;
                                _receiveMainService.Add(recvMain);
                                _receiveMainService.Save();
                                _receiveDetailsService.Save();
                            }

                            _purchaseMainService.Save();
                            _purchaseDetailService.Save();
                            _currentStockService.Save();
                            _tmpPurItemService.All().ToList().Where(y => y.PurNo == purMain.PurNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tmpPurItemService.Delete(x));
                            _tmpPurItemService.Save();
                            LoadDropDown.journalVoucherRemoval("AP", purMain.PurNo, purMain.VchrNo, Session["FinYear"].ToString());
                            LoadDropDown.ProvitionPurchase("AP", "I", purMain.PurNo, purMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), purMain.PurDate.ToString("yyyy-MM-dd"), purMain.Ca_bk_Op, Session["UserName"].ToString(), purMain.JobNo);
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Journal Voucher", "Update ", purMain.PurNo, LogName);
                            transaction.Complete();
                            return Json("0", JsonRequestBehavior.AllowGet);
                            #endregion
                        }
                        else
                        {
                            #region// For Save Purchase
                            var PID = _purchaseMainService.All().LastOrDefault();
                            if (PID == null)
                            {
                                purMain.PurMainID = 1;
                            }
                            else
                            {
                                purMain.PurMainID = PID.PurMainID + 1;
                            }
                            _purchaseMainService.Add(purMain);
                            List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                            List<VM_PurchaseVM> VMPurReg1List = new List<VM_PurchaseVM>();
                            ReceiveMain recvMain = new ReceiveMain();
                            double amount = 0.0;
                            foreach (var item in GetAllTempDataList(purMain.PurNo))
                            {
                                PurchaseDetail deatil = new PurchaseDetail();
                                deatil.PurMainID = purMain.PurMainID;
                                deatil.ItemCode = item.ItemName;
                                deatil.LotNo = item.LotNo;
                                deatil.LocNo = item.LocNo;
                                deatil.UPrice = item.UPrice;
                                deatil.PurQty = item.PurQty;
                                deatil.Amount = item.Amount;
                                deatil.OBQty = item.OBQty;
                                deatil.OBAmt = item.OBAmt;
                                deatil.SupTaxAmt = item.SupTaxAmt;
                                deatil.Description = item.Description;
                                deatil.VATAmt = item.VATAmt;
                                deatil.VATType = item.VATType;
                                _purchaseDetailService.Add(deatil);

                                if (purMain.MakeRecvAuto == "MakeRecvAuto")
                                {
                                    ReceiveDetails receivDetails = new ReceiveDetails();
                                    receivDetails.RcvNo = rcvNo;
                                    receivDetails.ItemCode = item.ItemName;
                                    receivDetails.SubCode = purMain.SupCode;
                                    receivDetails.Description = item.Description;
                                    receivDetails.Qty = (double)item.PurQty;
                                    receivDetails.Price = (double)item.UPrice;
                                    receivDetails.ExQty = (double)item.OBQty;
                                    receivDetails.BadQty = 0;
                                    receivDetails.LotNo = item.LotNo;

                                    amount = amount + Convert.ToDouble((item.PurQty * item.UPrice));
                                    receveDetailsList.Add(receivDetails);
                                    //_receiveDetailsService.Add(receivDetails);
                                    //_receiveDetailsService.Save();
                                }

                                #region For VAT VM_PurRegister1_6P1
                                if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                                {
                                    VM_PurchaseVM VMPurReg1 = new VM_PurchaseVM();
                                    VMPurReg1.SerialNo = 0;
                                    VMPurReg1.PurRegID = purMain.PurMainID;
                                    VMPurReg1.PurDate = purMain.PurDate;
                                    VMPurReg1.OBQty = item.OBQty;
                                    VMPurReg1.OBValue = item.OBAmt;
                                    VMPurReg1.Ch_BENO = purMain.B_C_No;
                                    VMPurReg1.Ch_BEDate = purMain.B_C_Date;
                                    VMPurReg1.SuppCode = purMain.SupCode;
                                    VMPurReg1.SuppName = _subsidiaryInfoService.All().Where(s => s.SubCode == purMain.SupCode).Select(s => s.SubName).FirstOrDefault();
                                    VMPurReg1.SuppAddr = _subSidiaryExtService.All().Where(s => s.SubCode == purMain.SupCode).Select(s => s.SubAddress).FirstOrDefault();
                                    VMPurReg1.R_E_N_No = _subSidiaryExtService.All().Where(s => s.SubCode == purMain.SupCode).Select(s => s.RegNo).FirstOrDefault();
                                    VMPurReg1.ItemName = _itemInfoService.All().Where(s => s.ItemCode == item.ItemName).Select(s => s.ItemName).FirstOrDefault();
                                    VMPurReg1.PurQty = item.PurQty;
                                    VMPurReg1.PurValue = item.UPrice;
                                    VMPurReg1.SuppTax = item.SupTaxAmt;
                                    VMPurReg1.VATAmt = item.VATAmt;
                                    VMPurReg1.Remarks = purMain.Remarks;
                                    VMPurReg1.TranNo = purMain.PurNo;
                                    VMPurReg1.TranType = "Purchase";
                                    VMPurReg1.ItemCode = item.ItemName;
                                    VMPurReg1.VATType = item.VATType;
                                    VMPurReg1.PurType = purMain.PurType;
                                    VMPurReg1.SDRate = (decimal)item.SDRate;
                                    VMPurReg1.VATRate = (decimal)item.VATRate;
                                    VMPurReg1List.Add(VMPurReg1);
                                }
                                #endregion

                            }


                            if (purMain.MakeRecvAuto == "MakeRecvAuto")
                            {
                                recvMain.Amount = amount;
                                recvMain.RcvNo = rcvNo;
                                recvMain.BranchCode = Session["BranchCode"].ToString();
                                recvMain.RcvDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.Accode = purMain.Accode;
                                recvMain.RefNo = purMain.PurNo;
                                recvMain.RefDate = (DateTime)purMain.PurDate;
                                recvMain.Remarks = purMain.Remarks;
                                recvMain.AppByCode = purMain.EntryBy.ToString();
                                recvMain.RcvdTime = purMain.EntryDateTime;
                                recvMain.StoreLocCode = purMain.LocNo;
                                recvMain.RecvFromSubCode = purMain.SupCode;
                                recvMain.VchrNo = purMain.VchrNo;
                                recvMain.FinYear = Session["FinYear"].ToString();
                                recvMain.GLPost = false;
                                recvMain.RcvdDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                recvMain.expenseStatus = false;
                                recvMain.CreditPur = false;
                                recvMain.Source = "";
                                recvMain.RcvDetails = receveDetailsList;
                                recvMain.EntrySrc = "AP";
                                recvMain.EntrySrcNo = rcvNo;
                                _receiveMainService.Add(recvMain);
                                _receiveMainService.Save();
                                //_receiveDetailsService.Save();

                            }

                            #region For LOT OR AVP
                            if (Session["MaintLot"].ToString() == "True")
                            {
                                foreach (var currentItem in GetAllTempDataList(purMain.PurNo))
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemName &&
                                        m.LocCode == currentItem.LocNo && m.LotNo == currentItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = (double)currentItem.PurQty + currentStocks.CurrQty;
                                        currentStocks.UnitPrice = (double)currentItem.UPrice;
                                        _currentStockService.Update(currentStocks);
                                    }
                                    else
                                    {
                                        CurrentStock currStock = new CurrentStock();
                                        currStock.LocCode = purMain.LocNo;
                                        currStock.LotNo = currentItem.LotNo;
                                        currStock.ItemCode = currentItem.ItemName;
                                        currStock.CurrQty = (double)currentItem.PurQty;
                                        currStock.UnitPrice = (double)currentItem.UPrice;
                                        _currentStockService.Add(currStock);
                                    }

                                    var LotDt = _lotDTService.All().ToList().Where(m => m.LotNo == currentItem.LotNo).FirstOrDefault();
                                    if (LotDt == null)
                                    {
                                        LotDT lotDt = new LotDT();
                                        lotDt.LotNo = currentItem.LotNo;
                                        lotDt.LotDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                        lotDt.RefNo = purMain.PurNo;
                                        lotDt.RefSource = "Purchase";
                                        _lotDTService.Add(lotDt);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var cItem in GetAllTempDataList(purMain.PurNo))
                                {
                                    CostLedger cLedger = new CostLedger();
                                    cLedger.RecQty = (double)cItem.PurQty;
                                    cLedger.RecRate = (double)cItem.UPrice;
                                    cLedger.RecTotal = System.Math.Round(cLedger.RecQty * cLedger.RecRate, 2);

                                    double CurrQty = 0, BalTotal = 0;
                                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == cItem.ItemName && x.LocNo == cItem.LocNo.Trim()).ToList();
                                    if (existCurrStoc.Count != 0)
                                    {
                                        var date = existCurrStoc.Max(x => x.TrDate);
                                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == cItem.ItemName && x.LocNo == cItem.LocNo.Trim() && x.TrDate == date).ToList();
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

                                    cLedger.LocNo = cItem.LocNo;
                                    cLedger.ItemCode = cItem.ItemName;
                                    cLedger.TrDate = purMain.PurDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                    cLedger.UpdSrcNo = purMain.PurNo;
                                    cLedger.UpdSrc = "AP";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                CostLedgerAppService.Save();
                            }


                            #endregion

                            _purchaseMainService.Save();
                            _purchaseDetailService.Save();
                            _currentStockService.Save();
                            _tmpPurItemService.All().ToList().Where(y => y.PurNo == purMain.PurNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tmpPurItemService.Delete(x));
                            _tmpPurItemService.Save();

                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Purchase", "Save", purMain.PurNo, LogName);
                            LoadDropDown.ProvitionPurchase("AP", "I", purMain.PurNo, purMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), purMain.PurDate.ToString("yyyy-MM-dd"), purMain.Ca_bk_Op, Session["UserName"].ToString(), purMain.JobNo);
                            //For VAT api 
                            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                            {
                                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                                string json = JsonConvert.SerializeObject(VMPurReg1List, Formatting.Indented, serializerSettings);
                                var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_PurRegister1_6P1", VMPurReg1List).Result;
                                returnValue = respse.Content.ReadAsAsync<string>().Result;

                                //For VDS payment
                                if (VDSPayment != null)
                                {
                                    HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_VDS_Payment/PostVM_VDS_Payment", VDSPayment).Result;
                                    var content = response.StatusCode.ToString();
                                    if (content != "OK")
                                    {
                                        //return Exception;
                                    }
                                }                               

                            }
                            transaction.Complete();
                            return Json(new { Msg = "1", returnValue = returnValue }, JsonRequestBehavior.AllowGet);
                            #endregion
                        }
                    }
                    else
                    {
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult SaveTempPurTbl(tmpPurItem addOnTempPurTbl)
        {
            try
            {
                if (addOnTempPurTbl.tPurId == 0)
                {
                    addOnTempPurTbl.FinYear = Session["FinYear"].ToString();
                    addOnTempPurTbl.LoginName = Session["UserName"].ToString();
                    var existTmpPurtbl = _tmpPurItemService.All().ToList().FirstOrDefault(x => x.ItemName == addOnTempPurTbl.ItemName && x.PurNo == addOnTempPurTbl.PurNo && x.LotNo == addOnTempPurTbl.LotNo);
                    if (existTmpPurtbl == null)
                    {
                        _tmpPurItemService.Add(addOnTempPurTbl);
                    }
                    else
                    {
                        _tmpPurItemService.Delete(existTmpPurtbl);
                        _tmpPurItemService.Save();

                        _tmpPurItemService.Add(addOnTempPurTbl);
                    }
                    _tmpPurItemService.Save();
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _tmpPurItemService.Update(addOnTempPurTbl);
                    _tmpPurItemService.Save();
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetTempPurTbl(int tPurId)
        {
            try
            {
                var tpurTbl = _tmpPurItemService.All().ToList().FirstOrDefault(x => x.tPurId == tPurId);
                return Json(tpurTbl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DelTempTbl(int Id)
        {
            var isExist = _tmpPurItemService.All().ToList().FirstOrDefault(x => x.tPurId == Id);
            if (isExist != null)
            {
                _tmpPurItemService.Delete(isExist);
                _tmpPurItemService.Save();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetNewPurNo()
        {
            return Json(GeneratePurNo(Session["BranchCode"].ToString()), JsonRequestBehavior.AllowGet);
        }
        public string GeneratePurNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string purNo = "";
            var rcvNo = _purchaseMainService.All().ToList().LastOrDefault(x => x.BranchCode.Trim() == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var recvNo = _purchaseMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    purNo = "00" + (Convert.ToInt64(recvNo.PurNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    purNo = "00000001";
                }
            }
            else
            {
                if (rcvNo != null)
                {
                    purNo = branchCode + (Convert.ToInt64(rcvNo.PurNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    purNo = branchCode + "000001";
                }

            }

            return purNo;

        }

        public SelectList LoadIssueBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Purchase";
            string functionName = "Entry By";
            string LogName = Session["UserName"].ToString();
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "','" + form + "', '" + LogName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }

        public ActionResult GetJournalVoucher(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Purchase_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("Purchase", "Purchase", new { errMsg });
            }
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "' "
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac");

            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                return RedirectToAction("Purchase", "Purchase", new { errMsg });

            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Purchase_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("Purchase", "Purchase", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "', '" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("Purchase", "Purchase", new { errMsg });
            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGlECountN(string VType, DateTime TrDate)
        {
            return Json(CountEntries(VType, TrDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGlECount(string VType)
        {
            return Json(Count(VType), JsonRequestBehavior.AllowGet);
        }

        public string CountEntries(string pageType, DateTime TrDate)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'and pvm.VDate='" + TrDate.ToString("MM/dd/yyyy") + "'");
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

        public string Count(string pageType)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'");
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