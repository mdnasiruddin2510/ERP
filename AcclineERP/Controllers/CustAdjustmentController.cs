using App.Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using App.Domain.ViewModel;

namespace AcclineERP.Controllers
{
    public class CustAdjustmentController : Controller
    {
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ILocationAppService _locationService;
        private readonly ISalesMainAppService _SalesMainService;
        private readonly ISalesDetailAppService _SaleDetailService;
        private readonly IEmployeeFuncAppService _EmpFuncService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IDefACAppService _defACService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IMoneyReceiptAppService _moneyReceiptService;
        private readonly IVchrSetAppService _vchrSetService;
        private readonly ICustAdjustmentAppService _custAdjustmentService;
        private readonly IFYDDAppService _FYDDAppService;
        private readonly ICustAdjExtAppService _custAdjExtService;
        private readonly ITransactionLogAppService _transactionLogService;
        public CustAdjustmentController(IJarnalVoucherAppService _jarnalVoucherService, ILocationAppService _locationService, ISalesMainAppService _SalesMainService,
            ISalesDetailAppService _SaleDetailService, IEmployeeFuncAppService _EmpFuncService,
            IEmployeeAppService _EmployeeService, ISysSetAppService _sysSetService, IDefACAppService _defACService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IMoneyReceiptAppService _moneyReceiptService,
            IVchrSetAppService _vchrSetService, ICustAdjustmentAppService _custAdjustmentService, IFYDDAppService _FYDDAppService,
            ICustAdjExtAppService _custAdjExtService, ITransactionLogAppService _transactionLogService)
        {
            this._jarnalVoucherService = _jarnalVoucherService;
            this._locationService = _locationService;
            this._SalesMainService = _SalesMainService;
            this._SaleDetailService = _SaleDetailService;
            this._EmpFuncService = _EmpFuncService;
            this._EmployeeService = _EmployeeService;
            this._sysSetService = _sysSetService;
            this._defACService = _defACService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._moneyReceiptService = _moneyReceiptService;
            this._vchrSetService = _vchrSetService;
            this._custAdjustmentService = _custAdjustmentService;
            this._FYDDAppService = _FYDDAppService;
            this._custAdjExtService = _custAdjExtService;
            this._transactionLogService = _transactionLogService;
        }
        // GET: CustAdjustment
        public ActionResult CustAdjustment()
        {
            if (Session["UserID"] != null)
            {
                DateTime datetime = DateTime.Now;
                ViewBag.GLProvition = Counter("JB");
                ViewBag.GLEntries = CountEntries("JB", datetime);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "1").OrderBy(x => x.SubName), "SubCode", "SubName");
                ViewBag.InvNo = LoadDropDown.LoadEmpDlList();
                ViewBag.AdjReason = LoadDropDown.LoadAdjReason();
                ViewBag.ApprBy = new SelectList(_EmployeeService.All().ToList(), "Id", "UserName");
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                ViewBag.AdjNo = LoadDropDown.GenerateAdjustmentNo(_custAdjustmentService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult GenerateAdjustmentNo()
        {
            return Json(LoadDropDown.GenerateAdjustmentNo(_custAdjustmentService, "", Session["BranchCode"].ToString(), "", Session["VchrConv"].ToString()), JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetInvMrNoByVchrType(string VchrType, string CustCode)
        {
            return Json(LoadDropDown.GetInvMrNoByVchrType(VchrType, CustCode, _SalesMainService, _moneyReceiptService, _FYDDAppService), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetInvNoByAmount(string InvNo, string VchrType)
        {
            if (VchrType == "Money Receipt")
            {
                var MRs = _moneyReceiptService.All().ToList().Where(x => x.MRNo == InvNo).Select(s => new
                {
                    MRAmount = s.MRAmount
                }).FirstOrDefault();
                return Json(new { MRs = MRs }, JsonRequestBehavior.AllowGet);
            }
            else if (VchrType == "Invoice")
            {
                var Sales = _SalesMainService.All().ToList().Where(x => x.SaleNo == InvNo).Select(s =>
                    new
                    {
                        TotSaleAmt = s.TotSaleAmt,
                        NetAmount = s.NetAmount,
                        ReceiptAmt = s.ReceiptAmt
                    }).FirstOrDefault();
                return Json(new { Sales = Sales }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveUpdateCustAdj(CustAdjustment CustAdj, List<CustAdjExt> CustAdjExt, string IsSave)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());

                    #region for Cust Adjustment transection
                    string trnType = "Save";
                    string Msg = "1";

                    //Firstly deleted the saved For Update
                    if (IsSave == "1")
                    {
                        if (!rUser.HasPermission("CustAdjustment_Update"))
                        {
                            return Json("U", JsonRequestBehavior.AllowGet);
                        }
                        trnType = "Update";
                        Msg = "2";
                        _custAdjExtService.All().ToList().Where(y => y.AdjNo == CustAdj.AdjNo).ToList().ForEach(x => _custAdjExtService.Delete(x));
                        _custAdjExtService.Save();
                        var cAdjDel = _custAdjustmentService.All().ToList().Where(y => y.AdjNo == CustAdj.AdjNo).FirstOrDefault();
                        _custAdjustmentService.Delete(cAdjDel);
                        _custAdjustmentService.Save();
                    }

                    if (!rUser.HasPermission("CustAdjustment_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    var IfExit = _custAdjustmentService.All().Where(x => x.AdjNo == CustAdj.AdjNo).FirstOrDefault();
                    if (IfExit == null)
                    {
                        if (CustAdjExt != null)
                        {
                            foreach (var CAdjExtItem in CustAdjExt)
                            {
                                CustAdjExt CAdjExt = new CustAdjExt();
                                CAdjExt.AdjNo = CAdjExtItem.AdjNo;
                                CAdjExt.AdjAmt = CAdjExtItem.AdjAmt;
                                CAdjExt.AdjOn = CAdjExtItem.AdjOn;
                                CAdjExt.AdjReason = CAdjExtItem.AdjReason;
                                CAdjExt.CustCode = CAdjExtItem.CustCode;
                                _custAdjExtService.Add(CAdjExt);
                                _custAdjExtService.Save();
                            }
                        }

                        CustAdj.FinYear = Session["FinYear"].ToString();
                        CustAdj.BranchCode = Session["BranchCode"].ToString();
                        CustAdj.ProjCode = Session["ProjCode"].ToString();
                        CustAdj.EntryDateTime = DateTime.Now;
                        if (CustAdj.AdjType == "Invoice")
                        {
                            if (CustAdj.DrCr == "Debit")
                            {
                                foreach (var item in CustAdjExt)
                                {
                                    var salesMain = _SalesMainService.All().Where(x => x.SaleNo == item.AdjOn).FirstOrDefault();

                                    salesMain.AdjAmt = (salesMain.AdjAmt == null ? 0 : salesMain.AdjAmt) + item.AdjAmt;
                                    if ((salesMain.ReceiptAmt == null ? 0 : salesMain.ReceiptAmt) + salesMain.AdjAmt == salesMain.NetAmount)
                                    {
                                        salesMain.IsPaid = true;
                                    }
                                    _SalesMainService.Update(salesMain);
                                    _SalesMainService.Save();
                                }
                            }
                            else
                            {
                                foreach (var item in CustAdjExt)
                                {
                                    var salesMain = _SalesMainService.All().Where(x => x.SaleNo == item.AdjOn).FirstOrDefault();

                                   salesMain.AdjAmt = (salesMain.AdjAmt == null ? 0 : salesMain.AdjAmt) + item.AdjAmt * (-1);
                                    if ((salesMain.ReceiptAmt == null ? 0 : salesMain.ReceiptAmt) + salesMain.AdjAmt == salesMain.NetAmount)
                                    {
                                        salesMain.IsPaid = true;
                                    }
                                    _SalesMainService.Update(salesMain);
                                    _SalesMainService.Save();
                                }
                            }

                        }
                        //else if (CustAdj.AdjType == "Money Receipt")
                        //{
                        //    if (CustAdj.DrCr == "Debit")
                        //    {
                        //        foreach (var item in CustAdjExt)
                        //        {
                        //            var salesMain = _SalesMainService.All().Where(x => x.SaleNo == item.AdjOn).FirstOrDefault();

                        //            salesMain.AdjAmt += item.AdjAmt;
                        //            if ((salesMain.ReceiptAmt == null ? 0 : salesMain.ReceiptAmt) + salesMain.AdjAmt == salesMain.NetAmount)
                        //            {
                        //                salesMain.IsPaid = true;
                        //            }
                        //            _SalesMainService.Update(salesMain);
                        //            _SalesMainService.Save();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        foreach (var item in CustAdjExt)
                        //        {
                        //            var salesMain = _SalesMainService.All().Where(x => x.SaleNo == item.AdjOn).FirstOrDefault();

                        //            salesMain.AdjAmt += item.AdjAmt * (-1);
                        //            if ((salesMain.ReceiptAmt == null ? 0 : salesMain.ReceiptAmt) + salesMain.AdjAmt == salesMain.NetAmount)
                        //            {
                        //                salesMain.IsPaid = true;
                        //            }
                        //            _SalesMainService.Update(salesMain);
                        //            _SalesMainService.Save();
                        //        }
                        //    }

                        //}
                        _custAdjustmentService.Add(CustAdj);
                        _custAdjustmentService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Customer Adjustment", trnType, CustAdj.AdjNo, Session["UserName"].ToString());
                        //LoadDropDown.ProvitionInvRSave("IR", "I", recvInfo.RcvNo, recvInfo.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), recvMain.RcvDate.ToString("yyyy-MM-dd"), Session["UserName"].ToString());
                        transaction.Complete();
                        return Json(Msg, JsonRequestBehavior.AllowGet);

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
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetAllByAdjNo(string AdjNo)
        {
            var custAdj = _custAdjustmentService.All().ToList().FirstOrDefault(x => x.AdjNo == AdjNo);
            var cAExt = _custAdjExtService.All().ToList().Where(x => x.AdjNo == AdjNo).ToList();
            return Json(new { custAdj = custAdj, cAExt = cAExt }, JsonRequestBehavior.AllowGet);
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
    }
}