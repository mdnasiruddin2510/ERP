using Application.Interfaces;
using System;
using App.Domain.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;
using AcclineERP.Models;
using System.Transactions;
using Data.Context;
namespace AcclineERP.Controllers
{
    public class ReceiveController : Controller
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
        private readonly IEmployeeFuncAppService _EmployeeFuncAppService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ILotDTAppService _lotDTService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IBranchAppService _branchService;
        private readonly IUnitAppService _unitService;
        private readonly IVchrMainAppService VchrMainService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly IOpeningStockAppService _OpeningStockService;

        public ReceiveController(ILocationAppService _locationInfoService,
            INewChartAppService _newChartService, IItemInfoAppService _itemInfoService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IEmployeeAppService _employeeInfoService,
            IReceiveDetailsAppService _receiveDetailsService, ICurrentStockAppService _currentStockService,
            IReceiveAppService _receiveMainService, IIssRecSrcDestAppService _issRecvSrcDestService,
            IEmployeeFuncAppService _EmployeeFuncAppService, ITransactionLogAppService _transactionLogService,
            ILotDTAppService _lotDTService, IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IItemTypeAppService _itemTypeService, IBranchAppService _branchService, IUnitAppService _unitService,
            IVchrMainAppService VchrMainService, ICommonPVVMAppService _CommonVmService, IFYDDAppService _FYDDService,
            ICostLedgerAppService CostLedgerAppService, IOpeningStockAppService _OpeningStockService)
        {

            this._locationInfoService = _locationInfoService;
            this._newChartService = _newChartService;
            this._itemInfoService = _itemInfoService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._receiveDetailsService = _receiveDetailsService;
            this._currentStockService = _currentStockService;
            this._receiveMainService = _receiveMainService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._EmployeeFuncAppService = _EmployeeFuncAppService;
            this._transactionLogService = _transactionLogService;
            this._lotDTService = _lotDTService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._itemTypeService = _itemTypeService;
            this._branchService = _branchService;
            this._unitService = _unitService;
            this.VchrMainService = VchrMainService;
            this._CommonVmService = _CommonVmService;
            this._FYDDService = _FYDDService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._OpeningStockService = _OpeningStockService;
        }
        public ActionResult Index(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).FirstOrDefault();
                ViewBag.ReceiveNo = GenerateRecvNo(branchCode);

                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("IR");
                ViewBag.GLEntries = CountEntries("IR", datetime);
                ViewBag.Purpose = LoadDropDown.onlyPurpose(_newChartService);//LoadDropDown.LoadAreaPurpuse("", _newChartService, _issRecvSrcDestService);
                ViewBag.RcvdTime = datetime.ToString("HH:mm:ss");
                ViewBag.FromLocCode = LoadStoreLocation(_locationInfoService);
                ViewBag.RecvItem = LoadDropDown.LoadItem(_itemInfoService);
                ViewBag.ToLocCode = new SelectList(_subsidiaryInfoService.All().ToList().OrderBy(x => x.SubName), "SubCode", "SubName");//LoadDropDown.LoadSubsidiary("", _subsidiaryInfoService, _issRecvSrcDestService);//new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "3"), "SubCode", "SubName"); (sub type chnge need to disscuss for specific subtype)
                ViewBag.RcvdByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName"); //LoadRecvBy(_employeeInfoService);
                ViewBag.AppByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");//LoadAppBy(_employeeInfoService);
                ViewBag.Source = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.Type == "S"), "SrcDestId", "SrcDestName");
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");

                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintLot = sysSet.MaintLot;
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
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
        public ActionResult GetAllByCode(string rcvNo)
        {
            RecvMainVM recvInfo = new RecvMainVM();
            RecvDetailsVM recvDetail = new RecvDetailsVM();
            recvInfo.Main = _receiveMainService.All().ToList().FirstOrDefault(x => x.RcvNo == rcvNo.Trim());
            var items = _receiveDetailsService.All().ToList().Where(x => x.RcvNo == rcvNo.Trim()).ToList();

            recvInfo.Main.RcvDetails = null;
            if (items.Count > 0)
            {
                int i = 0;
                foreach (var item in items)
                {

                    recvInfo.Details.Add(new RecvDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, item.ExQty, item.Price, item.LotNo, item.RcvNo, item.Price, item.Description));
                    //recvInfo.Details.Add(new RecvDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, item.Price, item.LotNo,item.ExQty));
                }
            }
            var check = Json(recvInfo, JsonRequestBehavior.AllowGet);
            return Json(recvInfo, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPurpuseBySrcDest(string Source)
        {
            return Json(LoadDropDown.LoadAreaPurpuse(Source, _newChartService, _issRecvSrcDestService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetSubsidiaryBySrcDest(string Source)
        {
            //ViewBag.ToLocCode = new[] { new SubsidiaryInfo { SubCode = "-1", SubName = "--- Select ---" } };

            return Json(LoadDropDown.LoadSubsidiary(Source, _subsidiaryInfoService, _issRecvSrcDestService), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRecvInfo(ReceiveMain recvInfo, List<ReceiveDetails> recvDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Receive_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    #region For Opeing Stock
                    var IssRcvSource = _issRecvSrcDestService.All().ToList().Where(x => x.Type == "S" && x.SrcDestId == recvInfo.Source).FirstOrDefault();
                    if (IssRcvSource.SrcDestName == "Opening Stock")
                    {
                        var Fydd = _FYDDService.All().ToList().Where(s => s.FinYear == Session["FinYear"].ToString()).FirstOrDefault();
                        foreach (var cItem in currentStock)
                        {
                            OpeningStock opStk = new OpeningStock();
                            opStk.LocCode = recvInfo.FromLocCode;
                            opStk.LotNo = cItem.LotNo;
                            opStk.ItemCode = cItem.ItemCode;
                            opStk.UnitPrice = cItem.UnitPrice;
                            opStk.OpenQty = cItem.CurrQty;
                            opStk.OpenDate = Fydd.FYDF;
                            opStk.ExpDate = Fydd.FYDT;
                            opStk.FinYear = Session["FinYear"].ToString();
                            opStk.BranchCode = Session["BranchCode"].ToString();
                            opStk.TotalPrice = cItem.CurrQty * cItem.UnitPrice;
                            opStk.Remarks = "";
                            var qntydetail = _itemInfoService.All().ToList().Where(x => x.ItemCode == cItem.ItemCode).FirstOrDefault();
                            if (qntydetail.Ratio == null)
                            {
                                opStk.QtyIndetail = 0;
                            }
                            else
                            {
                                int p = 0;
                                int q = 0;
                                String[] temp;
                                temp = qntydetail.Ratio.Split(':');
                                String x = temp[0];
                                p = int.Parse(x);
                                String y = temp[1];
                                q = int.Parse(y);
                                opStk.QtyIndetail = p * q;
                            }
                            _OpeningStockService.Add(opStk);
                            _OpeningStockService.Save();

                            if (Session["MaintLot"].ToString() == "True")
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == cItem.ItemCode && m.LocCode == recvInfo.FromLocCode &&
                                    m.LotNo == cItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = cItem.CurrQty + currentStocks.CurrQty;
                                    currentStocks.UnitPrice = cItem.UnitPrice;
                                    _currentStockService.Update(currentStocks);
                                    _currentStockService.Save();
                                }
                                else
                                {
                                    CurrentStock currStock = new CurrentStock();
                                    currStock.LocCode = recvInfo.StoreLocCode;
                                    currStock.LotNo = cItem.LotNo;
                                    currStock.ItemCode = cItem.ItemCode;
                                    currStock.CurrQty = cItem.CurrQty;
                                    currStock.UnitPrice = cItem.UnitPrice;
                                    _currentStockService.Add(currStock);
                                    _currentStockService.Save();
                                }
                                var lotNO = _lotDTService.All().ToList().Where(m => m.LotNo == cItem.LotNo).FirstOrDefault();
                                if (lotNO == null)
                                {
                                    LotDT lotDt = new LotDT();
                                    lotDt.LotNo = cItem.LotNo;
                                    lotDt.LotDate = recvInfo.RcvDate;
                                    lotDt.RefNo = recvInfo.RcvNo;
                                    lotDt.RefSource = "Receive";
                                    _lotDTService.Add(lotDt);
                                    _lotDTService.Save();
                                }

                            }
                            else
                            {
                                CostLedger costLedger = new CostLedger();
                                costLedger.RecQty = 0;
                                costLedger.RecRate = 0;
                                costLedger.RecTotal = 0;
                                costLedger.IssQty = 0;
                                costLedger.IssRate = 0;
                                costLedger.IssTotal = 0;
                                costLedger.BalQty = cItem.CurrQty;
                                costLedger.BalRate = cItem.UnitPrice;
                                costLedger.BalTotal = cItem.CurrQty * cItem.UnitPrice;
                                costLedger.LocNo = recvInfo.FromLocCode;
                                costLedger.ItemCode = cItem.ItemCode;
                                costLedger.TrDate = recvInfo.RcvDate;
                                costLedger.UpdSrcNo = "0";
                                costLedger.UpdSrc = "OB";
                                CostLedgerAppService.Add(costLedger);
                                CostLedgerAppService.Save();
                            }
                        }
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                    #region for Receive transection

                    var IfExit = _receiveMainService.All().Where(x => x.RcvNo == recvInfo.RcvNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        var todayDate = DateTime.Now;
                        ReceiveMain recvMain = new ReceiveMain();
                        recvMain.RcvNo = recvInfo.RcvNo;
                        recvMain.BranchCode = Session["BranchCode"].ToString();
                        recvMain.RcvDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        //recvMain.ToLocCode = recvInfo.ToLocCode;
                        recvMain.Source = recvInfo.Source;
                        recvMain.Accode = recvInfo.Accode;
                        recvMain.RefNo = recvInfo.RefNo;
                        recvMain.RefDate = recvInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.Remarks = recvInfo.Remarks;
                        recvMain.RcvdByCode = recvInfo.RcvdByCode;
                        recvMain.AppByCode = recvInfo.AppByCode;
                        recvMain.RcvdTime = recvInfo.RcvdTime;
                        recvMain.StoreLocCode = recvInfo.StoreLocCode;
                        recvMain.RecvFromSubCode = recvInfo.RecvFromSubCode;
                        recvMain.VchrNo = recvInfo.VchrNo;
                        recvMain.FinYear = Session["FinYear"].ToString();
                        recvMain.GLPost = false;
                        recvMain.RcvdDate = recvInfo.RcvdDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        recvMain.expenseStatus = false;
                        recvMain.CreditPur = recvInfo.CreditPur;
                        recvMain.EntrySrc = "IR";
                        recvMain.EntrySrcNo = recvInfo.RcvNo;

                        double amount = 0.0;
                        List<ReceiveDetails> receveDetailsList = new List<ReceiveDetails>();
                        foreach (var recvDetailsItem in recvDetails)
                        {
                            ReceiveDetails receivDetails = new ReceiveDetails();
                            receivDetails.RcvNo = recvDetailsItem.RcvNo;
                            receivDetails.ItemCode = recvDetailsItem.ItemCode;
                            receivDetails.SubCode = recvInfo.RecvFromSubCode;
                            receivDetails.Description = recvDetailsItem.Description;
                            receivDetails.Qty = recvDetailsItem.Qty;
                            receivDetails.Price = recvDetailsItem.Price;
                            receivDetails.ExQty = recvDetailsItem.ExQty;
                            receivDetails.LotNo = recvDetailsItem.LotNo;

                            amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                            receveDetailsList.Add(receivDetails);
                        }
                        recvMain.Amount = amount;

                        #region For AVP OR LOT

                        if (Session["MaintLot"].ToString() == "True")
                        {
                            #region LOT
                            foreach (var currentItem in currentStock)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == recvMain.StoreLocCode &&
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
                                    currStock.LocCode = recvInfo.StoreLocCode;
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
                                    lotDt.LotDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                                    lotDt.RefNo = recvInfo.RcvNo;
                                    lotDt.RefSource = "Receive";
                                    _lotDTService.Add(lotDt);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region AVP
                            var ISIRcvSource = _issRecvSrcDestService.All().ToList().Where(x => x.Type == "S" && x.SrcDestId == recvInfo.Source).FirstOrDefault();
                            if (ISIRcvSource.SrcDestName == "Local Purchase" || ISIRcvSource.SrcDestName == "Supplier Purchase" || ISIRcvSource.SrcDestName == "Direct Purchase"
                                || ISIRcvSource.SrcDestName == "Import Purchase" || ISIRcvSource.SrcDestName == "Production" || ISIRcvSource.SrcDestName == "Excess")
                            {
                                foreach (var cItem in currentStock)
                                {
                                    CostLedger cLedger = new CostLedger();
                                    cLedger.RecQty = cItem.CurrQty;
                                    cLedger.RecRate = cItem.UnitPrice;
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
                                    cLedger.TrDate = recvInfo.RcvdDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = recvInfo.RcvNo;
                                    cLedger.UpdSrc = "IR";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                CostLedgerAppService.Save();
                            }
                            #endregion
                        }

                        #endregion

                        recvMain.RcvDetails = receveDetailsList;
                        _receiveMainService.Add(recvMain);
                        _currentStockService.Save();
                        _receiveMainService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Save", recvMain.RcvNo, Session["UserName"].ToString());
                        LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);

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
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateRecvInfo(ReceiveMain recvInfo, List<ReceiveDetails> recvDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var vMain = VchrMainService.All().ToList().FirstOrDefault(s => s.VchrNo == recvInfo.VchrNo);
                    if (vMain == null)
                    {
                        var recvMain = _receiveMainService.All().FirstOrDefault(x => x.RcvNo == recvInfo.RcvNo);
                        if (recvMain != null)
                        {
                            var todayDate = DateTime.Now;
                            recvMain.RcvNo = recvInfo.RcvNo;
                            recvMain.BranchCode = Session["BranchCode"].ToString();
                            recvMain.RcvDate = recvInfo.RcvDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            recvMain.RecvFromSubCode = recvInfo.ToLocCode;
                            recvMain.Source = recvInfo.Source;
                            recvMain.Accode = recvInfo.Accode;
                            recvMain.RefNo = recvInfo.RefNo;
                            recvMain.RefDate = recvInfo.RefDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            recvMain.Remarks = recvInfo.Remarks;
                            recvMain.RcvdByCode = recvInfo.RcvdByCode;
                            recvMain.AppByCode = recvInfo.AppByCode;
                            recvMain.RcvdTime = recvInfo.RcvdTime;
                            recvMain.VchrNo = recvInfo.VchrNo;//by shahin(check is posted ?) 
                            recvMain.StoreLocCode = recvInfo.StoreLocCode;
                            recvMain.FinYear = Session["FinYear"].ToString();
                            recvMain.GLPost = false;
                            recvMain.RcvdDate = recvInfo.RcvdDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            recvMain.expenseStatus = false;
                            recvMain.RcvDetails.Clear();
                            recvMain.RcvDetails = new List<ReceiveDetails>();
                            double amount = 0.0;
                            recvMain.EntrySrc = "IR";
                            recvMain.EntrySrcNo = recvInfo.RcvNo;

                            foreach (var recvDetailsItem in recvDetails)
                            {
                                ReceiveDetails receivDetails = new ReceiveDetails();
                                receivDetails.RcvNo = recvMain.RcvNo;
                                receivDetails.ItemCode = recvDetailsItem.ItemCode;
                                receivDetails.Description = recvDetailsItem.Description;
                                receivDetails.Qty = recvDetailsItem.Qty;
                                receivDetails.SubCode = recvInfo.ToLocCode;
                                receivDetails.Price = recvDetailsItem.Price;
                                receivDetails.ExQty = recvDetailsItem.ExQty;
                                receivDetails.LotNo = recvDetailsItem.LotNo;
                                amount = amount + (recvDetailsItem.Qty * recvDetailsItem.Price);
                                recvMain.RcvDetails.Add(receivDetails);
                            }

                            recvMain.Amount = amount;

                            //if (recvInfo.AppByCode != "" || recvInfo.AppByCode != "---- Select ----")
                            //{
                                var recvDetail = _receiveDetailsService.All().ToList().Where(m => m.RcvNo == recvMain.RcvNo).ToList();
                                foreach (var item in recvDetail)
                                {
                                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == item.ItemCode &&
                                        m.LocCode == recvMain.StoreLocCode &&
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
                                        m.LocCode == recvMain.StoreLocCode &&
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
                                        currStock.LocCode = recvMain.StoreLocCode;
                                        currStock.LotNo = currentItem.LotNo;
                                        currStock.ItemCode = currentItem.ItemCode;
                                        currStock.CurrQty = currentItem.CurrQty;
                                        currStock.UnitPrice = currentItem.UnitPrice;

                                        _currentStockService.Add(currStock);
                                    }

                                }
                                //recvMain.RcvDetails = receveDetailsList;
                                _receiveMainService.Update(recvMain);
                                _receiveDetailsService.All().ToList().Where(y => string.IsNullOrEmpty(y.RcvNo)).ToList().ForEach(x => _receiveDetailsService.Delete(x));
                                _receiveMainService.Save();
                                _currentStockService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Update", recvMain.RcvNo, Session["UserName"].ToString());
                                //delete to provition
                                //var entryNo = recvInfo.RcvNo;
                                var entryNo = Convert.ToInt64(recvInfo.VchrNo.Substring(4, 8)).ToString().PadLeft(8, '0');
                                LoadDropDown.journalVoucherRemoval("IR", entryNo, recvInfo.VchrNo, Session["FinYear"].ToString());
                                LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                                transaction.Complete();
                                return Json("1", JsonRequestBehavior.AllowGet);
                            //}
                            //else
                            //{
                            //    //recvMain.RcvDetails = receveDetailsList;
                            //    _receiveMainService.Update(recvMain);
                            //    _receiveMainService.Save();
                            //    TransactionLogService.SaveTransactionLog(_transactionLogService, "Receive", "Update", recvMain.RcvNo, Session["UserName"].ToString());
                            //    //delete to provition
                            //    // var entryNo = recvInfo.RcvNo;
                            //    var entryNo = Convert.ToInt64(recvInfo.VchrNo.Substring(2, 10)).ToString().PadLeft(10, '0');
                            //    LoadDropDown.journalVoucherRemoval("IR", entryNo, recvInfo.VchrNo, Session["FinYear"].ToString());
                            //    LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                            //    transaction.Dispose();
                            //    return Json("1", JsonRequestBehavior.AllowGet);
                            //}
                        }
                        else
                        {
                            transaction.Dispose();
                            return Json("0", JsonRequestBehavior.AllowGet);
                        };
                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("VchrPosted", JsonRequestBehavior.AllowGet);
                    };
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetItemByQtyPrice(string Item, string LocNo)
        {

            if (!string.IsNullOrEmpty(Item))
            {
                var ItemUnit = (from i in _itemInfoService.All().ToList()
                                join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                where i.ItemCode == Item
                                select new { u.UnitName, i.HSCode, i.TaxHeadingNo }).FirstOrDefault();
                double qty = 0.0;
                CurrentStock currStocks = new CurrentStock();
                if (Session["MaintLot"].ToString() == "True")
                {

                    var existCurrStock = _currentStockService.All().Where(x => x.ItemCode == Item && x.LocCode == LocNo.Trim()).ToList();
                    foreach (var item in existCurrStock)
                    {
                        qty = qty + item.CurrQty;
                        currStocks.UnitPrice = item.UnitPrice;
                        currStocks.ItemCode = item.ItemCode;
                    }
                    currStocks.UnitName = ItemUnit.UnitName;
                    currStocks.HSCode = ItemUnit.HSCode;
                    currStocks.TaxHeadingNo = ItemUnit.TaxHeadingNo;
                    currStocks.CurrQty = qty;
                }
                else
                {
                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == Item && x.LocNo == LocNo.Trim()).ToList();
                    if (existCurrStoc.Count != 0)
                    {
                        var date = existCurrStoc.Max(x => x.TrDate);
                        var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == Item && x.LocNo == LocNo.Trim() && x.TrDate == date).FirstOrDefault();
                        currStocks.CurrQty = existCurrStock.BalQty;
                        currStocks.UnitPrice = existCurrStock.BalRate;
                        currStocks.ItemCode = existCurrStock.ItemCode;
                    }
                    else
                    {
                        currStocks.CurrQty = 0;
                        currStocks.UnitPrice = 0;
                        currStocks.ItemCode = "";
                    }
                    currStocks.UnitName = ItemUnit.UnitName;
                    currStocks.HSCode = ItemUnit.HSCode;
                    currStocks.TaxHeadingNo = ItemUnit.TaxHeadingNo;
                }

                return Json(currStocks, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetIsCreditBySupp(string isCredit)
        {
            if (isCredit == "on")
            {
                var supp = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.SrcDestName == "Supplier Purchase"), "SrcDestId", "SrcDestName");
                return Json(supp, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteFromDB(int Qty, string ItemCode, string RcvNo)
        {
            var LocNo = _receiveMainService.All().ToList().FirstOrDefault(m => m.RcvNo == RcvNo);
            var forLot = (from rcvM in _receiveMainService.All()
                          join rcvD in _receiveDetailsService.All()
                              on rcvM.RcvNo equals rcvD.RcvNo
                          where (rcvD.ItemCode == ItemCode)
                          select new
                          {
                              LotNo = rcvD.LotNo
                          }).Single();

            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == ItemCode &&
                                m.LocCode == LocNo.StoreLocCode &&
                                m.LotNo == forLot.LotNo);

            if (currentStocks != null && currentStocks.CurrQty >= 0)
            {
                currentStocks.CurrQty = currentStocks.CurrQty - Qty;
                _currentStockService.Update(currentStocks);
                _currentStockService.Save();
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public SelectList LoadStoreLocation(ILocationAppService _locationInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            //string logName = Session["UserName"].ToString();
            string sql = string.Format("EXEC loadLocation '" + branchCode + "'");
            var items = _locationInfoService.SqlQueary(sql).ToList()
                .Select(x => new { x.LocCode, x.LocName }).ToList();
            items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
        }

        public SelectList LoadRecvBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Receive";
            string logName = Session["UserName"].ToString();
            string functionName = "Received By";


            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }
        public SelectList LoadAppBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Receive";
            string logName = Session["UserName"].ToString();
            string functionName = "Approved By";
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }

        //public SelectList LoadPurpose(IItemInfoAppService _orderInfoService)
        //{
        //    var members = _orderInfoService.All().ToList();
        //    List<object> newList = new List<object>();
        //    foreach (var member in members)
        //        newList.Add(new
        //        {
        //            ItemCode = member.ItemCode,
        //            ItemName = member.ItemCode + " " + member.ItemName
        //        });
        //    return new SelectList(members.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
        //}


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
                return RedirectToAction("Index", "Receive", new { errMsg });

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
                return RedirectToAction("Index", "Receive", new { errMsg });
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
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("Index", "Receive", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }

        public ActionResult RcvNoIncreement(ReceiveMain model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var rcvNo = _receiveMainService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int No = Convert.ToInt32(rcvNo.RcvNo);
                string RcvNo = (No + 1).ToString();
                string IncRcvNo = "";
                if (RcvNo.Length > 7)
                {
                    IncRcvNo = RcvNo.ToString();
                }
                else
                {
                    IncRcvNo = "0" + RcvNo.ToString();
                }

                return Json(IncRcvNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetVchrView(string VchrNo, string Status)
        {
            var TranType = VchrNo.Substring(0, 2);

            string sql = string.Format("EXEC GetSourceEntry '" + Session["FinYear"].ToString() + "', '" + VchrNo + "','" + Status + "', '" + TranType + "'");
            IEnumerable<GetVchrViewVM> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<GetVchrViewVM>(sql).ToList();
            }
            ViewBag.VchrNo = VchrNo;
            return View("~/Views/JournalVoucher/GetVchrView.cshtml", VchrLst);

        }
    }
}
