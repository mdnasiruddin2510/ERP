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
    public class SecUserInGroupController : Controller
    {
        //
        // GET: /SecUserInGroup/

        private ISecUserGroupAppService _secUserGroupService;
        private ISecUserInfoAppService _seecUserInfoService;

        public SecUserInGroupController(
                                ISecUserGroupAppService _secUserGroupService,
                                ISecUserInfoAppService _seecUserInfoService
                                
                                )
        {

            this._secUserGroupService = _secUserGroupService;
            this._seecUserInfoService = _seecUserInfoService;

        }

        public ActionResult SecUserInGroup()
        {
            return View("~/Views/Security/SecUserInGroup.cshtml");
        }

        public ActionResult GetAllDataOfSecUserInfo()
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

    }
}
