using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class ListOfTransactionController : Controller
    {

        private readonly IJTrGrpAppService _jTrGrpService;
        private readonly IBranchAppService _BranchService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly IUserBranchAppService _userbranchService;
        private readonly IFYDDAppService _FYDDService;
        private readonly ISysSetAppService _sysSetService;

        public ListOfTransactionController(IBranchAppService _BranchService, IEmployeeAppService _employeeService,
            IJTrGrpAppService _jTrGrpService, IJarnalVoucherAppService _jarnalVoucherService,
            IUserBranchAppService _userbranchService, IFYDDAppService _FYDDService, ISysSetAppService _sysSetService)
        {
            this._jTrGrpService = _jTrGrpService;
            this._BranchService = _BranchService;
            this._employeeService = _employeeService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._userbranchService = _userbranchService;
            this._FYDDService = _FYDDService;
            this._sysSetService = _sysSetService;
        }
        //
        // GET: /ListOfTransection/

        public ActionResult Search(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                //ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == Session["BranchCode"].ToString()).BranchName;
                ViewBag.JTrGrpId = new SelectList(_jTrGrpService.All().ToList(), "VType", "TypeDesc");
                ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName"); //GardenSelection();
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        //public SelectList GardenSelection()
        //{
        //    var UserName = User.Identity.Name;
        //    var brans = _employeeService.All().Where(x => x.Email == UserName).ToList().FirstOrDefault();
        //    var UserId = brans.Id;
        //    var items = _userbranchService.All().ToList().Where(x => x.Userid == UserId.ToString()).ToList();
        //    List<Branch> branchList = new List<Branch>();
        //    foreach (var item in items)
        //    {
        //        var branch = _BranchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
        //        branchList.Add(branch);
        //    }
        //    branchList.Insert(0, new Branch() { BranchCode = "0", BranchName = "---- Select ----" });
        //    //List<Branch> BranchCode = new SelectList(branchList, "BranchCode", "BranchName");
        //    return new SelectList(branchList.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");

        //}
        public SelectList GardenSelection()
        {

            var UserName = Session["UserName"].ToString();
            var brans = _employeeService.All().Where(x => x.Email == UserName).FirstOrDefault();

            //shahin

            List<Branch> branchs = _BranchService.All().ToList();
            List<UserBranch> userbranch = _userbranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == brans.Id.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();

            if (branchInfo.Count == 1)
            {
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else if (branchInfo.Count > 1)
            {
                //List<Branch> branch = _BranchService.All().ToList();
                branchInfo.Insert(0, new { BranchCode = "0", BranchName = "All" });
                return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
            }
            else
            {
                return null;
            }


        }
        class InWordsAmt
        {
            public string InWords { get; set; }
        }

        [HttpPost]
        public ActionResult GetGLEntries(RptSearchVModel vmodel)
        {
            var ChkFYR = GetCompanyInfo.ValidateFinYearDateRange(Convert.ToString(vmodel.fDate), Convert.ToString(vmodel.toDate), Session["FinYear"].ToString());
            if (ChkFYR != "")
            {
                return RedirectToAction("Search", "ListOfTransaction", new { errMsg = ChkFYR });
            }

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("RptJournalVoucher_Preview"))
            {
                string errMsg = "No Preview Permission for this User !!";
                return RedirectToAction("Search", "ListOfTransaction", new { errMsg });
            }

            string finyear = Session["FinYear"].ToString();
            Session["Branch"] = vmodel.BranchCode;
            Session["LedgerType"] = vmodel.LedgerTypeCode;
            Session["fDate"] = vmodel.fDate;
            Session["tDate"] = vmodel.toDate;
            Session["Data"] = vmodel.JTrGrpId;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            if (vmodel.JTrGrpId == null)
            {
                vmodel.JTrGrpId = "";
            }
            // Session["NullData"] = vmodel.JTrGrpId = "";

            string sql = string.Format("EXEC rptListTran '" + finyear + "','01','" + vmodel.BranchCode + "','" + vmodel.JTrGrpId.Trim() + "','" + vmodel.fDate.ToString("MM-dd-yyyy") + "','" + vmodel.toDate.ToString("MM-dd-yyyy") + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            // Session["Data"] = glReport;
            //if (glReport.Count == 0)
            //{
            //    string errMsg = "Data not found !!!";
            //    return RedirectToAction("Search", "ListOfTransaction", new { errMsg });
            //}
            //else
            //{
                double amt = 0;
                List<JarnalVoucher> sList = new List<JarnalVoucher>();//InWord.ConvertToWords(amt.ToString()).ToList();

                foreach (var item in glReport)
                {
                    //if (item.CrAmount != 0)
                    // {
                    if (item.DrAmount > 0)
                    {
                        amt = item.DrAmount;
                    }
                    else if (item.CrAmount > 0)
                    {
                        amt = item.CrAmount;
                    }

                    JarnalVoucher sAmt = new JarnalVoucher();
                    sAmt.ACCode = item.ACCode;
                    sAmt.VDate = item.VDate;
                    sAmt.AcName = item.AcName;
                    sAmt.InWords = InWord.ConvertToWords(amt.ToString());
                    sAmt.AcName = item.AcName;
                    sAmt.Narration = item.Narration;
                    sAmt.SubSidiary = item.SubSidiary;
                    sAmt.CrAmount = item.CrAmount;
                    sAmt.DrAmount = item.DrAmount;
                    sAmt.VchrNo = item.VchrNo;
                    sAmt.Posted = item.Posted;
                    sAmt.Sub_Ac = item.Sub_Ac;
                    sList.Add(sAmt);
                    // }

                }
                return View("~/Views/ListOfTransaction/TransactionList.cshtml", sList);
            //}


        }

        //public ActionResult ListOfTransectionPreview(RptSearchVModel vmodel, string pType)
        //{
        //    string finyear = Session["FinYear"].ToString();
        //    Session["Branch"] = vmodel.BranchCode;
        //    Session["LedgerType"] = vmodel.LedgerTypeCode;
        //    Session["fDate"] = vmodel.fDate;
        //    Session["tDate"] = vmodel.tDate;

        //    var BranchCode = Session["BranchCode"].ToString();
        //    if (BranchCode != null)
        //    {
        //        ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == BranchCode).BranchName.ToString();
        //    }
        //    else
        //    {
        //        ViewBag.Branch = "All";
        //    }
        //    string sql = string.Format("EXEC rptListTran '" + finyear + "','01','" + vmodel.BranchCode + "','" + vmodel.JTrGrpId + "','" + Convert.ToDateTime(vmodel.fDate) + "','" + Convert.ToDateTime(vmodel.tDate) + "'");
        //    List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
        //    if (glReport.Count == 0)
        //    {
        //        string errMsg = "Data not found !!!";
        //        //ViewBag.msg = errMsg;
        //        return RedirectToAction("CashOperation", "CashOperation", new { errMsg });
        //    }
        //    else
        //    {
        //        if (pType == "A4") { return new Rotativa.ViewAsPdf("~/Views/CashOperation/ListOfTransectionPreview.cshtml", glReport) { PageSize = Rotativa.Options.Size.A4 }; }
        //        else if (pType == "A3") { return new Rotativa.ViewAsPdf("~/Views/CashOperation/ListOfTransectionPreview.cshtml", glReport) { PageSize = Rotativa.Options.Size.A3 }; }
        //        else { return new Rotativa.ViewAsPdf("~/Views/CashOperation/ListOfTransectionPreview.cshtml", glReport) { PageSize = Rotativa.Options.Size.A5 }; }
        //    }
        //}


        public ActionResult ListOfTransactionPreview(RptSearchVModel vmodel)
        {


            vmodel.fDate = Convert.ToDateTime(Session["fDate"]);
            vmodel.toDate = Convert.ToDateTime(Session["tDate"]);
            vmodel.BranchCode = Session["Branch"].ToString();
            string finyear = Session["FinYear"].ToString();

            if (Session["Data"] == null)
            {
                vmodel.JTrGrpId = "";
            }
            else
            {
                vmodel.JTrGrpId = Session["Data"].ToString();

            }
            //vmodel.JTrGrpId = Session["Data"].ToString();
            //if (vmodel.JTrGrpId == null)
            //{
            //    vmodel.JTrGrpId = "";
            //}

            // string x = "" = Session["NullData"].ToString();
            string sql = string.Format("EXEC rptListTran '" + finyear + "','01','" + vmodel.BranchCode + "','" + vmodel.JTrGrpId + "','" + vmodel.fDate.ToString("MM/dd/yyyy") + "','" + vmodel.toDate.ToString("MM/dd/yyyy") + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            ViewBag.BranchCode = vmodel.BranchCode;
            ViewBag.fDate = InWord.GetAbbrMonthNameDate(vmodel.fDate);
            ViewBag.tDate = InWord.GetAbbrMonthNameDate(vmodel.toDate);
            ViewBag.HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
            if (ViewBag.HasBranch == true)
            {
                if (vmodel.BranchCode != null && vmodel.BranchCode != "0")
                {
                    ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BranchCode == vmodel.BranchCode.Trim()).BranchName.ToString();
                }
                else
                {
                    ViewBag.Branch = "All";
                }
            }            
            //  var glReport = Session["Data"].ToString();
            //if (glReport == null)
            //{
            //    string errMsg = "Data not found !!!";
            //    return RedirectToAction("ListOfTransaction", "Search", new { errMsg });
            //}
            //else
            //{
                double amt = 0;
                //foreach (var item in glReport)
                //{
                //    double amt = 0;
                List<JarnalVoucher> sList = new List<JarnalVoucher>();//InWord.ConvertToWords(amt.ToString()).ToList();

                foreach (var item in glReport)
                {
                    if (item.DrAmount > 0)
                    {
                        amt = item.DrAmount;
                    }
                    else if (item.CrAmount > 0)
                    {
                        amt = item.CrAmount;
                    }

                    JarnalVoucher sAmt = new JarnalVoucher();
                    sAmt.ACCode = item.ACCode;
                    sAmt.VDate = item.VDate;
                    sAmt.AcName = item.AcName;
                    sAmt.InWords = InWord.ConvertToWords(amt.ToString());
                    sAmt.AcName = item.AcName;
                    sAmt.Narration = item.Narration;
                    sAmt.SubSidiary = item.SubSidiary;
                    sAmt.CrAmount = item.CrAmount;
                    sAmt.DrAmount = item.DrAmount;
                    sAmt.VchrNo = item.VchrNo;
                    sAmt.Posted = item.Posted;
                    sAmt.Sub_Ac = item.Sub_Ac;
                    sList.Add(sAmt);
                    // }
                }
                //string InWordsamt = InWord.ConvertToWords(amt.ToString());
                //ViewBag.InWordsAmt = InWordsamt;

                // ViewBag.glDate = date;
                //  return View("~/Views/ListOfTransection/TransectionList.cshtml", glReport);
                return new Rotativa.ViewAsPdf("ListOfTransactionPreview", sList) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
            //}


        }
    }
}