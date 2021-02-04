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
using System.Transactions;
using Newtonsoft.Json;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class SecFormRightController : Controller
    {
        //
        // GET: /SecFormRight/
        private ISecUserGroupAppService _secUserGroupService;
        private ISecFormProcessAppService _secFormProcessService;
        private ISecFormRightAppService _secFormRightService;
        private ISecFormListAppService _secFormListService;

        public SecFormRightController(
                                ISecUserGroupAppService _secUserGroupService,
                                ISecFormProcessAppService _secFormProcessService,
                                ISecFormRightAppService _secFormRightService,
                                ISecFormListAppService _secFormListService
                                )
        {

            this._secUserGroupService = _secUserGroupService;
            this._secFormProcessService = _secFormProcessService;
            this._secFormRightService = _secFormRightService;
            this._secFormListService = _secFormListService;

        }


        public ActionResult SecFormRight()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.GroupID = LoadDropDown.LoadAllSecUserGroups(_secUserGroupService);
                return View("~/Views/Security/SecFormRight.cshtml");
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        [HttpPost]
        public ActionResult SaveSecFormRightInfo(SecFormRight SecFrmRght, string[] SecFrmProc)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecFormRight_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }


            string eCode = "";

            using (var transaction = new TransactionScope())
            {
                try
                {

                    var UserGroup = _secFormRightService.All().ToList().FirstOrDefault(x => x.GroupID == SecFrmRght.GroupID);

                    if (UserGroup == null && SecFrmProc != null && SecFrmProc.Length > 0)
                    {

                        foreach (var FrmProcID in SecFrmProc)
                        {
                            SecFormRight SFRght = new SecFormRight();

                            SFRght.GroupID = SecFrmRght.GroupID;
                            SFRght.FormProcessID = Convert.ToInt32(FrmProcID);

                            _secFormRightService.Add(SFRght);
                        }

                        _secFormRightService.Save();


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


        public ActionResult GetSecFormRightInfoByGroup(int GroupID)
        {
            try
            {
                var SecGrpList = _secFormRightService.All().ToList().Where(x => x.GroupID == GroupID).ToList();

                SecUserGroup sug = new SecUserGroup();
                var SecGrp = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID);

                if (SecGrp != null && SecGrpList.Count > 0)
                {

                    sug.GroupID = SecGrp.GroupID;
                    sug.GroupName = SecGrp.GroupName;
                    sug.Description = SecGrp.Description;

                    foreach (var SFR in SecGrpList)
                    {
                        SecFormRight Sfrt = new SecFormRight();
                        Sfrt.FormRightID = SFR.FormRightID;
                        Sfrt.GroupID = SFR.GroupID;
                        Sfrt.FormProcessID = SFR.FormProcessID;

                        sug.SecFormRights.Add(Sfrt);
                    }

                }

                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                string json = JsonConvert.SerializeObject(sug, Formatting.Indented, serializerSettings);

                if (sug.SecFormRights.Count > 0)
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

        //public ActionResult GetAllDataForSecUsrGroup()
        //{
        //    try
        //    {

        //        List<SecUserGroup> SUGList = new List<SecUserGroup>();

        //        SUGList = LoadSecUserGroup();


        //        if (SUGList != null)
        //        {
        //            return Json(new { data = SUGList }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { data = new EmptyResult() }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json("0", JsonRequestBehavior.AllowGet);
        //    }

        //}

        //private List<SecUserGroup> LoadSecUserGroup()
        //{

        //    List<SecUserGroup> SUGList = new List<SecUserGroup>();

        //    var GList = _secUserGroupService.All().ToList();

        //    if (GList.Count > 0)
        //    {

        //        foreach (var MObj in GList)
        //        {
        //            SecUserGroup sug = new SecUserGroup();
        //            sug.GroupID = MObj.GroupID;
        //            sug.GroupName = MObj.GroupName;
        //            sug.Description = MObj.Description;

        //            SUGList.Add(sug);
        //        }

        //        if (SUGList != null)
        //        {
        //            SUGList = SUGList.OrderBy(x => x.GroupName).ToList();
        //        }

        //        return SUGList;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}


        //public ActionResult DeleteSecUserGroup(int GroupID)
        //{

            

        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {
        //            var IsExist = _secUserGroupService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID);
        //            if (IsExist != null)
        //            {
        //                _secUserGroupService.Delete(IsExist);
        //                _secUserGroupService.Save();
        //            }
        //            else
        //            {
        //                return Json("0", JsonRequestBehavior.AllowGet);
        //            }
        //            transaction.Complete();

        //            return Json("1", JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Dispose();
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}


        [HttpPost]
        public ActionResult UpdateSecFormRightInfo(SecFormRight SecFrmRght, string[] SecFrmProc)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("SecFormRight_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";

            using (var transaction = new TransactionScope())
            {
                try
                {

                    SecFormRight secfr = new SecFormRight();

                    //secfr = _secFormRightService.All().ToList().FirstOrDefault(x => x.GroupID == SecFrmRht.GroupID);

                    var SecFRList = _secFormRightService.All().ToList().Where(x => x.GroupID == SecFrmRght.GroupID).ToList();

                    if (SecFRList != null)
                    {

                        //secgr.GroupName = SecFrmRht.GroupName;
                        //secgr.Description = SecFrmRht.Description;

                        //_secUserGroupService.Update(secgr);
                        //_secUserGroupService.Save();

                        if (SecFRList.Count > 0 && SecFrmProc != null)
                        {
                            foreach (var SecUG in SecFRList)
                            {
                                _secFormRightService.Delete(SecUG);
                            }

                            foreach (var Fprcs in SecFrmProc)
                            {
                                SecFormRight SFRght = new SecFormRight();

                                SFRght.GroupID = SecFrmRght.GroupID;
                                SFRght.FormProcessID = Convert.ToInt32(Fprcs);

                                _secFormRightService.Add(SFRght);
                            }

                            _secFormRightService.Save();
                        }
                        else if (SecFRList.Count > 0 && SecFrmProc == null)
                        {
                            foreach (var SecUG in SecFRList)
                            {
                                _secFormRightService.Delete(SecUG);
                            }
                            _secFormRightService.Save();
                        }
                        else if (SecFRList.Count == 0 && SecFrmProc != null)
                        {
                            foreach (var Fprcs in SecFrmProc)
                            {
                                SecFormRight SFRght = new SecFormRight();

                                SFRght.GroupID = SecFrmRght.GroupID;
                                SFRght.FormProcessID = Convert.ToInt32(Fprcs);

                                _secFormRightService.Add(SFRght);
                            }

                            _secFormRightService.Save();
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
        public ActionResult GetSeccFormProcessToAddInRights()
        {
            try
            {

                var FrmProcList = (from mi in _secFormProcessService.All().ToList()
                                   join md in _secFormListService.All().ToList() on mi.FormID equals md.FormID
                                   select new
                                   {
                                       FormProcessID = mi.FormProcessID,
                                       FormID = mi.FormID,
                                       FormName = md.FormName,
                                       ProcessName = mi.ProcessName,
                                       ProcessDescription = mi.ProcessDescription

                                   }).ToList();

                //var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                //string json = JsonConvert.SerializeObject(SecGrp, Formatting.Indented, serializerSettings);

                if (FrmProcList != null)
                {
                    return Json(FrmProcList, JsonRequestBehavior.AllowGet);
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
