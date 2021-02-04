using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class CashReceiptController : Controller
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
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly ICashReceiptSubDetailsAppService _crSubDetailsService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly IIssueDetailsAppService _IssueDetailsService;
        private readonly IVchrMainAppService _VchrMainService;
        private readonly IVchrDetailAppService _VchrDetailService;
        private readonly IBranchAppService _BranchService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        //private readonly ICustomerOpeningAppService _CustomerOpeningService;
        private readonly IGsetAppService _gsetService;
        private readonly ISubsidiaryExtAppService _SubsidiaryExtService;
        private readonly IAcBRAppService _AcBrService;

        public CashReceiptController(ICashOperationAppService _CashOperationService, ICashReceiptAppService _cashReceiptAppService,
            IWithdrawAppService _withdrawService, IExpenseAppService _ExpenseService, IDepositToBankAppService _DepositToBankService,
            IEmployeeAppService _EmployeeService, ISubsidiaryInfoAppService _subsidiaryService, IItemInfoAppService _ItemService,
            INewChartAppService _NewChartService, ILocationAppService _LocationService, IUnitAppService _UnitService,
            ITransactionLogAppService _transactionLogService, IIssueMainAppService _issueMainService, IBranchAppService _BranchService,
            ICashReceiptSubDetailsAppService _crSubDetailsService, IIssueDetailsAppService _IssueDetailsAppService,
            IVchrMainAppService _VchrMainService, IVchrDetailAppService _VchrDetailService, IJarnalVoucherAppService _jarnalVoucherService,
            ISysSetAppService _sysSetService, IGsetAppService _gsetService, ISubsidiaryExtAppService _SubsidiaryExtService,
            IAcBRAppService _AcBrService)
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
            this._transactionLogService = _transactionLogService;
            this._crSubDetailsService = _crSubDetailsService;
            this._IssueDetailsService = _IssueDetailsAppService;
            this._VchrMainService = _VchrMainService;
            this._VchrDetailService = _VchrDetailService;
            this._BranchService = _BranchService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._gsetService = _gsetService;
            this._SubsidiaryExtService = _SubsidiaryExtService;
            this._AcBrService = _AcBrService;
        }


        [HttpPost]
        public ActionResult SaveCashReceipt(CashReceipt cashReceipt)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CashReceipt_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            string branchCode = Session["BranchCode"].ToString();
            ViewBag.branchCode = _BranchService.All().ToList().FirstOrDefault(x => x.BranchCode == branchCode).BranchName;
            //using (var transaction = new TransactionScope())
            //{

            var GCa = _AcBrService.All().Where(s => s.BranchCode == branchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
            if (GCa == null)
            {
                var gset = _gsetService.All().LastOrDefault();
                GCa = gset.GCa;
            }

            try
            {

                var IfExit = _cashReceiptAppService.All().Where(x => x.ReceiptNo == cashReceipt.ReceiptNo).FirstOrDefault();
                if (IfExit == null)
                {
                    if (!string.IsNullOrEmpty(cashReceipt.purAccode) && cashReceipt.purAccode.Trim() != "Select Purpose")
                    {
                        var sysSet = _sysSetService.All().FirstOrDefault();

                        cashReceipt.BranchCode = Session["BranchCode"].ToString();
                        cashReceipt.FinYear = Session["FinYear"].ToString();
                        cashReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == cashReceipt.purAccode.Trim());
                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        CashOperation cashOperation = new CashOperation(cashReceipt.ReceiptDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, cashReceipt.FinYear, false, cashReceipt.BranchCode);
                        var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == cashReceipt.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == cashReceipt.ReceiptDate.ToString("MM-dd-yyyy"));
                        if (isAlreadyClosed == null)
                        {
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {

                                    _CashOperationService.Add(cashOperation);
                                    _CashOperationService.Save();

                                    _cashReceiptAppService.Add(cashReceipt);
                                    _cashReceiptAppService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());

                                    transaction.Complete();

                                }
                                catch (Exception)
                                {
                                    transaction.Dispose();
                                }
                            }
                            return Json(GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode), JsonRequestBehavior.AllowGet);
                        }
                        else if (isAlreadyClosed != null && sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                        {

                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _cashReceiptAppService.Add(cashReceipt);
                                    _cashReceiptAppService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), cashReceipt.BranchCode, cashReceipt.FinYear, Convert.ToDecimal(cashReceipt.Amount), cashReceipt.ReceiptDate);

                                    }
                                    transaction.Complete();
                                }
                                catch (Exception)
                                {
                                    transaction.Dispose();
                                }
                            }
                            return Json(GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            
                            return Json("2", JsonRequestBehavior.AllowGet);

                        }
                    }

                    else
                    {
                        //transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    // transaction.Complete();
                    return Json("3", JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception)
            {
                //transaction.Dispose();
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            //}
        }


        public ActionResult UpdateCashReceipt(CashReceipt cashReceipt, DateTime dateTime)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CashReceipt_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();

                if (ModelState.IsValid)
                {
                    cashReceipt.BranchCode = Session["BranchCode"].ToString();

                    var GCa = _AcBrService.All().Where(s => s.BranchCode == cashReceipt.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                    if (GCa == null)
                    {
                        var gset = _gsetService.All().LastOrDefault();
                        GCa = gset.GCa;
                    }

                    cashReceipt.FinYear = Session["FinYear"].ToString();
                    cashReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == cashReceipt.purAccode.Trim());
                    CashOperation cashOperation = new CashOperation();
                    var isClosed = _CashOperationService.All().LastOrDefault(x => x.BranchCode == cashReceipt.BranchCode && Convert.ToDateTime(x.TrDate.ToShortDateString()) <= Convert.ToDateTime(dateTime.ToShortDateString()));
                    if (isClosed.IsClosed == false)
                    {

                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _cashReceiptAppService.Update(cashReceipt);
                                _cashReceiptAppService.Save();
                                TransactionLogService.SaveTransactionLog(_transactionLogService, "Cash Receipt", "Update", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                //delete to provition 
                                //  var entryNo = Convert.ToInt64(cashReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                var entryNo = cashReceipt.ReceiptNo;
                                LoadDropDown.journalVoucherRemoval("CR", entryNo, cashReceipt.VoucherNo, cashReceipt.FinYear);
                                var sysSet = _sysSetService.All().FirstOrDefault(); 
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), cashReceipt.BranchCode, cashReceipt.FinYear, Convert.ToDecimal(cashReceipt.Amount), cashReceipt.ReceiptDate);

                                }
                                //insert to provition
                                LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());

                                CashOPVM = GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode);

                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }

                        }

                        return Json(CashOPVM, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveCRWithIssue(CashReceipt cashReceipt, IssueMain issueMain, List<IssueDetails> issueDetails)
        {
            try
            {
                if (!string.IsNullOrEmpty(cashReceipt.purAccode) && cashReceipt.purAccode.Trim() != "Select Purpose")
                {
                    cashReceipt.BranchCode = Session["BranchCode"].ToString();
                    cashReceipt.FinYear = Session["FinYear"].ToString();
                    cashReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == cashReceipt.purAccode.Trim());
                    //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                    CashOperation cashOperation = new CashOperation(cashReceipt.ReceiptDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, cashReceipt.FinYear, false, cashReceipt.BranchCode);
                    var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.TrDate.ToString("MM-dd-yyyy") == cashReceipt.ReceiptDate.ToString("MM-dd-yyyy"));
                    if (isAlreadyClosed == null)
                    {
                        IssueMain issueInfo = new IssueMain();
                        issueInfo.IssueNo = issueMain.IssueNo;
                        issueInfo.BranchCode = Session["BranchCode"].ToString();
                        issueInfo.IssueDate = issueMain.IssueDate;
                        issueInfo.FromLocCode = issueMain.FromLocCode;
                        issueInfo.ToLocCode = "No_Location";
                        issueInfo.IssueToSubCode = issueMain.IssueToSubCode;
                        issueInfo.DesLocCode = issueMain.DesLocCode;
                        issueInfo.Accode = cashReceipt.purAccode;
                        issueInfo.RefNo = issueMain.RefNo;
                        issueInfo.RefDate = issueMain.RefDate;
                        issueInfo.Remarks = issueMain.Remarks;
                        issueInfo.IssueByCode = issueMain.IssueByCode;
                        issueInfo.AppByCode = issueMain.AppByCode;
                        issueInfo.IssTime = issueMain.IssTime;
                        issueInfo.FinYear = Session["FinYear"].ToString();
                        issueInfo.GLPost = false;
                        issueInfo.IssDate = issueMain.IssDate;
                        issueInfo.cashReceiptStatus = true;
                        issueInfo.IsReceived = false;

                        double amount = 0.0;

                        List<IssueDetails> issuDetailsList = new List<IssueDetails>();

                        foreach (var issuDetailsItem in issueDetails)
                        {
                            IssueDetails issueDetailsInfo = new IssueDetails();
                            issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                            issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                            issueDetailsInfo.SubCode = issuDetailsItem.SubCode;//issuDetailsItem.SubCode;
                            issueDetailsInfo.Description = issuDetailsItem.Description;
                            issueDetailsInfo.Qty = issuDetailsItem.Qty;
                            //issueDetailsInfo.Price = issuDetailsItem.Price;
                            issueDetailsInfo.Price = issuDetailsItem.Qty * issuDetailsItem.Price;
                            issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                            issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                            amount = amount + issuDetailsItem.Price;
                            issuDetailsList.Add(issuDetailsItem);
                            issueInfo.IssueDetails.Add(issuDetailsItem);
                        }

                        issueInfo.Amount = amount;

                        _issueMainService.Add(issueInfo);
                        // _IssueDetailsService.Add(issuDetailsList);
                        _CashOperationService.Add(cashOperation);
                        _cashReceiptAppService.Add(cashReceipt);

                        _CashOperationService.Save();
                        _cashReceiptAppService.Save();
                        _issueMainService.Save();
                        //_currentStockService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Issue", "Save", issueInfo.IssueNo, Session["UserName"].ToString());
                        return Json(GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode), JsonRequestBehavior.AllowGet);
                    }
                    else if (isAlreadyClosed != null && isAlreadyClosed.IsClosed == false)
                    {
                        IssueMain issueInfo = new IssueMain();
                        issueInfo.IssueNo = issueMain.IssueNo;
                        issueInfo.BranchCode = Session["BranchCode"].ToString();
                        issueInfo.IssueDate = issueMain.IssueDate;
                        issueInfo.FromLocCode = issueMain.FromLocCode;
                        issueInfo.ToLocCode = "No_Location";
                        issueInfo.IssueToSubCode = issueMain.IssueToSubCode;
                        issueInfo.DesLocCode = issueMain.DesLocCode;
                        issueInfo.Accode = cashReceipt.purAccode;
                        issueInfo.RefNo = issueMain.RefNo;
                        issueInfo.RefDate = issueMain.RefDate;
                        issueInfo.Remarks = issueMain.Remarks;
                        issueInfo.IssueByCode = issueMain.IssueByCode;
                        issueInfo.AppByCode = issueMain.AppByCode;
                        issueInfo.IssTime = issueMain.IssTime;
                        issueInfo.FinYear = Session["FinYear"].ToString();
                        issueInfo.GLPost = false;
                        issueInfo.IssDate = issueMain.IssDate;
                        issueInfo.cashReceiptStatus = true;
                        issueInfo.IsReceived = false;

                        double amount = 0.0;

                        List<IssueDetails> issuDetailsList = new List<IssueDetails>();
                        foreach (var issuDetailsItem in issueDetails)
                        {
                            IssueDetails issueDetailsInfo = new IssueDetails();
                            issueDetailsInfo.IssueNo = issuDetailsItem.IssueNo;
                            issueDetailsInfo.ItemCode = issuDetailsItem.ItemCode;
                            issueDetailsInfo.SubCode = issuDetailsItem.SubCode;
                            issueDetailsInfo.Description = issuDetailsItem.Description;
                            issueDetailsInfo.Qty = issuDetailsItem.Qty;
                            //issueDetailsInfo.Price = issuDetailsItem.Price;
                            issueDetailsInfo.Price = issuDetailsItem.Qty * issuDetailsItem.Price;
                            issueDetailsInfo.ExQty = issuDetailsItem.ExQty;
                            issueDetailsInfo.LotNo = issuDetailsItem.LotNo;
                            amount = amount + issuDetailsItem.Price;
                            //issueInfo.IssueDetails.Add(issueDetailsInfo);
                            issuDetailsList.Add(issueDetailsInfo);
                        }

                        issueInfo.Amount = amount;

                        issueInfo.IssueDetails = issuDetailsList;
                        _issueMainService.Add(issueInfo);

                        _cashReceiptAppService.Add(cashReceipt);

                        _issueMainService.Save();
                        _cashReceiptAppService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Issue", "Save", issueInfo.IssueNo, Session["UserName"].ToString());
                        return Json(GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode), JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveCRWithSubsidiary(CashReceipt cashReceipt, List<CashReceiptSubDetails> crDetails)
        {
            try
            {

                List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
                // var IfExit = _cashReceiptAppService.All().Where(x => x.ReceiptNo == cashReceipt.ReceiptNo).FirstOrDefault();
                var ifexit = _crSubDetailsService.All().Where(x => x.CashReceiptNo == cashReceipt.ReceiptNo).FirstOrDefault();
                if (ifexit == null)
                {
                    if (!string.IsNullOrEmpty(cashReceipt.purAccode) && cashReceipt.purAccode.Trim() != "Select Purpose")
                    {
                        cashReceipt.BranchCode = Session["BranchCode"].ToString();

                        var GCa = _AcBrService.All().Where(s => s.BranchCode == cashReceipt.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                        if (GCa == null)
                        {
                            var gset = _gsetService.All().LastOrDefault();
                            GCa = gset.GCa;
                        }
                        var sysSet = _sysSetService.All().FirstOrDefault();

                        cashReceipt.FinYear = Session["FinYear"].ToString();
                        cashReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == cashReceipt.purAccode.Trim());
                        //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                        CashOperation cashOperation = new CashOperation(cashReceipt.ReceiptDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, cashReceipt.FinYear, false, cashReceipt.BranchCode);
                        var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == cashReceipt.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == cashReceipt.ReceiptDate.ToString("MM-dd-yyyy"));
                        if (isAlreadyClosed == null)
                        {
                            List<CashReceiptSubDetails> crSubDetails = new List<CashReceiptSubDetails>();
                            foreach (var subsidiary in crDetails)
                            {
                                CashReceiptSubDetails subDetails = new CashReceiptSubDetails();
                                //subDetails.CashReceiptsubCode = cashReceipt.ReceiptNo;
                                subDetails.SubCode = subsidiary.SubCode;
                                subDetails.Accode = cashReceipt.purAccode;
                                subDetails.Description = subsidiary.Description;
                                subDetails.Amount = subsidiary.Amount;
                                subDetails.CashReceiptNo = cashReceipt.ReceiptNo;

                                crSubDetails.Add(subDetails);
                            }

                            cashReceipt.CashReceiptSubDetails = crSubDetails;

                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _CashOperationService.Add(cashOperation);
                                    _cashReceiptAppService.Add(cashReceipt);
                                    _CashOperationService.Save();
                                    _cashReceiptAppService.Save();

                                    //_crSubDetailsService.Save();

                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());
                                    CashOPVM = GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode);
                                    transaction.Complete();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Dispose();
                                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                                }
                                return Json(CashOPVM, JsonRequestBehavior.AllowGet);

                            }

                        }
                        else if (isAlreadyClosed != null && sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                        {
                            List<CashReceiptSubDetails> crSubDetails = new List<CashReceiptSubDetails>();
                            foreach (var subsidiary in crDetails)
                            {
                                CashReceiptSubDetails subDetails = new CashReceiptSubDetails();
                                //subDetails.CashReceiptsubCode = cashReceipt.ReceiptNo;
                                subDetails.SubCode = subsidiary.SubCode;
                                subDetails.Accode = cashReceipt.purAccode;
                                subDetails.Description = subsidiary.Description;
                                subDetails.Amount = subsidiary.Amount;
                                subDetails.CashReceiptNo = cashReceipt.ReceiptNo;
                                crSubDetails.Add(subDetails);
                            }

                            cashReceipt.CashReceiptSubDetails = crSubDetails;
                            using (var transaction = new TransactionScope())
                            {
                                try
                                {
                                    _cashReceiptAppService.Add(cashReceipt);
                                    //_crSubDetailsService.Save();
                                    _cashReceiptAppService.Save();
                                    TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                    LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());
                                    if (sysSet.CashRule == "O")
                                    {
                                        LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), cashReceipt.BranchCode, cashReceipt.FinYear, Convert.ToDecimal(cashReceipt.Amount), cashReceipt.ReceiptDate);

                                    }
                                    CashOPVM = GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode);
                                    transaction.Complete();
                                }
                                catch (Exception)
                                {
                                    transaction.Dispose();
                                }
                                return Json(CashOPVM, JsonRequestBehavior.AllowGet);

                            }
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
                else
                {
                    return Json("3", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMoneyReceiptPdf(string ReceiptNo)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("CashReceipt_Print"))
            {
                string errMsg = "P";
                return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
            }
            MoneyReceiptVM MRVM = new MoneyReceiptVM();
            var cashRPfd = _cashReceiptAppService.All().FirstOrDefault(x => x.ReceiptNo == ReceiptNo);
            var CRWithSub = _crSubDetailsService.All().FirstOrDefault(s => s.CashReceiptNo == ReceiptNo);
            if (cashRPfd != null && CRWithSub != null)
            {
                MRVM.MRNo = cashRPfd.ReceiptNo;
                MRVM.CustCode = CRWithSub.SubCode.Trim();
                MRVM.CustName = (from s in _subsidiaryService.All().ToList() where s.SubCode == CRWithSub.SubCode.Trim() select s.SubName).FirstOrDefault();
                MRVM.Address = (from se in _SubsidiaryExtService.All().ToList() where se.SubCode.Trim() == CRWithSub.SubCode.Trim() select se.SubAddress).FirstOrDefault();
                //MRVM.CustName = _subsidiaryService.All().FirstOrDefault(s => s.SubCode == CRWithSub.SubCode.Trim()).SubName;
                //MRVM.Address = _SubsidiaryExtService.All().FirstOrDefault(s => s.SubCode.Trim() == CRWithSub.SubCode.Trim()).SubAddress; 
                MRVM.InWord = InWord.ConvertToWords(Convert.ToString(CRWithSub.Amount));
                MRVM.AcType = _NewChartService.All().FirstOrDefault(s => s.Accode == cashRPfd.purAccode).AcName;
                MRVM.MRDate = cashRPfd.ReceiptDate;
                MRVM.Amount = Convert.ToDecimal(CRWithSub.Amount);
                MRVM.PayMode = "ChshReceipt";
                return new Rotativa.ViewAsPdf("~/Views/CashReceipt/MoneyReceiptPdf.cshtml", MRVM) { PageSize = Rotativa.Options.Size.A4 };
            }
            else
            {
                //string errMsg = "E";
                //return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
                MRVM.PayMode = "ChshReceipt";
                return new Rotativa.ViewAsPdf("~/Views/CashReceipt/MoneyReceiptPdf.cshtml", MRVM) { PageSize = Rotativa.Options.Size.A4 };
            }
        }

        //change by shahin for subsidiary with update 
        public ActionResult UpdateCRWithSubsidiary(CashReceipt cashReceipt, List<CashReceiptSubDetails> crDetails)
        {
            try
            {// var IfExit = _cashReceiptAppService.All().Where(x => x.ReceiptNo == cashReceipt.ReceiptNo).FirstOrDefault();
                //var ifexit = _crSubDetailsService.All().Where(x => x.CashReceiptNo == cashReceipt.ReceiptNo).FirstOrDefault();
                //if (ifexit == null)
                //{

                if (!string.IsNullOrEmpty(cashReceipt.purAccode) && cashReceipt.purAccode.Trim() != "Select Purpose")
                {
                    List<CashOperationVModel> CashOPVM = new List<CashOperationVModel>();
                    cashReceipt.BranchCode = Session["BranchCode"].ToString();
                    var sysSet = _sysSetService.All().FirstOrDefault(); 

                    var GCa = _AcBrService.All().Where(s => s.BranchCode == cashReceipt.BranchCode && s.Ca_Ba == "Ca").Select(x => x.Accode).FirstOrDefault();
                    if (GCa == null)
                    {
                        var gset = _gsetService.All().LastOrDefault();
                        GCa = gset.GCa;
                    }

                    cashReceipt.FinYear = Session["FinYear"].ToString();
                    cashReceipt.NewChart = _NewChartService.All().ToList().FirstOrDefault(x => x.Accode == cashReceipt.purAccode.Trim());
                    //cashReceipt.Customer = _CustomerService.All().ToList().FirstOrDefault(x => x.CustCode == cashReceipt.CustCode);
                    CashOperation cashOperation = new CashOperation(cashReceipt.ReceiptDate, 0.0, 0.0, 0.0, 0.0, "", DateTime.Now, false, cashReceipt.FinYear, false, cashReceipt.BranchCode);
                    var isAlreadyClosed = _CashOperationService.All().ToList().FirstOrDefault(x => x.BranchCode == cashReceipt.BranchCode && x.TrDate.ToString("MM-dd-yyyy") == cashReceipt.ReceiptDate.ToString("MM-dd-yyyy"));
                    if (isAlreadyClosed == null)
                    {
                        _crSubDetailsService.All().ToList().Where(y => y.CashReceiptNo == cashReceipt.ReceiptNo).ToList().ForEach(x => _crSubDetailsService.Delete(x));
                        _crSubDetailsService.Save();
                        List<CashReceiptSubDetails> crSubDetails = new List<CashReceiptSubDetails>();
                        foreach (var subsidiary in crDetails)
                        {

                            CashReceiptSubDetails subDetails = new CashReceiptSubDetails();
                            //subDetails.CashReceiptsubCode = cashReceipt.ReceiptNo;
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = cashReceipt.purAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;
                            subDetails.CashReceiptNo = cashReceipt.ReceiptNo;

                            //crSubDetails.Add(subDetails);
                            _crSubDetailsService.Add(subDetails);
                        }

                        cashReceipt.CashReceiptSubDetails = crSubDetails;

                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _CashOperationService.Add(cashOperation);
                                _cashReceiptAppService.Update(cashReceipt);
                                _CashOperationService.Save();
                                _crSubDetailsService.Save();

                                //_crSubDetailsService.Save();

                                TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                //delete to provition 
                                //  var entryNo = Convert.ToInt64(cashReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                var entryNo = cashReceipt.ReceiptNo;
                                LoadDropDown.journalVoucherRemoval("CR", entryNo, cashReceipt.VoucherNo, cashReceipt.FinYear);

                                LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());
                                CashOPVM = GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }

                        }

                        return Json(CashOPVM, JsonRequestBehavior.AllowGet);
                    }
                    else if (isAlreadyClosed != null &&  sysSet.CashRule == "O" ? true : isAlreadyClosed.IsClosed == false)
                    {
                        _crSubDetailsService.All().ToList().Where(y => y.CashReceiptNo == cashReceipt.ReceiptNo).ToList().ForEach(x => _crSubDetailsService.Delete(x));
                        _crSubDetailsService.Save();

                        List<CashReceiptSubDetails> crSubDetails = new List<CashReceiptSubDetails>();
                        foreach (var subsidiary in crDetails)
                        {

                            CashReceiptSubDetails subDetails = new CashReceiptSubDetails();
                            //subDetails.CashReceiptsubCode = cashReceipt.ReceiptNo;
                            subDetails.SubCode = subsidiary.SubCode;
                            subDetails.Accode = cashReceipt.purAccode;
                            subDetails.Description = subsidiary.Description;
                            subDetails.Amount = subsidiary.Amount;
                            subDetails.CashReceiptNo = cashReceipt.ReceiptNo;


                            //crSubDetails.Add(subDetails);
                            _crSubDetailsService.Add(subDetails);


                        }

                        cashReceipt.CashReceiptSubDetails = crSubDetails;

                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                _cashReceiptAppService.Update(cashReceipt);
                                //_crSubDetailsService.Save();
                                _cashReceiptAppService.Save();
                                _crSubDetailsService.Save();

                                TransactionLogService.SaveTransactionLog(_transactionLogService, "CashReceipt", "Save", cashReceipt.ReceiptNo, Session["UserName"].ToString());
                                //delete to provition 
                                //  var entryNo = Convert.ToInt64(cashReceipt.VoucherNo.Substring(2, 8)).ToString().PadLeft(8, '0');
                                var entryNo = cashReceipt.ReceiptNo;
                                LoadDropDown.journalVoucherRemoval("CR", entryNo, cashReceipt.VoucherNo, cashReceipt.FinYear);
                                if (sysSet.CashRule == "O")
                                {
                                    LoadDropDown.CashRecalculate(Session["ProjCode"].ToString(), cashReceipt.BranchCode, cashReceipt.FinYear, Convert.ToDecimal(cashReceipt.Amount), cashReceipt.ReceiptDate);

                                }
                                LoadDropDown.journalVoucherSave("CR", "I", cashReceipt.ReceiptNo, cashReceipt.VoucherNo, cashReceipt.FinYear, "01", cashReceipt.BranchCode, cashReceipt.ReceiptDate, GCa, Session["UserName"].ToString());
                                CashOPVM = GetAllCashReceiptAndWithdraw(cashReceipt.ReceiptDate, cashReceipt.BranchCode);
                                transaction.Complete();
                            }
                            catch (Exception)
                            {
                                transaction.Dispose();
                                return Json("0", JsonRequestBehavior.AllowGet);
                            }

                        }

                        return Json(CashOPVM, JsonRequestBehavior.AllowGet);
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
                //}
                //else
                //{
                //    return Json("3", JsonRequestBehavior.AllowGet);
                //}
            }

            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetReceiptNo(string branchCode)
        {
            return Json(GenerateReceiptNo(branchCode), JsonRequestBehavior.AllowGet);
        }
        public string GenerateReceiptNo(string branchCode)
        {
            branchCode = branchCode == null ? Session["BranchCode"].ToString() : branchCode;
            string receiptNo = "";
            var cashReceipt = _cashReceiptAppService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var cashRcpt = _cashReceiptAppService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (cashRcpt != null)
                {

                    receiptNo = "00" + (Convert.ToInt64(cashRcpt.ReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
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
                    receiptNo = branchCode + (Convert.ToInt32(cashReceipt.ReceiptNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');

                }
                else
                {
                    receiptNo = branchCode + "000001";
                }

            }

            return receiptNo;

        }

        #region Generate Voucher No
        public ActionResult GetNewVoucherNo(string VType)
        {
            string finYear = Session["FinYear"].ToString();

            string voucherNo = "";
            //string VLen = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sql = string.Format("exec [GetNewVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "'  ,'" + finYear + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sql).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExistVoucherNo(string VType, DateTime TrDate)
        {

            string voucherNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sqls = string.Format("exec [GetExistVoucherNo] '" + VType + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "','" + TrDate.ToString("MM-dd-yyyy") + "'");
            voucherNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sqls).ToList().FirstOrDefault().VchrNo.ToString());

            return Json(voucherNo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult GetCashReceiptBYReceiptNo(string receiptNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + receiptNo + "' and EntrySource='CR'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();

            var cashReceipt = _cashReceiptAppService.All().FirstOrDefault(x => x.ReceiptNo == receiptNo.Trim());
            if (cashReceipt != null && IsExit.Count != 0)
            {
                return Json(cashReceipt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

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


        #region Generate Journal Voucher No

        public ActionResult GetJVNo()
        {
            return Json(GenerateJVNo(), JsonRequestBehavior.AllowGet);
        }

        public string GenerateJVNo()
        {
            string branchCode = Session["Garden"].ToString();
            string JVNo = "";
            var items = _VchrMainService.All().ToList().LastOrDefault(x => x.BranchCode == branchCode);
            if (items != null)
            {
                JVNo = "JV" + branchCode + (Convert.ToInt64(items.VchrNo.Substring(4, 6)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                JVNo = "JV" + branchCode + "00001";
            }
            return JVNo;
        }

        #endregion

        #region Journal Voucher PDF

        public ActionResult CashRcptJVPDF(CashReceipt crJV)
        {
            VchrMain vchrMain = new VchrMain();
            //vchrMain.VchrNo = GenerateJVNo();
            vchrMain.BranchCode = Session["Garden"].ToString();
            ViewBag.Date = DateTime.Now;
            ViewBag.VchrNo = GenerateJVNo();
            ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vchrMain.BranchCode.Trim()).BranchName.ToString();
            ViewBag.Remarks = crJV.Remarks;
            ViewBag.Accode = crJV.purAccode;
            ViewBag.AcName = _NewChartService.All().FirstOrDefault(x => x.Accode == crJV.purAccode.Trim()).AcName.ToString();
            ViewBag.Amount = crJV.Amount;
            return new Rotativa.ViewAsPdf("CashRcptJVPDF", crJV);
        }

        #endregion





        public ActionResult GetCashReceipt(string receiptNo)
        {
            var cashReceiptDet = _crSubDetailsService.All().Where(x => x.CashReceiptNo == receiptNo.Trim()).ToList();//shahin Change x => x.SubCode == re
            if (cashReceiptDet.Count > 0)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }
        }


        //shahin
        public ActionResult GetCashReceiptSubBYReceiptNo(string receiptNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + receiptNo + "' and EntrySource='CR'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();

            var cashReceipt = _cashReceiptAppService.All().FirstOrDefault(x => x.ReceiptNo == receiptNo.Trim());
            if (cashReceipt != null && IsExit.Count != 0)
            {
                return Json(cashReceipt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }


        //calling  off by shahin (For subsidiary edit )
        public ActionResult GetAllByReceiptNo(string receiptNo)
        {
            string sql = string.Format("Select pvt.VchrMainId as VNo from PVchrTrack as pvt  inner join PVchrMain as pvm  on pvt.VchrMainId=pvm.PVchrMainId where EntryNo='" + receiptNo + "' and EntrySource='CR'");

            List<JarnalVoucher> IsExit = _jarnalVoucherService.SqlQueary(sql).ToList();

            AcclineERPContext DbContext = new AcclineERPContext();

            DbContext.CashReceipt.AsNoTracking().FirstOrDefault();

            CashReceiptVModel cashReceipt = new CashReceiptVModel();
            //CRSubDetailsVModel vmode = new CRSubDetailsVModel();
            cashReceipt.Main = _cashReceiptAppService.All().ToList().FirstOrDefault(x => x.ReceiptNo == receiptNo.Trim());

            //AcclineERPContext _dbcon = new AcclineERPContext();

            //_dbcon.Configuration.ProxyCreationEnabled = false;

            var items = _crSubDetailsService.All().Where(x => x.CashReceiptNo == receiptNo.Trim()).ToList();//shahin Change x => x.SubCode == receiptNo.Trim())
            if (items.Count > 0)
            {
                //int i = 0;
                DbContext.CashReceiptSubDetails.AsNoTracking().FirstOrDefault();
                foreach (var item in items)
                {
                    cashReceipt.Details.Add(new CRSubDetailsVModel(item.SubCode, item.Amount, item.Description, item.SubsidiaryInfo.SubName));
                }
            }

            List<CRSubDetailsVModel> Details = new List<CRSubDetailsVModel>();
            Details = cashReceipt.Details;
            CashReceipt Main = new CashReceipt();
            Main = cashReceipt.Main;

            //jsonconvert()

            //PreserveReferencesHandling fd = PreserveReferencesHandling.Objects;
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

            string json = JsonConvert.SerializeObject(cashReceipt, Formatting.Indented, serializerSettings);

            var check = Json(json, JsonRequestBehavior.AllowGet);
            if (json != null && IsExit.Count != 0)
            {
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            // return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CashReceiptNoIncreement(CashReceipt model)
        {
            try
            {
                model.BranchCode = Session["BranchCode"].ToString();
                var expenseNo = _cashReceiptAppService.All().Where(x => x.BranchCode == model.BranchCode).LastOrDefault();
                int exNo = Convert.ToInt32(expenseNo.ReceiptNo);
                string ExpenseNo = (exNo + 1).ToString();
                string IncInvcNo = "";
                if (ExpenseNo.Length > 7)
                {
                    IncInvcNo = ExpenseNo.ToString();
                }
                else
                {
                    IncInvcNo = "0" + ExpenseNo.ToString();
                }

                return Json(IncInvcNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
    }
}
