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
    public class SecUserGroupController : Controller
    {
        //
        // GET: /SecUserGroup/

        private ISecUserGroupAppService _secUserGroupService;
        private ISecUserInfoAppService _secUserInfoService;
        private ISecUserInGroupAppService _secUserInGroupService;

        public SecUserGroupController(ISecUserGroupAppService _secUserGroupService,
                                ISecUserInfoAppService _secUserInfoService,
                                ISecUserInGroupAppService _secUserInGroupService )
        {
            this._secUserGroupService = _secUserGroupService;
            this._secUserInfoService = _secUserInfoService;
            this._secUserInGroupService = _secUserInGroupService;

        }

        public ActionResult SecUserGroup()
        {
            if (Session["UserID"] != null)
            {
                return View("~/Views/Security/SecUserGroup.cshtml");
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        [HttpPost]
        public ActionResult SaveSecUsrGroupInfo(SecUserGroup SecUsrGrp, string[] SecUsers)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserGroup_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";

            using (var transaction = new TransactionScope())
            {
                try
                {

                    var UserGroup = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupName == SecUsrGrp.GroupName);

                    if (UserGroup == null)
                    {

                        _secUserGroupService.Add(SecUsrGrp);
                        _secUserGroupService.Save();
                       

                        if (SecUsers != null && SecUsers.Length > 0)
                        {

                            foreach (var user in SecUsers)
                            {
                                SecUserInGroup SuInGrp = new SecUserInGroup();

                                SuInGrp.GroupID = SecUsrGrp.GroupID;
                                SuInGrp.UserID = Convert.ToInt32(user);

                                _secUserInGroupService.Add(SuInGrp);
                            }

                            _secUserInGroupService.Save();
                        }

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


        public ActionResult GetSecUsrGroupInfoByNameNID(int GroupID, string GroupName)
        {
            try
            {

                SecUserGroup sug = new SecUserGroup();
                var SecGrp = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID && x.GroupName == GroupName);

                var SUInGrpLst = _secUserInGroupService.All().ToList().Where(x => x.GroupID == GroupID).ToList();

                sug.GroupID = SecGrp.GroupID;
                sug.GroupName = SecGrp.GroupName;
                sug.Description = SecGrp.Description;

                if (SUInGrpLst.Count > 0)
                {
                    foreach (var SIG in SUInGrpLst)
                    {
                        SecUserInGroup objSIG = new SecUserInGroup();
                        objSIG.UserGroupID = SIG.UserGroupID;
                        objSIG.GroupID = SIG.GroupID;
                        objSIG.UserID = SIG.UserID;

                        sug.SecUserInGroups.Add(objSIG);
                    }
                }


                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                string json = JsonConvert.SerializeObject(sug, Formatting.Indented, serializerSettings);

                if (json != null)
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

        public ActionResult GetAllDataForSecUsrGroup()
        {
            try
            {

                List<SecUserGroup> SUGList = new List<SecUserGroup>();

                SUGList = LoadSecUserGroup();


                if (SUGList != null)
                {
                    return Json(new { data = SUGList }, JsonRequestBehavior.AllowGet);
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

        private List<SecUserGroup> LoadSecUserGroup()
        {

            List<SecUserGroup> SUGList = new List<SecUserGroup>();

            var GList = _secUserGroupService.All().ToList();

            if (GList.Count > 0)
            {

                foreach (var MObj in GList)
                {
                    SecUserGroup sug = new SecUserGroup();
                    sug.GroupID = MObj.GroupID;
                    sug.GroupName = MObj.GroupName;
                    sug.Description = MObj.Description;

                    SUGList.Add(sug);
                }

                if (SUGList != null)
                {
                    SUGList = SUGList.OrderBy(x => x.GroupName).ToList();
                }

                return SUGList;
            }
            else
            {
                return null;
            }

        }


        public ActionResult DeleteSecUserGroup(int GroupID)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserGroup_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var IsExist = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID);
                    if (IsExist != null)
                    {
                        _secUserGroupService.Delete(IsExist);
                        _secUserGroupService.Save();
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
        public ActionResult UpdateSecUsrGroupInfo(SecUserGroup SecUsrGrp, string[] SecUsers)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecUserGroup_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";

            using (var transaction = new TransactionScope())
            {
                try
                {

                    SecUserGroup secgr = new SecUserGroup();

                    secgr = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupID == SecUsrGrp.GroupID);

                    var SecUGList = _secUserInGroupService.All().ToList().Where(x => x.GroupID == SecUsrGrp.GroupID).ToList();

                    if (secgr != null)
                    {

                        secgr.GroupName = SecUsrGrp.GroupName;
                        secgr.Description = SecUsrGrp.Description;

                        _secUserGroupService.Update(secgr);
                        _secUserGroupService.Save();

                        if (SecUGList.Count > 0 && SecUsers != null)
                        {
                            foreach (var SecUG in SecUGList)
                            {
                                _secUserInGroupService.Delete(SecUG);
                            }

                            foreach (var user in SecUsers)
                            {
                                SecUserInGroup SuInGrp = new SecUserInGroup();

                                SuInGrp.GroupID = secgr.GroupID;
                                SuInGrp.UserID = Convert.ToInt32(user);

                                _secUserInGroupService.Add(SuInGrp);
                            }

                            _secUserInGroupService.Save();
                        }
                        else if (SecUGList.Count > 0 && SecUsers == null)
                        {
                            foreach (var SecUG in SecUGList)
                            {
                                _secUserInGroupService.Delete(SecUG);
                            }
                            _secUserInGroupService.Save();
                        }
                        else if (SecUGList.Count == 0 && SecUsers != null)
                        {
                            foreach (var user in SecUsers)
                            {
                                SecUserInGroup SuInGrp = new SecUserInGroup();

                                SuInGrp.GroupID = secgr.GroupID;
                                SuInGrp.UserID = Convert.ToInt32(user);

                                _secUserInGroupService.Add(SuInGrp);
                            }

                            _secUserInGroupService.Save();
                        }

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


        //Add User To Security Group
        public ActionResult GetSecUsrInfoToAddInGroup()
        {
            try
            {

                var SecGrp = _secUserInfoService.All().ToList().Select(x => new { x.UserID, x.UserName, x.Email }).ToList();

                //var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                //string json = JsonConvert.SerializeObject(SecGrp, Formatting.Indented, serializerSettings);

                if (SecGrp != null)
                {
                    return Json(SecGrp, JsonRequestBehavior.AllowGet);
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

    }
}
