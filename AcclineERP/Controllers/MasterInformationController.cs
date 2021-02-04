using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using App.Domain;
using Application.Interfaces;
using Data.Context;
using System.Data.SqlClient;
using System.Data;
using AcclineERP.Models;
using System.Transactions;

namespace AcclineERP.Controllers
{
    //[RBAC]
    public class MasterInformationController : Controller
    {
        //
        // GET: /MasterInformation/
        private IProjInfoAppService _projInfoService;
        private IBranchAppService _brInfoService;
        private IDepartmentAppService _departmentService;
        private IDesignationAppService _designationService;
        private IUnitInfoAppService _unitInfoService;
        private IBankInfoAppService _bankInfoService;
        private IMasterFormAppService _masterFormService;
        private IDynaCapAppService _dynaCapService;
        private ITransactionLogAppService _transactionLogService;
        private ILocationAppService _LocationService;

        public MasterInformationController(
                                IProjInfoAppService _projInfoService,
                                IBranchAppService _brInfoService,
                                IDepartmentAppService _departmentService,
                                IDesignationAppService _designationService,
                                IUnitInfoAppService _unitInfoService,
                                IBankInfoAppService _bankInfoService,
                                IMasterFormAppService _masterFormService,
                                IDynaCapAppService _dynaCapService,
                                ITransactionLogAppService _transactionLogService,
                                ILocationAppService _LocationService
                                )
        {

            this._projInfoService = _projInfoService;
            this._brInfoService = _brInfoService;
            this._departmentService = _departmentService;
            this._designationService = _designationService;
            this._unitInfoService = _unitInfoService;
            this._bankInfoService = _bankInfoService;
            this._masterFormService = _masterFormService;
            this._dynaCapService = _dynaCapService;
            this._transactionLogService = _transactionLogService;
            this._LocationService = _LocationService;

        }

        public ActionResult MasterInformation()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.FormCode = LoadDropDown.LoadAllMasterForms(_masterFormService, _dynaCapService);
                //ViewBag.BankId = LoadDropDown.LoadAllBanks(_bankInfoService);
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        [HttpPost]
        public ActionResult SaveMasterFormInfo(VMMasterForm MstrInf)
        {

            string eCode = "";
            string UserName = Session["UserName"].ToString();
            RBACUser rUser = new RBACUser(UserName);
            if (!rUser.HasPermission("MasterInformation_Insert"))
            {
                eCode = "No Save Permission for this User!!";
                return Json(eCode, JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {

                    //int FormID = MstrInf.FormID;
                    string FormName = MstrInf.FormCode;//_masterFormService.All().ToList().FirstOrDefault(x => x.FormID == FormID).FormName;

                    switch (FormName)
                    {
                        case "Proj":
                            eCode = SaveProjectInfo(MstrInf);
                            break;
                        case "Branch":
                            eCode = SaveBranchInfo(MstrInf);
                            break;
                        case "Unit":
                            eCode = SaveUnitInfo(MstrInf);
                            break;
                        case "Dept":
                            eCode = SaveDepartmentInfo(MstrInf);
                            break;
                        case "Desig":
                            eCode = SaveDesignationInfo(MstrInf);
                            break;
                        case "Loc":
                            eCode = SaveLocInfo(MstrInf);
                            break;
                        case "ItemGrp":
                            eCode = SaveItemGrp(MstrInf);
                            break;
                        case "ItemSGrp":
                            eCode = SaveItemSGrp(MstrInf);
                            break;
                        case "ItemSSGrp":
                            eCode = SaveItemSSGrp(MstrInf);
                            break;
                        default:
                            break;
                    }


                    transaction.Complete();

                    return Json(eCode, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }


        }

        private string SaveProjectInfo(VMMasterForm MstrInf)
        {

            var ProjCode = _projInfoService.All().ToList().FirstOrDefault(x => x.ProjCode == MstrInf.Code);

            if (ProjCode == null)
            {
                ProjInfo _proj = new ProjInfo();

                _proj.ProjCode = MstrInf.Code;
                _proj.ProjName = MstrInf.Name;
                _proj.ProjLocalName = MstrInf.LocalName;
                _proj.ProjDescrip = MstrInf.Descr;

                _projInfoService.Add(_proj);
                _projInfoService.Save();

                var ProjID = _proj.ProjID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveProjectInfo",
                   ProjID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }


        }

        private string SaveBranchInfo(VMMasterForm MstrInf)
        {
            var BrCode = _brInfoService.All().ToList().FirstOrDefault(x => x.BranchCode == MstrInf.Code);

            if (BrCode == null)
            {

                Branch _brInfo = new Branch();

                _brInfo.BranchCode = MstrInf.Code;
                _brInfo.BranchName = MstrInf.Name;
                _brInfo.BrLocalName = MstrInf.LocalName;
                _brInfo.BrAddress = MstrInf.Descr;

                _brInfoService.Add(_brInfo);
                _brInfoService.Save();

                var BrID = _brInfo.BrID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveBranchInfo",
                   BrID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveUnitInfo(VMMasterForm MstrInf)
        {
            var UnitCode = _unitInfoService.All().ToList().FirstOrDefault(x => x.UnitCode == MstrInf.Code);

            if (UnitCode == null)
            {

                UnitInfo _unit = new UnitInfo();

                _unit.UnitCode = MstrInf.Code;
                _unit.UnitName = MstrInf.Name;
                _unit.UnitLocalName = MstrInf.LocalName;
                _unit.UnitDesc = MstrInf.Descr;

                _unitInfoService.Add(_unit);
                _unitInfoService.Save();

                var UnitID = _unit.UnitID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveUnitInfo",
                   UnitID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }
        }

        private string SaveDepartmentInfo(VMMasterForm MstrInf)
        {
            var DeptCode = _departmentService.All().ToList().FirstOrDefault(x => x.DeptCode == MstrInf.Code);

            if (DeptCode == null)
            {
                Department _dept = new Department();

                _dept.DeptCode = MstrInf.Code;
                _dept.DeptName = MstrInf.Name;
                _dept.DeptLocalName = MstrInf.LocalName;
                _dept.DeptDesc = MstrInf.Descr;

                _departmentService.Add(_dept);
                _departmentService.Save();

                var DeptID = _dept.DeptID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDepartmentInfo",
                   DeptID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveDesignationInfo(VMMasterForm MstrInf)
        {
            var DesigCode = _designationService.All().ToList().FirstOrDefault(x => x.DesigCode == MstrInf.Code);

            if (DesigCode == null)
            {
                Designation _desig = new Designation();

                _desig.DesigCode = MstrInf.Code;
                _desig.DesigName = MstrInf.Name;
                _desig.DesigLocalName = MstrInf.LocalName;
                _desig.DesigDesc = MstrInf.Descr;

                _designationService.Add(_desig);
                _designationService.Save();

                var DesigID = _desig.DesigID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDesignationInfo",
                   DesigID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveLocInfo(VMMasterForm MstrInf)
        {
            var LocCode = _LocationService.All().ToList().FirstOrDefault(x => x.LocCode == MstrInf.Code);

            if (LocCode == null)
            {
                Location _Loc = new Location();

                _Loc.LocCode = MstrInf.Code;
                _Loc.LocName = MstrInf.Name;

                _LocationService.Add(_Loc);
                _LocationService.Save();

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDesignationInfo",
                   _Loc.LocCode, Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveItemGrp(VMMasterForm MstrInf)
        {
            var DesigCode = _designationService.All().ToList().FirstOrDefault(x => x.DesigCode == MstrInf.Code);

            if (DesigCode == null)
            {
                Designation _desig = new Designation();

                _desig.DesigCode = MstrInf.Code;
                _desig.DesigName = MstrInf.Name;
                _desig.DesigLocalName = MstrInf.LocalName;
                _desig.DesigDesc = MstrInf.Descr;

                _designationService.Add(_desig);
                _designationService.Save();

                var DesigID = _desig.DesigID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDesignationInfo",
                   DesigID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveItemSGrp(VMMasterForm MstrInf)
        {
            var DesigCode = _designationService.All().ToList().FirstOrDefault(x => x.DesigCode == MstrInf.Code);

            if (DesigCode == null)
            {
                Designation _desig = new Designation();

                _desig.DesigCode = MstrInf.Code;
                _desig.DesigName = MstrInf.Name;
                _desig.DesigLocalName = MstrInf.LocalName;
                _desig.DesigDesc = MstrInf.Descr;

                _designationService.Add(_desig);
                _designationService.Save();

                var DesigID = _desig.DesigID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDesignationInfo",
                   DesigID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }

        private string SaveItemSSGrp(VMMasterForm MstrInf)
        {
            var DesigCode = _designationService.All().ToList().FirstOrDefault(x => x.DesigCode == MstrInf.Code);

            if (DesigCode == null)
            {
                Designation _desig = new Designation();

                _desig.DesigCode = MstrInf.Code;
                _desig.DesigName = MstrInf.Name;
                _desig.DesigLocalName = MstrInf.LocalName;
                _desig.DesigDesc = MstrInf.Descr;

                _designationService.Add(_desig);
                _designationService.Save();

                var DesigID = _desig.DesigID;

                TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "SaveDesignationInfo",
                   DesigID.ToString(), Session["UserName"].ToString());

                return "1";
            }
            else
            {
                return "2";
            }

        }



        public ActionResult GetMasterFormInfoByFormCodeNID(int ID, string FormCode)
        {
            try
            {

                VMMasterForm vmf = new VMMasterForm();

                if (FormCode == "Proj")
                {
                    var ProjInfo = _projInfoService.All().ToList().FirstOrDefault(x => x.ProjID == ID);

                    if (ProjInfo != null)
                    {
                        vmf.FormCode = "Proj";
                        vmf.ID = ProjInfo.ProjID;
                        vmf.Code = ProjInfo.ProjCode;
                        vmf.Name = ProjInfo.ProjName;
                        vmf.LocalName = ProjInfo.ProjLocalName;
                        vmf.Descr = ProjInfo.ProjDescrip;
                    }
                }
                else if (FormCode == "Branch")
                {
                    var BrInfo = _brInfoService.All().ToList().FirstOrDefault(x => x.BrID == ID);
                    if (BrInfo != null)
                    {
                        vmf.FormCode = "Branch";
                        vmf.ID = BrInfo.BrID;
                        vmf.Code = BrInfo.BranchCode;
                        vmf.Name = BrInfo.BranchName;
                        vmf.LocalName = BrInfo.BrLocalName;
                        vmf.Descr = BrInfo.BrAddress;
                    }
                }
                else if (FormCode == "Unit")
                {
                    var UnitInfo = _unitInfoService.All().ToList().FirstOrDefault(x => x.UnitID == ID);
                    if (UnitInfo != null)
                    {
                        vmf.FormCode = "Unit";
                        vmf.ID = UnitInfo.UnitID;
                        vmf.Code = UnitInfo.UnitCode;
                        vmf.Name = UnitInfo.UnitName;
                        vmf.LocalName = UnitInfo.UnitLocalName;
                        vmf.Descr = UnitInfo.UnitDesc;
                    }
                }
                else if (FormCode == "Dept")
                {
                    var DeptInfo = _departmentService.All().ToList().FirstOrDefault(x => x.DeptID == ID);
                    if (DeptInfo != null)
                    {
                        vmf.FormCode = "Dept";
                        vmf.ID = DeptInfo.DeptID;
                        vmf.Code = DeptInfo.DeptCode;
                        vmf.Name = DeptInfo.DeptName;
                        vmf.LocalName = DeptInfo.DeptLocalName;
                        vmf.Descr = DeptInfo.DeptDesc;
                    }
                }
                else if (FormCode == "Desig")
                {
                    var DesigInfo = _designationService.All().ToList().FirstOrDefault(x => x.DesigID == ID);
                    if (DesigInfo != null)
                    {
                        vmf.FormCode = "Desig";
                        vmf.ID = DesigInfo.DesigID;
                        vmf.Code = DesigInfo.DesigCode;
                        vmf.Name = DesigInfo.DesigName;
                        vmf.LocalName = DesigInfo.DesigLocalName;
                        vmf.Descr = DesigInfo.DesigDesc;
                    }

                }
                else if (FormCode == "Loc")
                {
                    var LocInfo = _LocationService.All().ToList().FirstOrDefault(x => x.LocId == ID);
                    if (LocInfo != null)
                    {
                        vmf.FormCode = "Loc";
                        vmf.ID = LocInfo.LocId;
                        vmf.Code = LocInfo.LocCode;
                        vmf.Name = LocInfo.LocName;
                    }

                }

                return Json(vmf, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetAllDataForMasterFormType(string FormName)
        {
            try
            {

                List<VMMasterForm> MFList = new List<VMMasterForm>();

                switch (FormName)
                {
                    case "Proj":
                        MFList = LoadProjectInfo();
                        break;
                    case "Branch":
                        MFList = LoadBranchInfo();
                        break;
                    case "Unit":
                        MFList = LoadUnitInfo();
                        break;
                    case "Dept":
                        MFList = LoadDepartmentInfo();
                        break;
                    case "Desig":
                        MFList = LoadDesignationInfo();
                        break;
                    case "Loc":
                        MFList = LoadLocationInfo();
                        break;
                    default:
                        break;
                }
                if (MFList != null)
                {
                    return Json(MFList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        private List<VMMasterForm> LoadProjectInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _projInfoService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Proj";
                    vmf.ID = MObj.ProjID;
                    vmf.Code = MObj.ProjCode;
                    vmf.Name = MObj.ProjName;
                    vmf.LocalName = MObj.ProjLocalName;
                    vmf.Descr = MObj.ProjDescrip;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }

        }

        private List<VMMasterForm> LoadBranchInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _brInfoService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Branch";
                    vmf.ID = MObj.BrID;
                    vmf.Code = MObj.BranchCode;
                    vmf.Name = MObj.BranchName;
                    vmf.LocalName = MObj.BrLocalName;
                    vmf.Descr = MObj.BrAddress;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }


        }

        private List<VMMasterForm> LoadUnitInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _unitInfoService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Unit";
                    vmf.ID = MObj.UnitID;
                    vmf.Code = MObj.UnitCode;
                    vmf.Name = MObj.UnitName;
                    vmf.LocalName = MObj.UnitLocalName;
                    vmf.Descr = MObj.UnitDesc;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }

        }

        private List<VMMasterForm> LoadDepartmentInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _departmentService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Dept";
                    vmf.ID = MObj.DeptID;
                    vmf.Code = MObj.DeptCode;
                    vmf.Name = MObj.DeptName;
                    vmf.LocalName = MObj.DeptLocalName;
                    vmf.Descr = MObj.DeptDesc;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }

        }

        private List<VMMasterForm> LoadDesignationInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _designationService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Desig";
                    vmf.ID = MObj.DesigID;
                    vmf.Code = MObj.DesigCode;
                    vmf.Name = MObj.DesigName;
                    vmf.LocalName = MObj.DesigLocalName;
                    vmf.Descr = MObj.DesigDesc;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }

        }

        private List<VMMasterForm> LoadLocationInfo()
        {

            List<VMMasterForm> VMList = new List<VMMasterForm>();

            var MList = _LocationService.All().ToList();

            if (MList.Count > 0)
            {

                foreach (var MObj in MList)
                {
                    VMMasterForm vmf = new VMMasterForm();
                    vmf.FormCode = "Loc";
                    vmf.ID = MObj.LocId;
                    vmf.Code = MObj.LocCode;
                    vmf.Name = MObj.LocName;

                    VMList.Add(vmf);
                }

                if (VMList != null)
                {
                    VMList = VMList.OrderBy(x => x.Code).ToList();
                }

                return VMList;
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        public ActionResult UpdateMasterFormInfo(VMMasterForm MstrInf)
        {

            string eCode = "";

            string UserName = Session["UserName"].ToString();
            RBACUser rUser = new RBACUser(UserName);
            if (!rUser.HasPermission("MasterInformation_Update"))
            {
                eCode = "No Update Permission for this User!!";
                return Json(eCode, JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {

                    //int FormID = MstrInf.FormID;
                    string FormName = MstrInf.FormCode;//_masterFormService.All().ToList().FirstOrDefault(x => x.FormID == FormID).FormName;

                    switch (FormName)
                    {
                        case "Proj":
                            eCode = UpdateProjectInfo(MstrInf);
                            break;
                        case "Branch":
                            eCode = UpdateBranchInfo(MstrInf);
                            break;
                        case "Unit":
                            eCode = UpdateUnitInfo(MstrInf);
                            break;
                        case "Dept":
                            eCode = UpdateDepartmentInfo(MstrInf);
                            break;
                        case "Desig":
                            eCode = UpdateDesignationInfo(MstrInf);
                            break;
                        case "Loc":
                            eCode = UpdateLocationInfo(MstrInf);
                            break;
                        default:
                            break;
                    }


                    transaction.Complete();

                    return Json(eCode, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }

        }

        private string UpdateProjectInfo(VMMasterForm MstrInf)
        {

            var ProjInfo = _projInfoService.All().ToList().FirstOrDefault(x => x.ProjID == MstrInf.ID);

            if (ProjInfo != null)
            {

                var PInfList = _projInfoService.All().ToList().Where(c => c.ProjID != MstrInf.ID).ToList();
                var IsCodExst = PInfList.FirstOrDefault(x => x.ProjCode == MstrInf.Code);

                if (IsCodExst == null)
                {
                    ProjInfo.ProjCode = MstrInf.Code;
                    ProjInfo.ProjName = MstrInf.Name;
                    ProjInfo.ProjLocalName = MstrInf.LocalName;
                    ProjInfo.ProjDescrip = MstrInf.Descr;

                    _projInfoService.Update(ProjInfo);
                    _projInfoService.Save();

                    var ProjID = ProjInfo.ProjID;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateProjectInfo",
                       ProjID.ToString(), Session["UserName"].ToString());


                    return "1";
                }
                else
                {
                    return "3";
                }


            }
            else
            {
                return "2";
            }


        }

        private string UpdateBranchInfo(VMMasterForm MstrInf)
        {
            var BrInfo = _brInfoService.All().ToList().FirstOrDefault(x => x.BrID == MstrInf.ID);

            if (BrInfo != null)
            {

                var BInfList = _brInfoService.All().ToList().Where(c => c.BrID != MstrInf.ID).ToList();
                var IsCodExst = BInfList.FirstOrDefault(x => x.BranchCode == MstrInf.Code);

                if (IsCodExst == null)
                {
                    //BrInfo.BranchCode = MstrInf.Code;
                    BrInfo.BranchName = MstrInf.Name;
                    BrInfo.BrLocalName = MstrInf.LocalName;
                    BrInfo.BrAddress = MstrInf.Descr;

                    _brInfoService.Update(BrInfo);
                    _brInfoService.Save();


                    var BrID = BrInfo.BrID;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateBranchInfo",
                       BrID.ToString(), Session["UserName"].ToString());

                    return "1";

                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }

        }

        private string UpdateUnitInfo(VMMasterForm MstrInf)
        {
            var UnitInfo = _unitInfoService.All().ToList().FirstOrDefault(x => x.UnitID == MstrInf.ID);

            if (UnitInfo != null)
            {
                var UInfList = _unitInfoService.All().ToList().Where(c => c.UnitID != MstrInf.ID).ToList();
                var IsCodExst = UInfList.FirstOrDefault(x => x.UnitCode == MstrInf.Code);

                if (IsCodExst == null)
                {

                    UnitInfo.UnitCode = MstrInf.Code;
                    UnitInfo.UnitName = MstrInf.Name;
                    UnitInfo.UnitLocalName = MstrInf.LocalName;
                    UnitInfo.UnitDesc = MstrInf.Descr;

                    _unitInfoService.Update(UnitInfo);
                    _unitInfoService.Save();

                    var UnitID = UnitInfo.UnitID;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateUnitInfo",
                       UnitID.ToString(), Session["UserName"].ToString());

                    return "1";
                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }
        }

        private string UpdateDepartmentInfo(VMMasterForm MstrInf)
        {
            var DeptInfo = _departmentService.All().ToList().FirstOrDefault(x => x.DeptID == MstrInf.ID);

            if (DeptInfo != null)
            {
                var DepInfList = _departmentService.All().ToList().Where(c => c.DeptID != MstrInf.ID).ToList();
                var IsCodExst = DepInfList.FirstOrDefault(x => x.DeptCode == MstrInf.Code);

                if (IsCodExst == null)
                {
                    DeptInfo.DeptCode = MstrInf.Code;
                    DeptInfo.DeptName = MstrInf.Name;
                    DeptInfo.DeptLocalName = MstrInf.LocalName;
                    DeptInfo.DeptDesc = MstrInf.Descr;

                    _departmentService.Update(DeptInfo);
                    _departmentService.Save();

                    var DeptID = DeptInfo.DeptID;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateDepartmentInfo",
                       DeptID.ToString(), Session["UserName"].ToString());

                    return "1";
                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }

        }

        private string UpdateDesignationInfo(VMMasterForm MstrInf)
        {
            var DesigInfo = _designationService.All().ToList().FirstOrDefault(x => x.DesigID == MstrInf.ID);

            if (DesigInfo != null)
            {
                var DsgInfList = _designationService.All().ToList().Where(c => c.DesigID != MstrInf.ID).ToList();
                var IsCodExst = DsgInfList.FirstOrDefault(x => x.DesigCode == MstrInf.Code);

                if (IsCodExst == null)
                {
                    DesigInfo.DesigCode = MstrInf.Code;
                    DesigInfo.DesigName = MstrInf.Name;
                    DesigInfo.DesigLocalName = MstrInf.LocalName;
                    DesigInfo.DesigDesc = MstrInf.Descr;

                    _designationService.Update(DesigInfo);
                    _designationService.Save();

                    var DesigID = DesigInfo.DesigID;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateDesignationInfo",
                       DesigID.ToString(), Session["UserName"].ToString());


                    return "1";
                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }

        }

        private string UpdateLocationInfo(VMMasterForm MstrInf)
        {

            var LocInfo = _LocationService.All().ToList().FirstOrDefault(x => x.LocId == MstrInf.ID);

            if (LocInfo != null)
            {
                var DsgInfList = _LocationService.All().ToList().Where(c => c.LocId != MstrInf.ID).ToList();
                var IsCodExst = DsgInfList.FirstOrDefault(x => x.LocCode == MstrInf.Code);

                if (IsCodExst == null)
                {
                   
                    LocInfo.LocName = MstrInf.Name;

                    _LocationService.Update(LocInfo);
                    _LocationService.Save();

                    var DesigID = LocInfo.LocCode;

                    TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "UpdateDesignationInfo",
                       DesigID, Session["UserName"].ToString());


                    return "1";
                }
                else
                {
                    return "3";
                }
            }
            else
            {
                return "2";
            }                     

        }

        public ActionResult DeleteMasterRecord(string Type, string DeleteID)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MasterInformation_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    //DeleteMemberImages(MemberId);
                    int DelStatus = 0;
                    using (AcclineERPContext dbContext = new AcclineERPContext())
                    {
                        var resultParameter = new SqlParameter("@Result", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        dbContext.Database.ExecuteSqlCommand("MasterRecordDelete @Type, @DeleteID, @Result out",
                        new SqlParameter("@Type", Type),
                        new SqlParameter("@DeleteID", DeleteID),
                        resultParameter);

                        DelStatus = (int)resultParameter.Value;

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "MasterInformation", "Delete",
                        string.Empty, Session["UserName"].ToString());

                    }

                    transaction.Complete();

                    return Json(DelStatus, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }



    }
}
