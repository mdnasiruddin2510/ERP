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

namespace AcclineERP.Controllers
{
    public class StoreTransferController : Controller
    {
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly ILocationAppService _locationInfoService;
        private readonly INewChartAppService _newChartService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IIssueDetailsAppService _issueDetailsService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly ILotDTAppService _lotDtService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IBranchAppService _branchService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IUnitAppService _unitService;
        private readonly ICostLedgerAppService CostLedgerAppService;

        public StoreTransferController(ILocationAppService _locationInfoService,
            INewChartAppService _newChartService, IItemInfoAppService _itemInfoService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IEmployeeAppService _employeeInfoService,
            IReceiveDetailsAppService _receiveDetailsService, ICurrentStockAppService _currentStockService,
            IIssRecSrcDestAppService _issRecvSrcDestService, IReceiveAppService _receiveMainService,
            IIssueMainAppService _issueMainService, ITransactionLogAppService _transactionLogService,
            IIssueDetailsAppService _issueDetailsService, ILotDTAppService _lotDtService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IBranchAppService _branchService, ICommonPVVMAppService _CommonVmService,
            IItemTypeAppService _itemTypeService, IUnitAppService _unitService, ICostLedgerAppService CostLedgerAppService)
        {
            this._lotDtService = _lotDtService;
            this._locationInfoService = _locationInfoService;
            this._newChartService = _newChartService;
            this._itemInfoService = _itemInfoService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._receiveDetailsService = _receiveDetailsService;
            this._currentStockService = _currentStockService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._receiveMainService = _receiveMainService;
            this._issueMainService = _issueMainService;
            this._transactionLogService = _transactionLogService;
            this._issueDetailsService = _issueDetailsService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._branchService = _branchService;
            this._CommonVmService = _CommonVmService;
            this._itemTypeService = _itemTypeService;
            this._unitService = _unitService;
            this.CostLedgerAppService = CostLedgerAppService;
        }
        public ActionResult StoreTransfer()
        {
            if (Session["UserID"] != null)
            {
                string StoreType = "";
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                ViewBag.ReceiveNo = GenerateRecvNo(branchCode);
                ViewBag.IssueNo = GenerateIssueNo(branchCode);
                //ViewBag.VchrNo = GenerateVoucherNo(DateTime.Now);
                DateTime datetime = DateTime.Now;
                ViewBag.Time = datetime.ToString("HH:mm:ss");
                ViewBag.FromLocCode = LoadStoreLocation(StoreType, _locationInfoService);
                ViewBag.ToLocCode = LoadDropDown.LoadAllLocation(StoreType, _locationInfoService);
                ViewBag.ItemCode = LoadDropDown.LoadItemStore(_itemInfoService);
                //ViewBag.IssueAppByCode = LoadIssueAppBy(_employeeInfoService);
                ViewBag.IssueByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");// LoadRecvBy(StoreType, _employeeInfoService);
                ViewBag.AppByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName"); //LoadAppBy(_employeeInfoService);
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");

                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintLot = sysSet.MaintLot;
                Session["MaintVAT"] = sysSet.MaintVAT;
                ViewBag.MaintVAT = sysSet.MaintVAT;
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }


        #region Generate Voucher No
        public ActionResult GetNewVoucherNo(string VType)
        {
            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            //string VLen = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sql = string.Format("exec [GetNewVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sql).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExistVoucherNo(string VType, DateTime TrDate)
        {
            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sqls = string.Format("exec [GetExistVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + TrDate + "','" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sqls).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult GetIssueRcvBy(string IssueByCode)
        {
            if (IssueByCode == "2")
            {

                var items = _employeeInfoService.All().ToList()
                .Select(x => new { x.Id, x.UserName }).ToList();
                items.Insert(0, new { Id = 0, UserName = "---- Select ----" });
                return Json(new SelectList(items, "Id", "UserName"), JsonRequestBehavior.AllowGet);

                // return Json(items, JsonRequestBehavior.AllowGet);

                //var x = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");
                // return Json(x, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var items = _employeeInfoService.All().ToList()
             .Select(x => new { x.Id, x.UserName }).ToList();
                items.Insert(0, new { Id = 0, UserName = "---- Select ----" });
                return Json(new SelectList(items, "Id", "UserName"), JsonRequestBehavior.AllowGet);


            }
        }
        public ActionResult GetFromLoctionToRecv(string ToLocCode)
        {
            return Json(LoadStoreLocation(ToLocCode, _locationInfoService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetToLoctionToRecv(string StoreType)
        {
            return Json(LoadDropDown.LoadAllLocation(StoreType, _locationInfoService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFromLoctionToIssue(string ToLocCode)
        {
            return Json(LoadDropDown.LoadAllLocation(ToLocCode, _locationInfoService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetToLoctionToIssue(string StoreType)
        {
            return Json(LoadStoreLocation(StoreType, _locationInfoService), JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveStoreTransfer(StoreTransferVM storeTreansferInfo, List<ReceiveDetails> recvDetails, List<IssueDetails> issueDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("StoreTransfer_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var todayDate = DateTime.Now;
                    bool IsLotMaintain = false;
                    bool IsAutoLot = true;
                    #region Store receive
                    if (storeTreansferInfo.StoreType == "2")
                    {
                        ReceiveMain recvMain = new ReceiveMain();
                        recvMain.RcvNo = storeTreansferInfo.ReceiveNo;
                        recvMain.BranchCode = Session["BranchCode"].ToString();
                        recvMain.RcvDate = storeTreansferInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.FromLocCode = storeTreansferInfo.FromLocCode;
                        recvMain.ToLocCode = storeTreansferInfo.ToLocCode;
                        //recvMain.Accode = storeTreansferInfo.;
                        recvMain.RefNo = storeTreansferInfo.RefNo;
                        recvMain.RefDate = storeTreansferInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.Remarks = storeTreansferInfo.Remarks;
                        recvMain.RcvdByCode = storeTreansferInfo.ReceiveByCode;
                        recvMain.AppByCode = storeTreansferInfo.AppByCode;
                        recvMain.RcvdTime = storeTreansferInfo.Time;
                        recvMain.VchrNo = storeTreansferInfo.VchrNo;
                        recvMain.FinYear = Session["FinYear"].ToString();
                        recvMain.GLPost = false;
                        recvMain.RcvdDate = storeTreansferInfo.AppDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.expenseStatus = false;
                        recvMain.EntrySrc = "ST";
                        recvMain.EntrySrcNo = storeTreansferInfo.ReceiveNo;
                        //double amount = 0.0;
                        double crQty = 0.0;
                        string lotNo = "";
                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();

                        if (storeTreansferInfo.AppByCode != "" || storeTreansferInfo.AppByCode != "---- Select ----")
                        {
                            if (Session["MaintLot"].ToString() == "True")
                            {
                                #region LOT
                                if (IsLotMaintain == true)
                                {
                                    if (IsAutoLot == true)
                                    {
                                        #region auto lot
                                        foreach (var currentItem in currentStock)
                                        {
                                            crQty = currentItem.CurrQty;
                                            lotNo = currentItem.LotNo;

                                            List<LotDT> itemLots;
                                            double Qty = 0.0;
                                            var _currStockSrc = (from old in _currentStockService.All()
                                                                 join newID in recvDetails
                                                                 on old.ItemCode equals newID.ItemCode
                                                                 where old.CurrQty > 0
                                                                 select old).ToList();
                                            CurrentStock currStock = new CurrentStock();
                                            List<CurrentStock> CurrStockItems = new List<CurrentStock>();
                                            foreach (var item in recvDetails)
                                            {
                                                itemLots = new List<LotDT>();

                                                Qty = item.Qty;
                                                CurrStockItems = _currStockSrc.Where(i => i.ItemCode == item.ItemCode).ToList();
                                                foreach (var itemct in CurrStockItems)
                                                {
                                                    var lotdt = _lotDtService.All().FirstOrDefault(i => i.LotNo == itemct.LotNo);
                                                    itemLots.Add(lotdt);
                                                }
                                                itemLots = itemLots.OrderBy(d => d.LotDate).ToList();
                                                foreach (var lot in itemLots)
                                                {
                                                    currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == "900" && m.LotNo == lot.LotNo);
                                                    if (currStock == null)
                                                    { continue; }
                                                    var Lotwisestockqty = currStock.CurrQty;
                                                    if (Lotwisestockqty > Qty && Qty > 0)
                                                    {
                                                        currStock.CurrQty = currStock.CurrQty - Qty;
                                                        currStock.UnitPrice = currStock.UnitPrice;
                                                        _currentStockService.Update(currStock);
                                                        _currentStockService.Save();

                                                        var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                        m.LocCode == storeTreansferInfo.ToLocCode && m.LotNo == currentItem.LotNo);

                                                        if (currentStocks != null)
                                                        {
                                                            currentStocks.CurrQty = currentStocks.CurrQty + Qty;
                                                            currentStocks.UnitPrice = currentItem.UnitPrice;
                                                            _currentStockService.Update(currentStocks);
                                                            _currentStockService.Save();
                                                        }
                                                        else
                                                        {
                                                            CurrentStock currtStock = new CurrentStock();
                                                            currtStock.LocCode = storeTreansferInfo.ToLocCode;
                                                            currtStock.LotNo = lot.LotNo;
                                                            currtStock.ItemCode = currentItem.ItemCode;
                                                            currtStock.CurrQty = Qty;
                                                            currtStock.UnitPrice = storeTreansferInfo.Price;
                                                            _currentStockService.Add(currtStock);
                                                            _currentStockService.Save();

                                                        }

                                                        ReceiveDetails receivDetails = new ReceiveDetails();
                                                        receivDetails.RcvNo = storeTreansferInfo.ReceiveNo;
                                                        receivDetails.ItemCode = item.ItemCode;
                                                        receivDetails.Description = item.Description;
                                                        receivDetails.Qty = Qty;
                                                        receivDetails.Price = item.Price;//recvDetailsItem.Qty * recvDetailsItem.Price;
                                                        receivDetails.ExQty = Lotwisestockqty;
                                                        receivDetails.LotNo = lot.LotNo;
                                                        //amount = amount + (Qty * item.Price);
                                                        receveDetailsList.Add(receivDetails);

                                                        Qty -= Qty;
                                                        break;
                                                    }
                                                    else if (Qty > 0 && Lotwisestockqty <= Qty && Lotwisestockqty > 0)
                                                    {
                                                        currStock.CurrQty = currStock.CurrQty - Lotwisestockqty;
                                                        currStock.UnitPrice = currStock.UnitPrice;
                                                        _currentStockService.Update(currStock);
                                                        _currentStockService.Save();

                                                        var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                        m.LocCode == storeTreansferInfo.ToLocCode && m.LotNo == currentItem.LotNo);

                                                        if (currentStocks != null)
                                                        {
                                                            currentStocks.CurrQty = currentStocks.CurrQty + Lotwisestockqty;
                                                            currentStocks.UnitPrice = currentItem.UnitPrice;
                                                            _currentStockService.Update(currentStocks);
                                                            _currentStockService.Save();
                                                        }
                                                        else
                                                        {
                                                            CurrentStock currtStock = new CurrentStock();
                                                            currtStock.LocCode = storeTreansferInfo.ToLocCode;
                                                            currtStock.LotNo = lot.LotNo;
                                                            currtStock.ItemCode = currentItem.ItemCode;
                                                            currtStock.CurrQty = Lotwisestockqty;
                                                            currtStock.UnitPrice = storeTreansferInfo.Price;
                                                            _currentStockService.Add(currtStock);
                                                            _currentStockService.Save();

                                                        }

                                                        ReceiveDetails receivDetails = new ReceiveDetails();
                                                        receivDetails.RcvNo = storeTreansferInfo.ReceiveNo;
                                                        receivDetails.ItemCode = item.ItemCode;
                                                        receivDetails.Description = item.Description;
                                                        receivDetails.Qty = Lotwisestockqty;
                                                        receivDetails.Price = item.Price;//recvDetailsItem.Qty * recvDetailsItem.Price;
                                                        receivDetails.ExQty = Lotwisestockqty;
                                                        receivDetails.LotNo = lot.LotNo;
                                                        //amount = amount + (Lotwisestockqty * item.Price);
                                                        receveDetailsList.Add(receivDetails);

                                                        Qty -= (int)Lotwisestockqty;
                                                    }
                                                    else if (Qty == 0)
                                                    {
                                                        break;
                                                    }

                                                }

                                            }



                                        }
                                        #endregion auto lot
                                    }
                                    else
                                    {
                                        #region not auto lot
                                        foreach (var currentItem in currentStock)
                                        {
                                            crQty = currentItem.CurrQty;
                                            lotNo = currentItem.LotNo;

                                            var currentStockInTransits = _currentStockService.All().ToList()
                                                .FirstOrDefault(m => m.ItemCode == currentItem.ItemCode && m.LocCode == "900" && m.LotNo == currentItem.LotNo);

                                            var currentStocksPrice = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                               m.LocCode == storeTreansferInfo.ToLocCode &&
                                               m.LotNo == currentItem.LotNo).UnitPrice;

                                            if (currentStockInTransits != null)
                                            {
                                                if (currentStockInTransits.CurrQty >= crQty)
                                                {
                                                    currentStockInTransits.CurrQty = currentStockInTransits.CurrQty - crQty;
                                                    currentStockInTransits.UnitPrice = currentStockInTransits.UnitPrice;
                                                    _currentStockService.Update(currentStockInTransits);
                                                    _currentStockService.Save();
                                                }
                                                else
                                                {
                                                    transaction.Dispose();
                                                    return Json("0", JsonRequestBehavior.AllowGet);
                                                }


                                                if (currentItem.LotNo == null || currentItem.LotNo != "01")
                                                {
                                                    var curStck = _currentStockService.All().ToList().LastOrDefault(x => x.LocCode == storeTreansferInfo.FromLocCode);
                                                    currentItem.LotNo = curStck.LotNo;
                                                }
                                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                    m.LocCode == storeTreansferInfo.ToLocCode &&
                                                    m.LotNo == currentItem.LotNo);

                                                if (currentStocks != null)
                                                {
                                                    currentStocks.CurrQty = currentStocks.CurrQty + crQty;
                                                    currentStocks.UnitPrice = currentStocks.UnitPrice;
                                                    _currentStockService.Update(currentStocks);
                                                    _currentStockService.Save();
                                                }
                                                else
                                                {
                                                    CurrentStock currtStock = new CurrentStock();
                                                    currtStock.LocCode = storeTreansferInfo.ToLocCode;
                                                    currtStock.LotNo = lotNo;
                                                    currtStock.ItemCode = currentItem.ItemCode;
                                                    currtStock.CurrQty = crQty;
                                                    currtStock.UnitPrice = currentStocksPrice;
                                                    _currentStockService.Add(currtStock);
                                                    _currentStockService.Save();

                                                }

                                            }

                                        }
                                        foreach (var recvDetailsItem in recvDetails)
                                        {
                                            ReceiveDetails receivDetails = new ReceiveDetails();
                                            receivDetails.RcvNo = storeTreansferInfo.ReceiveNo;
                                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                            receivDetails.Description = recvDetailsItem.Description;
                                            receivDetails.Qty = recvDetailsItem.Qty;
                                            receivDetails.Price = recvDetailsItem.Price;//recvDetailsItem.Qty * recvDetailsItem.Price;
                                            receivDetails.ExQty = recvDetailsItem.ExQty;
                                            receivDetails.LotNo = recvDetailsItem.LotNo;
                                            //amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                            receveDetailsList.Add(receivDetails);


                                        }
                                        #endregion not auto lot
                                    }
                                }
                                else
                                {
                                    #region not lot
                                    foreach (var currentItem in currentStock)
                                    {
                                        crQty = currentItem.CurrQty;
                                        lotNo = "01";

                                        var currentStockInTransits = _currentStockService.All().ToList()
                                            .FirstOrDefault(m => m.ItemCode == currentItem.ItemCode && m.LocCode == "900" && m.LotNo == "01");

                                        if (currentStockInTransits != null)
                                        {
                                            if (currentStockInTransits.CurrQty >= crQty)
                                            {
                                                currentStockInTransits.CurrQty = currentStockInTransits.CurrQty - crQty;
                                                currentStockInTransits.UnitPrice = currentStockInTransits.UnitPrice;
                                                _currentStockService.Update(currentStockInTransits);
                                                _currentStockService.Save();
                                            }
                                            else
                                            {
                                                transaction.Dispose();
                                                return Json("0", JsonRequestBehavior.AllowGet);
                                            }


                                            if (currentItem.LotNo == null || currentItem.LotNo != "01")
                                            {
                                                var curStck = _currentStockService.All().ToList().LastOrDefault(x => x.LocCode == storeTreansferInfo.FromLocCode);
                                                currentItem.LotNo = curStck.LotNo;
                                            }
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                m.LocCode == storeTreansferInfo.ToLocCode &&
                                                m.LotNo == "01");

                                            if (currentStocks != null)
                                            {
                                                currentStocks.CurrQty = currentStocks.CurrQty + crQty;
                                                currentStocks.UnitPrice = currentStockInTransits.UnitPrice;
                                                _currentStockService.Update(currentStocks);
                                                _currentStockService.Save();
                                            }
                                            else
                                            {
                                                CurrentStock currtStock = new CurrentStock();
                                                currtStock.LocCode = storeTreansferInfo.ToLocCode;
                                                currtStock.LotNo = lotNo;
                                                currtStock.ItemCode = currentItem.ItemCode;
                                                currtStock.CurrQty = crQty;
                                                currtStock.UnitPrice = currentStockInTransits.UnitPrice;
                                                _currentStockService.Add(currtStock);
                                                _currentStockService.Save();

                                            }

                                        }

                                    }
                                    foreach (var recvDetailsItem in recvDetails)
                                    {
                                        ReceiveDetails receivDetails = new ReceiveDetails();
                                        receivDetails.RcvNo = storeTreansferInfo.ReceiveNo;
                                        receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                        receivDetails.Description = recvDetailsItem.Description;
                                        receivDetails.Qty = recvDetailsItem.Qty;
                                        receivDetails.Price = recvDetailsItem.Price;//recvDetailsItem.Qty * recvDetailsItem.Price;
                                        receivDetails.ExQty = recvDetailsItem.ExQty;
                                        receivDetails.LotNo = "01";
                                        //amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                        receveDetailsList.Add(receivDetails);

                                        //crQty = recvDetailsItem.Qty;
                                        //lotNo = recvDetailsItem.LotNo;

                                    }
                                    #endregion not lot
                                }
                                #endregion
                            }
                            else
                            {
                                #region AVP
                                foreach (var cItem in currentStock)
                                {
                                    CostLedger cLedger = new CostLedger();
                                    cLedger.RecQty = cItem.CurrQty;
                                    cLedger.RecRate = cItem.UnitPrice;
                                    cLedger.RecTotal = System.Math.Round(cLedger.RecQty * cLedger.RecRate, 2);

                                    double CurrQty = 0, BalTotal = 0;
                                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == cItem.ItemCode && x.LocNo == recvMain.ToLocCode.Trim()).ToList();
                                    if (existCurrStoc.Count != 0)
                                    {
                                        var date = existCurrStoc.Max(x => x.TrDate);
                                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == cItem.ItemCode && x.LocNo == recvMain.ToLocCode.Trim() && x.TrDate == date).ToList();
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

                                    cLedger.LocNo = recvMain.ToLocCode;
                                    cLedger.ItemCode = cItem.ItemCode;
                                    cLedger.TrDate = storeTreansferInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = storeTreansferInfo.ReceiveNo;
                                    cLedger.UpdSrc = "ST";
                                    CostLedgerAppService.Add(cLedger);
                                    CostLedgerAppService.Save();
                                    // For store in transit
                                    cLedger.LocNo = "900";
                                    cLedger.RecQty = 0;
                                    cLedger.RecRate = 0;
                                    cLedger.RecTotal = 0;
                                    cLedger.BalQty = 0;
                                    cLedger.BalRate = 0;
                                    cLedger.BalTotal = 0;
                                    CostLedgerAppService.Add(cLedger);
                                    CostLedgerAppService.Save();
                                }

                                foreach (var recvDetailsItem in recvDetails)
                                {
                                    ReceiveDetails receivDetails = new ReceiveDetails();
                                    receivDetails.RcvNo = storeTreansferInfo.ReceiveNo;
                                    receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                    receivDetails.Description = recvDetailsItem.Description;
                                    receivDetails.Qty = recvDetailsItem.Qty;
                                    receivDetails.Price = recvDetailsItem.Price;
                                    receivDetails.ExQty = recvDetailsItem.ExQty;
                                    receivDetails.LotNo = "01";
                                    receveDetailsList.Add(receivDetails);

                                }
                                #endregion
                            }

                            recvMain.Amount = (double)storeTreansferInfo.TotAmt;
                            recvMain.RcvDetails = receveDetailsList;
                            _receiveMainService.Add(recvMain);
                            _receiveMainService.Save();


                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Store Transfer", "Receive", recvMain.RcvNo, Session["UserName"].ToString());

                            //bool isStor = true;
                            //var isExist = _issueMainService.All().ToList().FirstOrDefault(x => x.FromLocCode == storeTreansferInfo.FromLocCode && x.ToLocCode == storeTreansferInfo.ToLocCode && x.IsReceived == false );
                            //isExist.IsReceived = true;
                            //_issueMainService.Update(isExist);
                            transaction.Complete();
                            return Json(new { Msg = "1" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            recvMain.RcvDetails = receveDetailsList;
                            _receiveMainService.Add(recvMain);
                            _receiveMainService.Save();
                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                    #endregion

                    #region Store issue
                    else
                    {
                        try
                        {
                            IssueMain issueInfo = new IssueMain();
                            issueInfo.IssueNo = storeTreansferInfo.IssueNo;
                            issueInfo.BranchCode = Session["BranchCode"].ToString();
                            issueInfo.IssueDate = storeTreansferInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.FromLocCode = storeTreansferInfo.FromLocCode;
                            issueInfo.ToLocCode = storeTreansferInfo.ToLocCode;
                            issueInfo.RefNo = storeTreansferInfo.RefNo;
                            issueInfo.RefDate = storeTreansferInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.Remarks = storeTreansferInfo.Remarks;
                            issueInfo.IssueByCode = storeTreansferInfo.IssueAppByCode;
                            issueInfo.AppByCode = storeTreansferInfo.AppByCode;
                            issueInfo.IssTime = storeTreansferInfo.Time;
                            issueInfo.FinYear = Session["FinYear"].ToString();
                            issueInfo.GLPost = false;
                            issueInfo.IssDate = storeTreansferInfo.AppDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.cashReceiptStatus = false;
                            issueInfo.IsReceived = false;
                            issueInfo.VchrNo = storeTreansferInfo.VchrNo;
                            issueInfo.Amount = (double)storeTreansferInfo.TotAmt;
                            issueInfo.EntrySrc = "ST";
                            issueInfo.EntrySrcNo = storeTreansferInfo.IssueNo;
                            List<IssueDetails> issuDetailsList = new List<IssueDetails>();
                            List<VM_6p5> VM6p5List = new List<VM_6p5>();
                            if (issueInfo.AppByCode != "" || issueInfo.AppByCode != "---- Select ----")
                            {
                                #region For LOT OR AVP
                                if (Session["MaintLot"].ToString() == "True")
                                {
                                    if (IsAutoLot == true)
                                    {
                                        #region AutoLot by issueDetail

                                        foreach (var item in issueDetails)
                                        {

                                            List<LotDT> itemLots;
                                            double Qty = 0.0;
                                            var _currStockSrc = (from old in _currentStockService.All()
                                                                 join newID in issueDetails
                                                                 on old.ItemCode equals newID.ItemCode
                                                                 where old.CurrQty > 0
                                                                 select old).ToList();
                                            CurrentStock currStock = new CurrentStock();
                                            List<CurrentStock> CurrStockItems = new List<CurrentStock>();

                                            itemLots = new List<LotDT>();

                                            Qty = item.Qty;
                                            CurrStockItems = _currStockSrc.Where(i => i.ItemCode == item.ItemCode).ToList();
                                            foreach (var itemct in CurrStockItems)
                                            {
                                                var lotdt = _lotDtService.All().FirstOrDefault(i => i.LotNo == itemct.LotNo);
                                                itemLots.Add(lotdt);
                                            }
                                            itemLots = itemLots.OrderBy(d => d.LotDate).ToList();
                                            foreach (var lot in itemLots)
                                            {
                                                currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == storeTreansferInfo.FromLocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
                                                if (currStock == null)
                                                { continue; }
                                                var Lotwisestockqty = currStock.CurrQty;
                                                if (Lotwisestockqty > Qty && Qty > 0)
                                                {
                                                    CurrentStock currtStock = new CurrentStock();
                                                    currtStock.LocCode = "900";
                                                    currtStock.LotNo = currStock.LotNo;
                                                    currtStock.ItemCode = currStock.ItemCode;
                                                    currtStock.CurrQty = Qty;
                                                    currtStock.UnitPrice = currStock.UnitPrice;
                                                    _currentStockService.Add(currtStock);

                                                    IssueDetails issueDetailsInfo = new IssueDetails();
                                                    issueDetailsInfo.IssueNo = item.IssueNo;
                                                    issueDetailsInfo.ItemCode = item.ItemCode;
                                                    issueDetailsInfo.SubCode = item.SubCode;
                                                    issueDetailsInfo.Description = item.Description;
                                                    issueDetailsInfo.Qty = Qty;
                                                    issueDetailsInfo.Price = item.Price;//
                                                    issueDetailsInfo.ExQty = Lotwisestockqty;
                                                    issueDetailsInfo.LotNo = lot.LotNo;
                                                    issuDetailsList.Add(issueDetailsInfo);

                                                    currStock.CurrQty -= Qty;
                                                    Qty -= Qty;
                                                    _currentStockService.Update(currStock);
                                                    break;
                                                }
                                                else if (Qty > 0 && Lotwisestockqty <= Qty && Lotwisestockqty > 0)
                                                {
                                                    CurrentStock currtStock = new CurrentStock();
                                                    currtStock.LocCode = "900";
                                                    currtStock.LotNo = currStock.LotNo;
                                                    currtStock.ItemCode = currStock.ItemCode;
                                                    currtStock.CurrQty = Lotwisestockqty;
                                                    currtStock.UnitPrice = currStock.UnitPrice;
                                                    _currentStockService.Add(currtStock);

                                                    IssueDetails issueDetailsInfo = new IssueDetails();
                                                    issueDetailsInfo.IssueNo = item.IssueNo;
                                                    issueDetailsInfo.ItemCode = item.ItemCode;
                                                    issueDetailsInfo.SubCode = item.SubCode;
                                                    issueDetailsInfo.Description = item.Description;
                                                    issueDetailsInfo.Qty = Lotwisestockqty;
                                                    issueDetailsInfo.Price = item.Price;
                                                    issueDetailsInfo.ExQty = Lotwisestockqty;
                                                    issueDetailsInfo.LotNo = lot.LotNo;
                                                    issuDetailsList.Add(issueDetailsInfo);

                                                    currStock.CurrQty = 0;
                                                    Qty -= (int)Lotwisestockqty;
                                                    _currentStockService.Update(currStock);
                                                }
                                                else if (Qty == 0)
                                                {
                                                    break;
                                                }

                                            }

                                        }
                                        #endregion  autolot end
                                    }
                                    else
                                    {
                                        #region Not AutoLot by issueDetail
                                        foreach (var currentItem in currentStock)
                                        {
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                                m.LocCode == storeTreansferInfo.FromLocCode &&
                                                m.LotNo == currentItem.LotNo);
                                            if (currentStocks != null && currentStocks.CurrQty >= currentItem.CurrQty)
                                            {
                                                currentStocks.CurrQty = currentStocks.CurrQty - currentItem.CurrQty;
                                                currentStocks.UnitPrice = currentItem.UnitPrice;
                                                _currentStockService.Update(currentStocks);

                                                //write on 11/06/18
                                                var currentStockInTransits = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode && m.LocCode == "900" && m.LotNo == currentItem.LotNo);
                                                if (currentStockInTransits != null)
                                                {
                                                    currentStockInTransits.CurrQty = currentItem.CurrQty;
                                                    _currentStockService.Update(currentStockInTransits);
                                                }
                                                else
                                                {
                                                    CurrentStock currtStock = new CurrentStock();
                                                    currtStock.LocCode = "900";
                                                    currtStock.LotNo = currentItem.LotNo;
                                                    currtStock.ItemCode = currentItem.ItemCode;
                                                    currtStock.CurrQty = currentItem.CurrQty;
                                                    currtStock.UnitPrice = currentStocks.UnitPrice;
                                                    _currentStockService.Add(currtStock);
                                                }

                                            }
                                            else
                                            {
                                                transaction.Dispose();
                                                return Json("0", JsonRequestBehavior.AllowGet);

                                            }

                                        }
                                        foreach (var issuDetailsItem in issueDetails)
                                        {
                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                            issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                            issueDetailsInfo.SubCode = issuDetailsItem.SubCode;
                                            issueDetailsInfo.Description = issuDetailsItem.Description;
                                            issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                            issueDetailsInfo.Price = issuDetailsItem.Price;
                                            issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                            issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                            issuDetailsList.Add(issueDetailsInfo);

                                            #region For VAT VM_6p5
                                            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                                            {
                                                VM_6p5 VM6p5 = new VM_6p5();
                                                VM6p5.SerialNo = 0;
                                                VM6p5.ItemCode = issuDetailsItem.ItemCode;
                                                VM6p5.ItemName = _itemInfoService.All().Where(s => s.ItemCode == issuDetailsItem.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                                                VM6p5.UnitIn = (from i in _itemInfoService.All().ToList()
                                                                join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                                                where i.ItemCode == issuDetailsItem.ItemCode
                                                                select u.UnitName).FirstOrDefault();
                                                VM6p5.ChallanQty = (decimal)issuDetailsItem.Qty;
                                                VM6p5.UnitPrice = (decimal)issuDetailsItem.Price;
                                                VM6p5.TotalValue = storeTreansferInfo.TotAmt;
                                                VM6p5.SuppTaxAmt = issuDetailsItem.SupTaxAmt;
                                                VM6p5.VATRate = 0;
                                                VM6p5.VATAmt = issuDetailsItem.VATAmt;
                                                VM6p5.NetValue = storeTreansferInfo.TotAmt;
                                                VM6p5.ChallanNo = storeTreansferInfo.IssueNo;
                                                VM6p5.ChallanDate = storeTreansferInfo.IssueDate;
                                                VM6p5.ChallanTime = TimeSpan.FromTicks(storeTreansferInfo.Time.ToUniversalTime().Minute);
                                                VM6p5.IssueAddr = (from i in _locationInfoService.All() where i.LocCode == storeTreansferInfo.FromLocCode select i.LocName).FirstOrDefault();
                                                VM6p5.ShipAddr = (from i in _locationInfoService.All() where i.LocCode == storeTreansferInfo.ToLocCode select i.LocName).FirstOrDefault();
                                                VM6p5.IssueNo = storeTreansferInfo.IssueNo;
                                                VM6p5.IssuedBy = (from i in _employeeInfoService.All() where i.Id == Convert.ToInt32(storeTreansferInfo.IssueByCode) select i.UserName).FirstOrDefault();
                                                VM6p5.IssuedDesig = "";
                                                VM6p5.IssueTime = TimeSpan.FromTicks(storeTreansferInfo.Time.ToUniversalTime().Ticks);
                                                VM6p5List.Add(VM6p5);
                                            }
                                            #endregion
                                        }
                                        #endregion not autolot end
                                    }
                                }
                                else
                                {
                                    #region for Cost ledger

                                    foreach (var costLedger in issueDetails)
                                    {
                                        CostLedger cLedger = new CostLedger();
                                        cLedger.IssQty = costLedger.Qty;
                                        cLedger.IssRate = costLedger.Price;
                                        cLedger.IssTotal = cLedger.IssQty * cLedger.IssRate;

                                        cLedger.RecQty = 0;
                                        cLedger.RecRate = 0;
                                        cLedger.RecTotal = 0;

                                        var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == storeTreansferInfo.FromLocCode.Trim()).ToList();
                                        if (existCurrStoc.Count != 0)
                                        {
                                            var date = existCurrStoc.Max(x => x.TrDate);
                                            var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == storeTreansferInfo.FromLocCode.Trim() && x.TrDate == date).FirstOrDefault();

                                            cLedger.BalQty = existCurrStock.BalQty - cLedger.IssQty;
                                            cLedger.BalRate = System.Math.Round(existCurrStock.BalRate, 2);
                                            cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                            cLedger.LocNo = storeTreansferInfo.FromLocCode;
                                            cLedger.ItemCode = costLedger.ItemCode;
                                            cLedger.TrDate = storeTreansferInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                            cLedger.UpdSrcNo = storeTreansferInfo.IssueNo;
                                            cLedger.UpdSrc = "ST";
                                            CostLedgerAppService.Add(cLedger);
                                            CostLedgerAppService.Save();
                                            // For store in transit 
                                            cLedger.LocNo = "900";
                                            CostLedgerAppService.Add(cLedger);
                                            CostLedgerAppService.Save();

                                        }
                                        else if (existCurrStoc.Count == 0)
                                        {
                                            cLedger.CurrQty = 0;
                                            cLedger.UnitPrice = 0;
                                            cLedger.BalTotal = 0;

                                            cLedger.BalQty = cLedger.BalQty - cLedger.IssQty;
                                            cLedger.BalRate = System.Math.Round(cLedger.IssRate, 2);
                                            cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                            cLedger.LocNo = storeTreansferInfo.ToLocCode;
                                            cLedger.ItemCode = costLedger.ItemCode;
                                            cLedger.TrDate = storeTreansferInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                            cLedger.UpdSrcNo = storeTreansferInfo.IssueNo;
                                            cLedger.UpdSrc = "ST";
                                            CostLedgerAppService.Add(cLedger);
                                            CostLedgerAppService.Save();

                                            // For store in transit 
                                            cLedger.LocNo = "900";
                                            CostLedgerAppService.Add(cLedger);
                                            CostLedgerAppService.Save();
                                        }

                                    }

                                    foreach (var issuDetailsItem in issueDetails)
                                    {
                                        IssueDetails issueDetailsInfo = new IssueDetails();
                                        issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                        issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                        issueDetailsInfo.SubCode = issuDetailsItem.SubCode;
                                        issueDetailsInfo.Description = issuDetailsItem.Description;
                                        issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                        issueDetailsInfo.Price = issuDetailsItem.Price;// 
                                        issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                        issueDetailsInfo.LotNo = "01";
                                        issuDetailsList.Add(issueDetailsInfo);

                                        #region For VAT VM_6p5
                                        if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                                        {
                                            VM_6p5 VM6p5 = new VM_6p5();
                                            VM6p5.SerialNo = 0;
                                            VM6p5.ItemCode = issuDetailsItem.ItemCode;
                                            VM6p5.ItemName = _itemInfoService.All().Where(s => s.ItemCode == issuDetailsItem.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                                            VM6p5.UnitIn = (from i in _itemInfoService.All().ToList()
                                                            join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                                            where i.ItemCode == issuDetailsItem.ItemCode
                                                            select u.UnitName).FirstOrDefault();
                                            VM6p5.ChallanQty = (decimal)issuDetailsItem.Qty;
                                            VM6p5.UnitPrice = (decimal)issuDetailsItem.Price;
                                            VM6p5.TotalValue = storeTreansferInfo.TotAmt;
                                            VM6p5.SuppTaxAmt = issuDetailsItem.SupTaxAmt;
                                            VM6p5.VATRate = 0;
                                            VM6p5.VATAmt = issuDetailsItem.VATAmt;
                                            VM6p5.NetValue = storeTreansferInfo.TotAmt;
                                            VM6p5.ChallanNo = storeTreansferInfo.IssueNo;
                                            VM6p5.ChallanDate = storeTreansferInfo.IssueDate;
                                            VM6p5.ChallanTime = TimeSpan.Parse(storeTreansferInfo.Time.ToString("hh:mm tt"));
                                            VM6p5.IssueAddr = (from i in _locationInfoService.All() where i.LocCode == storeTreansferInfo.FromLocCode select i.LocName).FirstOrDefault();
                                            VM6p5.ShipAddr = (from i in _locationInfoService.All() where i.LocCode == storeTreansferInfo.ToLocCode select i.LocName).FirstOrDefault();
                                            VM6p5.IssueNo = storeTreansferInfo.IssueNo;
                                            VM6p5.IssuedBy = (from i in _employeeInfoService.All() where i.Id == Convert.ToInt32(storeTreansferInfo.IssueByCode) select i.UserName).FirstOrDefault();
                                            VM6p5.IssuedDesig = "";
                                            VM6p5.IssueTime = TimeSpan.Parse(storeTreansferInfo.Time.ToString("hh:mm tt"));
                                            VM6p5List.Add(VM6p5);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion
                                _currentStockService.Save();
                                issueInfo.IssueDetails = issuDetailsList;
                                _issueMainService.Add(issueInfo);
                                _issueMainService.Save();
                                //For VAT api 
                                string returnValue = "";
                                if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                                {
                                    var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                                    string json = JsonConvert.SerializeObject(VM6p5List, Formatting.Indented, serializerSettings);
                                    var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_6P5", VM6p5List).Result;
                                    returnValue = respse.Content.ReadAsAsync<string>().Result;
                                }
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Store Transfer", "Issue", issueInfo.IssueNo, Session["UserName"].ToString());
                                transaction.Complete();
                                return Json(new { Msg = "1", returnValue = returnValue }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                issueInfo.IssueDetails = issuDetailsList;
                                _issueMainService.Add(issueInfo);
                                _issueMainService.Save();
                                transaction.Complete();
                                return Json("1", JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Dispose();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetItemByQtyInTransit(string Item)
        {

            if (!string.IsNullOrEmpty(Item))
            {
                double qty = 0.0;
                CurrentStock currStocks = new CurrentStock();
                if (Session["MaintLot"].ToString() == "True")
                {
                    var existCurrStock = _currentStockService.All().Where(x => x.ItemCode == Item && x.LocCode == "900").ToList();
                    foreach (var item in existCurrStock)
                    {
                        qty = qty + item.CurrQty;
                        currStocks.UnitPrice = item.UnitPrice;
                    }
                    currStocks.CurrQty = qty;
                }
                else
                {
                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == Item && x.LocNo == "900").ToList();
                    if (existCurrStoc.Count != 0)
                    {
                        var date = existCurrStoc.Max(x => x.TrDate);
                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == Item && x.LocNo == "900" && x.TrDate == date).FirstOrDefault();
                        currStocks.CurrQty = existCurrStock.IssQty;
                        currStocks.UnitPrice = existCurrStock.IssRate;
                    }
                    else
                    {
                        currStocks.CurrQty = 0;
                        currStocks.UnitPrice = 0;
                    }
                }
                return Json(currStocks, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAllByRefNo(string refNo)
        {

            IssueMainVM recvInfo = new IssueMainVM();
            IssueDetailsVM recvDetail = new IssueDetailsVM();
            recvInfo.Main = _issueMainService.All().ToList().FirstOrDefault(x => x.RefNo == refNo.Trim() && x.IsReceived == false);
            var items = _issueDetailsService.All().ToList().Where(x => x.IssueNo == recvInfo.Main.IssueNo.Trim()).ToList();
            //var ite = _currentStockService.All().ToList().Where(x => x.ItemCode == items.Where());
            recvInfo.Main.IssueDetails = null;
            if (items.Count > 0)
            {
                int i = 0;
                foreach (var item in items)
                {

                    recvInfo.Details.Add(new IssueDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, item.Price, item.IssueNo, item.LotNo, item.Price, item.Description, item.ExQty));

                }
            }
            var check = Json(recvInfo, JsonRequestBehavior.AllowGet);
            return Json(recvInfo, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GenerateIssueNo()
        {
            return Json(GenerateIssueNo(Session["BranchCode"].ToString()), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateRecvNo()
        {
            return Json(GenerateRecvNo(Session["BranchCode"].ToString()), JsonRequestBehavior.AllowGet);
        }
        public string GenerateIssueNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string issueNo = "";
            var rcvNo = _issueMainService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var recvNo = _issueMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    issueNo = "00" + (Convert.ToInt64(recvNo.IssueNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    issueNo = "00000001";
                }
            }
            else
            {
                if (rcvNo != null)
                {
                    issueNo = branchCode + (Convert.ToInt64(rcvNo.IssueNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    issueNo = branchCode + "000001";
                }

            }

            return issueNo;

        }

        public string GenerateRecvNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string receivedNo = "";
            var rcvNo = _receiveMainService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var recvNo = _receiveMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    receivedNo = "00" + (Convert.ToInt64(recvNo.RcvNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receivedNo = "00000001";
                }
            }
            else
            {
                if (rcvNo != null)
                {
                    receivedNo = branchCode + (Convert.ToInt64(rcvNo.RcvNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receivedNo = branchCode + "000001";
                }

            }

            return receivedNo;

        }

        public string GenerateReceiveNo()
        {
            var IssueInfo = _locationInfoService.All().LastOrDefault();
            string IssueNo = "";
            if (IssueInfo != null)
            {
                IssueNo = (Convert.ToInt32(IssueInfo.LocCode) + 1).ToString().PadLeft(5, '0');
            }
            else
            {
                IssueNo = "00001";
            }
            return IssueNo;
        }

        public SelectList LoadStoreLocation(string StoreType, ILocationAppService _locationInfoService)
        {
            if (!string.IsNullOrEmpty(StoreType))
            {
                string branchCode = Session["BranchCode"].ToString();
                // string logName = User.Identity.Name;
                string sql = string.Format("EXEC loadLocation '" + branchCode + "'");
                var items = _locationInfoService.SqlQueary(sql).ToList()
                    .Select(x => new { x.LocCode, x.LocName }).ToList();
                items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public SelectList LoadIssueAppBy(string IssueByCode, IEmployeeAppService _employeeInfoService)
        {
            if (!string.IsNullOrEmpty(IssueByCode))
            {
                string branchCode = Session["BranchCode"].ToString();
                string form = "Store Transfer";
                string logName = Session["UserName"].ToString();
                string functionName = "Issued By";

                string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
                var items = _employeeInfoService.SqlQueary(sql)
                    .Select(x => new { x.Id, x.UserName }).ToList();
                return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public SelectList LoadRecvBy(string StoreType, IEmployeeAppService _employeeInfoService)
        {
            if (!string.IsNullOrEmpty(StoreType))
            {
                string branchCode = Session["BranchCode"].ToString();
                string form = "Store Transfer";
                string logName = Session["UserName"].ToString();
                string functionName = "Received By";
                string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
                var items = _employeeInfoService.SqlQueary(sql)
                    .Select(x => new { x.Id, x.UserName }).ToList();
                return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public SelectList LoadAppBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Store Transfer";
            string logName = Session["UserName"].ToString();
            string functionName = "Approved By";

            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }
    }
}
