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
using Newtonsoft.Json;

namespace AcclineERP.Controllers
{
    public class SecUserInfoController : Controller
    {
        //
        // GET: /SecUserInfo/
        private ISecUserGroupAppService _secUserGroupService;
        private ISecFormProcessAppService _secFormProcessService;
        private ISecUserInfoAppService _secUserInfoService;
        private ISecFormListAppService _secFormListService;
        private IUserBranchAppService _userBranchService;

        public SecUserInfoController(
                                ISecUserGroupAppService _secUserGroupService,
                                ISecFormProcessAppService _secFormProcessService,
                                ISecUserInfoAppService _secUserInfoService,
                                ISecFormListAppService _secFormListService,
                                IUserBranchAppService _userBranchService
                                )
        {

            this._secUserGroupService = _secUserGroupService;
            this._secFormProcessService = _secFormProcessService;
            this._secUserInfoService = _secUserInfoService;
            this._secFormListService = _secFormListService;
            this._userBranchService = _userBranchService;

        }


        public ActionResult SecUserInfo()
        {
            if (Session["UserID"] != null)
            {
                //ViewBag.UserID = LoadDropDown.LoadAllSecUserGroups(_secUserGroupService);
                return View("~/Views/Security/SecUserInfo.cshtml");
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        [HttpPost]
        public ActionResult SaveSecUserInfo(SecUserInfo SecUsrInfo)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserInfo_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";
            
            using (var transaction = new TransactionScope())
            {
                try
                {

                    var UserInfo = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserName == SecUsrInfo.UserName);

                    if (UserInfo == null)
                    {                        
                        SecUserInfo SecUinf = new SecUserInfo();
                        SecUinf.UserName = SecUsrInfo.UserName;
                        SecUinf.Password = SHA1.Encode(SecUsrInfo.Password);
                        SecUinf.Email = SecUsrInfo.Email;
                        SecUinf.CreateDate = System.DateTime.Now;
                        _secUserInfoService.Add(SecUinf);
                        _secUserInfoService.Save();
                        //For user branch table by Farhad
                        var userId = _secUserInfoService.All().ToList().Where(x => x.UserName == SecUsrInfo.UserName).Select(s => s.UserID).FirstOrDefault();
                        UserBranch UB = new UserBranch();
                        UB.Userid = userId.ToString();
                        UB.BranchCode = "01";
                        _userBranchService.Add(UB);
                        _userBranchService.Save();
                        eCode = "1";
                    }
                    else
                    {
                        eCode = "2";
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


        public ActionResult GetSecUserInfoByID(int UserID)
        {
            try
            {

                SecUserInfo sug = new SecUserInfo();

                var SecUsr = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == UserID);

                //SecUserInfo sug = new SecUserInfo();
                //var SecGrp = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == UserID);

                if (SecUsr != null)
                {

                    sug.UserID = SecUsr.UserID;
                    sug.UserName = SecUsr.UserName;
                    sug.Password = SecUsr.Password;
                    sug.ConfPassword = SecUsr.Password;
                    sug.Email = SecUsr.Email;
                    sug.CreateDate = SecUsr.CreateDate;
                }
                //    foreach (var SFR in SecGrpList)
                //    {
                //        SecUserInfo Sfrt = new SecUserInfo();
                //        Sfrt.FormRightID = SFR.FormRightID;
                //        Sfrt.UserID = SFR.UserID;
                //        Sfrt.FormProcessID = SFR.FormProcessID;

                //        sug.SecUserInfos.Add(Sfrt);
                //    }

                //}

                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                string json = JsonConvert.SerializeObject(sug, Formatting.Indented, serializerSettings);

                if (sug != null && sug.UserID > 0)
                {
                    return Json(json, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetAllDataForSecUserInfo()
        {
            try
            {

                List<SecUserInfo> SUInfList = new List<SecUserInfo>();

                SUInfList = LoadSecUserInfo();


                if (SUInfList != null)
                {
                    return Json(new { data = SUInfList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = new EmptyResult() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        private List<SecUserInfo> LoadSecUserInfo()
        {

            List<SecUserInfo> SUInfList = new List<SecUserInfo>();

            var UList = _secUserInfoService.All().ToList();

            if (UList.Count > 0)
            {

                foreach (var uObj in UList)
                {
                    SecUserInfo sui = new SecUserInfo();
                    sui.UserID = uObj.UserID;
                    sui.UserName = uObj.UserName;
                    sui.Password = uObj.Password;
                    sui.Email = uObj.Email;
                    sui.CreateDate = uObj.CreateDate;

                    SUInfList.Add(sui);
                }

                if (SUInfList != null)
                {
                    SUInfList = SUInfList.OrderBy(x => x.UserName).ToList();
                }

                return SUInfList;
            }
            else
            {
                return null;
            }

        }


        public ActionResult DeleteSecUserInfo(int UserID)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserInfo_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var IsExist = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == UserID);
                    var IsUserBr = _userBranchService.All().ToList().FirstOrDefault(x => x.Userid == UserID.ToString());
                    if (IsExist != null && IsUserBr != null)
                    {
                        _secUserInfoService.Delete(IsExist);
                        _secUserInfoService.Save();
                        //For user branch table by Farhad
                        _userBranchService.Delete(IsUserBr);
                        _userBranchService.Save();
                    }
                    else
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    transaction.Complete();

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public ActionResult UpdateSecUserInfo(SecUserInfo SecUsrInfo)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserInfo_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";

            using (var transaction = new TransactionScope())
            {
                try
                {

                    SecUserInfo secuinf = new SecUserInfo();

                    //secfr = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == SecFrmRht.UserID);

                    secuinf = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == SecUsrInfo.UserID);

                    if (secuinf != null)
                    {

                        secuinf.UserName = SecUsrInfo.UserName;
                        secuinf.Password = SHA1.Encode(SecUsrInfo.Password);
                        secuinf.Email = SecUsrInfo.Email;
                        secuinf.CreateDate = System.DateTime.Now;//SecUsrInfo.CreateDate;

                        _secUserInfoService.Update(secuinf);
                        _secUserInfoService.Save();

                       
                        eCode = "1";
                    }
                    else
                    {
                        eCode = "2";
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


        ////Add User To Security Group
        //public ActionResult GetSecUserInfoToAddInRights()
        //{
        //    try
        //    {

        //        var FrmProcList = (from mi in _secUserInfoService.All().ToList()
        //                           //join md in _secUserInfoService.All().ToList() on mi.FormID equals md.FormID
        //                           select new
        //                           {
        //                               UserID = mi.UserID,
        //                               FormID = mi.UserName,
        //                               Password = mi.Password,
        //                               Email = mi.Email

        //                           }).ToList();

        //        //var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

        //        //string json = JsonConvert.SerializeObject(SecGrp, Formatting.Indented, serializerSettings);

        //        if (FrmProcList != null)
        //        {
        //            return Json(FrmProcList, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return Json("0", JsonRequestBehavior.AllowGet);
        //    }

        //}

    }
}
