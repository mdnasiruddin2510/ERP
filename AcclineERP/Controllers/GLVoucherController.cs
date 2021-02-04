using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using AcclineERP.Models;
using Data.Context;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Transactions;

namespace AcclineERP.Controllers
{
    public class GLVoucherController : Controller
    {
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IEmployeeAppService _employeeInfoService;
        private readonly IIssRecSrcDestAppService _issRecvSrcDestService;
        private readonly INewChartAppService _newChartService;
        private readonly IBranchAppService _BranchService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IPVchrMainAppService _pVchrMainService;
        private readonly IPVchrDetailAppService _pVchchrDetailService;
        private readonly IPVchrMainExtAppService _PVchrMainExtService;
        private readonly ITVchrDetailAppService _tVchrDetailService;
        private readonly ITVchrMainAppService _tVchrMainService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IUserProfileAppService _userProfileService;
        private readonly ISecUserInfoAppService _secUserInfoService;
        private readonly IPVchrTrackAppService _PVchrTrackService;
        private readonly IDynaCapAppService _dynaCapService;
        private readonly IVchrMainAppService _VchrMainService;
        private readonly IVchrDetailAppService _VchchrDetailService;

        public GLVoucherController(ISubsidiaryInfoAppService _subsidiaryInfoService, IEmployeeAppService _employeeInfoService,
            IIssRecSrcDestAppService _issRecvSrcDestService, INewChartAppService _newChartService, IBranchAppService _BranchService,
            ITransactionLogAppService _transactionLogService, IPVchrDetailAppService _pVchchrDetailService,
            ITVchrDetailAppService _tVchchrDetailService, IPVchrMainAppService _pVchrMainService,
            IPVchrMainExtAppService _PVchrMainExtService, ITVchrMainAppService _tVchrMainService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IUserProfileAppService _userProfileService, ISecUserInfoAppService _secUserInfoService,
            IPVchrTrackAppService _PVchrTrackService, IDynaCapAppService _dynaCapService, IVchrMainAppService _VchrMainService,
            IVchrDetailAppService _VchchrDetailService)
        {
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._employeeInfoService = _employeeInfoService;
            this._issRecvSrcDestService = _issRecvSrcDestService;
            this._newChartService = _newChartService;
            this._BranchService = _BranchService;
            this._transactionLogService = _transactionLogService;
            this._pVchchrDetailService = _pVchchrDetailService;
            this._tVchrDetailService = _tVchchrDetailService;
            this._pVchrMainService = _pVchrMainService;
            this._PVchrMainExtService = _PVchrMainExtService;
            this._tVchrMainService = _tVchrMainService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._userProfileService = _userProfileService;
            this._secUserInfoService = _secUserInfoService;
            this._PVchrTrackService = _PVchrTrackService;
            this._dynaCapService = _dynaCapService;
            this._VchrMainService = _VchrMainService;
            this._VchchrDetailService = _VchchrDetailService;
        }

        public class retvalpro
        {
            public string TVchrNo { get; set; }
            public string PVchrNo { get; set; }
        }

        public ActionResult GLVoucher()
        {
            if (Session["UserID"] != null)
            {
                var todayDate = DateTime.Now;
                var BranchCode = Session["BranchCode"].ToString();
                var finYear = Session["FinYear"].ToString();
                //var LogName = _employeeInfoService.All().ToList().FirstOrDefault(x => x.UserName == User.Identity.Name).Id;

                ViewBag.Accode = LoadDropDown.LoadGLVAccode("1", _newChartService, _issRecvSrcDestService);
                //ViewBag.Accode = new SelectList(_newChartService.All().ToList(), "Accode", "AcName", "ParentCode", 1); //for parent wise show

                //ViewBag.UnitCode = LoadDropDown.LoadAllUnits(_unitInfoService);
                //ViewBag.DeptCode = LoadDropDown.LoadAllDepartments(_departmentService);
                //ViewBag.PrepBy = LoadDropDown.LoadPrepApprvBy(_employeeInfoService, "JournalVoucher", "Approved By");
                //ViewBag.ApprBy = LoadDropDown.LoadPrepApprvBy(_employeeInfoService, "JournalVoucher", "Issued By");
                ViewBag.UnitCode = LoadDropDown.LoadEmpDlList();
                ViewBag.DeptCode = LoadDropDown.LoadEmpDlList(); // LoadDept(_sysSetService);
                ViewBag.PrepBy = LoadIssueBy(_employeeInfoService);
                ViewBag.ApprBy = new SelectList(_employeeInfoService.All().Where(s=> s.UserName != "admin").ToList(), "Id", "UserName");//LoadAppBy(_employeeInfoService);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                if (sysSet.OnlyGL == false)
                {
                    ViewBag.Sub_Ac = LoadDropDown.LoadSubsidiary(_subsidiaryInfoService);
                }
                else
                {
                    ViewBag.Sub_Ac = LoadDropDown.LoadSubsidiary("1", _subsidiaryInfoService, _issRecvSrcDestService);
                }
                ViewBag.VType = LoadDropDown.LoadVType(sysSet.OnlyGL);
                ViewBag.GLProvition = Count("JVE");
                ViewBag.GLEntries = CountEntries("JVE");
                ViewBag.VchrNo = getVoucherNo(LoadDropDown.LoadVType(sysSet.OnlyGL).First().Value.TrimEnd());                
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }
        public ActionResult GLVoucherNo(string caller)
        {
            return Json(getVoucherNo(caller), JsonRequestBehavior.AllowGet);
        }

        public string getVoucherNo(string caller)
        {
            #region run sp for voucherNo
            string finYear = Session["FinYear"].ToString();
            string VchrNo = "";
            var VLen = _sysSetService.All().ToList().FirstOrDefault().VchrLen.ToString();
            string sql = string.Format("exec [GetNewVoucherNo] '" + caller + "','" + Session["BranchCode"].ToString() + "','" + VLen + "','" + Session["UserName"].ToString() + "'  ,'" + finYear + "'");
            VchrNo = Convert.ToString(_jarnalVoucherService.SqlQueary(sql).ToList().FirstOrDefault().VchrNo.ToString());
            return VchrNo;
            #endregion
        }

        public ActionResult GetCrAmt(string VchrNo)
        {
            double TotCrAmt = 0.0;
            var tMainList = _tVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo);
            var crAmtList = _tVchrDetailService.All().ToList().Where(x => x.VchrNo == VchrNo && x.FinYear == tMainList.FinYear && tMainList.LoginName == Session["UserName"].ToString());
            foreach (var item in crAmtList)
            {
                TotCrAmt = TotCrAmt + item.DrAmount;
            }
            return Json(TotCrAmt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelTempTbl(int PVchrDetailId)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("GLVoucher_Delete"))
                {
                    return Json("D", JsonRequestBehavior.AllowGet);
                }

                var IsExist = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.PVchrDetailId == PVchrDetailId);
                var isExistTMain = _tVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == IsExist.VchrNo && x.FinYear == IsExist.FinYear);
                if (IsExist != null || isExistTMain != null)
                {
                    _tVchrDetailService.Delete(IsExist);
                    _tVchrDetailService.Save();
                    IsExist = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.VchrNo == IsExist.VchrNo && x.FinYear == IsExist.FinYear);
                    if (isExistTMain != null && IsExist == null)
                    {
                        _tVchrMainService.Delete(isExistTMain);
                        _tVchrMainService.Save();
                    }


                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }
        }

        public List<TvchrDetailVM> GetAllTempDataList(string vchrNo)
        {
            var iData = new List<TvchrDetailVM>();
            int i = 0;
            _tVchrDetailService.All().ToList().Where(x => x.VchrNo == vchrNo).ToList().ForEach(x => iData.Add(
                new TvchrDetailVM(++i, x.PVchrDetailId, x.Accode, x.Narration, x.CrAmount, x.DrAmount, x.Sub_Ac, x.DeptCode, x.UnitCode, x.VchrNo, x.FinYear)
            ));
            return iData;
        }

        public ActionResult GelDatatableOnly(string VchrNo)
        {
            List<TvchrDetailVM> TvchrLst = GetAllTempDataList(VchrNo);
            foreach (var item in TvchrLst)
            {
                item.Accode = _newChartService.All().Where(s => s.Accode == item.Accode).Select(s => s.AcName).FirstOrDefault();
                item.Sub_Ac = _subsidiaryInfoService.All().Where(s => s.SubCode == item.Sub_Ac).Select(s => s.SubName).FirstOrDefault();
                //item.DeptCode = _departmentService.All().Where(s => s.DeptCode == item.DeptCode).Select(s => s.DeptName).FirstOrDefault();
                //item.UnitCode = _unitInfoService.All().Where(s => s.UnitCode == item.UnitCode).Select(s => s.UnitName).FirstOrDefault();
            }
            return Json(new { data = TvchrLst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByVchrNo(string VchrNo)
        {
            JVEditMainVM JVMain = new JVEditMainVM();
            TvchrDetailVM issueDetail = new TvchrDetailVM();
            var pMain = _pVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo.Trim());
            if (pMain != null)
            {
                JVMain.pMainExt = _PVchrMainExtService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo.Trim());
                var items = _pVchchrDetailService.All().ToList().Where(x => x.VchrNo == VchrNo.Trim()).ToList();
                JVMain.pMainExt.VType = pMain.VType;
                JVMain.pMainExt.VDesc = pMain.VDesc;

                TempTbl(items);
                return Json(JVMain, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public void TempTbl(List<PVchrDetail> pDetail)
        {
            string vcrhNo = pDetail.FirstOrDefault().VchrNo.ToString();
            var todayDate = DateTime.Now;
            var BranchCode = Session["BranchCode"].ToString();
            var finYear = Session["FinYear"].ToString();
            var LogName = Session["UserName"].ToString();//User.Identity.Name;
            var isExistTDeatil = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.VchrNo == vcrhNo && x.FinYear == finYear);
            if (isExistTDeatil != null)
            {
                _tVchrMainService.All().ToList().Where(y => y.VchrNo == vcrhNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tVchrMainService.Delete(x));
                _tVchrDetailService.All().ToList().Where(y => y.VchrNo == vcrhNo && y.FinYear == finYear).ToList().ForEach(x => _tVchrDetailService.Delete(x));
                _tVchrMainService.Save();
                _tVchrDetailService.Save();
            }
            foreach (var item in pDetail)
            {
                if (pDetail.Count != 0)
                {
                    var isExistTMain = _tVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == item.VchrNo && x.FinYear == finYear);

                    if (isExistTMain == null)
                    {
                        TVchrMain tMainInfo = new TVchrMain();
                        tMainInfo.VchrNo = item.VchrNo;
                        //tMainInfo.VDate = pMain.VDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        //tMainInfo.VDesc = pMain.VDesc;
                        tMainInfo.FinYear = finYear;
                        tMainInfo.VType = "JV";
                        tMainInfo.Posted = false;
                        tMainInfo.ProjCode = Session["ProjCode"].ToString();
                        tMainInfo.BranchCode = BranchCode;
                        tMainInfo.LoginName = LogName;
                        _tVchrMainService.Add(tMainInfo);
                        _tVchrMainService.Save();
                    }
                    TVchrDetail deatil = new TVchrDetail();
                    deatil.Accode = item.Accode;
                    deatil.Narration = item.Narration;
                    deatil.CrAmount = item.CrAmount;
                    deatil.DrAmount = item.DrAmount;
                    deatil.Sub_Ac = item.Sub_Ac;
                    deatil.VchrNo = item.VchrNo;
                    deatil.DeptCode = item.DeptCode;
                    deatil.UnitCode = item.UnitCode;
                    deatil.FinYear = finYear;
                    deatil.ChqDate = DateTime.Now;
                    _tVchrDetailService.Add(deatil);
                    _tVchrDetailService.Save();

                }
                else
                {
                    break;
                }

            }

        }


        public ActionResult GetAllByReccVchrNo(string ReccVchrNo, string VchrNo)
        {
            JVEditMainVM JVMain = new JVEditMainVM();
            //TvchrDetailVM issueDetail = new TvchrDetailVM();
            var vMain = _VchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == ReccVchrNo.Trim());
            if (vMain != null)
            {
                using (AcclineERPContext db = new AcclineERPContext())
                {
                    JVMain.MainExt = (from vme in db.VchrMainExt where vme.VchrNo == ReccVchrNo.Trim() select vme).FirstOrDefault();
                }
                //JVMain.MainExt = _VchrMainExtService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo.Trim());
                var items = _VchchrDetailService.All().ToList().Where(x => x.VchrNo == ReccVchrNo.Trim()).ToList();
                JVMain.MainExt.VType = vMain.VType;
                JVMain.MainExt.VDesc = vMain.VDesc;
                TempTblRecc(items, VchrNo);

                var check = Json(JVMain, JsonRequestBehavior.AllowGet);

                return Json(JVMain, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public void TempTblRecc(List<VchrDetail> pDetail, string VchrNo)
        {
            var todayDate = DateTime.Now;
            var BranchCode = Session["BranchCode"].ToString();
            var finYear = Session["FinYear"].ToString();
            var LogName = Session["UserName"].ToString();//User.Identity.Name;
            var isExistTDeatil = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo && x.FinYear == finYear);
            if (isExistTDeatil != null)
            {
                _tVchrMainService.All().ToList().Where(y => y.VchrNo == VchrNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tVchrMainService.Delete(x));
                _tVchrDetailService.All().ToList().Where(y => y.VchrNo == VchrNo && y.FinYear == finYear).ToList().ForEach(x => _tVchrDetailService.Delete(x));
                _tVchrMainService.Save();
                _tVchrDetailService.Save();
            }
            foreach (var item in pDetail)
            {
                if (pDetail.Count != 0)
                {
                    var isExistTMain = _tVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo && x.FinYear == finYear);

                    if (isExistTMain == null)
                    {
                        TVchrMain tMainInfo = new TVchrMain();
                        tMainInfo.VchrNo = VchrNo;
                        //tMainInfo.VDate = pMain.VDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                        //tMainInfo.VDesc = pMain.VDesc;
                        tMainInfo.FinYear = finYear;
                        tMainInfo.VType = "JV";
                        tMainInfo.Posted = false;
                        tMainInfo.ProjCode = Session["ProjCode"].ToString();
                        tMainInfo.BranchCode = BranchCode;
                        tMainInfo.LoginName = LogName;
                        _tVchrMainService.Add(tMainInfo);
                        _tVchrMainService.Save();
                    }
                    TVchrDetail deatil = new TVchrDetail();
                    deatil.Accode = item.Accode;
                    deatil.Narration = item.Narration;
                    deatil.CrAmount = item.CrAmount;
                    deatil.DrAmount = item.DrAmount;
                    deatil.Sub_Ac = item.Sub_Ac;
                    deatil.VchrNo = VchrNo;
                    deatil.DeptCode = item.DeptCode;
                    deatil.UnitCode = item.UnitCode;
                    deatil.FinYear = finYear;
                    deatil.ChqDate = DateTime.Now;
                    _tVchrDetailService.Add(deatil);
                    _tVchrDetailService.Save();

                }
                else
                {
                    break;
                }

            }

        }

        public ActionResult SaveTempTbl(TVchrDetail tDetail, TVchrMain tMain)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GLVoucher_Add"))
            {
                return Json("A", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var todayDate = DateTime.Now;
                var BranchCode = Session["BranchCode"].ToString();
                var finYear = Session["FinYear"].ToString();
                var LogName = Session["UserName"].ToString();//User.Identity.Name;

                var isExistTMain = _tVchrMainService.All().ToList().FirstOrDefault(x => x.VchrNo == tMain.VchrNo && x.FinYear == finYear);
                if (isExistTMain == null)
                {
                    TVchrMain tMainInfo = new TVchrMain();
                    tMainInfo.VchrNo = tMain.VchrNo;
                    tMainInfo.VDate = tMain.VDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);
                    tMainInfo.VDesc = tMain.VDesc;
                    tMainInfo.FinYear = finYear;
                    tMainInfo.VType = tMain.VType;
                    tMainInfo.Posted = false;
                    tMainInfo.ProjCode = Session["ProjCode"].ToString();
                    tMainInfo.BranchCode = BranchCode;
                    tMainInfo.LoginName = LogName;
                    _tVchrMainService.Add(tMainInfo);
                    _tVchrMainService.Save();
                }
                TVchrDetail deatil = new TVchrDetail();
                deatil.Accode = tDetail.Accode;
                deatil.Narration = tDetail.Narration;
                deatil.CrAmount = tDetail.CrAmount;
                deatil.DrAmount = tDetail.DrAmount;
                deatil.Sub_Ac = tDetail.Sub_Ac;
                deatil.VchrNo = tDetail.VchrNo;
                deatil.DeptCode = tDetail.DeptCode;
                deatil.UnitCode = tDetail.UnitCode;
                deatil.FinYear = finYear;
                deatil.ChqDate = DateTime.Now;
                _tVchrDetailService.Add(deatil);
                _tVchrDetailService.Save();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SavePVchrTble()
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var PhotoFile = Request.Files["PhotoFile"];
                    var pMain = Request.Form["pMain"];
                    var pMainExt = Request.Form["pMainExt"];
                    var obj1 = JObject.Parse(pMain);
                    var obj = JObject.Parse(pMainExt);

                    string isSave = (string)obj["pMainExt"]["isSave"];

                    if (isSave == "0")
                    {
                        RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                        if (!rUser.HasPermission("GLVoucher_Update"))
                        {
                            return Json("U", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                        if (!rUser.HasPermission("GLVoucher_Insert"))
                        {
                            return Json("X", JsonRequestBehavior.AllowGet);
                        }
                    }

                    string fileName = "";
                    string NewPhotoID = "";
                    string ImgPrefix = "";
                    string fileId = Guid.NewGuid().ToString().Replace("-", "");
                    string ext = "";
                    string PhotoPath = "";
                    string PhysicalPhPath = "";
                    string DBPhotoPath = "";
                    try
                    {
                        if (PhotoFile != null)
                        {
                            fileName = Path.GetFileName(PhotoFile.FileName);
                            ImgPrefix = "Photo";
                            int fSize = PhotoFile.ContentLength;
                            //var path = Path.Combine(Server.MapPath("~/Uploads/Photo/"), MemberCode, fileId);

                            ext = System.IO.Path.GetExtension(fileName);
                            if (ext.ToLower().Contains("gif") || ext.ToLower().Contains("jpg") || ext.ToLower().Contains("jpeg")
                                || ext.ToLower().Contains("png") || ext.ToLower().Contains(".xls") || ext.ToLower().Contains(".docx")
                                || ext.ToLower().Contains(".xlsx") || ext.ToLower().Contains(".pdf") && fSize <= 100000)
                            {
                                NewPhotoID = ImgPrefix + fileId + ext;
                                PhotoPath = ConfigurationManager.AppSettings["PhotoPath"];
                                PhysicalPhPath = Path.Combine(Server.MapPath(PhotoPath), NewPhotoID);
                                //var path = Path.Combine(Server.MapPath("~/Images/Photo"), fileName);
                                PhotoFile.SaveAs(PhysicalPhPath);

                                DBPhotoPath = PhotoPath + "\\" + NewPhotoID;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }

                    var vchrNo = (string)obj1["pMain"]["VchrNo"];
                    var rcrNo = (string)obj1["pMain"]["RecurrVchrNo"];

                    DateTime todayDate = DateTime.Now;
                    var BranchCode = Session["BranchCode"].ToString();
                    var finYear = Session["FinYear"].ToString();
                    var LogName = Session["UserName"].ToString();
                    string vNo = "";
                    if (rcrNo != null)
                        vNo = rcrNo;
                    else
                        vNo = vchrNo;

                    PVchrMain pMainInfo = new PVchrMain();
                    PVchrMainExt pMainExtInfo = new PVchrMainExt();
                    pMainExtInfo.VchrNo = (string)obj1["pMain"]["VchrNo"];
                    pMainExtInfo.FinYear = finYear;
                    pMainExtInfo.VchrSrc = "JV";
                    pMainExtInfo.RecurrVchrNo = (string)obj["pMainExt"]["RecurrVchrNo"];
                    pMainExtInfo.AdjVchrNo = (string)obj["pMainExt"]["AdjVchrNo"];
                    pMainExtInfo.PrepBy = (string)obj["pMainExt"]["PrepBy"];
                    pMainExtInfo.PrepComment = (string)obj["pMainExt"]["PrepComment"];
                    pMainExtInfo.PrepDate = DateTime.Parse(obj["pMainExt"]["PrepDate"].ToString());
                    pMainExtInfo.ApprBy = (string)obj["pMainExt"]["ApprBy"];
                    pMainExtInfo.ApprComment = (string)obj["pMainExt"]["ApprComment"];
                    pMainExtInfo.ApprDate = DateTime.Parse(obj["pMainExt"]["ApprDate"].ToString());
                    pMainExtInfo.JobNo = (string)obj["pMainExt"]["JobNo"];
                    pMainExtInfo.VchrSrcRef = (string)obj1["pMain"]["VchrNo"];
                    pMainExtInfo.VchrAttach = DBPhotoPath;

                    pMainInfo.VchrNo = vchrNo;
                    pMainInfo.VDate = DateTime.Parse(obj1["pMain"]["VDate"].ToString());
                    pMainInfo.VDesc = (string)obj1["pMain"]["VDesc"];
                    pMainInfo.FinYear = finYear;
                    pMainInfo.VType = (string)obj1["pMain"]["VType"];
                    pMainInfo.Posted = false;
                    pMainInfo.ProjCode = Session["ProjCode"].ToString();
                    pMainInfo.BranchCode = BranchCode;
                    pMainInfo.LoginName = LogName;

                    List<PVchrDetail> detailList = new List<PVchrDetail>();
                    foreach (var item in GetAllTempDataList(vchrNo))
                    {
                        PVchrDetail deatil = new PVchrDetail();
                        if (rcrNo != null)
                        {
                            deatil.VchrNo = vchrNo;
                        }
                        else
                        {
                            deatil.VchrNo = item.VchrNo;
                        }
                        deatil.Accode = item.Accode;
                        deatil.Narration = item.Narration;
                        deatil.CrAmount = item.CrAmount;
                        deatil.DrAmount = item.DrAmount;
                        deatil.Sub_Ac = item.Sub_Ac;
                        deatil.DeptCode = item.DeptCode;
                        deatil.UnitCode = item.UnitCode;
                        deatil.ChqDate = DateTime.Now;
                        deatil.FinYear = item.FinYear;
                        deatil.RefNo = "";
                        deatil.ChqNo = "";
                        deatil.BankName = "";
                        deatil.BankBranch = "";
                        deatil.EntryNo = 0;
                        detailList.Add(deatil);
                    }
                    Session["VchrNo"] = vchrNo;
                    pMainInfo.PVchrDetail = detailList;
                    if (isSave == "0")
                    {

                        _pVchrMainService.Add(pMainInfo);
                        _pVchrMainService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _pVchrMainService.Delete(x));
                        _pVchrMainService.Save();

                        pMainExtInfo.PVchrMainId = _pVchrMainService.All().ToList().LastOrDefault().PVchrMainId;
                        _PVchrMainExtService.Add(pMainExtInfo);
                        _PVchrMainExtService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear).ToList().ForEach(x => _PVchrMainExtService.Delete(x));
                        _PVchrMainExtService.Save();

                        _tVchrMainService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tVchrMainService.Delete(x));
                        _tVchrDetailService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear).ToList().ForEach(x => _tVchrDetailService.Delete(x));
                        _tVchrMainService.Save();
                        _tVchrDetailService.Save();

                        var pDetail = _pVchchrDetailService.All().Where(x => x.VchrNo == vchrNo).ToList();
                        string EntryNo = vchrNo.Substring(2);
                        _PVchrTrackService.All().ToList().Where(y => y.EntryNo == EntryNo).ToList().ForEach(x => _PVchrTrackService.Delete(x));
                        foreach (var pItem in pDetail)
                        {
                            PVchrTrack PVchrTrack = new PVchrTrack();
                            PVchrTrack.EntryNo = EntryNo;
                            PVchrTrack.EntrySource = "JV";
                            PVchrTrack.VchrMainId = pItem.PVchrMainId;
                            PVchrTrack.VchrDetailId = pItem.PVchrDetailId;
                            _PVchrTrackService.Add(PVchrTrack);
                        }
                        _PVchrTrackService.Save();

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Journal Voucher", "Update ", "", LogName);
                        transaction.Complete();
                        return Json("2", JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        var pVMain = _pVchrMainService.All().ToList().Where(s => s.VchrNo == pMainInfo.VchrNo).FirstOrDefault();
                        if (pVMain == null)
                        {
                            _pVchrMainService.Add(pMainInfo);
                            _pVchrMainService.Save();

                            var pVchrMain = _pVchrMainService.All().ToList();
                            int PVchrMainId = 1;
                            if (pVchrMain.Count != 0)
                            {
                                PVchrMainId = pVchrMain.LastOrDefault().PVchrMainId;
                            }
                            pMainExtInfo.PVchrMainId = PVchrMainId;
                            _PVchrMainExtService.Add(pMainExtInfo);
                            _PVchrMainExtService.Save();

                            _tVchrMainService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear && y.LoginName == LogName).ToList().ForEach(x => _tVchrMainService.Delete(x));
                            _tVchrDetailService.All().ToList().Where(y => y.VchrNo == vNo && y.FinYear == finYear).ToList().ForEach(x => _tVchrDetailService.Delete(x));
                            _tVchrMainService.Save();
                            _tVchrDetailService.Save();

                            var pDetail = _pVchchrDetailService.All().Where(x => x.VchrNo == vchrNo).ToList();
                            string EntryNo = vchrNo.Substring(2);
                            foreach (var pItem in pDetail)
                            {
                                PVchrTrack PVchrTrack = new PVchrTrack();
                                PVchrTrack.EntryNo = EntryNo;
                                PVchrTrack.EntrySource = "JV";
                                PVchrTrack.VchrMainId = pItem.PVchrMainId;
                                PVchrTrack.VchrDetailId = pItem.PVchrDetailId;
                                _PVchrTrackService.Add(PVchrTrack);
                            }
                            _PVchrTrackService.Save();

                            string sqlQuery = string.Format("delete from VouNoAssign where VchrNo = '" + vchrNo + "'");
                            using (AcclineERPContext dbContext = new AcclineERPContext())
                            {
                                dbContext.Database.ExecuteSqlCommand(sqlQuery);
                            }
                            TransactionLogService.SaveTransactionLog(_transactionLogService, "Journal Voucher", "Save", "", LogName);

                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Msg = "The '" + pMainInfo.VchrNo + "' Voucher No is Already Exist.", rStatus = "0" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public SelectList LoadIssueBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Journal Voucher";
            string functionName = "Prepared By";
            //string logName = User.Identity.Name;
            string LogName = Session["UserName"].ToString();
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "','" + form + "', '" + LogName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }

        public SelectList LoadAppBy(IEmployeeAppService _employeeInfoService)
        {
            string branchCode = Session["BranchCode"].ToString();
            string form = "Journal Voucher";
            string functionName = "Approved By";
            //string logName = User.Identity.Name;
            string LogName = Session["UserName"].ToString();
            string sql = string.Format("EXEC loadRecvIssuBy '" + branchCode + "','" + form + "', '" + LogName + "', '" + functionName + "'");
            var items = _employeeInfoService.SqlQueary(sql)
                .Select(x => new { x.Id, x.UserName }).ToList();
            return new SelectList(items.OrderBy(x => x.UserName), "Id", "UserName");
        }

        public ActionResult GLPreviewPdf()
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GLVoucher_Preview"))
            {
                string errMsg = "No Preview Prmission For This User !!";
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });
            }
            //ViewBag.PrintDate = DateTime.Now.ToShortDateString();

            string sql = string.Format("Select td.VchrNo, td.UnitCode, td.DeptCode, nc.AcName as Accode, td.DrAmount, td.CrAmount,si.SubName as Sub_Ac " +
                ",td.Narration, tm.VDate from TVchrDetail as td left join TVchrMain as tm on td.VchrNo = tm.VchrNo and td.FinYear = tm.FinYear " +
                "left join NewChart as nc on nc.Accode = td.Accode left join SubsidiaryInfo as si on si.SubCode = td.Sub_Ac" +
                " where tm.LoginName = '" + Session["UserName"].ToString() + "'");

            List<JarnalVoucher> glReportPreview = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReportPreview.Count == 0)
            {
                //string errMsg = "Data not found !!!";
                //return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });
                return new Rotativa.ViewAsPdf("~/Views/GLVoucher/GLPreview.cshtml", glReportPreview)
                {
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }
            else
            {
                double amt = 0;
                foreach (var item in glReportPreview)
                {
                    if (item.CrAmount != 0)
                    {
                        amt += item.CrAmount;
                    }
                }
                string InWordsamt = InWord.ConvertToWords(amt.ToString());
                ViewBag.InWordsAmt = InWordsamt;

                ViewBag.VTypeName = GetCompanyInfo.GetVTypeNameByTVoucher(glReportPreview.First().VchrNo);

                return new Rotativa.ViewAsPdf("~/Views/GLVoucher/GLPreview.cshtml", glReportPreview)
                {
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }

        }

        public ActionResult GLPreviewPdfAfterSave(string VType)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GLVoucher_Preview"))
            {
                string errMsg = "No View Prmission For This User !!";
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });
            }

            //ViewBag.SCriteria = GetCompanyInfo.GetReportCriteria(null, Session["BranchCode"].ToString(), null, null, Session["FinYear"].ToString());


            string sql = string.Format("Select td.VchrNo, td.UnitCode, td.DeptCode, nc.AcName as Accode, td.DrAmount, td.CrAmount,si.SubName as Sub_Ac  ,td.Narration, tm.VDate from PVchrDetail as td " +
                                    " join PVchrMain as tm on td.VchrNo = tm.VchrNo and td.FinYear = tm.FinYear " +
                                    "left join NewChart as nc on nc.Accode = td.Accode left join SubsidiaryInfo as si on si.SubCode = td.Sub_Ac " +
                                     "where tm.LoginName = '" + Session["UserName"].ToString() + "' and tm.VType = '" + VType + "' and tm.VchrNo = '" + Session["VchrNo"] + "'");
            List<JarnalVoucher> glReportPreview = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReportPreview.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });

            }
            else
            {
                Session["VchrNo"] = "";
                double amt = 0;
                foreach (var item in glReportPreview)
                {
                    if (item.CrAmount != 0)
                    {
                        amt += item.CrAmount;
                    }
                }
                string InWordsamt = InWord.ConvertToWords(amt.ToString());
                ViewBag.InWordsAmt = InWordsamt;
                ViewBag.VTypeName = GetCompanyInfo.GetVTypeNameByPVoucher(glReportPreview.First().VchrNo);

                return new Rotativa.ViewAsPdf("~/Views/GLVoucher/GLPreview.cshtml", glReportPreview)
                {
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }

        }

        public ActionResult GetJournalVoucher(DateTime date, string pageType)
        {

            string sql = GetCompanyInfo.JvWaitingPostSQL(pageType);

            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });

            }
            else
            {
                ViewBag.glDate = date;
                //return View("~/Views/VchrPreview/JournalVoucher.cshtml", glReport);
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "', '" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });
            }
            else
            {
                ViewBag.glDate = date;
                //return View("~/Views/VchrPreview/GLEntres.cshtml", glReport);
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGlECountN(string VType)
        {
            return Json(CountEntries(VType), JsonRequestBehavior.AllowGet);
        }

        public string CountEntries(string pageType)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
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

        public ActionResult GetGlECount(string VType)
        {
            return Json(Count(VType), JsonRequestBehavior.AllowGet);
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

        public ActionResult GlPostedByVoucherNo(List<string> vochreNo)
        {
            foreach (var vItem in vochreNo)
            {
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

        public ActionResult GetGLEntriesPdf(string vchrNo, string pType)
        {
            var BranchCode = Session["BranchCode"].ToString();
            //if (BranchCode != null)
            //{
            //    ViewBag.Branch = _BranchService.All().FirstOrDefault(x => x.BrCode == BranchCode).BrName.ToString();
            //    ViewBag.ProjName = _ProjInfoService.All().FirstOrDefault().ProjName.ToString();

            //}
            //else
            //{
            //    ViewBag.Branch = "All";
            //}
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as Accode,(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary,pvd.Narration,"
                + "  pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from VchrMain as pvm "
                + "join VchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear  where pvm.VchrNo ='" + vchrNo + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("GLVoucher", "GLVoucher", new { errMsg });
            }
            else
            {
                if (pType == "A4")
                {
                    return new Rotativa.ViewAsPdf("~/Views/VchrPreview/GLEntriesRcvPdf.cshtml", glReport)
                    {
                        PageSize = Rotativa.Options.Size.A4,
                        CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""

                    };
                }
                else if (pType == "A3")
                {
                    return new Rotativa.ViewAsPdf("~/Views/VchrPreview/GLEntriesRcvPdf.cshtml", glReport)
                    {
                        PageSize = Rotativa.Options.Size.A3,
                        CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                    };
                }
                else
                {
                    return new Rotativa.ViewAsPdf("~/Views/VchrPreview/GLEntriesRcvPdf.cshtml", glReport)
                    {
                        PageSize = Rotativa.Options.Size.A5,
                        CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                    };
                }
            }
        }


        public ActionResult GetDynaSysSet()
        {
            //Have to retrieve both MemInfo and Detail and combine then in ViewModel and then return

            VMDynSysSet DsSet = new VMDynSysSet();

            DsSet.SysSet = _sysSetService.All().ToList().FirstOrDefault();
            DsSet.DynaCap = _dynaCapService.All().ToList().FirstOrDefault();

            if (DsSet.SysSet != null && DsSet.DynaCap != null)
            {
                return Json(DsSet, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetTempVchr(int tVchrDetailId)
        {
            try
            {
                var tVchrDetail = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.PVchrDetailId == tVchrDetailId);
                return Json(tVchrDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult UpdateTempTbl(TVchrDetail tDetail)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var tVchrDetail = _tVchrDetailService.All().ToList().FirstOrDefault(s => s.PVchrDetailId == tDetail.PVchrDetailId);
                    if (tVchrDetail != null)
                    {
                        tVchrDetail.Accode = tDetail.Accode;
                        tVchrDetail.Narration = tDetail.Narration;
                        tVchrDetail.CrAmount = tDetail.CrAmount;
                        tVchrDetail.DrAmount = tDetail.DrAmount;
                        tVchrDetail.Sub_Ac = tDetail.Sub_Ac;
                        _tVchrDetailService.Update(tVchrDetail);
                        _tVchrDetailService.Save();
                    }
                    transaction.Complete();
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult delTVchr(string VchrNo)
        {
            var IsExist = _tVchrDetailService.All().ToList().FirstOrDefault(x => x.VchrNo == VchrNo);
            if (IsExist != null)
            {
                _tVchrMainService.All().ToList().Where(y => y.VchrNo == VchrNo && y.FinYear == IsExist.FinYear).ToList().ForEach(x => _tVchrMainService.Delete(x));
                _tVchrDetailService.All().ToList().Where(y => y.VchrNo == VchrNo && y.FinYear == IsExist.FinYear).ToList().ForEach(x => _tVchrDetailService.Delete(x));
                _tVchrMainService.Save();
                _tVchrDetailService.Save();
            }
            return Json("1", JsonRequestBehavior.AllowGet);

        }
    }
}
