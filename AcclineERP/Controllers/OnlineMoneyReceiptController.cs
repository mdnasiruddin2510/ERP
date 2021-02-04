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
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class OnlineMoneyReceiptController : Controller
    {
        private readonly IBankReceiptAppService _bankReceiptService;
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
        private readonly IBankInfoAppService _bankInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly IAcBRAppService _AcBrService;

        public OnlineMoneyReceiptController(
            IBankReceiptAppService _bankReceiptService,
            INewChartAppService _newChartService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, ICurrentStockAppService _currentStockService,
            ITransactionLogAppService _transactionLogService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IBranchAppService _branchService, IProjInfoAppService _ProjInfoService,
            IVchrSetAppService _vchrSetService, IDefACAppService _defACService,
            IMoneyReceiptAppService _moneyReceiptService, IMoneyReceiptExtAppService _moneyReceiptExtService,
            ISalesMainAppService _salesMainService, IGsetAppService _gsetService, ICashReceiptAppService _CashReceiptService,
            IBankInfoAppService _bankInfoService, IEmployeeAppService _employeeInfoService, IAcBRAppService _AcBrService)
        {
            this._bankReceiptService = _bankReceiptService;
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
            this._bankInfoService = _bankInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._AcBrService = _AcBrService;
        }

        // GET: OnlineMoneyReceipt
        public ActionResult OnlineMoneyReceipt()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.MRAgainst = LoadDropDown.LoadMRAgainstDDL();
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().Where(x => x.SubType == "1").ToList(), "SubCode", "SubName");
                ViewBag.bankCode = new SelectList(_bankInfoService.All().ToList(), "BankCode", "BankName");
                ViewBag.DepositBank = LoadDropDown.LoadDepositBankDDL();
                ViewBag.CollectedBy = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");
                var CashAc = _defACService.All().ToList().FirstOrDefault().CashAc;
                ViewBag.Accode = LoadDropDown.LoadGLAc(CashAc, _newChartService);
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("BR");
                ViewBag.GLEntries = CountEntries("BR", datetime);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                ViewBag.GetwayId = LoadDropDown.LoadGetwayProvider();
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
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
        public ActionResult SaveMROnline(MROnlineVM MrVM, List<MoneyReceiptExt> AdjBills)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("MoneyReceiptOnline_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var IfExit = _moneyReceiptService.All().Where(x => x.MRSL == MrVM.MRSL && x.MRNo == MrVM.MRNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        //var todayDate = DateTime.Now;
                        //CashReceipt CR = new CashReceipt();
                        BankReceipt br = new BankReceipt();
                        MoneyReceipt MR = new MoneyReceipt();
                        MR.MRSL = MrVM.MRSL;
                        MR.BranchCode = (MrVM.BranchCode == null) ? Session["BranchCode"].ToString() : MrVM.BranchCode;
                        MR.ProjCode = (MrVM.ProjCode == null) ? Session["ProjCode"].ToString() : MrVM.ProjCode;
                        MR.MRNo = MrVM.MRNo;
                        MR.MRDate = MrVM.MRDate; //.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        MR.MRAgainst = MrVM.MRAgainst;
                        MR.VchrNo = MrVM.VchrNo;
                        MR.PayMode = "Ca";
                        //MR.Posted = MrVM.Posted;
                        //MR.AdjWithBill = MrVM.AdjWithBill;
                        MR.MRAmount = MrVM.MRAmount;
                        MR.Accode = MrVM.MRAgainst;
                        //if (MrVM.Accode == "0")
                        //{
                        //    MR.Accode = _defACService.All().ToList().FirstOrDefault().CashAc;
                        //}
                        MR.CustCode = MrVM.CustCode;
                        MR.FinYear = Session["FinYear"].ToString();
                        MR.Remarks = MrVM.Remarks;
                        MR.JobNo = MrVM.JobNo;
                        MR.CollectedBy = MrVM.CollectedBy;
                        MR.GetwayId = MrVM.GetwayId;
                        MR.DepositBank = MrVM.DepositBank;
                        MR.EncashDate = MrVM.DepositDate;

                        br.BReceiptNo = MrVM.MRSL;
                        br.BReceiptDate = (DateTime)MrVM.MRDate;
                        br.purAccode = MrVM.MRAgainst;
                        br.RefNo = MrVM.MRNo;
                        br.bankAccode = MrVM.DepositBank;
                        br.ChequeNo = "Online";
                        br.ChequeDate = DateTime.Now;
                        br.Amount = (double)MrVM.MRAmount;
                        br.Advance = false;
                        br.Remarks = MrVM.Remarks;
                        br.FinYear = Session["FinYear"].ToString();
                        br.GLPost = false;
                        br.BranchCode = (MrVM.BranchCode == null) ? Session["BranchCode"].ToString() : MrVM.BranchCode;
                        br.VoucherNo = MrVM.VchrNo;
                        br.SubCode = MrVM.CustCode;
                        br.bankCode = MrVM.DepositBank;


                        //CR.ReceiptNo = MrVM.MRNo;
                        //CR.ReceiptDate = (DateTime)MrVM.MRDate;
                        //CR.purAccode = MrVM.MRAgainst;
                        ////if (MrVM.Accode == "0")
                        ////{
                        ////    CR.purAccode = _defACService.All().ToList().FirstOrDefault().CashAc;
                        ////}
                        //CR.RefNo = MrVM.MRSL;
                        //CR.Amount = (Double)MrVM.MRAmount;
                        //CR.Advance = false;
                        //CR.Remarks = MrVM.Remarks;
                        //CR.GLPost = false;
                        //CR.BranchCode = MrVM.BranchCode;
                        //CR.VoucherNo = MrVM.VchrNo;
                        //CR.JobNo = MrVM.JobNo;
                        //CR.FinYear = Session["FinYear"].ToString();
                        _moneyReceiptService.Add(MR);
                        //_CashReceiptService.Add(CR);
                        //_CashReceiptService.Save();
                        _bankReceiptService.Add(br);

                        _moneyReceiptService.Save();

                        _bankReceiptService.Save();


                        //List<MoneyReceiptExt> MRExtList = new List<MoneyReceiptExt>();

                        if (AdjBills != null)
                        {
                            decimal TotAmt = MrVM.MRAmount;
                            foreach (var bill in AdjBills)
                            {
                                MoneyReceiptExt MRExt = new MoneyReceiptExt();
                                var saleMain = _salesMainService.All().Where(s => s.SaleNo == bill.SaleNo).FirstOrDefault();
                                MRExt.MRId = _moneyReceiptService.All().OrderBy(x => x.MRId).LastOrDefault().MRId;
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
                                    }
                                    else
                                    {
                                        saleMain.IsPaid = false;
                                        MRExt.Amount = TotAmt;
                                    }

                                    saleMain.ReceiptNo = MrVM.MRNo;
                                    saleMain.ReceiptAmt = bill.Amount;
                                    //MRExt.Amount = TotAmt;
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

                            }

                        }

                        //var GCa = _AcBrService.All().Where(s => s.BranchCode == MR.BranchCode && s.Ca_Ba == "Ba").Select(x => x.Accode).FirstOrDefault();
                        //if (GCa == null)
                        //{
                        //    var gset = _gsetService.All().LastOrDefault();
                        //    GCa = gset.GCa;
                        //}

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Online MR", "Save", MR.MRSL, Session["UserName"].ToString());

                        LoadDropDown.journalVoucherSave("BR", "I", MrVM.MRSL, MrVM.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), Convert.ToDateTime(MrVM.MRDate), MrVM.DepositBank, Session["UserName"].ToString());

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

        public ActionResult UpdateOnlineMR(MROnlineVM MrVM)
        {
            using (var transaction = new TransactionScope())
            {

                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("MoneyReceiptOnline_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    var MR = _moneyReceiptService.All().Where(x => x.MRSL == MrVM.MRSL && x.MRNo == MrVM.MRNo && MrVM.Posted == false).FirstOrDefault();
                    if (MR != null)
                    {
                        //For MR
                        MR.MRSL = MrVM.MRSL;
                        MR.BranchCode = (MrVM.BranchCode == null) ? Session["BranchCode"].ToString() : MrVM.BranchCode;
                        MR.ProjCode = (MrVM.ProjCode == null) ? Session["ProjCode"].ToString() : MrVM.ProjCode;
                        MR.MRNo = MrVM.MRNo;
                        MR.MRDate = MrVM.MRDate;
                        MR.MRAgainst = MrVM.MRAgainst;
                        MR.VchrNo = MrVM.VchrNo;
                        //MR.Posted = MrVM.Posted;
                        //MR.AdjWithBill = MrVM.AdjWithBill;
                        MR.MRAmount = MrVM.MRAmount;
                        MR.Accode = MrVM.Accode;
                        if (MrVM.Accode == "0" || MrVM.Accode == null)
                        {
                            MR.Accode = _defACService.All().ToList().FirstOrDefault().CashAc;
                        }
                        MR.CustCode = MrVM.CustCode;
                        MR.FinYear = Session["FinYear"].ToString();
                        MR.JobNo = MrVM.JobNo;
                        MR.Remarks = MrVM.Remarks;
                        MR.DepositBank = MrVM.DepositBank;
                        MR.EncashDate = MrVM.DepositDate;

                        _moneyReceiptService.Update(MR);
                        _moneyReceiptService.Save();

                        var GCa = _AcBrService.All().Where(s => s.BranchCode == MR.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Online MR", "Update", MR.MRSL, Session["UserName"].ToString());
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

    }
}