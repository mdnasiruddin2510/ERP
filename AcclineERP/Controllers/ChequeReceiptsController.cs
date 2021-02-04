using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Interfaces;
using AcclineERP.Models;
using App.Domain;
using System.Transactions;
using App.Domain.ViewModel;
using Data.Context;

namespace AcclineERP.Controllers
{
    public class ChequeReceiptsController : Controller
    {
        private readonly IBranchAppService _branchService;
        private readonly ISubsidiaryInfoAppService _subsidiaryService;
        private readonly ISubsidiaryCtrlAppService _subsidiaryCtrlService;
        private readonly IBankInfoAppService _bankInfoService;
        private readonly IChequeReceiptAppService _chequeReceiptService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISubsidiaryExtAppService _SubsidiaryExtService;
        private readonly INewChartAppService _NewChartService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly IChequeReceiptExtAppService _chqRecExtService;
        private readonly ISalesMainAppService _salesMainService;
        private readonly IChequeArchiveAppService _chequeArchiveService;
        private readonly IBankReceiptAppService _bankReceiptService;
        private readonly IVchrMainAppService _vchrMainService;
        private readonly IMoneyReceiptAppService _moneyReceiptService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IVchrSetAppService _vchrSetService;
        private readonly IMoneyReceiptExtAppService _moneyReceiptExtService;

        public ChequeReceiptsController(IBranchAppService _branchService, ISubsidiaryInfoAppService _subsidiaryService,
            ISubsidiaryCtrlAppService _subsidiaryCtrlService, IBankInfoAppService _bankInfoService, IChequeReceiptAppService _chequeReceiptService,
            ITransactionLogAppService _transactionLogService, ISubsidiaryExtAppService _SubsidiaryExtService, IChequeReceiptExtAppService _chqRecExtService,
            INewChartAppService _NewChartService, IEmployeeAppService _employeeInfoService, IJarnalVoucherAppService _jarnalVoucherService,
            ISalesMainAppService _salesMainService, IChequeArchiveAppService _chequeArchiveService, IBankReceiptAppService _bankReceiptService,
            IVchrMainAppService _vchrMainService, IMoneyReceiptAppService _moneyReceiptService,
            ISysSetAppService _sysSetService, IVchrSetAppService _vchrSetService, IMoneyReceiptExtAppService _moneyReceiptExtService)
        {
            this._branchService = _branchService;
            this._subsidiaryService = _subsidiaryService;
            this._subsidiaryCtrlService = _subsidiaryCtrlService;
            this._bankInfoService = _bankInfoService;
            this._chequeReceiptService = _chequeReceiptService;
            this._transactionLogService = _transactionLogService;
            this._SubsidiaryExtService = _SubsidiaryExtService;
            this._NewChartService = _NewChartService;
            this._employeeInfoService = _employeeInfoService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._chqRecExtService = _chqRecExtService;
            this._salesMainService = _salesMainService;
            this._chequeArchiveService = _chequeArchiveService;
            this._bankReceiptService = _bankReceiptService;
            this._vchrMainService = _vchrMainService;
            this._moneyReceiptService = _moneyReceiptService;
            this._sysSetService = _sysSetService;
            this._vchrSetService = _vchrSetService;
            this._moneyReceiptExtService = _moneyReceiptExtService;
        }
        public ActionResult ChequeReceipts(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.Accode = LoadDropDown.LoadMRAgainstDDL();
                ViewBag.SubCode = new SelectList(_subsidiaryService.All().Where(x => x.SubType == "1").ToList(), "SubCode", "SubName");//LoadDropDown.LoadSubsidiaryByPurpuse("", _subsidiaryService, _subsidiaryCtrlService);
                ViewBag.BankCode = new SelectList(_bankInfoService.All().ToList(), "BankCode", "BankName");
                ViewBag.DepositBank = LoadDropDown.LoadDepositBankDDL();
                ViewBag.UpdateBy = new SelectList(_employeeInfoService.All().ToList(), "Id", "UserName");
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("BR");
                ViewBag.GLEntries = CountEntries("BR", datetime);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        class billShow
        {
            public string BillNo { get; set; }
            public DateTime BillDate { get; set; }
            public decimal BillAmount { get; set; }
            public decimal NetAmount { get; set; }
            public bool IsPaid { get; set; }
            public Nullable<decimal> ReceiptAmt { get; set; }
        }

        [HttpPost]
        public ActionResult SaveCheRec(ChequeReceipt CheRec, List<ChequeReceiptExt> AdjBills)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("ChequeReceipt_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    string BrNo = "";
                    var items = _chequeReceiptService.All().ToList().FirstOrDefault(x => x.ChqReceiptNo == CheRec.ChqReceiptNo);
                    var chqArc = _chequeArchiveService.All().ToList().Where(x => x.ChqNo == CheRec.ChqNo).Select(s => s.ChqStatus).ToList();
                    if (chqArc.Count == 0 || chqArc.Contains("Deposit") && CheRec.ChqStatus != "Receive" || chqArc.Contains("Encash") && CheRec.ChqStatus != "Deposit"
                        || chqArc.Contains("Dishonour") && CheRec.ChqStatus != "Encash" || chqArc.Contains("Return") && CheRec.ChqStatus != "Dishonour")
                    {
                        if (items == null)
                        {
                            ChequeReceipt cr = new ChequeReceipt();
                            cr = CheRec;
                            cr.MRTing = 0;
                            cr.MRTingTime = null;
                            cr.ProjCode = Session["ProjCode"].ToString();
                            cr.FinYear = Session["FinYear"].ToString();
                            _chequeReceiptService.Add(cr);
                            _chequeReceiptService.Save();

                            ChequeArchive CA = new ChequeArchive();
                            CA.ChqNo = CheRec.ChqNo;
                            CA.ChqStatus = CheRec.ChqStatus;
                            CA.UpdateBy = CheRec.UpdateBy;
                            CA.UpdateDate = CheRec.UpdateDate;
                            CA.Reason = CheRec.Reason;
                            CA.Remarks = CheRec.Remarks;
                            _chequeArchiveService.Add(CA);
                            _chequeArchiveService.Save();

                            BankReceipt br = new BankReceipt();
                            br.BReceiptNo = GenerateBankReceiptNo(CheRec.BranchCode);
                            BrNo = br.BReceiptNo;
                            br.BReceiptDate = CheRec.ChqReceiptDate;
                            br.purAccode = CheRec.MRAgainst.ToString();
                            br.RefNo = CheRec.ChqReceiptNo;
                            br.bankAccode = CheRec.DepositBank;
                            br.ChequeNo = CheRec.ChqNo;
                            br.ChequeDate = CheRec.ChqDate;
                            br.Amount = (double)CheRec.Amount;
                            br.Advance = false;
                            br.Remarks = CheRec.Remarks;
                            br.FinYear = Session["FinYear"].ToString();
                            br.GLPost = CheRec.GLPost;
                            br.BranchCode = CheRec.BranchCode;
                            br.VoucherNo = CheRec.VchrNo;
                            br.SubCode = CheRec.SubCode;
                            br.bankCode = CheRec.BankCode;
                            _bankReceiptService.Add(br);
                            _bankReceiptService.Save();

                            if (AdjBills != null)
                            {
                                decimal TotAmt = CheRec.Amount;
                                foreach (var bill in AdjBills)
                                {
                                    ChequeReceiptExt chqExt = new ChequeReceiptExt();
                                    var saleMain = _salesMainService.All().Where(s => s.SaleNo == bill.BillNo).FirstOrDefault();
                                    chqExt.ChqReceiptID = _chequeReceiptService.All().LastOrDefault().ChqReceiptId;
                                    chqExt.BillNo = bill.BillNo;
                                    chqExt.BillDate = saleMain.SaleDate;
                                    if (TotAmt >= bill.BillAmount)
                                    {
                                        chqExt.BillAmount = bill.BillAmount;
                                        TotAmt = TotAmt - bill.BillAmount;

                                        saleMain.ReceiptNo = CheRec.ChqReceiptNo;
                                        saleMain.ReceiptAmt = bill.BillAmount;
                                        saleMain.IsPaid = true;
                                        _salesMainService.Update(saleMain);
                                        _salesMainService.Save();

                                        _chqRecExtService.Add(chqExt);
                                        _chqRecExtService.Save();
                                    }
                                    else
                                    {

                                        if (bill.IsPaid_mre)
                                        {
                                            saleMain.IsPaid = true;
                                            chqExt.AdjAmount = TotAmt;

                                        }
                                        else
                                        {
                                            saleMain.IsPaid = false;
                                            chqExt.AdjAmount = TotAmt;

                                        }

                                        chqExt.BillAmount = TotAmt;
                                        _chqRecExtService.Add(chqExt);
                                        _chqRecExtService.Save();

                                        string sql = string.Format("select sm.ReceiptAmt, sm.SaleNo as BillNo, sm.SaleDate as BillDate, (sm.NetAmount - isnull(sum(cre.BillAmount),0)) as Billamount from SalesMain sm left join ChequeReceiptExt cre on cre.BillNo = sm.SaleNo where sm.CustCode = " + CheRec.SubCode + " and sm.IsPaid = 'false'  group by sm.ReceiptAmt, cre.BillNo, sm.NetAmount, sm.SaleNo, sm.SaleDate, cre.BillAmount order by sm.SaleNo");
                                        IEnumerable<billShow> BillLst;
                                        using (AcclineERPContext dbContext = new AcclineERPContext())
                                        {
                                            BillLst = dbContext.Database.SqlQuery<billShow>(sql).ToList();
                                        }
                                        foreach (var Salebill in BillLst)
                                        {
                                            if (Salebill.BillAmount == 0)
                                            {
                                                saleMain.ReceiptNo = CheRec.ChqReceiptNo;
                                                saleMain.ReceiptAmt = ((Salebill.ReceiptAmt == null) ? 0 : Salebill.ReceiptAmt) + TotAmt;
                                                saleMain.IsPaid = true;
                                                _salesMainService.Update(saleMain);
                                                _salesMainService.Save();
                                            }
                                        }
                                    }
                                }
                            }

                            TransactionLogService.SaveTransactionLog(_transactionLogService, "ChequeReceipt", "Save", CheRec.ChqReceiptNo, Session["UserName"].ToString());
                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("2", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
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

        public ActionResult GetAllByReceiptNo(string ReceiptNo)
        {
            var item = (from cr in _chequeReceiptService.All().ToList()
                        join ca in _chequeArchiveService.All().ToList() on cr.ChqNo equals ca.ChqNo
                        where cr.ChqReceiptNo == ReceiptNo && ca.ChqStatus == cr.ChqStatus
                        select new
                        {
                            chqRec = cr,
                            Reason = ca.Reason
                        }).FirstOrDefault();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCheRec(ChequeReceipt CheRec)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("ChequeReceipt_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var VchrExist = _vchrMainService.All().FirstOrDefault(s => s.VchrNo == CheRec.VchrNo);
                    var chqArcExist = _chequeArchiveService.All().ToList().Where(x => x.ChqNo == CheRec.oldChqNo).Select(s => s.ChqStatus).ToList();
                    if (chqArcExist.Contains("Receive") && CheRec.ChqStatus == "Receive" || chqArcExist.Contains("Receive") && CheRec.ChqStatus == "Deposit" || chqArcExist.Contains("Deposit") && CheRec.ChqStatus == "Encash"
                        || chqArcExist.Contains("Encash") && CheRec.ChqStatus != "Dishonour" && CheRec.ChqStatus != "Return")
                    {
                        if (VchrExist == null)
                        {
                            ChequeReceipt cr = new ChequeReceipt();
                            cr = CheRec;
                            cr.MRTing = 0;
                            cr.MRTingTime = null;
                            cr.ProjCode = Session["ProjCode"].ToString();
                            cr.FinYear = Session["FinYear"].ToString();

                            _chequeReceiptService.Update(cr);
                            _chequeReceiptService.Save();

                            var chqArc = _chequeArchiveService.All().FirstOrDefault(s => s.ChqNo == CheRec.oldChqNo && s.ChqStatus == CheRec.ChqStatus);
                            if (chqArc != null && chqArc.ChqStatus == CheRec.ChqStatus)
                            {
                                chqArc.ChqNo = CheRec.ChqNo;
                                chqArc.UpdateBy = CheRec.UpdateBy;
                                chqArc.UpdateDate = CheRec.UpdateDate;
                                chqArc.Reason = CheRec.Reason;
                                chqArc.Remarks = CheRec.Remarks;
                                _chequeArchiveService.Update(chqArc);
                                _chequeArchiveService.Save();
                            }
                            else
                            {
                                ChequeArchive ca = new ChequeArchive();
                                ca.ChqStatus = CheRec.ChqStatus;
                                ca.ChqNo = CheRec.ChqNo;
                                ca.UpdateBy = CheRec.UpdateBy;
                                ca.UpdateDate = CheRec.UpdateDate;
                                ca.Reason = CheRec.Reason;
                                ca.Remarks = CheRec.Remarks;
                                _chequeArchiveService.Add(ca);
                                _chequeArchiveService.Save();
                            }

                            BankReceipt br = _bankReceiptService.All().Where(s => s.RefNo == CheRec.ChqReceiptNo && s.ChequeNo == CheRec.ChqNo).FirstOrDefault();
                            if (CheRec.ChqStatus == "Deposit")
                            {
                                br.BReceiptDate = CheRec.ChqReceiptDate;
                                br.purAccode = CheRec.MRAgainst.ToString();
                                br.RefNo = CheRec.ChqReceiptNo;
                                br.bankAccode = CheRec.DepositBank;
                                br.ChequeNo = CheRec.ChqNo;
                                br.ChequeDate = CheRec.ChqDate;
                                br.Amount = (double)CheRec.Amount;
                                br.Advance = false;
                                br.Remarks = CheRec.Remarks;
                                br.FinYear = Session["FinYear"].ToString();
                                br.GLPost = CheRec.GLPost;
                                br.BranchCode = CheRec.BranchCode;
                                br.VoucherNo = CheRec.VchrNo;
                                br.SubCode = CheRec.SubCode;
                                br.bankCode = CheRec.BankCode;
                                _bankReceiptService.Update(br);
                                _bankReceiptService.Save();
                            }

                            var MoneyR = _moneyReceiptService.All().Where(s => s.MRNo == CheRec.ChqReceiptNo).FirstOrDefault();
                            //insert to provision and money receipt table
                            if (CheRec.ChqStatus == "Encash" && (MoneyR == null ? "000000" : MoneyR.MRNo) != CheRec.ChqReceiptNo)
                            {
                                MoneyReceipt MR = new MoneyReceipt();
                                MR.MRSL = LoadDropDown.GenerateRecvSlNo(_moneyReceiptService, CheRec.BranchCode, Session["BranchCode"].ToString(), CheRec.ProjCode, Session["VchrConv"].ToString());
                                MR.BranchCode = (CheRec.BranchCode == null) ? Session["BranchCode"].ToString() : CheRec.BranchCode;
                                MR.ProjCode = (CheRec.ProjCode == null) ? Session["ProjCode"].ToString() : CheRec.ProjCode;
                                MR.MRNo = CheRec.ChqReceiptNo;
                                MR.MRDate = CheRec.ChqReceiptDate;
                                MR.MRAgainst = CheRec.MRAgainst;
                                MR.VchrNo = CheRec.VchrNo;
                                MR.PayMode = "Cq";
                                MR.Posted = CheRec.PostDated;
                                MR.AdjWithBill = CheRec.AdjWithBill;
                                MR.MRAmount = CheRec.Amount;
                                MR.Accode = CheRec.MRAgainst;
                                MR.CustCode = CheRec.SubCode;
                                MR.FinYear = Session["FinYear"].ToString();
                                MR.Remarks = CheRec.Remarks;
                                MR.JobNo = CheRec.JobNo;
                                MR.Ca_Bk = CheRec.DepositBank;
                                MR.ChqNo = CheRec.ChqNo;
                                MR.ChqDate = CheRec.ChqDate;
                                MR.EncashDate = CheRec.EncashDate;
                                MR.DepositBank = CheRec.DepositBank;
                                _moneyReceiptService.Add(MR);
                                _moneyReceiptService.Save();

                                var chqExt = _chqRecExtService.All().ToList().Where(s => s.ChqReceiptID == CheRec.ChqReceiptId);
                                foreach (var chqExtitem in chqExt)
                                {
                                    MoneyReceiptExt MRExt = new MoneyReceiptExt();
                                    MRExt.MRId = _moneyReceiptService.All().OrderBy(s => s.MRId).LastOrDefault().MRId;
                                    MRExt.SaleNo = chqExtitem.BillNo;
                                    MRExt.Amount = chqExtitem.BillAmount;
                                    _moneyReceiptExtService.Add(MRExt);
                                    _moneyReceiptExtService.Save();
                                }
                                LoadDropDown.journalVoucherSave("BR", "I", br.BReceiptNo, CheRec.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), CheRec.BranchCode, CheRec.ChqReceiptDate, CheRec.DepositBank, Session["UserName"].ToString());
                            }
                            ////update to provision
                            //LoadDropDown.journalVoucherRemoval("BR", CheRec.ChqReceiptNo, CheRec.VchrNo, Session["FinYear"].ToString());
                            //LoadDropDown.journalVoucherSave("BR", "I", CheRec.ChqReceiptNo, CheRec.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), CheRec.BranchCode, CheRec.ChqReceiptDate, CheRec.DepositBank, Session["UserName"].ToString());
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "ChequeReceipt", "Update", CheRec.ChqReceiptNo, Session["UserName"].ToString());

                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("2", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
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

            MoneyReceiptVM MRVM = new MoneyReceiptVM();
            var crPfd = _chequeReceiptService.All().FirstOrDefault(x => x.ChqReceiptNo == ReceiptNo);
            if (crPfd != null)
            {
                var todayDate = DateTime.Now;
                var TingCount = _chequeReceiptService.All().FirstOrDefault(x => x.ChqReceiptNo == ReceiptNo);
                TingCount.MRTing = TingCount.MRTing + 1;
                TingCount.MRTingTime = todayDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                MRVM.MRNo = crPfd.ChqReceiptNo;
                MRVM.CustCode = crPfd.SubCode.Trim();
                MRVM.ChequeDate = (DateTime)crPfd.ChqDate;
                MRVM.CustName = (from s in _subsidiaryService.All().ToList() where s.SubCode == crPfd.SubCode.Trim() select s.SubName).FirstOrDefault();
                MRVM.Address = (from se in _SubsidiaryExtService.All().ToList() where se.SubCode.Trim() == crPfd.SubCode.Trim() select se.SubAddress).FirstOrDefault();
                MRVM.InWord = InWord.ConvertToWords(crPfd.Amount.ToString());
                MRVM.AcType = LoadDropDown.GetMrAgainst(crPfd.MRAgainst);
                MRVM.MRDate = crPfd.ChqReceiptDate;
                MRVM.Amount = crPfd.Amount;
                MRVM.PayMode = "ChequeReceipt";
                MRVM.ChequeNo = crPfd.ChqNo;
                MRVM.BankName = _bankInfoService.All().FirstOrDefault(s => s.BankCode == crPfd.BankCode).BankName; ;
                MRVM.Branch = crPfd.BranchName;
                _chequeReceiptService.Update(TingCount);
                _chequeReceiptService.Save();
                return new Rotativa.ViewAsPdf("~/Views/CashReceipt/MoneyReceiptPdf.cshtml", MRVM) { PageSize = Rotativa.Options.Size.A4 };

            }
            else
            {
                string errMsg = "NF";
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
            }
        }

        public ActionResult GetReceiptNo(string branchCode)
        {
            return Json(GenerateReceiptNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateReceiptNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string receiptNo = "";
            var cashReceipt = _chequeReceiptService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);
            if (string.IsNullOrEmpty(branchCode))
            {
                var cashRcpt = _chequeReceiptService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (cashRcpt != null)
                {
                    receiptNo = "00" + (Convert.ToInt64(cashRcpt.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = "00000001";
                }
            }
            else
            {
                if (cashReceipt != null)
                {
                    receiptNo = branchCode + (Convert.ToInt64(cashReceipt.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = branchCode + "000001";
                }
            }
            return receiptNo;
        }

        public ActionResult GetJournalVoucher(string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MoneyReceiptChq_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
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
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
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
            if (!rUser.HasPermission("MoneyReceiptChq_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "','" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
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
                return RedirectToAction("ChequeReceipts", "ChequeReceipts", new { errMsg });
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/CashOperation/GLEntriesRcvPdf.cshtml", glReport);
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

        public string GenerateBankReceiptNo(string branchCode)
        {
            string receiptNo = "";
            var bankReceipt = _bankReceiptService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var bankRcpt = _bankReceiptService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (bankRcpt != null)
                {

                    receiptNo = "00" + (Convert.ToInt64(bankRcpt.BReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = "00000001";
                }
            }
            else
            {
                if (bankReceipt != null)
                {
                    receiptNo = branchCode + (Convert.ToInt64(bankReceipt.BReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    receiptNo = branchCode + "000001";
                }

            }

            return receiptNo;

        }
    }
}