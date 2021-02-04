using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class SalesF1Controller : Controller
    {
        private readonly ILocationAppService _locationService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IItemWiseDisVatAppService _ItemWiseDisVatService;
        private readonly ISalesMainAppService _SalesMainService;
        private readonly ISalesDetailAppService _SaleDetailService;
        private readonly IIssueMainAppService _IssueMainService;
        private readonly IIssueDetailsAppService _IssueDetailService;
        private readonly IEmployeeFuncAppService _EmpFuncService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;

        public SalesF1Controller(ILocationAppService _locationService, IItemTypeAppService _itemTypeService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IItemInfoAppService _itemInfoService,
            ICurrentStockAppService _currentStockService, ICommonPVVMAppService _CommonVmService,
            IItemWiseDisVatAppService _ItemWiseDisVatService, ISalesMainAppService _SalesMainService,
            ISalesDetailAppService _SaleDetailService, IIssueMainAppService _IssueMainService,
            IIssueDetailsAppService _IssueDetailService, IEmployeeFuncAppService _EmpFuncService,
            IEmployeeAppService _EmployeeService, IIssueMainAppService _issueMainService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService)
        {
            this._locationService = _locationService;
            this._itemTypeService = _itemTypeService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._itemInfoService = _itemInfoService;
            this._currentStockService = _currentStockService;
            this._CommonVmService = _CommonVmService;
            this._ItemWiseDisVatService = _ItemWiseDisVatService;
            this._SalesMainService = _SalesMainService;
            this._SaleDetailService = _SaleDetailService;
            this._IssueMainService = _IssueMainService;
            this._IssueDetailService = _IssueDetailService;
            this._EmpFuncService = _EmpFuncService;
            this._EmployeeService = _EmployeeService;
            this._issueMainService = _issueMainService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
        }
        public AcclineERPContext db = new AcclineERPContext();

        // GET: SalesF1
        public ActionResult SalesF1(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.SaleNo = GenerateSaleNo(branchCode);
                ViewBag.LocCode = new SelectList(_locationService.All().ToList(), "LocCode", "LocName");
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "1").OrderBy(x => x.SubName), "SubCode", "SubName");
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                //ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                //ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                //ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                ViewBag.ItemCode = LoadDropDown.LoadItemBySSGroupID("", "", "", "", _itemInfoService, _CommonVmService);
                ViewBag.InvNo = GenerateInvNo();
                ViewBag.GLProvition = Count("AR");
                ViewBag.GLEntries = CountEntries("AR", DateTime.Now);
                //For Packing List
                ViewBag.PackItemCode = LoadDropDown.LoadEmpDlList();
                ViewBag.PackItem = LoadDropDown.LoadEmpDlList();

                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public string GenerateInvNo()
        {
            string invSql = string.Format("exec GetNewInvoiceNo 'B','01','01','" + Session["BranchCode"].ToString() + "',6");
            string InvNo = _ItemWiseDisVatService.SqlQueary(invSql).ToList().FirstOrDefault().InvoiceNo;
            return InvNo;
        }

        public string GenerateSaleNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string issueNo = "";
            var rcvNo = _SalesMainService.All().ToList().LastOrDefault(x => x.BranchCode.Trim() == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var recvNo = _SalesMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
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

        public ActionResult GetJournalVoucher(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Sales_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "' "
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac");

            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("Sales", "Sales", new { errMsg });

            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Sales_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "', '" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGlECountN(string VType, DateTime TrDate)
        {
            return Json(CountEntries(VType, TrDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGlECount(string VType)
        {
            return Json(Count(VType), JsonRequestBehavior.AllowGet);
        }

        public string CountEntries(string pageType, DateTime TrDate)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'and pvm.VDate='" + TrDate.ToString("MM/dd/yyyy") + "'");
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

        public string Count(string pageType)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'");
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