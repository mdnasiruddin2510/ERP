using AcclineERP.Models;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AcclineERP.Controllers
{
    public class IssueRecvInfoController : Controller
    {
        private readonly ILotDTAppService _lotDTService;
        private readonly IIM_InvACAppService _iM_InvACService;
        private readonly IPM_IssRecAppService _pM_IssRecService;
        private readonly IIssueDetailsAppService _issueDetailsService;
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly ILocationAppService _locationInfoService;
        private readonly INewChartAppService _newChartService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        private readonly IEmployeeFuncAppService _EmployeeFuncAppService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ILotDTAppService _lotDtService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IBranchAppService _branchService;
        private readonly IVchrMainAppService VchrMainService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly IUnitAppService _unitService;

        public IssueRecvInfoController(ILotDTAppService _lotDTService, IIM_InvACAppService _iM_InvACService, IPM_IssRecAppService _pM_IssRecService, IReceiveDetailsAppService _receiveDetailsService, IIssueDetailsAppService _issueDetailsService, IReceiveAppService _receiveMainService, IIssueMainAppService _issueMainService, ILocationAppService _locationInfoService, INewChartAppService _newChartService,
            IItemInfoAppService _itemInfoService, ISubsidiaryInfoAppService _subsidiaryInfoService,
            IEmployeeAppService _employeeInfoService, ICurrentStockAppService _currentStockService,
            IIssRecSrcDestAppService _issRecvSrcDestService, IEmployeeFuncAppService _EmployeeFuncAppService,
            ITransactionLogAppService _transactionLogService, ILotDTAppService _lotDtService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IItemTypeAppService _itemTypeService, IBranchAppService _branchService, IVchrMainAppService VchrMainService,
            ICommonPVVMAppService _CommonVmService, ICostLedgerAppService CostLedgerAppService, IUnitAppService _unitService
            )
        {
            this._lotDTService = _lotDTService;
            this._iM_InvACService = _iM_InvACService;
            this._pM_IssRecService = _pM_IssRecService;
            this._receiveDetailsService = _receiveDetailsService;
            this._issueDetailsService = _issueDetailsService;
            this._receiveMainService = _receiveMainService;
            this._issueMainService = _issueMainService;
            this._locationInfoService = _locationInfoService;
            this._newChartService = _newChartService;
            this._itemInfoService = _itemInfoService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._currentStockService = _currentStockService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._EmployeeFuncAppService = _EmployeeFuncAppService;
            this._transactionLogService = _transactionLogService;
            this._lotDtService = _lotDtService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._itemTypeService = _itemTypeService;
            this._branchService = _branchService;
            this.VchrMainService = VchrMainService;
            this._CommonVmService = _CommonVmService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._unitService = _unitService;
        }
        public ActionResult IssueRecvInfo(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).FirstOrDefault();
                ViewBag.BatchNo = GenerateBatchNo();

                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("IP");
                ViewBag.GLEntries = CountEntries("IP", datetime);

                ViewBag.IssueNo = GenerateIssueNo(branchCode);
                ViewBag.FromLocCode = LoadDropDown.LoadFromLocation(_locationInfoService);
                ViewBag.ItemTypeCode = LoadDropDown.LoadItemType();//new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.ItemCode = LoadDropDown.LoadItemByItemType("", _CommonVmService);
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                ViewBag.ReceiveNo = GenerateRecvNo(branchCode);
                ViewBag.ProducesItem = LoadDropDown.LoadPItems(_itemInfoService);
                ViewBag.ReceivedByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintLot = sysSet.MaintLot;

                //#endregion
                ViewBag.errMsg = errMsg;
                return View();
            }

            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult LoadItemByItemType(string ItemTypeCode)
        {
            return Json(LoadDropDown.LoadItemByItemType(ItemTypeCode, _CommonVmService), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveIssueRecvInfo(IssueRecvInfoVM issueRecvInfo, List<IssueDetails> issueDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("IssueRecvInfo_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var issAccode = _iM_InvACService.All().Where(x => x.ItemType == issueRecvInfo.ItemTypeCode).Select(x => x.Accode).FirstOrDefault();

                    var ProduceItemTyp = _iM_InvACService.All().Where(x => x.ItemType == 1 || x.ItemType == 2).Select(x => x.ItemType).FirstOrDefault();
                    var recAcCode = _iM_InvACService.All().Where(x => x.ItemType == ProduceItemTyp).Select(x => x.Accode).FirstOrDefault();

                    var IfExit = _pM_IssRecService.All().Where(x => x.BatchNo == issueRecvInfo.BatchNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        bool IsAutoLot = true;
                        PM_IssRec pM_IssRec = new PM_IssRec();
                        var todayDate = DateTime.Now;
                        pM_IssRec.BatchNo = issueRecvInfo.BatchNo;
                        pM_IssRec.JobNo = issueRecvInfo.JobNo;
                        pM_IssRec.RefNo = issueRecvInfo.RefNo;
                        pM_IssRec.RefDate = issueRecvInfo.RefDate;
                        pM_IssRec.IssueNo = issueRecvInfo.IssueNo;
                        pM_IssRec.ReceiveNo = issueRecvInfo.ReceiveNo;
                        pM_IssRec.IssAcCode = issAccode;
                        pM_IssRec.RecAcCode = recAcCode;


                        IssueMain issueInfo = new IssueMain();
                        issueInfo.IssueNo = issueRecvInfo.IssueNo;
                        issueInfo.BranchCode = Session["BranchCode"].ToString();
                        issueInfo.IssueDate = issueRecvInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        issueInfo.FromLocCode = issueRecvInfo.FromLocCode;
                        issueInfo.IssTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        issueInfo.IssDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        issueInfo.FinYear = Session["FinYear"].ToString();
                        issueInfo.GLPost = false;
                        issueInfo.Accode = issAccode;
                        issueInfo.cashReceiptStatus = false;
                        issueInfo.IsReceived = false;
                        issueInfo.EntrySrc = "IP";
                        issueInfo.EntrySrcNo = issueRecvInfo.BatchNo;
                        double amount = 0.0;

                        List<IssueDetails> issuDetailsList = new List<IssueDetails>();

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
                                        currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == issueRecvInfo.FromLocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
                                        if (currStock == null)
                                        { continue; }
                                        var Lotwisestockqty = currStock.CurrQty;
                                        if (Lotwisestockqty > Qty && Qty > 0)
                                        {
                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = item.IssueNo;
                                            issueDetailsInfo.ItemCode = item.ItemCode;
                                            issueDetailsInfo.SubCode = item.SubCode;
                                            issueDetailsInfo.Description = item.Description;
                                            issueDetailsInfo.Qty = Qty;
                                            issueDetailsInfo.Price = item.Price;
                                            issueDetailsInfo.ExQty = Lotwisestockqty;
                                            issueDetailsInfo.LotNo = lot.LotNo;
                                            amount = amount + (item.Qty * item.Price);
                                            issuDetailsList.Add(issueDetailsInfo);

                                            currStock.CurrQty -= Qty;
                                            Qty -= Qty;
                                            _currentStockService.Update(currStock);
                                            break;
                                        }
                                        else if (Qty > 0 && Lotwisestockqty <= Qty && Lotwisestockqty > 0)
                                        {

                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = item.IssueNo;
                                            issueDetailsInfo.ItemCode = item.ItemCode;
                                            issueDetailsInfo.SubCode = item.SubCode;
                                            issueDetailsInfo.Description = item.Description;
                                            issueDetailsInfo.Qty = Lotwisestockqty;
                                            issueDetailsInfo.Price = item.Price;
                                            issueDetailsInfo.ExQty = Lotwisestockqty;
                                            issueDetailsInfo.LotNo = lot.LotNo;
                                            amount = amount + (item.Qty * item.Price);
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
                                #endregion Not autolot end
                            }
                            else
                            {
                                #region Not AutoLot by issueDetail
                                foreach (var currentItem in currentStock)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                        m.LocCode == issueRecvInfo.FromLocCode &&
                                        m.LotNo == currentItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty - currentItem.CurrQty;
                                        currentStocks.UnitPrice = currentItem.UnitPrice;
                                        _currentStockService.Update(currentStocks);
                                    }
                                    else
                                    {
                                        CurrentStock currStock = new CurrentStock();
                                        currStock.LocCode = issueRecvInfo.FromLocCode;
                                        currStock.LotNo = currentItem.LotNo;
                                        currStock.ItemCode = currentItem.ItemCode;
                                        currStock.CurrQty = 0 - currentItem.CurrQty;
                                        currStock.UnitPrice = currentItem.UnitPrice;
                                        _currentStockService.Add(currStock);
                                    }
                                }


                                foreach (var issuDetailsItem in issueDetails)
                                {
                                    IssueDetails issueDetailsInfo = new IssueDetails();
                                    issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                    issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                    issueDetailsInfo.Description = issuDetailsItem.Description;
                                    issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                    issueDetailsInfo.Price = issuDetailsItem.Price;
                                    issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                    issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                    amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                    issuDetailsList.Add(issueDetailsInfo);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region for cost ledger
                            foreach (var costLedger in issueDetails)
                            {
                                CostLedger cLedger = new CostLedger();
                                cLedger.IssQty = costLedger.Qty;
                                cLedger.IssRate = costLedger.Price;
                                cLedger.IssTotal = cLedger.IssQty * cLedger.IssRate;

                                cLedger.RecQty = 0;
                                cLedger.RecRate = 0;
                                cLedger.RecTotal = 0;

                                var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == issueRecvInfo.FromLocCode.Trim()).ToList();
                                if (existCurrStoc.Count != 0)
                                {
                                    var date = existCurrStoc.Max(x => x.TrDate);
                                    var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == issueRecvInfo.FromLocCode.Trim() && x.TrDate == date).FirstOrDefault();

                                    cLedger.BalQty = existCurrStock.BalQty - cLedger.IssQty;
                                    cLedger.BalRate = System.Math.Round(existCurrStock.BalRate, 2);
                                    cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                    cLedger.LocNo = issueRecvInfo.FromLocCode;
                                    cLedger.ItemCode = costLedger.ItemCode;
                                    cLedger.TrDate = issueRecvInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = issueRecvInfo.IssueNo;
                                    cLedger.UpdSrc = "IP";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                else if (existCurrStoc.Count == 0)
                                {
                                    cLedger.CurrQty = 0;
                                    cLedger.UnitPrice = 0;
                                    cLedger.BalTotal = 0;

                                    cLedger.BalQty = cLedger.BalQty - cLedger.IssQty;
                                    cLedger.BalRate = System.Math.Round(cLedger.IssRate, 2);
                                    cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                    cLedger.LocNo = issueRecvInfo.FromLocCode;
                                    cLedger.ItemCode = costLedger.ItemCode;
                                    cLedger.TrDate = issueRecvInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = issueRecvInfo.IssueNo;
                                    cLedger.UpdSrc = "IP";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                CostLedgerAppService.Save();
                            }

                            foreach (var issuDetailsItem in issueDetails)
                            {
                                IssueDetails issueDetailsInfo = new IssueDetails();
                                issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                //issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                issueDetailsInfo.Description = issuDetailsItem.Description;
                                issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                issueDetailsInfo.Price = issuDetailsItem.Price;
                                issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                issuDetailsList.Add(issueDetailsInfo);
                            }
                            #endregion
                        }
                        #endregion
                        issueInfo.Amount = amount;
                        issueInfo.IssueDetails = issuDetailsList;
                        _issueMainService.Add(issueInfo);
                        _issueMainService.Save();
                        _currentStockService.Save();

                        ReceiveMain receiveInfo = new ReceiveMain();
                        receiveInfo.RcvNo = issueRecvInfo.ReceiveNo;
                        receiveInfo.BranchCode = Session["BranchCode"].ToString();
                        receiveInfo.FromLocCode = issueRecvInfo.FromLocCode;
                        receiveInfo.RcvdByCode = issueRecvInfo.ReceivedByCode;
                        receiveInfo.RcvdTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        receiveInfo.RcvdDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        receiveInfo.FinYear = Session["FinYear"].ToString();
                        receiveInfo.GLPost = false;
                        receiveInfo.Accode = recAcCode;
                        receiveInfo.AppByCode = "";
                        receiveInfo.ChallanNo = "";
                        receiveInfo.CreditPur = false;
                        receiveInfo.expenseStatus = false;
                        receiveInfo.RcvDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        receiveInfo.RecvFromSubCode = "";
                        receiveInfo.RefDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        receiveInfo.RefNo = "";
                        receiveInfo.Source = "";
                        receiveInfo.StoreLocCode = "";
                        receiveInfo.ToLocCode = "";
                        receiveInfo.VchrNo = "";
                        receiveInfo.EntrySrc = "IP";
                        receiveInfo.EntrySrcNo = issueRecvInfo.BatchNo;

                        double amountR = 0.0;
                        amountR = amountR + Convert.ToDouble(issueRecvInfo.ProduceQty * issueRecvInfo.PricePerUnit);
                        receiveInfo.Amount = amountR;


                        //ReceiveDetails receiveDetails = new ReceiveDetails();
                        //receiveDetails.RcvNo = issueRecvInfo.ReceiveNo;
                        //receiveDetails.ItemCode = issueRecvInfo.ProducesItem;
                        //receiveDetails.Qty = Convert.ToDouble(issueRecvInfo.ProduceQty);
                        //receiveDetails.Price = Convert.ToDouble(issueRecvInfo.PricePerUnit);
                        //receiveDetails.ExQty = 0;
                        //receiveDetails.BadQty = 0;


                        List<ReceiveDetails> recvDetailsList = new List<ReceiveDetails>();
                        recvDetailsList.Add(new ReceiveDetails()
                        {
                            RcvNo = issueRecvInfo.ReceiveNo,
                            ItemCode = issueRecvInfo.ProducesItem,
                            Qty = Convert.ToDouble(issueRecvInfo.ProduceQty),
                            Price = Convert.ToDouble(issueRecvInfo.PricePerUnit),
                            ExQty = Convert.ToDouble(issueRecvInfo.RExtQty),
                            LotNo = issueRecvInfo.RecvLotNo,
                            BadQty = 0
                        });

                        foreach (var recvListInfo in recvDetailsList)
                        {
                            ReceiveDetails receiveDetails = new ReceiveDetails();
                            receiveDetails = recvListInfo;
                            //recvDetailsList.Add(receiveDetails);
                            _receiveDetailsService.Add(receiveDetails);
                        }

                        #region For AVP OR LOT

                        if (Session["MaintLot"].ToString() == "True")
                        {
                            #region LOT
                            foreach (var currentItem in currentStock)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == issueRecvInfo.FromLocCode &&
                                    m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentItem.CurrQty + currentStocks.CurrQty;
                                    currentStocks.UnitPrice = currentItem.UnitPrice;
                                    _currentStockService.Update(currentStocks);
                                }
                                else
                                {
                                    CurrentStock currStock = new CurrentStock();
                                    currStock.LocCode = issueRecvInfo.FromLocCode;
                                    currStock.LotNo = currentItem.LotNo;
                                    currStock.ItemCode = currentItem.ItemCode;
                                    currStock.CurrQty = currentItem.CurrQty;
                                    currStock.UnitPrice = currentItem.UnitPrice;
                                    _currentStockService.Add(currStock);
                                }

                                var lotNO = _lotDTService.All().ToList().Where(m => m.LotNo == currentItem.LotNo).FirstOrDefault();
                                if (lotNO == null)
                                {
                                    LotDT lotDt = new LotDT();
                                    lotDt.LotNo = currentItem.LotNo;
                                    lotDt.LotDate = issueRecvInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                    lotDt.RefNo = issueRecvInfo.ReceiveNo;
                                    lotDt.RefSource = "Receive";
                                    _lotDTService.Add(lotDt);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region AVP

                            CostLedger cLedger = new CostLedger();
                            cLedger.RecQty = Convert.ToDouble(issueRecvInfo.ProduceQty);
                            cLedger.RecRate = Convert.ToDouble(issueRecvInfo.PricePerUnit);
                            cLedger.RecTotal = System.Math.Round(cLedger.RecQty * cLedger.RecRate, 2);

                            double CurrQty = 0, BalTotal = 0;
                            var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == issueRecvInfo.ProducesItem && x.LocNo == issueRecvInfo.FromLocCode.Trim()).ToList();
                            if (existCurrStoc.Count != 0)
                            {
                                var date = existCurrStoc.Max(x => x.TrDate);
                                var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == issueRecvInfo.ProducesItem && x.LocNo == issueRecvInfo.FromLocCode.Trim() && x.TrDate == date).ToList();
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

                            cLedger.LocNo = issueRecvInfo.FromLocCode;
                            cLedger.ItemCode = issueRecvInfo.ProducesItem;
                            cLedger.TrDate = issueRecvInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                            cLedger.UpdSrcNo = issueRecvInfo.ReceiveNo;
                            cLedger.UpdSrc = "IP";
                            CostLedgerAppService.Add(cLedger);

                            CostLedgerAppService.Save();
                        }
                            #endregion
                        #endregion


                        receiveInfo.RcvDetails = recvDetailsList;
                        _receiveMainService.Add(receiveInfo);
                        _currentStockService.Save();
                        _receiveMainService.Save();
                        pM_IssRec.AmountR = Convert.ToDecimal(receiveInfo.Amount);
                        _pM_IssRecService.Add(pM_IssRec);
                        _pM_IssRecService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "IssueReceiveInfo", "Save", pM_IssRec.BatchNo, Session["UserName"].ToString());
                        transaction.Complete();
                        return Json(new { Msg = "1" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult UpdateIssueRecvInfo(IssueRecvInfoVM issueRecvInfo, List<IssueDetails> issueDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("IssueRecvInfo_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    var issAccode = _iM_InvACService.All().Where(x => x.ItemType == issueRecvInfo.ItemTypeCode).Select(x => x.Accode).FirstOrDefault();

                    var ProduceItemTyp = _iM_InvACService.All().Where(x => x.ItemType == 1 || x.ItemType == 2).Select(x => x.ItemType).FirstOrDefault();
                    var recAcCode = _iM_InvACService.All().Where(x => x.ItemType == ProduceItemTyp).Select(x => x.Accode).FirstOrDefault();

                    var IssRecvInfo = _pM_IssRecService.All().Where(x => x.BatchNo == issueRecvInfo.BatchNo).FirstOrDefault();
                    var IssueMain = _issueMainService.All().Where(x => x.EntrySrcNo == issueRecvInfo.BatchNo).FirstOrDefault();
                    var ReceiveMain = _receiveMainService.All().Where(x => x.EntrySrcNo == issueRecvInfo.BatchNo).FirstOrDefault();
                    if (IssueMain != null && IssueMain != null && ReceiveMain != null)
                    {
                        bool IsLotMaintain = false;
                        bool IsAutoLot = true;
                        //PM_IssRec pM_IssRec = new PM_IssRec();
                        var todayDate = DateTime.Now;
                        IssRecvInfo.BatchNo = issueRecvInfo.BatchNo;
                        IssRecvInfo.JobNo = issueRecvInfo.JobNo;
                        IssRecvInfo.RefNo = issueRecvInfo.RefNo;
                        IssRecvInfo.RefDate = issueRecvInfo.RefDate;
                        IssRecvInfo.IssueNo = issueRecvInfo.IssueNo;
                        IssRecvInfo.ReceiveNo = issueRecvInfo.ReceiveNo;
                        IssRecvInfo.IssAcCode = issAccode;
                        IssRecvInfo.RecAcCode = recAcCode;


                        //IssueMain issueInfo = new IssueMain();
                        IssueMain.IssueNo = issueRecvInfo.IssueNo;
                        IssueMain.BranchCode = Session["BranchCode"].ToString();
                        IssueMain.IssueDate = issueRecvInfo.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        IssueMain.FromLocCode = issueRecvInfo.FromLocCode;
                        IssueMain.IssTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        IssueMain.IssDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        IssueMain.FinYear = Session["FinYear"].ToString();
                        IssueMain.GLPost = false;
                        IssueMain.Accode = issAccode;
                        IssueMain.cashReceiptStatus = false;
                        IssueMain.IsReceived = false;
                        IssueMain.EntrySrc = "IP";
                        IssueMain.EntrySrcNo = issueRecvInfo.BatchNo;
                        IssueMain.IssueDetails.Clear();
                        IssueMain.IssueDetails = new List<IssueDetails>();
                        double amount = 0.0;

                        if (IsLotMaintain == true)
                        {
                            if (IsAutoLot == true)
                            {
                                #region AutoLot by issueDetail
                                var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueRecvInfo.IssueNo).ToList();
                                foreach (var issItem in issueDtls)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                       m.LocCode == issueRecvInfo.FromLocCode &&
                                       m.LotNo == issItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty + issItem.Qty;
                                        _currentStockService.Update(currentStocks);
                                    }
                                }

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
                                        currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == issueRecvInfo.FromLocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
                                        if (currStock == null)
                                        { continue; }
                                        var Lotwisestockqty = currStock.CurrQty;
                                        if (Lotwisestockqty > Qty && Qty > 0)
                                        {

                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = item.IssueNo;
                                            issueDetailsInfo.ItemCode = item.ItemCode;
                                            issueDetailsInfo.SubCode = item.SubCode;
                                            issueDetailsInfo.Description = item.Description;
                                            issueDetailsInfo.Qty = Qty;
                                            issueDetailsInfo.Price = item.Price;// issuDetailsItem.Qty * issuDetailsItem.Price;
                                            issueDetailsInfo.ExQty = Lotwisestockqty;
                                            issueDetailsInfo.LotNo = lot.LotNo;
                                            amount = amount + (item.Qty * item.Price);
                                            IssueMain.IssueDetails.Add(issueDetailsInfo);

                                            currStock.CurrQty -= Qty;
                                            Qty -= Qty;
                                            _currentStockService.Update(currStock);
                                            break;
                                        }
                                        else if (Qty > 0 && Lotwisestockqty <= Qty && Lotwisestockqty > 0)
                                        {

                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = item.IssueNo;
                                            issueDetailsInfo.ItemCode = item.ItemCode;
                                            issueDetailsInfo.SubCode = item.SubCode;
                                            issueDetailsInfo.Description = item.Description;
                                            issueDetailsInfo.Qty = Lotwisestockqty;
                                            issueDetailsInfo.Price = item.Price;// issuDetailsItem.Qty * issuDetailsItem.Price;
                                            issueDetailsInfo.ExQty = Lotwisestockqty;
                                            issueDetailsInfo.LotNo = lot.LotNo;
                                            amount = amount + (item.Qty * item.Price);
                                            IssueMain.IssueDetails.Add(issueDetailsInfo);

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
                                #endregion Not autolot end
                            }
                            else
                            {
                                #region Not AutoLot by issueDetail

                                var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueRecvInfo.IssueNo).ToList();
                                foreach (var issItem in issueDtls)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                       m.LocCode == issueRecvInfo.FromLocCode &&
                                       m.LotNo == issItem.LotNo);
                                    if (currentStocks != null)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty + issItem.Qty;
                                        _currentStockService.Update(currentStocks);
                                    }
                                }

                                foreach (var currentItem in currentStock)
                                {

                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                        m.LocCode == issueRecvInfo.FromLocCode &&
                                        m.LotNo == currentItem.LotNo);
                                    if (currentStocks != null && currentStocks.CurrQty >= currentItem.CurrQty)
                                    {
                                        currentStocks.CurrQty = currentStocks.CurrQty - currentItem.CurrQty;
                                        currentStocks.UnitPrice = currentItem.UnitPrice;
                                        _currentStockService.Update(currentStocks);
                                    }
                                    else
                                    {
                                        transaction.Dispose();
                                        return Json("0", JsonRequestBehavior.AllowGet);

                                    }

                                }
                                foreach (var issuDetailsItem in issueDetails)
                                {
                                    string sql = string.Format("Select * from IssueDetails where IssueNo = '" + issuDetailsItem.IssueNo + "' and ItemCode = '" + issuDetailsItem.ItemCode + "' and LotNo = '01'");
                                    var exIssQty = _issueDetailsService.SqlQueary(sql).ToList();

                                    IssueDetails issueDetailsInfo = new IssueDetails();
                                    issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                    issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                    //issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                    issueDetailsInfo.Description = issuDetailsItem.Description;
                                    issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                    issueDetailsInfo.Price = issuDetailsItem.Price;
                                    issueDetailsInfo.ExQty = (exIssQty.Count != 0) ? exIssQty.FirstOrDefault().ExQty : issuDetailsItem.ExQty;
                                    issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                    amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                    IssueMain.IssueDetails.Add(issueDetailsInfo);

                                }


                                #endregion autolot end
                            }
                        }
                        else
                        {
                            #region Not AutoLot by issueDetail


                            var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueRecvInfo.IssueNo).ToList();
                            foreach (var issItem in issueDtls)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                   m.LocCode == issueRecvInfo.FromLocCode &&
                                   m.LotNo == issItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentStocks.CurrQty + issItem.Qty;
                                    _currentStockService.Update(currentStocks);
                                }
                            }

                            foreach (var currentItem in currentStock)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                   m.LocCode == issueRecvInfo.FromLocCode &&
                                   m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentStocks.CurrQty - currentItem.CurrQty;
                                    currentStocks.UnitPrice = currentItem.UnitPrice;
                                    _currentStockService.Update(currentStocks);
                                }
                                else
                                {
                                    CurrentStock currStock = new CurrentStock();
                                    currStock.LocCode = issueRecvInfo.FromLocCode;
                                    currStock.LotNo = currentItem.LotNo;
                                    currStock.ItemCode = currentItem.ItemCode;
                                    currStock.CurrQty = 0 - currentItem.CurrQty;
                                    currStock.UnitPrice = currentItem.UnitPrice;
                                    _currentStockService.Add(currStock);
                                }
                            }


                            foreach (var issuDetailsItem in issueDetails)
                            {
                                string sql = string.Format("Select * from IssueDetails where IssueNo = '" + issuDetailsItem.IssueNo + "' and ItemCode = '" + issuDetailsItem.ItemCode + "' and LotNo = '01'");
                                var exIssQty = _issueDetailsService.SqlQueary(sql).ToList();

                                IssueDetails issueDetailsInfo = new IssueDetails();
                                issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                //issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                issueDetailsInfo.Description = issuDetailsItem.Description;
                                issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                issueDetailsInfo.Price = issuDetailsItem.Price;
                                issueDetailsInfo.ExQty = (exIssQty.Count != 0) ? exIssQty.FirstOrDefault().ExQty : issuDetailsItem.ExQty;
                                issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                IssueMain.IssueDetails.Add(issueDetailsInfo);
                            }
                            #endregion autolot end
                        }
                        IssueMain.Amount = amount;
                        _issueMainService.Update(IssueMain);
                        _issueDetailsService.All().ToList().Where(y => string.IsNullOrEmpty(y.IssueNo)).ToList().ForEach(x => _issueDetailsService.Delete(x));
                        _issueMainService.Save();
                        _currentStockService.Save();


                        //ReceiveMain receiveInfo = new ReceiveMain();
                        ReceiveMain.RcvNo = issueRecvInfo.ReceiveNo;
                        ReceiveMain.BranchCode = Session["BranchCode"].ToString();
                        ReceiveMain.FromLocCode = issueRecvInfo.FromLocCode;
                        ReceiveMain.RcvdByCode = issueRecvInfo.ReceivedByCode;
                        ReceiveMain.RcvdTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        ReceiveMain.RcvdDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        ReceiveMain.FinYear = Session["FinYear"].ToString();
                        ReceiveMain.GLPost = false;
                        ReceiveMain.Accode = recAcCode;
                        ReceiveMain.AppByCode = "";
                        ReceiveMain.ChallanNo = "";
                        ReceiveMain.CreditPur = false;
                        ReceiveMain.expenseStatus = false;
                        ReceiveMain.RcvDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        ReceiveMain.RecvFromSubCode = "";
                        ReceiveMain.RefDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        ReceiveMain.RefNo = "";
                        ReceiveMain.Source = "";
                        ReceiveMain.StoreLocCode = "";
                        ReceiveMain.ToLocCode = "";
                        ReceiveMain.VchrNo = "";
                        ReceiveMain.EntrySrc = "IP";
                        ReceiveMain.EntrySrcNo = issueRecvInfo.BatchNo;
                        ReceiveMain.RcvDetails.Clear();
                        ReceiveMain.RcvDetails = new List<ReceiveDetails>();
                        double amountR = 0.0;
                        amountR = amountR + Convert.ToDouble(issueRecvInfo.ProduceQty * issueRecvInfo.PricePerUnit);
                        ReceiveMain.Amount = amountR;

                        List<ReceiveDetails> recvDetailsList = new List<ReceiveDetails>();
                        recvDetailsList.Add(new ReceiveDetails()
                        {
                            RcvNo = issueRecvInfo.ReceiveNo,
                            ItemCode = issueRecvInfo.ProducesItem,
                            Qty = Convert.ToDouble(issueRecvInfo.ProduceQty),
                            Price = Convert.ToDouble(issueRecvInfo.PricePerUnit),
                            ExQty = Convert.ToDouble(issueRecvInfo.RExtQty),
                            LotNo = issueRecvInfo.RecvLotNo,
                            BadQty = 0
                        });

                        foreach (var recvListInfo in recvDetailsList)
                        {
                            ReceiveDetails receiveDetails = new ReceiveDetails();
                            receiveDetails = recvListInfo;
                            //recvDetailsList.Add(receiveDetails);
                            _receiveDetailsService.Add(receiveDetails);
                        }
                        var recvDetail = _receiveDetailsService.All().ToList().Where(m => m.RcvNo == ReceiveMain.RcvNo).ToList();
                        foreach (var item in recvDetail)
                        {
                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == item.ItemCode &&
                                m.LocCode == issueRecvInfo.FromLocCode &&
                                m.LotNo == item.LotNo);
                            if (currentStocks != null)
                            {
                                currentStocks.CurrQty = currentStocks.CurrQty - item.Qty;
                                _currentStockService.Update(currentStocks);
                                _currentStockService.Save();
                            }
                        }
                        foreach (var currentItem in currentStock)
                        {
                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                m.LocCode == issueRecvInfo.FromLocCode &&
                                m.LotNo == currentItem.LotNo);
                            if (currentStocks != null)
                            {
                                currentStocks.CurrQty = currentItem.CurrQty + currentStocks.CurrQty;
                                currentStocks.UnitPrice = currentItem.UnitPrice;
                                _currentStockService.Update(currentStocks);
                            }
                            else
                            {
                                CurrentStock currStock = new CurrentStock();
                                currStock.LocCode = issueRecvInfo.FromLocCode;
                                currStock.LotNo = currentItem.LotNo;
                                currStock.ItemCode = currentItem.ItemCode;
                                currStock.CurrQty = currentItem.CurrQty;
                                currStock.UnitPrice = currentItem.UnitPrice;

                                _currentStockService.Add(currStock);
                            }

                        }
                        //recvMain.RcvDetails = receveDetailsList;
                        _receiveMainService.Update(ReceiveMain);
                        _receiveDetailsService.All().ToList().Where(y => string.IsNullOrEmpty(y.RcvNo)).ToList().ForEach(x => _receiveDetailsService.Delete(x));
                        _receiveMainService.Save();
                        _currentStockService.Save();

                        IssRecvInfo.AmountR = Convert.ToDecimal(ReceiveMain.Amount);
                        _pM_IssRecService.Update(IssRecvInfo);
                        _pM_IssRecService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "IssueReceiveInfo", "Update", IssRecvInfo.BatchNo, Session["UserName"].ToString());
                        transaction.Complete();
                        return Json(new { Msg = "1" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult GetAllByBatchNo(string batchNo)
        {
            //IssRcvViewModel model = new IssRcvViewModel();

            var IssRcvInfo = _pM_IssRecService.All().ToList().Where(x => x.BatchNo == batchNo).FirstOrDefault();
            var IssMain = _issueMainService.All().ToList().Where(x => x.IssueNo == IssRcvInfo.IssueNo).Select(x =>new { x.IssDate,x.FromLocCode}).FirstOrDefault();
            var IssueDetail = _issueDetailsService.All().ToList().Where(x => x.IssueNo == IssRcvInfo.IssueNo).Select(s => new
            { 
                s.ItemCode,
                ItemName = _itemInfoService.All().Where(x => x.ItemCode == s.ItemCode).Select(x=>x.ItemName).FirstOrDefault(),
                s.LotNo,
                s.Qty,
                s.Price            
            }).ToList();
            var RcvdByCode = _receiveMainService.All().ToList().Where(x => x.RcvNo == IssRcvInfo.ReceiveNo).Select(x => x.RcvdByCode).FirstOrDefault();
            var ReceiveDetail = _receiveDetailsService.All().ToList().Where(x => x.RcvNo == IssRcvInfo.ReceiveNo).Select(x => new {
                x.ItemCode,
                x.Qty,
                x.Price,
                x.LotNo
            }).ToList();
            return Json(new { IssRcvInfo = IssRcvInfo, IssMain = IssMain, IssueDetail = IssueDetail, RcvdByCode = RcvdByCode, ReceiveDetail = ReceiveDetail }, JsonRequestBehavior.AllowGet); 
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
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain where VType='" + VType + "' and vDate ='" + datetime.ToString("yyyy/MM/dd") + "' and BranchCode='" + branchCode + "'");
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
        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Receive_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("Index", "Receive", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear left outer join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "' and pvm.BranchCode ='" + Session["BranchCode"].ToString() + "'"
                + "and pvm.BranchCode= '" + branchCode + "'"
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac");


            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("IssueRecvInfo", "IssueRecvInfo", new { errMsg });

            }
            else
            {
                ViewBag.branchCode = _branchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).FirstOrDefault();
                // ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("IssueRecvInfo", "IssueRecvInfo", new { errMsg });
            }
            else
            {
                //string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).FirstOrDefault();
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntriesPdf(string vchrNo)
        {
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as Accode,(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary,pvd.Narration,"
                + "pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from VchrMain as pvm "
                + "join VchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear  where pvm.VchrNo ='" + vchrNo + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("IssueRecvInfo", "IssueRecvInfo", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }

        public ActionResult GetIssueNo(string branchCode)
        {
            return Json(GenerateIssueNo(branchCode), JsonRequestBehavior.AllowGet);
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

        public ActionResult GetRecvNo(string branchCode)
        {
            return Json(GenerateRecvNo(branchCode), JsonRequestBehavior.AllowGet);
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

        public ActionResult GetBatchNo()
        {
            return Json(GenerateBatchNo(), JsonRequestBehavior.AllowGet);
        }
        public string GenerateBatchNo()
        {
            int batchNo = Convert.ToInt32(_pM_IssRecService.All().OrderBy(s => s.PM_IssRecId).Select(x => x.BatchNo).LastOrDefault());
            var InBatchNo = Convert.ToString(batchNo + 1);
            return InBatchNo;
        }
        public ActionResult LoadandSaveJob(string nJobNo)
        {
            return Json(LoadDropDown.LoadandSaveJob(nJobNo), JsonRequestBehavior.AllowGet);
        }
    }
}