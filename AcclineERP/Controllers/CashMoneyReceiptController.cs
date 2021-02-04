using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AcclineERP.Models;
using System.Globalization;
using System.Threading;

namespace AcclineERP.Controllers
{
    public class CashMoneyReceiptController : Controller
    {
        private readonly ICashReceiptSubDetailsAppService _CashReceiptSubDetailsAppService;
        private readonly IAcBRAppService _AcBrService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly INewChartAppService _newChartService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IBranchAppService _branchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly IVchrSetAppService _vchrSetService;
        private readonly IDefACAppService _defACService;
        private readonly IMoneyReceiptAppService _moneyReceiptService;
        private readonly IMoneyReceiptExtAppService _moneyReceiptExtService;
        private readonly ISalesMainAppService _salesMainService;
        private readonly IGsetAppService _gsetService;
        private readonly ICashReceiptAppService _CashReceiptService;
        private readonly CashReceiptController _CashReceiptController;
        private readonly ISubsidiaryExtAppService _SubsidiaryExtService;
        private readonly IPVchrMainAppService _pVchrmainService;
        public CashMoneyReceiptController(
            ICashReceiptSubDetailsAppService _CashReceiptSubDetailsAppService,
            IAcBRAppService _AcBrService,
            INewChartAppService _newChartService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, ICurrentStockAppService _currentStockService,
            ITransactionLogAppService _transactionLogService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IBranchAppService _branchService, IProjInfoAppService _ProjInfoService,
            IVchrSetAppService _vchrSetService, IDefACAppService _defACService,
            IMoneyReceiptAppService _moneyReceiptService, IMoneyReceiptExtAppService _moneyReceiptExtService,
            ISalesMainAppService _salesMainService, IGsetAppService _gsetService, ICashReceiptAppService _CashReceiptService,
            CashReceiptController _CashReceiptController, ISubsidiaryExtAppService _SubsidiaryExtService, IPVchrMainAppService _pVchrmainService)
        {
            this._CashReceiptSubDetailsAppService = _CashReceiptSubDetailsAppService;
            this._AcBrService = _AcBrService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._newChartService = _newChartService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._currentStockService = _currentStockService;
            this._transactionLogService = _transactionLogService;
            this._sysSetService = _sysSetService;
            this._branchService = _branchService;
            this._ProjInfoService = _ProjInfoService;
            this._vchrSetService = _vchrSetService;
            this._defACService = _defACService;
            this._moneyReceiptService = _moneyReceiptService;
            this._moneyReceiptExtService = _moneyReceiptExtService;
            this._salesMainService = _salesMainService;
            this._gsetService = _gsetService;
            this._CashReceiptService = _CashReceiptService;
            this._CashReceiptController = _CashReceiptController;
            this._SubsidiaryExtService = _SubsidiaryExtService;
            this._pVchrmainService = _pVchrmainService;
        }
        // GET: CashMoneyReceipt
        public ActionResult CashMoneyReceipt(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.MRAgainst = LoadDropDown.LoadMRAgainstDDL();
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "1"), "SubCode", "SubName");
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                ViewBag.MRSL = LoadDropDown.GenerateRecvSlNo(_moneyReceiptService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString());
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("CR");
                ViewBag.GLEntries = CountEntries("CR", datetime);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult GetAllByMRNo(string MRNo)
        {
            var GetMR = (from mr in _moneyReceiptService.All().ToList()
                         where mr.MRNo == MRNo && mr.PayMode == "Ca"
                         select new
                         {
                             MRSL = mr.MRSL,
                             ProjCode = mr.ProjCode,
                             BranchCode = mr.BranchCode,
                             MRNo = mr.MRNo,
                             MRDate = mr.MRDate,
                             MRAgainst = mr.MRAgainst,
                             GetwayId = mr.GetwayId,
                             CustCode = mr.CustCode,
                             MRAmount = mr.MRAmount,
                             VchrNo = mr.VchrNo,
                             Accode = mr.Accode,
                             Posted = mr.Posted,
                             AdjWithBill = mr.AdjWithBill,
                             JobNo = mr.JobNo,
                             Remarks = mr.Remarks,
                             DepositBank = mr.DepositBank,
                             DepositDate = mr.EncashDate
                         }).FirstOrDefault();

            return Json(new { GetMR = GetMR }, JsonRequestBehavior.AllowGet);
        }

        class billShow
        {
            public string SaleNo { get; set; }
            public DateTime SaleDate { get; set; }
            public decimal BillAmount { get; set; }
            public decimal NetAmount { get; set; }
            public bool IsPaid { get; set; }
            public bool Paid { get; set; }
            public Nullable<decimal> ReceiptAmt { get; set; }
            public decimal Billamt { get; set; }
        }

        public ActionResult GetAdjBills(string CustCode, string MRAgainst, decimal Amount, string isEdit)
        {
            var Bills = default(dynamic);
            string sql = "";
            string MrAg = LoadDropDown.GetMrAgainstType(MRAgainst);
            if (MrAg == null) MrAg = MRAgainst;//temporary Ashik
            if (MrAg == "Sales")
            {
                if (isEdit == "1")
                {
                    Bills = (from mr in _moneyReceiptService.All().ToList()
                             join mre in _moneyReceiptExtService.All().ToList() on mr.MRId equals mre.MRId
                             where mr.CustCode == CustCode
                             select new
                             {
                                 SaleNo = mre.SaleNo,
                                 SaleDate = mr.MRDate,
                                 BillAmount = mre.Amount,
                                 IsPaid = true
                             }).ToList();

                }
                else if (isEdit == "ChqEdit")
                {
                    sql = string.Format("select sm.IsPaid, sm.NetAmount as Billamount,  sm.SaleNo, sm.SaleDate , (sm.NetAmount - isnull(sum(cre.BillAmount),0)) as Billamt from SalesMain sm left join ChequeReceiptExt cre on cre.BillNo = sm.SaleNo  where sm.CustCode = " + CustCode + "   group by sm.IsPaid, cre.BillNo, sm.NetAmount, sm.SaleNo, sm.SaleDate order by sm.SaleNo ");
                    //sql = string.Format("select sm.IsPaid, sm.NetAmount as Billamount,  sm.SaleNo, sm.SaleDate , (sm.NetAmount - isnull(sum(cre.BillAmount),0)) as Billamt from SalesMain sm left join ChequeReceiptExt cre on cre.BillNo = sm.SaleNo  where sm.CustCode = " + CustCode + " and sm.Ispaid = 'true' and Billamt = 0 group by sm.IsPaid, cre.BillNo, sm.NetAmount, sm.SaleNo, sm.SaleDate, cre.BillAmount order by sm.SaleNo ");
                    IEnumerable<billShow> BillLst;
                    using (AcclineERPContext dbContext = new AcclineERPContext())
                    {
                        BillLst = dbContext.Database.SqlQuery<billShow>(sql).ToList();
                        Bills = BillLst;
                    }
                }
                else
                {
                    //decimal MrExtAmt = 0;
                    //Bills = (from sm in _salesMainService.All().ToList()
                    //         join mre in _moneyReceiptExtService.All().ToList() on sm.SaleNo equals mre.SaleNo into SmMre
                    //         from leftJ in SmMre.DefaultIfEmpty()
                    //         where sm.CustCode == CustCode && sm.IsPaid == false
                    //         //group leftJ by leftJ.SaleNo into G
                    //         select new
                    //         {
                    //             SaleNo = sm.SaleNo,
                    //             SaleDate = sm.SaleDate,
                    //             BillAmount = sm.NetAmount - (MrExtAmt = (leftJ == null) ? 0 : leftJ.Amount)
                    //         }).GroupBy(x => x.SaleNo).ToList();

                    sql = string.Format("select sm.SaleNo, sm.SaleDate, (sm.NetAmount - (select isnull(sum(mre.Amount),0) as Billamount from MoneyReceiptExt mre where SaleNo = sm.SaleNo)) as Billamount from SalesMain sm  where sm.CustCode = " + CustCode + " and sm.IsPaid = 'false'  group by sm.NetAmount, sm.SaleNo, sm.SaleDate order by sm.SaleNo  ");
                    IEnumerable<billShow> BillLst;
                    using (AcclineERPContext dbContext = new AcclineERPContext())
                    {
                        BillLst = dbContext.Database.SqlQuery<billShow>(sql).ToList();
                        Bills = BillLst;
                    }

                }
            }
            //else if (true)
            //{

            //}

            return Json(new { data = Bills }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveCashMR(MoneyReceiptVM MrVM, List<MoneyReceiptExt> AdjBills)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("MoneyReceiptCash_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    //----Edit Nasir---//
                    //var cabkop = "";
                    //var sysSet = _sysSetService.All().FirstOrDefault();
                    //if (sysSet.ActualBasis == true)
                    //{
                    //var cabkop = _defACService.All().ToList().FirstOrDefault().CashAc;
                    //}
                    //else
                    //{
                    //    cabkop = _defACService.All().ToList().FirstOrDefault().CashAc;
                    //}
                    //---  ---//
                    string CRNo = "";
                    var IfExit = _moneyReceiptService.All().Where(x => x.MRNo == MrVM.MRNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        //var todayDate = DateTime.Now;
                        CashReceipt CR = new CashReceipt();
                        CashReceiptSubDetails CRSD = new CashReceiptSubDetails();
                        MoneyReceipt MR = new MoneyReceipt();
                        MR.MRSL = MrVM.MRSL;
                        MR.BranchCode = (MrVM.BranchCode == null) ? Session["BranchCode"].ToString() : MrVM.BranchCode;
                        MR.ProjCode = (MrVM.ProjCode == null) ? Session["ProjCode"].ToString() : MrVM.ProjCode;
                        MR.MRNo = MrVM.MRNo;
                        MR.MRDate = MrVM.MRDate; //.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        MR.MRAgainst = MrVM.MRAgainst;
                        MR.VchrNo = MrVM.VchrNo;
                        MR.PayMode = "Ca";
                        MR.Posted = MrVM.Posted;
                        MR.AdjWithBill = MrVM.AdjWithBill;
                        MR.MRAmount = MrVM.Amount;

                        // Accode ????
                        MR.Accode = MrVM.MRAgainst; // "1.2.001";

                        MR.CustCode = MrVM.CustCode;
                        MR.FinYear = Session["FinYear"].ToString();
                        MR.Remarks = MrVM.Remarks;
                        MR.JobNo = MrVM.JobNo;
                        //MR.Ca_Bk = cabkop;

                        // CR.ReceiptNo = _CashReceiptController.GenerateReceiptNo(MR.BranchCode).Substring(6, 0);//MrVM.MRNo;
                        CR.ReceiptNo = _CashReceiptController.GenerateReceiptNo(MR.BranchCode);
                        // CR.ReceiptNo = MrVM.MRNo;
                        CRNo = CR.ReceiptNo;
                        CR.ReceiptDate = (DateTime)MrVM.MRDate;
                        CR.purAccode = MrVM.MRAgainst;// "1.2.004";
                        CR.RefNo = MrVM.MRSL;
                        CR.Amount = (Double)MrVM.Amount;
                        CR.Advance = false;
                        CR.Remarks = MrVM.Remarks;
                        CR.GLPost = false;
                        CR.BranchCode = MrVM.BranchCode;
                        CR.VoucherNo = MrVM.VchrNo;
                        CR.BranchCode = MR.BranchCode;
                        CR.FinYear = Session["FinYear"].ToString();

                        CRSD.SubCode = MrVM.CustCode;
                        CRSD.Accode = MrVM.MRAgainst;
                        CRSD.Amount = (Double)MrVM.Amount;
                        CRSD.CashReceiptNo = CRNo;

                        //CR.purAccode = cabkop; // "1.2.004";
                        _moneyReceiptService.Add(MR);
                        _CashReceiptService.Add(CR);
                        _CashReceiptSubDetailsAppService.Add(CRSD);
                        _CashReceiptService.Save();
                        _moneyReceiptService.Save();
                        _CashReceiptSubDetailsAppService.Save();

                        //List<MoneyReceiptExt> MRExtList = new List<MoneyReceiptExt>();

                        if (AdjBills != null)
                        {

                            decimal TotAmt = MrVM.Amount;
                            foreach (var bill in AdjBills)
                            {
                                MoneyReceiptExt MRExt = new MoneyReceiptExt();
                                var saleMain = _salesMainService.All().Where(s => s.SaleNo == bill.SaleNo).FirstOrDefault();
                                MRExt.MRId = _moneyReceiptService.All().OrderBy(s => s.MRId).LastOrDefault().MRId;
                                MRExt.SaleNo = bill.SaleNo;
                                MRExt.InstallNo = _moneyReceiptExtService.All().OrderBy(s => s.InstallNo).Where(x => x.SaleNo == bill.SaleNo).Select(s => s.InstallNo).LastOrDefault();
                                MRExt.InstallNo = MRExt.InstallNo + 1;
                                MRExt.InstallDate = DateTime.Now;
                                if (TotAmt >= bill.Amount)
                                {
                                    MRExt.Amount = bill.Amount;
                                    TotAmt = TotAmt - bill.Amount;

                                    saleMain.ReceiptNo = MrVM.MRNo;
                                    saleMain.ReceiptAmt = bill.Amount;
                                    saleMain.IsPaid = true;
                                    _salesMainService.Update(saleMain);
                                    _salesMainService.Save();

                                    _moneyReceiptExtService.Add(MRExt);
                                    _moneyReceiptExtService.Save();
                                }
                                else
                                {

                                    if (bill.IsPaid_mre)
                                    {
                                        saleMain.IsPaid = true;
                                        MRExt.Amount = TotAmt;

                                        //TotAmt = TotAmt - bill.Amount;
                                    }
                                    else
                                    {
                                        saleMain.IsPaid = false;
                                        MRExt.Amount = TotAmt;
                                        // MRExt.Amount = bill.Amount;
                                        //TotAmt = TotAmt - bill.Amount;
                                    }


                                    saleMain.ReceiptNo = MrVM.MRNo;
                                    saleMain.ReceiptAmt = TotAmt;

                                    _moneyReceiptExtService.Add(MRExt);
                                    _moneyReceiptExtService.Save();

                                    string sql = string.Format("select sm.ReceiptAmt,sm.SaleNo, sm.SaleDate, (sm.NetAmount - isnull(sum(mre.Amount),0)) as Billamount from SalesMain sm left join MoneyReceiptExt mre on mre.SaleNo = sm.SaleNo where sm.CustCode = " + MrVM.CustCode + " and sm.IsPaid = 'false'  group by sm.ReceiptAmt, mre.SaleNo, sm.NetAmount, sm.SaleNo, sm.SaleDate, mre.Amount order by sm.SaleNo");
                                    IEnumerable<billShow> BillLst;
                                    using (AcclineERPContext dbContext = new AcclineERPContext())
                                    {
                                        BillLst = dbContext.Database.SqlQuery<billShow>(sql).ToList();
                                    }
                                    foreach (var Salebill in BillLst)
                                    {
                                        if (Salebill.BillAmount == 0)
                                        {
                                            saleMain.ReceiptNo = MrVM.MRNo;
                                            saleMain.ReceiptAmt = ((Salebill.ReceiptAmt == null) ? 0 : Salebill.ReceiptAmt) + TotAmt;
                                            saleMain.IsPaid = true;
                                            _salesMainService.Update(saleMain);
                                            _salesMainService.Save();
                                        }
                                    }
                                }

                            } //end of  foreach

                        }
                        // New Add By Nasir
                        var GCa = _AcBrService.All().Where(s => s.BranchCode == MR.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }

                        //------------------
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Cash MR", "Save", MR.MRSL, Session["UserName"].ToString());
                        LoadDropDown.journalVoucherSave("CR", "I", CRNo, MrVM.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), Convert.ToDateTime(MrVM.MRDate), GCa, Session["UserName"].ToString());

                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                }

                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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

        public ActionResult GetRecvNo(string branchCode, string ProjCode)
        {
            return Json(LoadDropDown.GenerateRecvSlNo(_moneyReceiptService, branchCode, Session["BranchCode"].ToString(), ProjCode, Session["VchrConv"].ToString()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MoneyReceiptCash_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "'"
                + "and pvm.BranchCode= '" + branchCode + "'"
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac");


            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            else
            {
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MoneyReceiptCash_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            else
            {
                ViewBag.branchCode = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
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
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
            }
        }

        public ActionResult UpdateCashMR(MoneyReceiptVM MrVM)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("MoneyReceiptCash_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }

                    var VchrExist = _pVchrmainService.All().FirstOrDefault(s => s.VchrNo == MrVM.VchrNo);
                    if (VchrExist == null)
                    {
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }

                    var MR = _moneyReceiptService.All().Where(x => x.MRSL == MrVM.MRSL && x.MRNo == MrVM.MRNo && MrVM.Posted == false).FirstOrDefault();
                    var CR = _CashReceiptService.All().Where(s => s.RefNo == MrVM.MRSL).FirstOrDefault();
                    var CRSD = _CashReceiptSubDetailsAppService.All().Where(s => s.CashReceiptNo == CR.ReceiptNo).FirstOrDefault();

                    if (MR != null && CR != null && CRSD != null)
                    {
                        //var cabkop = _defACService.All().ToList().FirstOrDefault().CashAc;
                        var GCa = _AcBrService.All().Where(s => s.BranchCode == MR.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }

                        //For MR
                        MR.MRSL = MrVM.MRSL;
                        MR.BranchCode = (MrVM.BranchCode == null) ? Session["BranchCode"].ToString() : MrVM.BranchCode;
                        MR.ProjCode = (MrVM.ProjCode == null) ? Session["ProjCode"].ToString() : MrVM.ProjCode;
                        MR.MRNo = MrVM.MRNo;
                        MR.MRDate = MrVM.MRDate;
                        MR.MRAgainst = MrVM.MRAgainst;
                        MR.VchrNo = MrVM.VchrNo;
                        MR.Posted = MrVM.Posted;
                        MR.AdjWithBill = MrVM.AdjWithBill;
                        MR.MRAmount = MrVM.Amount;
                        MR.Accode = MrVM.Accode;
                        MR.CustCode = MrVM.CustCode;
                        MR.FinYear = Session["FinYear"].ToString();
                        MR.JobNo = MrVM.JobNo;
                        MR.Remarks = MrVM.Remarks;
                        MR.Ca_Bk = GCa;


                        CR.ReceiptDate = (DateTime)MrVM.MRDate;
                        CR.purAccode = MrVM.MRAgainst;// "1.2.004";
                        CR.RefNo = MrVM.MRSL;
                        CR.Amount = (Double)MrVM.Amount;
                        CR.Remarks = MrVM.Remarks;
                        CR.VoucherNo = MrVM.VchrNo;

                        CRSD.SubCode = MrVM.CustCode;
                        CRSD.Accode = MrVM.MRAgainst;
                        CRSD.Amount = (Double)MrVM.Amount;


                        _moneyReceiptService.Update(MR);
                        _CashReceiptService.Update(CR);
                        _CashReceiptSubDetailsAppService.Update(CRSD);
                        _CashReceiptService.Save();
                        _CashReceiptSubDetailsAppService.Save();
                        _moneyReceiptService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Cash MR", "Update", MR.MRSL, Session["UserName"].ToString());
                        LoadDropDown.journalVoucherRemoval("CR", MrVM.MRNo, MrVM.VchrNo, Session["FinYear"].ToString());
                        LoadDropDown.journalVoucherSave("CR", "I", MrVM.MRNo, MrVM.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), Convert.ToDateTime(MrVM.MRDate), GCa, Session["UserName"].ToString());
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        transaction.Dispose();
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                }

                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetMoneyReceiptPdf(string ReceiptNo)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MoneyReceiptCash_Print"))
            {
                string errMsg = "P";
                return RedirectToAction("CashMoneyReceipt", "CashMoneyReceipt", new { errMsg });
            }
            MoneyReceiptVM MRVM = new MoneyReceiptVM();
            var cashRPfd = _moneyReceiptService.All().FirstOrDefault(x => x.MRNo == ReceiptNo && x.PayMode == "Ca");
            if (cashRPfd != null)
            {


                MRVM.MRNo = cashRPfd.MRNo;
                MRVM.CustCode = cashRPfd.CustCode.Trim();
                MRVM.CustName = (from s in _subsidiaryInfoService.All().ToList() where s.SubCode == cashRPfd.CustCode.Trim() select s.SubName).FirstOrDefault();
                MRVM.Address = (from se in _SubsidiaryExtService.All().ToList() where se.SubCode.Trim() == cashRPfd.CustCode.Trim() select se.SubAddress).FirstOrDefault();
                MRVM.InWord = InWord.ConvertToWords(Convert.ToString(cashRPfd.MRAmount));
                MRVM.AcType = _newChartService.All().FirstOrDefault(s => s.Accode == cashRPfd.Accode).AcName;
                MRVM.MRDate = cashRPfd.MRDate;

                #region //For us Culture Ex: 0.00
                const string culture = "en-US";
                CultureInfo ci = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                #endregion

                MRVM.Amount = Convert.ToDecimal(cashRPfd.MRAmount);
                MRVM.PayMode = "Ca";
                return new Rotativa.ViewAsPdf("~/Views/CashReceipt/MoneyReceiptPdf.cshtml", MRVM) { PageSize = Rotativa.Options.Size.A4 };
            }
            else
            {
                //string errMsg = "E";
                //return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                MRVM.PayMode = "Ca";
                return new Rotativa.ViewAsPdf("~/Views/CashReceipt/MoneyReceiptPdf.cshtml", MRVM) { PageSize = Rotativa.Options.Size.A4 };
            }
        }
    }
}