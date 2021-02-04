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

namespace AcclineERP.Controllers
{
    public class IssueController : Controller
    {
        private readonly IIssueDetailsAppService _issueDetailsService;
        private readonly ILocationAppService _locationInfoService;
        private readonly IReceiveAppService _receiveMainService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly INewChartAppService _newChartService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        private readonly IIssueMainAppService _issueMainService;
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

        public IssueController(ILocationAppService _locationInfoService,
            INewChartAppService _newChartService, IItemInfoAppService _itemInfoService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IEmployeeAppService _employeeInfoService,
            IIssueDetailsAppService _issueDetailsService, ICurrentStockAppService _currentStockService,
            IIssRecSrcDestAppService _issRecvSrcDestService, IIssueMainAppService _issueMainService,
            IEmployeeFuncAppService _EmployeeFuncAppService, ITransactionLogAppService _transactionLogService,
            IReceiveAppService _receiveMainService, IReceiveDetailsAppService _receiveDetailsService,
            ILotDTAppService _lotDtService, IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IItemTypeAppService _itemTypeService, IBranchAppService _branchService, IVchrMainAppService VchrMainService,
            ICommonPVVMAppService _CommonVmService, ICostLedgerAppService CostLedgerAppService, IUnitAppService _unitService)
        {
            this.VchrMainService = VchrMainService;
            this._locationInfoService = _locationInfoService;
            this._newChartService = _newChartService;
            this._itemInfoService = _itemInfoService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._issueDetailsService = _issueDetailsService;
            this._currentStockService = _currentStockService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._issueMainService = _issueMainService;
            this._EmployeeFuncAppService = _EmployeeFuncAppService;
            this._transactionLogService = _transactionLogService;
            this._receiveMainService = _receiveMainService;
            this._receiveDetailsService = _receiveDetailsService;
            this._lotDtService = _lotDtService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._itemTypeService = _itemTypeService;
            this._branchService = _branchService;
            this._CommonVmService = _CommonVmService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._unitService = _unitService;
        }


        public ActionResult Issue(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                ViewBag.IssueNo = GenerateIssueNo(branchCode);


                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("II");
                ViewBag.GLEntries = CountEntries("II", datetime);
                // ViewBag.VchrNo = GenerateVoucherNo(DateTime.Now);

                ViewBag.IssTime = datetime.ToString("HH:mm:ss");
                ViewBag.FromLocCode = LoadStoreLocation(_locationInfoService);
                ViewBag.DesLocCode = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.Type == "D"), "SrcDestId", "SrcDestName");  //LoadDropDown.LoadIssuetype("D", _newChartService, _issRecvSrcDestService);

                ViewBag.Accode = LoadDropDown.onlyPurpose(_newChartService);//LoadDropDown.LoadAreaPurpuse("", _newChartService, _issRecvSrcDestService);
                ViewBag.ToLocCode = new SelectList(_subsidiaryInfoService.All().ToList(), "SubCode", "SubName");//LoadDropDown.LoadSubsidiary("", _subsidiaryInfoService, _issRecvSrcDestService);
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.ItemCode = LoadDropDown.LoadItem(_itemInfoService);
                ViewBag.IssueByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName"); //LoadIssueBy(_employeeInfoService);
                ViewBag.AppByCode = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName"); //LoadAppBy(_employeeInfoService);
                //ViewBag.ReceiveFrom = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "3"), "SubCode", "SubName");
                //ViewBag.AppByCode = new SelectList(_employeeInfoService.All().ToList().Where(x => x.Email == Session["UserName"].ToString()), "Id", "UserName");
                ViewBag.issPurAccode = LoadDropDown.LoadAreaPurpuse("", _newChartService, _issRecvSrcDestService);
                ViewBag.IssueToSubCode = LoadDropDown.LoadSubsidiary("", _subsidiaryInfoService, _issRecvSrcDestService);
                ViewBag.Source = new SelectList(_issRecvSrcDestService.All().ToList().Where(x => x.Type == "S"), "SrcDestId", "SrcDestName");

                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);

                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                //ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintLot = sysSet.MaintLot;
                ViewBag.MaintVAT = sysSet.MaintVAT;
                Session["MaintVAT"] = sysSet.MaintVAT;
                ViewBag.OnlyVAT = sysSet.OnlyVAT;
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

        public ActionResult GetExistVoucherNo(string VType)
        {
            string finYear = Session["FinYear"].ToString();
            string voucherNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sqls = string.Format("exec [GetNewVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sqls).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult SaveIssueInfo(IssueMain issueMain, List<IssueDetails> issueDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Issue_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var IfExit = _issueMainService.All().Where(x => x.IssueNo == issueMain.IssueNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        bool IsAutoLot = true;
                        var todayDate = DateTime.Now;
                        IssueMain issueInfo = new IssueMain();
                        issueInfo.IssueNo = issueMain.IssueNo;
                        issueInfo.BranchCode = Session["BranchCode"].ToString();
                        issueInfo.IssueDate = issueMain.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        issueInfo.FromLocCode = issueMain.FromLocCode;
                        //issueInfo.StoreLocCode = issueMain.StoreLocCode;
                        issueInfo.IssueToSubCode = issueMain.IssueToSubCode;
                        issueInfo.DesLocCode = issueMain.DesLocCode;
                        issueInfo.Accode = issueMain.Accode;
                        issueInfo.RefNo = issueMain.RefNo;
                        issueInfo.RefDate = issueMain.RefDate;
                        issueInfo.Remarks = issueMain.Remarks;
                        issueInfo.IssueByCode = issueMain.IssueByCode;
                        issueInfo.AppByCode = issueMain.AppByCode;
                        issueInfo.IssTime = issueMain.IssTime;
                        issueInfo.FinYear = Session["FinYear"].ToString();
                        issueInfo.GLPost = false;
                        issueInfo.IssDate = issueMain.IssDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        issueInfo.cashReceiptStatus = false;
                        issueInfo.IsReceived = false;
                        issueInfo.VchrNo = issueMain.VchrNo;
                        double amount = 0.0;
                        issueInfo.EntrySrc = "II";
                        issueInfo.EntrySrcNo = issueMain.IssueNo;


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
                                        currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == issueMain.FromLocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
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
                                        m.LocCode == issueMain.FromLocCode &&
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
                                        currStock.LocCode = issueMain.FromLocCode;
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
                                    issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                    issueDetailsInfo.Description = issuDetailsItem.Description;
                                    issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                    issueDetailsInfo.Price = issuDetailsItem.Price;
                                    issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                    issueDetailsInfo.LotNo = "01";
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

                                var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == issueMain.StoreLocCode.Trim()).ToList();
                                if (existCurrStoc.Count != 0)
                                {
                                    var date = existCurrStoc.Max(x => x.TrDate);
                                    var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == issueMain.StoreLocCode.Trim() && x.TrDate == date).FirstOrDefault();

                                    cLedger.BalQty = existCurrStock.BalQty - cLedger.IssQty;
                                    cLedger.BalRate = System.Math.Round( existCurrStock.BalRate, 2);
                                    cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                    cLedger.LocNo = issueMain.FromLocCode;
                                    cLedger.ItemCode = costLedger.ItemCode;
                                    cLedger.TrDate = issueMain.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = issueMain.IssueNo;
                                    cLedger.UpdSrc = "II";
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

                                    cLedger.LocNo = issueMain.FromLocCode;
                                    cLedger.ItemCode = costLedger.ItemCode;
                                    cLedger.TrDate = issueMain.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                    cLedger.UpdSrcNo = issueMain.IssueNo;
                                    cLedger.UpdSrc = "II";
                                    CostLedgerAppService.Add(cLedger);
                                }
                                CostLedgerAppService.Save();
                            }

                            foreach (var issuDetailsItem in issueDetails)
                            {
                                IssueDetails issueDetailsInfo = new IssueDetails();
                                issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                issueDetailsInfo.Description = issuDetailsItem.Description;
                                issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                issueDetailsInfo.Price = issuDetailsItem.Price;
                                issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                                issueDetailsInfo.LotNo = "01";
                                amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                issuDetailsList.Add(issueDetailsInfo);
                            }
                            #endregion
                        }
                        #endregion

                        List<VM_PurRegister1_6P1> VMPurReg1List = new List<VM_PurRegister1_6P1>();
                        List<VM_6P4> VM_6P4List = new List<VM_6P4>();
                        string ProdType = "";
                        if (issueMain.DesLocCodeText == "Contractual Production Issue")
                        {
                            ProdType = "6p4";
                        }
                        else if (issueMain.DesLocCodeText == "Production Issue")
                        {
                            ProdType = "6p1";
                        }


                        if (Convert.ToBoolean(Session["MaintVAT"]) == true && (ProdType == "6p1" || ProdType == "6p4"))
                        {
                            foreach (var item in issueDetails)
                            {
                                #region For VAT VM_PurRegister1_6P1
                                if (ProdType == "6p1")
                                {
                                    VM_PurRegister1_6P1 VMPurReg1 = new VM_PurRegister1_6P1();
                                    VMPurReg1.SerialNo = 0;
                                    VMPurReg1.PurDate = issueMain.IssueDate;
                                    VMPurReg1.OBQty = (decimal)item.ExQty;
                                    VMPurReg1.OBValue = 0;
                                    VMPurReg1.Ch_BENO = issueMain.IssueNo;
                                    VMPurReg1.Ch_BEDate = issueMain.IssueDate;
                                    VMPurReg1.SuppCode = issueMain.IssueToSubCode;
                                    VMPurReg1.SuppName = _subsidiaryInfoService.All().Where(s => s.SubCode == issueMain.IssueToSubCode).Select(s => s.SubName).FirstOrDefault();
                                    VMPurReg1.SuppAddr = "";
                                    VMPurReg1.R_E_N_No = "";
                                    VMPurReg1.ItemName = _itemInfoService.All().Where(s => s.ItemCode == item.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                                    VMPurReg1.PurQty = (decimal)item.Qty;
                                    VMPurReg1.PurValue = (decimal)item.Price;
                                    VMPurReg1.SuppTax = item.SupTaxAmt;
                                    VMPurReg1.Remarks = issueMain.Remarks;
                                    VMPurReg1.TranNo = issueMain.IssueNo;
                                    VMPurReg1.TranType = "Issue";
                                    VMPurReg1.ItemCode = issueMain.Remarks;
                                    VMPurReg1.ItemCode = item.ItemCode;
                                    VMPurReg1List.Add(VMPurReg1);
                                }
                                #endregion

                                #region For VAT VM_6P4
                                if (ProdType == "6p4")
                                {
                                    VM_6P4 VM_6P4 = new VM_6P4();
                                    VM_6P4.CPID = 0;
                                    VM_6P4.ChallanNo = issueMain.IssueNo;
                                    VM_6P4.ChallanDate = issueMain.IssueDate;
                                    VM_6P4.ChallanTime = new TimeSpan();
                                    VM_6P4.IssuedTo = issueMain.IssueNo;
                                    VM_6P4.IssuedToAddr = "";
                                    VM_6P4.IssuedToBIN = issueMain.IssueToSubCode;
                                    VM_6P4.UnitIn = (from i in _itemInfoService.All().ToList()
                                                     join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                                     where i.ItemCode == item.ItemCode
                                                     select u.UnitName).FirstOrDefault();
                                    VM_6P4.SerialNo = 0;
                                    VM_6P4.ChallanQty = (decimal)item.Qty;
                                    VM_6P4.ItemName = _itemInfoService.All().Where(s => s.ItemCode == item.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                                    VM_6P4.ItemCode = item.ItemCode;
                                    VM_6P4.HeadingNo = _itemInfoService.All().Where(s => s.ItemCode == item.ItemCode).Select(s => s.TaxHeadingNo).FirstOrDefault();
                                    VM_6P4.HSCode = _itemInfoService.All().Where(s => s.ItemCode == item.ItemCode).Select(s => s.HSCode).FirstOrDefault();

                                    VM_6P4.IssueNo = issueMain.IssueNo;
                                    VM_6P4.IssuePurpose = issueMain.DesLocCodeText;
                                    VM_6P4.IssuedBy = issueMain.IssueByCode;
                                    //VM_6P4.IssuedDesig = issueMain.Remarks;
                                    VM_6P4.IssueTime = TimeSpan.Parse(issueMain.IssTime.ToString("hh:mm tt"));
                                    VM_6P4List.Add(VM_6P4);
                                }
                                #endregion
                            }
                        }


                        issueInfo.Amount = amount;
                        issueInfo.IssueDetails = issuDetailsList;
                        _issueMainService.Add(issueInfo);
                        _issueMainService.Save();
                        _currentStockService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Issue", "Save", issueInfo.IssueNo, Session["UserName"].ToString());
                        LoadDropDown.ProvitionInvRSave("II", "I", issueMain.IssueNo, issueMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), issueInfo.IssueDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());

                        #region For VAT api
                        string returnValue = "";
                        if (Convert.ToBoolean(Session["MaintVAT"]) == true && ProdType == "6p4")
                        {
                            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                            string json = JsonConvert.SerializeObject(VM_6P4List, Formatting.Indented, serializerSettings);
                            var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_6P4", VM_6P4List).Result;
                            returnValue = respse.Content.ReadAsAsync<string>().Result;
                        }
                        else if (Convert.ToBoolean(Session["MaintVAT"]) == true && ProdType == "6p1")
                        {
                            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                            string json = JsonConvert.SerializeObject(VMPurReg1List, Formatting.Indented, serializerSettings);
                            var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_6P1", VMPurReg1List).Result;
                            returnValue = respse.Content.ReadAsAsync<string>().Result;
                        }
                        #endregion

                        transaction.Complete();
                        return Json(new { Msg = "1", returnValue = returnValue }, JsonRequestBehavior.AllowGet);

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

        public ActionResult UpdateIssueInfo(IssueMain issueMain, List<IssueDetails> issueDetails, List<CurrentStock> currentStock)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Issue_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    var vMain = VchrMainService.All().ToList().FirstOrDefault(s => s.VchrNo == issueMain.VchrNo);
                    if (vMain == null)
                    {
                        var issueInfo = _issueMainService.All().FirstOrDefault(x => x.IssueNo == issueMain.IssueNo);
                        if (issueInfo != null)
                        {
                            bool IsLotMaintain = false;
                            bool IsAutoLot = true;

                            var todayDate = DateTime.Now;
                            issueInfo.IssueNo = issueMain.IssueNo;
                            issueInfo.BranchCode = Session["BranchCode"].ToString();
                            issueInfo.IssueDate = issueMain.IssueDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.IssueToSubCode = issueMain.ToLocCode;
                            issueInfo.FromLocCode = issueMain.FromLocCode;
                            issueInfo.DesLocCode = issueMain.DesLocCode;
                            issueInfo.Accode = issueMain.Accode;
                            issueInfo.RefNo = issueMain.RefNo;
                            issueInfo.RefDate = issueMain.RefDate;
                            issueInfo.Remarks = issueMain.Remarks;
                            issueInfo.IssueByCode = issueMain.IssueByCode;
                            issueInfo.AppByCode = issueMain.AppByCode;
                            issueInfo.IssTime = issueMain.IssTime;
                            issueInfo.FinYear = Session["FinYear"].ToString();
                            issueInfo.GLPost = false;
                            issueInfo.IssDate = issueMain.IssDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                            issueInfo.cashReceiptStatus = false;
                            issueInfo.IsReceived = false;
                            issueInfo.IssueDetails.Clear();
                            issueInfo.IssueDetails = new List<IssueDetails>();
                            double amount = 0.0;
                            issueInfo.EntrySrc = "II";
                            issueInfo.EntrySrcNo = issueMain.IssueNo;

                            //if (issueInfo.AppByCode != "" || issueInfo.AppByCode != "---- Select ----")
                            //{


                                if (IsLotMaintain == true)
                                {
                                    if (IsAutoLot == true)
                                    {
                                        #region AutoLot by issueDetail
                                        var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueMain.IssueNo).ToList();
                                        foreach (var issItem in issueDtls)
                                        {
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                               m.LocCode == issueMain.FromLocCode &&
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
                                                currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == issueMain.FromLocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
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
                                                    issueInfo.IssueDetails.Add(issueDetailsInfo);

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
                                                    issueInfo.IssueDetails.Add(issueDetailsInfo);

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

                                        var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueMain.IssueNo).ToList();
                                        foreach (var issItem in issueDtls)
                                        {
                                            var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                               m.LocCode == issueMain.FromLocCode &&
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
                                                m.LocCode == issueMain.FromLocCode &&
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
                                            //var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issuDetailsItem.ItemCode &&
                                            //   m.LocCode == issueMain.FromLocCode &&
                                            //   m.LotNo == "01");
                                            string sql = string.Format("Select * from IssueDetails where IssueNo = '" + issuDetailsItem.IssueNo + "' and ItemCode = '" + issuDetailsItem.ItemCode + "' and LotNo = '01'");
                                            var exIssQty = _issueDetailsService.SqlQueary(sql).ToList();

                                            IssueDetails issueDetailsInfo = new IssueDetails();
                                            issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                                            issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                                            issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                            issueDetailsInfo.Description = issuDetailsItem.Description;
                                            issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                            issueDetailsInfo.Price = issuDetailsItem.Price;
                                            issueDetailsInfo.ExQty = (exIssQty.Count != 0) ? exIssQty.FirstOrDefault().ExQty : issuDetailsItem.ExQty;
                                            issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                                            amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                            issueInfo.IssueDetails.Add(issueDetailsInfo);

                                        }


                                        #endregion autolot end
                                    }
                                }
                                else
                                {
                                    #region Not AutoLot by issueDetail


                                    var issueDtls = _issueDetailsService.All().ToList().Where(m => m.IssueNo == issueMain.IssueNo).ToList();
                                    foreach (var issItem in issueDtls)
                                    {
                                        var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == issItem.ItemCode &&
                                           m.LocCode == issueMain.FromLocCode &&
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
                                           m.LocCode == issueMain.FromLocCode &&
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
                                            currStock.LocCode = issueMain.FromLocCode;
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
                                        issueDetailsInfo.SubCode = issueMain.IssueToSubCode;
                                        issueDetailsInfo.Description = issuDetailsItem.Description;
                                        issueDetailsInfo.Qty = issuDetailsItem.Qty;
                                        issueDetailsInfo.Price = issuDetailsItem.Price;
                                        issueDetailsInfo.ExQty = (exIssQty.Count != 0) ? exIssQty.FirstOrDefault().ExQty : issuDetailsItem.ExQty;
                                        issueDetailsInfo.LotNo = "01";
                                        amount = amount + (issuDetailsItem.Qty * issuDetailsItem.Price);
                                        issueInfo.IssueDetails.Add(issueDetailsInfo);
                                    }
                                    #endregion autolot end
                                }
                                issueInfo.Amount = amount;
                                _issueMainService.Update(issueInfo);
                                _issueDetailsService.All().ToList().Where(y => string.IsNullOrEmpty(y.IssueNo)).ToList().ForEach(x => _issueDetailsService.Delete(x));
                                _issueMainService.Save();
                                _currentStockService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Issue", "Update", issueInfo.IssueNo, Session["UserName"].ToString());
                                //delete to provition 
                                var entryNo = Convert.ToInt64(issueMain.VchrNo.Substring(4, 8)).ToString().PadLeft(8, '0');
                                LoadDropDown.journalVoucherRemoval("II", entryNo, issueMain.VchrNo, Session["FinYear"].ToString());
                                LoadDropDown.ProvitionInvRSave("II", "I", issueMain.IssueNo, issueMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), issueInfo.IssueDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                                transaction.Complete();

                                return Json("1", JsonRequestBehavior.AllowGet);
                            //}
                            //else
                            //{
                            //    _issueMainService.Update(issueInfo);
                            //    _issueMainService.Save();
                            //    TransactionLogService.SaveTransactionLog(_transactionLogService, "Issue", "Update", issueInfo.IssueNo, Session["UserName"].ToString());
                            //    //delete to provition 
                            //    var entryNo = Convert.ToInt64(issueMain.VchrNo.Substring(2, 10)).ToString().PadLeft(10, '0');
                            //    LoadDropDown.journalVoucherRemoval("II", entryNo, issueMain.VchrNo, Session["FinYear"].ToString());
                            //    LoadDropDown.ProvitionInvRSave("II", "I", issueMain.IssueNo, issueMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), issueInfo.IssueDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                            //    transaction.Complete();

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

        public ActionResult GetAllByRefNo(string refNo)
        {

            RecvMainVM recvInfo = new RecvMainVM();
            RecvDetailsVM recvDetail = new RecvDetailsVM();
            recvInfo.Main = _receiveMainService.All().ToList().FirstOrDefault(x => x.RefNo == refNo.Trim());
            var items = _receiveDetailsService.All().ToList().Where(x => x.RcvNo == recvInfo.Main.RcvNo.Trim()).ToList();

            recvInfo.Main.RcvDetails = null;
            if (items.Count > 0)
            {
                int i = 0;
                foreach (var item in items)
                {
                    var CurrStock = _currentStockService.All().FirstOrDefault(s => s.ItemCode == item.ItemCode && s.LocCode == recvInfo.Main.StoreLocCode && s.LotNo == item.LotNo);
                    recvInfo.Details.Add(new RecvDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, CurrStock.CurrQty, item.Price, item.LotNo, item.RcvNo, item.Price, item.Description));
                }
            }
            var check = Json(recvInfo, JsonRequestBehavior.AllowGet);
            return Json(recvInfo, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteFromDB(int Qty, string ItemCode, string IssueNo, string LotNo)
        {
            bool IsLotMaintain = false;
            bool IsAutoLot = true;
            if (IsLotMaintain == true)
            {
                if (IsAutoLot == true)
                {
                    var forLot = (from issM in _issueMainService.All()
                                  join issD in _issueDetailsService.All()
                                      on issM.IssueNo equals issD.IssueNo
                                  where (issD.ItemCode == ItemCode)
                                  select new
                                  {
                                      LotNo = issD.LotNo
                                  }).ToList();
                    var LocNo = _issueMainService.All().ToList().FirstOrDefault(m => m.IssueNo == IssueNo).FromLocCode;
                    var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == ItemCode &&
                                        m.LocCode == LocNo &&
                                        m.LotNo == forLot.FirstOrDefault().LotNo);
                    if (currentStocks != null && currentStocks.CurrQty >= 0)
                    {
                        currentStocks.CurrQty = currentStocks.CurrQty + Qty;
                        _currentStockService.Update(currentStocks);
                        _currentStockService.Save();
                    }
                }
                else
                {
                    if (LotNo != null && LotNo != "01")
                    {
                        var LocNo = _issueMainService.All().ToList().FirstOrDefault(m => m.IssueNo == IssueNo).FromLocCode;
                        var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == ItemCode &&
                                            m.LocCode == LocNo &&
                                            m.LotNo == LotNo);
                        if (currentStocks != null && currentStocks.CurrQty >= 0)
                        {
                            currentStocks.CurrQty = currentStocks.CurrQty + Qty;
                            _currentStockService.Update(currentStocks);
                            _currentStockService.Save();
                        }
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                var LocNo = _issueMainService.All().ToList().FirstOrDefault(m => m.IssueNo == IssueNo).FromLocCode;
                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == ItemCode &&
                                    m.LocCode == LocNo &&
                                    m.LotNo == "01");
                if (currentStocks != null && currentStocks.CurrQty >= 0)
                {
                    currentStocks.CurrQty = currentStocks.CurrQty + Qty;
                    _currentStockService.Update(currentStocks);
                    _currentStockService.Save();
                }

            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByCode(string issueNo)
        {

            IssueMainVM issueInfo = new IssueMainVM();
            IssueDetailsVM issueDetail = new IssueDetailsVM();
            issueInfo.Main = _issueMainService.All().ToList().FirstOrDefault(x => x.IssueNo == issueNo.Trim());
            var items = _issueDetailsService.All().ToList().Where(x => x.IssueNo == issueNo.Trim()).ToList();

            issueInfo.Main.IssueDetails = null;
            if (items.Count > 0)
            {
                int i = 0;
                foreach (var item in items)
                {

                    // issueInfo.Details.Add(new IssueDetailsVM());
                    //issueInfo.Details.Add(new IssueDetailsVM(item.ItemCode, item.Description, item.Qty, item.Price, item.LotNo, item.ExQty, item.Items.ItemName));

                    issueInfo.Details.Add(new IssueDetailsVM(item.ItemCode, item.Items.ItemName, item.Qty, item.Price, item.IssueNo, item.LotNo, item.Price, item.Description, item.ExQty));

                }
            }

            string returnValue = "";
            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
            {
                string respse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetVM_6P1_6p4?TransNo=" + issueNo.ToString(), Session["token"].ToString());
                JavaScriptSerializer js = new JavaScriptSerializer();
                returnValue = js.Deserialize<string>(respse);
            }
            return Json(new { issueInfo = issueInfo, returnValue = returnValue }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPurpuseBySrcDest(string DesLocCode)
        {
            return Json(LoadDropDown.LoadAreaPurpuse(DesLocCode, _newChartService, _issRecvSrcDestService), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetSubsidiaryBySrcDest(string DesLocCode)
        {
            return Json(LoadDropDown.LoadSubsidiary(DesLocCode, _subsidiaryInfoService, _issRecvSrcDestService), JsonRequestBehavior.AllowGet);

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



        public SelectList LoadStoreLocation(ILocationAppService _locationInfoService)
        {
            // string logName = Session["UserName"].ToString();
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC loadLocation '" + branchCode + "'");
            var items = _locationInfoService.SqlQueary(sql).ToList()
                .Select(x => new { x.LocCode, x.LocName }).ToList();
            items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
        }
        public SelectList LoadIssueBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Issue";
            string logName = Session["UserName"].ToString();
            string functionName = "Issued By";
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }
        public SelectList LoadAppBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Issue";
            string logName = Session["UserName"].ToString();
            string functionName = "Approved By";
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "', '" + form + "', '" + logName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }

        public ActionResult GetItemByitemType(string ItemType)
        {

            return Json(LoadDropDown.ByItmeType(ItemType, _itemInfoService), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Issue_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("Issue", "Issue", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("Issue", "Issue", new { errMsg });
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
                return RedirectToAction("Issue", "Issue", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }


        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Issue_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("Issue", "Issue", new { errMsg });
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
                return RedirectToAction("Issue", "Issue", new { errMsg });

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

        public ActionResult IssueNoIncreement(IssueMain model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var issueNo = _issueMainService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int No = Convert.ToInt32(issueNo.IssueNo);
                string IssueNo = (No + 1).ToString();
                string IncIssueNo = "";
                if (IssueNo.Length > 7)
                {
                    IncIssueNo = IssueNo.ToString();
                }
                else
                {
                    IncIssueNo = "0" + IssueNo.ToString();
                }

                return Json(IncIssueNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
    }
}
