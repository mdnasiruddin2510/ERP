using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Context;
using AcclineERP.Models;
using System.Transactions;
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class CashOperationController : Controller
    {
        private readonly ICashOperationAppService _CashOperationService;
        private readonly ICashReceiptAppService _cashReceiptAppService;
        private readonly IWithdrawAppService _withdrawService;
        private readonly IExpenseAppService _ExpenseService;
        private readonly IDepositToBankAppService _DepositToBankService;
        private readonly INewChartAppService _NewChartService;
        private readonly ISubsidiaryInfoAppService _subsidiaryService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IItemInfoAppService _ItemService;
        private readonly ILocationAppService _LocationService;
        private readonly IUnitAppService _UnitService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IAcBRAppService _AcBRService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ISubsidiaryCtrlAppService _subsidiaryCtrlService;
        private readonly IOpenBalAppService _OpenBalService;
        private readonly IOpnBalAppService _OpnBalService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly IBranchAppService _BranchService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly IReceiveDetailsAppService _receiveDetailsService;
        private readonly IReceiveAppService _receiveMainService;
        private readonly IPayInvRecvAppService _PayInvRecvService;
        private readonly ISysSetAppService _sysSetService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IExpenseSubDetailsAppService _expenseSubDetailsService;
        private readonly ICashReceiptSubDetailsAppService _CashReceiptSubDetailsAppService;
        private readonly ISecUserInfoAppService _SecUserInfoService;
        //private readonly ICustomerOpeningAppService _CustomerOpeningService;

        public CashOperationController(ICashOperationAppService _CashOperationService, ICashReceiptAppService _cashReceiptAppService,
            IWithdrawAppService _withdrawService, IExpenseAppService _ExpenseService, IDepositToBankAppService _DepositToBankService,
            IEmployeeAppService _EmployeeService, ISubsidiaryInfoAppService _subsidiaryService, IItemInfoAppService _ItemService,
            INewChartAppService _NewChartService, ILocationAppService _LocationService, IUnitAppService _UnitService,
            IIssRecSrcDestAppService _issRecvSrcDestService, ITransactionLogAppService _transactionLogService,
            IAcBRAppService _AcBRService, ICurrentStockAppService _currentStockService,
            ISubsidiaryCtrlAppService _subsidiaryCtrlService, IOpenBalAppService _OpenBalService, IOpnBalAppService _OpnBalService,
            IJarnalVoucherAppService _jarnalVoucherService, IBranchAppService _BranchService,
            ICashReceiptSubDetailsAppService _CashReceiptSubDetailsAppService, IItemTypeAppService _itemTypeService,
            IExpenseSubDetailsAppService _expenseSubDetailsService, IReceiveDetailsAppService _receiveDetailsService,
            IReceiveAppService _receiveMainService, IPayInvRecvAppService _PayInvRecvService, ISysSetAppService _sysSetService,
            ICommonPVVMAppService _CommonVmService, ISecUserInfoAppService _SecUserInfoService)
        {
            this._CashOperationService = _CashOperationService;
            this._cashReceiptAppService = _cashReceiptAppService;
            this._withdrawService = _withdrawService;
            this._ExpenseService = _ExpenseService;
            this._DepositToBankService = _DepositToBankService;
            this._subsidiaryService = _subsidiaryService;
            this._NewChartService = _NewChartService;
            this._EmployeeService = _EmployeeService;
            this._subsidiaryService = _subsidiaryService;
            this._ItemService = _ItemService;
            this._LocationService = _LocationService;
            this._UnitService = _UnitService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._transactionLogService = _transactionLogService;
            this._AcBRService = _AcBRService;
            this._currentStockService = _currentStockService;
            this._subsidiaryCtrlService = _subsidiaryCtrlService;
            this._OpenBalService = _OpenBalService;
            this._OpnBalService = _OpnBalService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._expenseSubDetailsService = _expenseSubDetailsService;
            this._CashReceiptSubDetailsAppService = _CashReceiptSubDetailsAppService;
            this._BranchService = _BranchService;
            this._itemTypeService = _itemTypeService;
            this._receiveMainService = _receiveMainService;
            this._receiveDetailsService = _receiveDetailsService;
            this._PayInvRecvService = _PayInvRecvService;
            this._sysSetService = _sysSetService;
            this._CommonVmService = _CommonVmService;
            this._SecUserInfoService = _SecUserInfoService;
        }

        public ActionResult CashOperation(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.branchCode = _BranchService.All().ToList().Where(x => x.BranchCode == branchCode).Select(s => s.BranchName).FirstOrDefault();
                List<NewChart> charts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "3" || x.Accode.Substring(0, 1) == "2").ToList();
                List<NewChart> finalList = new List<NewChart>();

                foreach (var chart in charts)
                {
                    bool key = true;
                    foreach (var temp in charts)
                    {
                        if (chart == temp)
                        {

                        }
                        else
                        {
                            if (chart.Accode == temp.ParentAcCode)
                            {
                                key = false;
                            }
                        }
                    }
                    if (key)
                    {
                        finalList.Add(chart);
                    }
                }

                List<NewChart> echarts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "4").ToList();
                List<NewChart> efinalList = new List<NewChart>();

                foreach (var chart in echarts)
                {
                    bool key = true;
                    foreach (var temp in echarts)
                    {
                        if (chart == temp)
                        {

                        }
                        else
                        {
                            if (chart.Accode == temp.ParentAcCode)
                            {
                                key = false;
                            }
                        }
                    }
                    if (key)
                    {
                        efinalList.Add(chart);
                    }
                }
                var banks = _AcBRService.All().ToList().Where(x => x.BranchCode == Session["BranchCode"].ToString());
                //var banks = _AcBRService.All().ToList();
                List<NewChart> bfinalList = new List<NewChart>();

                foreach (var item in banks)
                {
                    var bank = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == item.Accode);
                    bfinalList.Add(bank);
                }

                string user = Session["UserName"].ToString();

                //var userInfo = _EmployeeService.All().ToList().FirstOrDefault(x => x.Email == user);
                var userInfo = _SecUserInfoService.All().ToList().FirstOrDefault(x => x.UserName == user);
                ViewBag.ClosedBy = userInfo.UserName;
                ViewBag.OpenBal = GetOpenBal(DateTime.Now);


                ViewBag.GLProvition = Count("COR");
                ViewBag.GLEntries = CountEntries("COR", DateTime.Now);
                ViewBag.GLProvitionB = Count("COP");
                ViewBag.GLEntriesB = CountEntries("COP", DateTime.Now);
                //ViewBag.GLProvition = Counter("COR", "PV", DateTime.Now.ToString("MM/dd/yyyy"));
                //ViewBag.GLEntries = Counter("COR","MV",DateTime.Now.ToString("MM/dd/yyyy"));
                //ViewBag.GLProvitionB = Counter("COP", "PV", DateTime.Now.ToString("MM/dd/yyyy"));
                //ViewBag.GLEntriesB = Counter("COP", "MV", DateTime.Now.ToString("MM/dd/yyyy"));


                //ViewBag.GLProvition = Counter("COR");
                //ViewBag.GLEntries = Counter("COR");
                //ViewBag.GLProvitionB = Counter("COP");
                //ViewBag.GLEntriesB = Counter("COP");

                DateTime datetime = DateTime.Now;
                ViewBag.ClosingTime = datetime.ToString("HH:mm:ss");
                ViewBag.ReceiptTime = datetime.ToString("HH:mm:ss");
                ViewBag.purAccode = new SelectList(finalList, "Accode", "AcName");
                ViewBag.EAccode = new SelectList(efinalList, "Accode", "AcName");
                ViewBag.BAccode = new SelectList(bfinalList, "Accode", "AcName");
                ViewBag.FromLocCode = LoadStoreLocation(_LocationService);

                ViewBag.Item = LoadDropDown.LoadItem(_ItemService);
                //ViewBag.Purpose = LoadPurpose(string destinationId);
                var cash = new List<CashOperationVModel>();
                ViewBag.Date = datetime.ToString("dd/MM/yyyy");

                // string branchCode = Session["BranchCode"].ToString();
                cash = GetAllCashReceiptAndWithdraw(datetime, branchCode);
                GetAllExpenseAndDeposit(datetime, branchCode).ForEach(x => cash.Add(x));
                ViewBag.Items = cash;

                //Pop Up for Receive
                ViewBag.rcvPurAccode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
                ViewBag.Source = LoadDropDown.LoadSrcDestByPurpose("", _NewChartService, _issRecvSrcDestService);
                ViewBag.ReceiveFrom = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
                ViewBag.RcvdByCode = LoadRcvdBy(_EmployeeService);
                ViewBag.RcvAppByCode = LoadRcvAppBy(_EmployeeService);

                //Pop Up for Issue
                ViewBag.issPurAccode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
                ViewBag.FromLocCode = LoadStoreLocation(_LocationService);
                ViewBag.DesLocCode = LoadDropDown.LoadSrcDestByPurpose("", _NewChartService, _issRecvSrcDestService);
                ViewBag.IssueToSubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
                ViewBag.IssueByCode = LoadIssuedBy(_EmployeeService);
                ViewBag.IssAppByCode = LoadIssAppBy(_EmployeeService);

                //Common Issue Receive Popup 

                ViewBag.Accode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
                ViewBag.ItemCode = LoadDropDown.LoadItem(_ItemService);
                ViewBag.Item = LoadDropDown.LoadItem(_ItemService);

                //Cashpayment Receive
                ViewBag.FromLocCode = LoadStoreLocation(_LocationService);
                ViewBag.RecvItem = LoadDropDown.LoadItem(_ItemService);
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                // Subsidiary Popup

                ViewBag.SubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
                var sysSet = _sysSetService.All().FirstOrDefault();
                ViewBag.MrConv = sysSet.MRConv;
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                ViewBag.MaintLot = sysSet.MaintLot;
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();

                //For Cash Payment Receive option show/hide
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (rUser.HasPermissionToMenu("Store") && rUser.HasPermissionToMenu("ItemWiseCashPayment"))
                {
                    ViewBag.CPReceive = true;
                }
                ViewBag.errMsg = errMsg;
                //ViewBag.CashRule = sysSet.CashRule;
                //For us Culture Ex: 0.00
                const string culture = "en-US";
                CultureInfo ci = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        //public ActionResult GetCountNo(DateTime date)
        //{
        //   ViewBag.GLProvition = Counter("COR", "PV", date.ToString());
        //   ViewBag.GLEntries = Counter("COR", "MV", date.ToString());
        //   ViewBag.GLProvitionB = Counter("COP", "PV", date.ToString());
        //   ViewBag.GLEntriesB = Counter("COP", "MV", date.ToString());

        //   return View("~/Views/CashOperation/CashOperation.cshtml");
        //   //return Json("0", JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult CashOperation(DateTime TrDate)
        {
            string branchCode = Session["BranchCode"].ToString();
            ViewBag.branchCode = _BranchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
            List<NewChart> charts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "3" || x.Accode.Substring(0, 1) == "2").ToList();
            List<NewChart> finalList = new List<NewChart>();

            foreach (var chart in charts)
            {
                bool key = true;
                foreach (var temp in charts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    finalList.Add(chart);
                }
            }

            List<NewChart> echarts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "4").ToList();
            List<NewChart> efinalList = new List<NewChart>();

            foreach (var chart in echarts)
            {
                bool key = true;
                foreach (var temp in echarts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    efinalList.Add(chart);
                }
            }

            var banks = _AcBRService.All().ToList().Where(x => x.BranchCode == Session["BranchCode"].ToString());
            //var banks = _AcBRService.All().ToList();
            List<NewChart> bfinalList = new List<NewChart>();

            foreach (var item in banks)
            {
                var bank = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == item.Accode);
                bfinalList.Add(bank);
            }


            //var cashOperation = _CashOperationService.All().ToList().FirstOrDefault(x => x.TrDate == TrDate);
            //if(cashOperation.IsClosed==true)
            //{
            //    ViewBag.ClosedBy = cashOperation.ClosedBy;
            //}
            //else
            //{
            string user = Session["UserName"].ToString();
            var userInfo = _SecUserInfoService.All().ToList().FirstOrDefault(x => x.UserName == user);
            //var userInfo = _EmployeeService.All().ToList().FirstOrDefault(x => x.Email == user);
            ViewBag.ClosedBy = userInfo.UserName;
            //}
            ViewBag.OpenBal = GetOpenBal(TrDate);


            DateTime datetime = TrDate;
            ViewBag.purAccode = new SelectList(finalList, "Accode", "AcName");
            ViewBag.EAccode = new SelectList(efinalList, "Accode", "AcName");
            ViewBag.BAccode = new SelectList(bfinalList, "Accode", "AcName");
            ViewBag.FromLocCode = LoadStoreLocation(_LocationService);

            ViewBag.Item = LoadDropDown.LoadItem(_ItemService);
            //ViewBag.Purpose = LoadPurpose(string destinationId);
            var cash = new List<CashOperationVModel>();
            ViewBag.Date = datetime.ToString("dd/MM/yyyy");


            ViewBag.GLProvition = Count("COR");
            ViewBag.GLEntries = CountEntries("COR", datetime);
            ViewBag.GLProvitionB = Count("COP");
            ViewBag.GLEntriesB = CountEntries("COP", datetime);

            cash = GetAllCashReceiptAndWithdraw(datetime, branchCode);
            GetAllExpenseAndDeposit(datetime, branchCode).ForEach(x => cash.Add(x));
            ViewBag.Items = cash;

            //Pop Up for Receive
            ViewBag.rcvPurAccode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
            ViewBag.Source = LoadDropDown.LoadSrcDestByPurpose("", _NewChartService, _issRecvSrcDestService);
            ViewBag.ReceiveFrom = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
            ViewBag.RcvdByCode = LoadRcvdBy(_EmployeeService);
            ViewBag.RcvAppByCode = LoadRcvAppBy(_EmployeeService);

            //Pop Up for Issue
            ViewBag.issPurAccode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
            ViewBag.FromLocCode = LoadStoreLocation(_LocationService);
            ViewBag.DesLocCode = LoadDropDown.LoadSrcDestByPurpose("", _NewChartService, _issRecvSrcDestService);
            ViewBag.IssueToSubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
            ViewBag.IssueByCode = LoadIssuedBy(_EmployeeService);
            ViewBag.IssAppByCode = LoadIssAppBy(_EmployeeService);

            //Common Issue Receive Popup 

            ViewBag.Accode = LoadDropDown.LoadAreaPurpuse("", _NewChartService, _issRecvSrcDestService);
            ViewBag.ItemCode = LoadDropDown.LoadItem(_ItemService);
            ViewBag.Item = LoadDropDown.LoadItem(_ItemService);


            //Cashpayment Receive
            ViewBag.FromLocCode = LoadStoreLocation(_LocationService);
            ViewBag.RecvItem = LoadDropDown.LoadItem(_ItemService);

            var sysSet = _sysSetService.All().FirstOrDefault();
            ViewBag.MrConv = sysSet.MRConv;
            ViewBag.MaintJob = sysSet.MaintJob;
            Session["MaintLot"] = sysSet.MaintLot;
            ViewBag.MaintLot = sysSet.MaintLot;
            #region For item Filtering option
            ViewBag.NoGrp = sysSet.NoGrp;
            ViewBag.OnlyGrp = sysSet.OnlyGrp;
            ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
            ViewBag.SubSubGrp = sysSet.SubSubGrp;
            #endregion

            ViewBag.JobNo = LoadDropDown.LoadJobInfo();
            ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
            ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
            ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
            ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
            // Subsidiary Popup

            ViewBag.SubCode = LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
            //For Cash Payment Receive option show/hide
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (rUser.HasPermissionToMenu("Store") && rUser.HasPermissionToMenu("ItemWiseCashPayment"))
            {
                ViewBag.CPReceive = true;
            }
            //ViewBag.CashRule = sysSet.CashRule;

            //For us Culture Ex: 0.00
            const string culture = "en-US";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View();

        }

        //public double GetOpenBal(DateTime date)
        //{
        //    double openBal = 0;
        //    var cashOperation = _CashOperationService.All().ToList().LastOrDefault(x => Convert.ToDateTime(x.TrDate.ToString("MM-dd-yyyy")) < Convert.ToDateTime(date.ToString("MM-dd-yyyy")));
        //    if (cashOperation != null)
        //    {
        //        openBal = Convert.ToDouble(cashOperation.CloseBal);
        //    }
        //    else
        //    {
        //        openBal = 0;
        //        var custOpenBal = _OpenBalService.All().ToList();
        //        foreach (var item in custOpenBal)
        //        {
        //            openBal = openBal + item.OpenBalance;
        //        }
        //    }
        //    return openBal;
        //}

        public double GetOpenBal(DateTime date)
        {
            double OpenBal = 0;
            string branchCode = Session["BranchCode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string sql = string.Format("EXEC GetCashOpenBal '" + finYear + "', '" + branchCode + "', '" + date.ToString("MM/dd/yyyy") + "'");
            //var d1 = _OpnBalService.SqlQueary(sql).ToList();
            //var d2 = d1.FirstOrDefault();
            //var d3 = d2.OpenBalance;

            OpenBal = Convert.ToDouble(_OpnBalService.SqlQueary(sql).ToList().FirstOrDefault().OpenBalance.ToString());
            return OpenBal;
        }


        public ActionResult GetSrcDestByPurpose(string purAccode)
        {
            return Json(LoadDropDown.LoadSrcDestByPurpose(purAccode, _NewChartService, _issRecvSrcDestService), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIssRcvPurByPurpose(string purAccode)
        {
            return Json(LoadDropDown.LoadPurposeCO(purAccode, _NewChartService), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubsidiaryByPurpose(string purAccode)
        {
            return Json(LoadDropDown.LoadSubsidiaryByPurpuse(purAccode, _subsidiaryService, _subsidiaryCtrlService), JsonRequestBehavior.AllowGet);

        }

        public List<CashOperationVModel> GetAllCashReceiptAndWithdraw(DateTime dateTime, string branchCode)
        {
            var cash = new List<CashOperationVModel>();
            int i = 0;
            _cashReceiptAppService.All().ToList().Where(x => x.BranchCode == branchCode && x.ReceiptDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                 new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.ReceiptNo, "CR", x.VoucherNo)
                ));
            _withdrawService.All().ToList().Where(x => x.BranchCode == branchCode && x.WithdrawDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.WithdrawNo, "BW", x.VoucherNo)));
            return cash;
        }

        public List<CashOperationVModel> GetAllExpenseAndDeposit(DateTime dateTime, string branchCode)
        {
            var cash = new List<CashOperationVModel>();
            int i = 0;
            _ExpenseService.All().ToList().Where(x => x.BranchCode == branchCode && x.ExpenseDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.ExpenseNo, "CP", x.VoucherNo)
               ));

            _DepositToBankService.All().ToList().Where(x => x.BranchCode == branchCode && x.DepositDate.ToString("MM-dd-yyyy") == dateTime.ToString("MM-dd-yyyy")).ToList().ForEach(x => cash.Add(
                new CashOperationVModel(++i, x.NewChart.AcName, x.Amount, x.DepositNo, "BD", x.VoucherNo)));
            return cash;
        }
        public SelectList LoadStoreLocation(ILocationAppService _LocationService)
        {
            string brCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC loadLocation '" + brCode + "'");
            var items = _LocationService.SqlQueary(sql).ToList()
                .Select(x => new { x.LocCode, x.LocName }).ToList();
            items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
        }

        public SelectList LoadRcvdBy(IEmployeeAppService _employeeInfoService)
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
        public SelectList LoadRcvAppBy(IEmployeeAppService _employeeInfoService)
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

        public SelectList LoadIssuedBy(IEmployeeAppService _employeeInfoService)
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
        public SelectList LoadIssAppBy(IEmployeeAppService _employeeInfoService)
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

        public SelectList LoadPurpose(string destinationId)
        {
            var IssRcv = _issRecvSrcDestService.All().ToList().FirstOrDefault(x => x.SrcDestId == destinationId);
            List<NewChart> pcharts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == IssRcv.ActionCtrl).ToList();
            List<NewChart> pfinalList = new List<NewChart>();

            foreach (var chart in pcharts)
            {
                bool key = true;
                foreach (var temp in pcharts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    pfinalList.Add(chart);
                }
            }

            return new SelectList(pfinalList, "Accode", "AcName");
        }

        public ActionResult DelCashOperation(string tag, string transactionNo, DateTime dateTime)
        {
            if (tag == "CR")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CashReceipt_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            if (tag == "BW")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("BankWithdraw_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            if (tag == "CP")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CashPayment_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            if (tag == "BD")
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("BankDeposit_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }
            }
            List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
            string branchCode = Session["BranchCode"].ToString();
            CashOperation cashOperation = new CashOperation();
            var isClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == branchCode && Convert.ToDateTime(x.TrDate) <= Convert.ToDateTime(dateTime));
            if (isClosed.IsClosed == false)
            {
                switch (tag)
                {
                    case "CR":
                        {
                            var cashReceipt = _cashReceiptAppService.All().ToList().FirstOrDefault(x => x.ReceiptNo == transactionNo.Trim());
                            var cashReceipts = _CashReceiptSubDetailsAppService.All().ToList().FirstOrDefault(x => x.CashReceiptNo == transactionNo.Trim());
                            if (cashReceipt != null || cashReceipts != null)
                            {
                                using (var transaction = new TransactionScope())
                                {
                                    try
                                    {
                                        if (cashReceipts != null)
                                        {
                                            _CashReceiptSubDetailsAppService.All().ToList().Where(y => y.CashReceiptNo == cashReceipt.ReceiptNo).ToList().ForEach(x => _CashReceiptSubDetailsAppService.Delete(x));
                                            // _CashReceiptSubDetailsAppService.Delete(cashReceipts);
                                            _CashReceiptSubDetailsAppService.Save();
                                        }
                                        _cashReceiptAppService.Delete(cashReceipt);
                                        _cashReceiptAppService.Save();
                                        // var entryNo = Convert.ToInt64(cashReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                        var entryNo = cashReceipt.ReceiptNo;
                                        LoadDropDown.journalVoucherRemoval("CR", entryNo, cashReceipt.VoucherNo, cashReceipt.FinYear);
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Cash Receipt", "Delete", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                        CashOPVM = GetAllCashReceiptAndWithdraw(dateTime, branchCode);
                                        transaction.Complete();
                                    }
                                    catch (Exception)
                                    {
                                        transaction.Dispose();
                                        return Json("0", JsonRequestBehavior.AllowGet);
                                    }
                                    return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                        }
                    case "BW":
                        {
                            var withdraw = _withdrawService.All().FirstOrDefault(x => x.WithdrawNo == transactionNo.Trim());
                            if (withdraw != null)
                            {
                                using (var transaction = new TransactionScope())
                                {
                                    try
                                    {
                                        _withdrawService.Delete(withdraw);
                                        _withdrawService.Save();
                                        // var entryNo = Convert.ToInt64(withdraw.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                        var entryNo = withdraw.WithdrawNo;
                                        LoadDropDown.journalVoucherRemoval("BW", entryNo, withdraw.VoucherNo, withdraw.FinYear);
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Withdraw", "Delete", withdraw.WithdrawNo, Session["UserName"].ToString());
                                        CashOPVM = GetAllCashReceiptAndWithdraw(dateTime, branchCode);
                                        transaction.Complete();
                                    }
                                    catch (Exception)
                                    {
                                        transaction.Dispose();
                                        return Json("0", JsonRequestBehavior.AllowGet);
                                    }
                                    return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                        }
                    case "CP":
                        {

                            var expense = _ExpenseService.All().ToList().FirstOrDefault(x => x.ExpenseNo == transactionNo.Trim());
                            var expensesub = _expenseSubDetailsService.All().ToList().FirstOrDefault(x => x.ExpenseNo == transactionNo.Trim());
                            if (expense != null || expensesub != null)
                            {
                                using (var transaction = new TransactionScope())
                                {
                                    try
                                    {
                                        if (expensesub != null)
                                        {
                                            _expenseSubDetailsService.All().ToList().Where(y => y.ExpenseNo == expense.ExpenseNo).ToList().ForEach(x => _expenseSubDetailsService.Delete(x));
                                            _expenseSubDetailsService.Save();
                                        }
                                        var row = _PayInvRecvService.All().FirstOrDefault(s => s.TranNo == transactionNo);
                                        string rcvNo = row != null ? row.RefNo : string.Empty;
                                        if (rcvNo != "")
                                        {
                                            var rcvDetail = _receiveDetailsService.All().ToList().Where(y => y.RcvNo == rcvNo).ToList();
                                            var strLocCode = _receiveMainService.All().ToList().FirstOrDefault(x => x.RcvNo == rcvNo).StoreLocCode;
                                            foreach (var currentItem in rcvDetail)
                                            {
                                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode && m.LotNo == currentItem.LotNo && m.LocCode == strLocCode);
                                                if (currentStocks != null)
                                                {
                                                    currentStocks.CurrQty = currentStocks.CurrQty - currentItem.Qty;
                                                    _currentStockService.Update(currentStocks);
                                                }
                                            }
                                            _currentStockService.Save();
                                            _receiveDetailsService.All().ToList().Where(y => y.RcvNo == rcvNo).ToList().ForEach(x => _receiveDetailsService.Delete(x));
                                            _receiveMainService.All().ToList().Where(y => y.RcvNo == rcvNo).ToList().ForEach(x => _receiveMainService.Delete(x));
                                            _PayInvRecvService.All().ToList().Where(y => y.RefNo == rcvNo).ToList().FirstOrDefault();
                                            var payInvRcv = _PayInvRecvService.All().ToList().FirstOrDefault(y => y.RefNo == rcvNo);
                                            _PayInvRecvService.Delete(payInvRcv);
                                        }
                                        _ExpenseService.Delete(expense);
                                        _ExpenseService.Save();

                                        var entryNo = expense.ExpenseNo;
                                        //  var entryNo = Convert.ToInt64(expense.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                        LoadDropDown.journalVoucherRemoval("CP", entryNo, expense.VoucherNo, expense.FinYear);
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Expene", "Delete", expense.ExpenseNo, Session["UserName"].ToString());
                                        CashOPVM = GetAllExpenseAndDeposit(dateTime, branchCode);

                                        transaction.Complete();
                                    }
                                    catch (Exception)
                                    {
                                        transaction.Dispose();
                                        return Json("0", JsonRequestBehavior.AllowGet);
                                    }
                                    return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                        }
                    case "BD":
                        {

                            var deposit = _DepositToBankService.All().FirstOrDefault(x => x.DepositNo == transactionNo.Trim());
                            if (deposit != null)
                            {
                                using (var transaction = new TransactionScope())
                                {
                                    try
                                    {
                                        _DepositToBankService.Delete(deposit);
                                        _DepositToBankService.Save();
                                        var entryNo = deposit.DepositNo;
                                        // var entryNo = Convert.ToInt64(deposit.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                        LoadDropDown.journalVoucherRemoval("BD", entryNo, deposit.VoucherNo, deposit.FinYear);
                                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Deposit To Bank", "Delete", deposit.DepositNo, Session["UserName"].ToString());
                                        CashOPVM = GetAllExpenseAndDeposit(dateTime, branchCode);

                                        transaction.Complete();
                                    }
                                    catch (Exception)
                                    {
                                        transaction.Dispose();
                                        return Json("0", JsonRequestBehavior.AllowGet);
                                    }
                                    return Json(CashOPVM, JsonRequestBehavior.AllowGet);

                                }
                            }
                            else
                            {
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }
                        }
                    default:
                        {
                            return Json(0, JsonRequestBehavior.AllowGet);
                        }
                }
            }
            else
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CloseCash(DateTime date)
        {
            string branchCode = Session["BranchCode"].ToString();
            string finYear = Session["FinYear"].ToString();
            string user = Session["UserName"].ToString();
            //var userInfo = _EmployeeService.All().ToList().FirstOrDefault(x => x.Email == user);
            var userInfo = _SecUserInfoService.All().ToList().FirstOrDefault(x => x.UserName == user); CashOperation cashOperation = new App.Domain.CashOperation();
            var lastDayCashOperation = _CashOperationService.All().LastOrDefault(x => x.BranchCode == branchCode && Convert.ToDateTime(x.TrDate) < Convert.ToDateTime(date));

            if (lastDayCashOperation != null && lastDayCashOperation.IsClosed == true)
            {
                var isTodayClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == branchCode && Convert.ToDateTime(x.TrDate) == Convert.ToDateTime(date));
                if (isTodayClosed.IsClosed == false)
                {
                    isTodayClosed.TrDate = Convert.ToDateTime(date);
                    isTodayClosed.OpenBal = GetOpenBal(date);
                    isTodayClosed.RecTotal = GetAllCashReceiptAndWithdraw(date, branchCode).Sum(x => x.Amount);
                    isTodayClosed.PayTotal = GetAllExpenseAndDeposit(date, branchCode).Sum(x => x.Amount);
                    isTodayClosed.CloseBal = isTodayClosed.OpenBal + isTodayClosed.RecTotal - isTodayClosed.PayTotal;
                    isTodayClosed.IsClosed = true;
                    isTodayClosed.ClosedBy = userInfo.UserName;
                    isTodayClosed.ClosingTime = DateTime.Now;
                    isTodayClosed.FinYear = finYear;
                    isTodayClosed.GLPost = false;
                    isTodayClosed.BranchCode = branchCode;

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {

                            _CashOperationService.Update(isTodayClosed);
                            _CashOperationService.Save();
                            transaction.Complete();
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                        }

                    }

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (isTodayClosed == null)
                {

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {

                            _CashOperationService.Add(cashOperation);
                            _CashOperationService.Save();
                            transaction.Complete();
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                        }

                    }

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

            }


            else if (lastDayCashOperation == null)
            {
                var isTodayClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == branchCode && Convert.ToDateTime(x.TrDate) == Convert.ToDateTime(date));
                if (isTodayClosed.IsClosed == false)
                {
                    isTodayClosed.TrDate = Convert.ToDateTime(date);
                    isTodayClosed.OpenBal = GetOpenBal(date);
                    isTodayClosed.RecTotal = GetAllCashReceiptAndWithdraw(date, branchCode).Sum(x => x.Amount);
                    isTodayClosed.PayTotal = GetAllExpenseAndDeposit(date, branchCode).Sum(x => x.Amount);
                    isTodayClosed.CloseBal = isTodayClosed.OpenBal + isTodayClosed.RecTotal - isTodayClosed.PayTotal;
                    isTodayClosed.IsClosed = true;
                    isTodayClosed.ClosedBy = userInfo.UserName;
                    isTodayClosed.ClosingTime = DateTime.Now;
                    isTodayClosed.FinYear = finYear;
                    isTodayClosed.GLPost = false;
                    isTodayClosed.BranchCode = branchCode;

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {

                            _CashOperationService.Update(isTodayClosed);
                            _CashOperationService.Save();
                            transaction.Complete();
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                        }

                    }

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (isTodayClosed == null)
                {
                    cashOperation.TrDate = Convert.ToDateTime(date);
                    cashOperation.OpenBal = GetOpenBal(date);
                    cashOperation.RecTotal = GetAllCashReceiptAndWithdraw(date, branchCode).Sum(x => x.Amount);
                    cashOperation.PayTotal = GetAllExpenseAndDeposit(date, branchCode).Sum(x => x.Amount);
                    cashOperation.CloseBal = cashOperation.OpenBal + cashOperation.RecTotal - cashOperation.PayTotal;
                    cashOperation.IsClosed = true;
                    cashOperation.ClosedBy = userInfo.UserName;
                    cashOperation.ClosingTime = DateTime.Now;
                    isTodayClosed.FinYear = finYear;
                    isTodayClosed.GLPost = false;
                    isTodayClosed.BranchCode = branchCode;

                    using (var transaction = new TransactionScope())
                    {
                        try
                        {

                            _CashOperationService.Add(cashOperation);
                            _CashOperationService.Save();
                            transaction.Complete();
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                        }

                    }

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        //newchart check  for Account head (If change newchart table then create map tabe with  subsidiary & stock)
        public ActionResult ChechSubStock(string purAccode)
        {
            var newchart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == purAccode);
            if (newchart.Subsidiary == true && newchart.Stock == true)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else if (newchart.Subsidiary == true && newchart.Stock == false)
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }
            else if (newchart.Subsidiary == false && newchart.Stock == true)
            {
                return Json("3", JsonRequestBehavior.AllowGet);
            }
            else if (newchart.Subsidiary == false && newchart.Stock == false)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetItemByQtyPrice(string Item)
        {

            if (!string.IsNullOrEmpty(Item))
            {
                CurrentStock currStock = null;
                currStock = _currentStockService.All().FirstOrDefault(x => x.ItemCode == Item);

                return Json(currStock, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public class retvalpro
        {
            public string VchrNo { get; set; }
        }
        public ActionResult GlPostedByVoucherNo(List<string> vochreNo)
        {
            foreach (var vItem in vochreNo)
            {

                //#region exce for JVFinal SP
                //string sqlVchrNo = string.Format("EXEC JVFinal '01', '" + Session["BranchCode"].ToString() + "','JV','" + Session["FinYear"].ToString() + "'");
                //string VchrNo;
                //IEnumerable<retvalpro> VchrLst;
                //using (AcclineERPContext dbContext = new AcclineERPContext())
                //{
                //    VchrLst = dbContext.Database.SqlQuery<retvalpro>(sqlVchrNo).ToList();
                //    VchrNo = VchrLst.Select(x => x.VchrNo).Single();
                //}
                //#endregion

                string sqlQuery;
                SqlParameter[] sqlParams;
                sqlQuery = "JPost2Book @vochreNo,@FY";
                sqlParams = new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@vochreNo",  Value = vItem , Direction = System.Data.ParameterDirection.Input},
                    new SqlParameter { ParameterName = "@FY",  Value = Session["FinYear"].ToString() , Direction = System.Data.ParameterDirection.Input}
                               
                };
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }



            }


            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (pageType == "COR")
            {
                if (!rUser.HasPermission("CashReceive_VchrWaitingForPosting"))
                {
                    string errMsg = "VWP";
                    return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                }
            }
            else if (pageType == "COP")
            {
                if (!rUser.HasPermission("CashPayment_VchrWaitingForPosting"))
                {
                    string errMsg = "VWP";
                    return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                }
            }

            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("select pvd.PVchrDetailId, pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear left outer join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "'  and pvm.BranchCode ='" + Session["BranchCode"].ToString() + "'"
                  + "and pvm.BranchCode= '" + branchCode + "'"
                + "group by pvd.PVchrDetailId, pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac order by pvd.PVchrDetailId");

            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                return RedirectToAction("CashOperation", "CashOperation", new { errMsg });

            }
            else
            {
                //ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }
             


        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (pageType == "COR")
            {
                if (!rUser.HasPermission("CashReceive_VchrList"))
                {
                    string errMsg = "VL";
                    return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                }
            }
            else if (pageType == "COP")
            {
                if (!rUser.HasPermission("CashPayment_VchrList"))
                {
                    string errMsg = "VL";
                    return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                }
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM/dd/yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
            }
            else
            {
                ViewBag.branchCode = _BranchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntriesPdf(string vchrNo, string pType)
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
            string sql = string.Format("select pvd.VchrDetailId,pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as Accode,(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary,pvd.Narration,"
                + "  pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from VchrMain as pvm "
                + "join VchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear  where pvm.VchrNo ='" + vchrNo + "'"
                + "group by pvd.VchrDetailId, pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac order by pvd.VchrDetailId");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not pound !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
            }
            else
            {
                if (pType == "A4") { return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport) { PageSize = Rotativa.Options.Size.A4 }; }
                else if (pType == "A3") { return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport) { PageSize = Rotativa.Options.Size.A3 }; }
                else { return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport) 
                { 
                    PageSize = Rotativa.Options.Size.A5 ,
                    CustomSwitches = "--minimum-font-size 12"
                }; }
            }
        }

               
        public ActionResult GetGlECountN(string VType, DateTime TrDate)
        {
            return Json(CountEntries(VType, TrDate), JsonRequestBehavior.AllowGet);
        }
        public string CountEntries(string pageType, DateTime TrDate)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'and pvm.VDate='" + TrDate.ToString("MM/dd/yyyy") + "'and BranchCode='" + branchCode + "'");
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
        public ActionResult GetGlECount(string VType)
        {
            return Json(Count(VType), JsonRequestBehavior.AllowGet);
        }
        public string Count(string pageType)
        {
            string branchCode = Session["BranchCode"].ToString();
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "' and BranchCode='" + branchCode + "'");
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
        public ActionResult DeleteVoucherPro(string remitNo, string tag)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + remitNo + "' and EntrySource='" + tag + "'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (IsExit.Count == 0)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }

        }


    }
}
