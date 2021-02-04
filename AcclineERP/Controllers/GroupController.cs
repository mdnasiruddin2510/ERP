using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace TopUp17Web.Controllers
{
    public class GroupController : Controller
    {

        private readonly IGroupInfoService _groupinfoService;
        private readonly ISGroupInfoService _sgroupinfoService;
        private readonly ISSGroupInfoService _ssgroupinfoService;

        public GroupController(ISubsidiaryInfoAppService _subsidiaryInfoService,

          IGroupInfoService _groupinfoService,
          ISGroupInfoService _sgroupinfoService,
          ISSGroupInfoService _ssgroupinfoService
            )
        {

            this._groupinfoService = _groupinfoService;
            this._sgroupinfoService = _sgroupinfoService;
            this._ssgroupinfoService = _ssgroupinfoService;

        }

        // GET: Group
        public ActionResult Group()
        {
            ViewBag.GroupTypeId = LoadDropDown.LoadGroupType();
            return View();
        }
        public ActionResult SaveSelectedGroupData(string GroupName, int GroupTypeId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Group_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    if (GroupTypeId == 1)
                    {
                        GroupInfo mrgroupadd = new GroupInfo();
                        mrgroupadd.GroupName = GroupName;
                        ////branch.BranchCode = _branchService.All().Select(x => x.BranchCode).LastOrDefault() + 1;
                        _groupinfoService.Add(mrgroupadd);
                        _groupinfoService.Save();

                        transaction.Complete();
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }
                    else if (GroupTypeId == 2)
                    {
                        SGroupInfo mrsgroupadd = new SGroupInfo();
                        mrsgroupadd.SGroupName = GroupName;
                        ////branch.BranchCode = _branchService.All().Select(x => x.BranchCode).LastOrDefault() + 1;
                        _sgroupinfoService.Add(mrsgroupadd);
                        _sgroupinfoService.Save();

                        transaction.Complete();
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }

                    else if (GroupTypeId == 3)
                    {
                        SSGroupInfo mrssgroupadd = new SSGroupInfo();
                        mrssgroupadd.SSGroupName = GroupName;
                        ////branch.BranchCode = _branchService.All().Select(x => x.BranchCode).LastOrDefault() + 1;
                        _ssgroupinfoService.Add(mrssgroupadd);
                        _ssgroupinfoService.Save();

                        transaction.Complete();
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        var ecode = 2;
                        transaction.Dispose();
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

       
        public ActionResult GetDataForSelectedGroup(int GroupTypeId)
        {
          
            if (GroupTypeId == 1)
            {
                string sql = string.Format("select * from GroupInfo");
                IEnumerable<GroupInfo> GroupinfoList;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    GroupinfoList = dbContext.Database.SqlQuery<GroupInfo>(sql).ToList();
                }
                return Json(new { GroupinfoList = GroupinfoList, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
            }
            else if (GroupTypeId == 2)
            {
                string sql = string.Format("select * from SGroupInfo");
                IEnumerable<SGroupInfo> GroupinfoList;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    GroupinfoList = dbContext.Database.SqlQuery<SGroupInfo>(sql).ToList();
                }
                return Json(new { GroupinfoList = GroupinfoList, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
            }
            else if (GroupTypeId == 3)
            {
                string sql = string.Format("select * from SSGroupInfo");
                IEnumerable<SSGroupInfo> GroupinfoList;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    GroupinfoList = dbContext.Database.SqlQuery<SSGroupInfo>(sql).ToList();
                }
                return Json(new { GroupinfoList = GroupinfoList, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
            }

            return Json(JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteSelectedGroupData(int GroupID, int GroupTypeId)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Group_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (GroupTypeId == 1)
                    {
                        var IsUserBr = _groupinfoService.All().Where(x => x.GroupID == GroupID).FirstOrDefault();
                        if (IsUserBr != null)
                        {

                            //For user branch table by Farhad
                            _groupinfoService.Delete(IsUserBr);
                            _groupinfoService.Save();
                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (GroupTypeId == 2)
                    {
                        var IsUserBr = _sgroupinfoService.All().Where(x => x.SGroupID == GroupID).FirstOrDefault();
                        if (IsUserBr != null)
                        {

                            //For user branch table by Farhad
                            _sgroupinfoService.Delete(IsUserBr);
                            _sgroupinfoService.Save();
                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (GroupTypeId == 3)
                    {
                        var IsUserBr = _ssgroupinfoService.All().Where(x => x.SSGroupID == GroupID).FirstOrDefault();
                        if (IsUserBr != null)
                        {

                            //For user branch table by Farhad
                            _ssgroupinfoService.Delete(IsUserBr);
                            _ssgroupinfoService.Save();
                            transaction.Complete();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json("2", JsonRequestBehavior.AllowGet);


                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetGroupByIdforEdit(int GroupID, int GroupTypeId)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Group_Edit"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (GroupTypeId == 1)
                {
                    GroupInfo GgName = new GroupInfo();
                    var data = _groupinfoService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID);
                    if (data != null)
                    {
                        GgName.GroupName = data.GroupName;
                    }
                    return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
                    //  return Json(GgName, JsonRequestBehavior.AllowGet);
                }
                else if (GroupTypeId == 2)
                {
                    SGroupInfo GgName = new SGroupInfo();
                    var data = _sgroupinfoService.All().ToList().FirstOrDefault(x => x.SGroupID == GroupID);
                    if (data != null)
                    {
                        GgName.SGroupName = data.SGroupName;
                    }
                    return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
                }
                else if (GroupTypeId == 3)
                {
                    SSGroupInfo GgName = new SSGroupInfo();
                    var data = _ssgroupinfoService.All().ToList().FirstOrDefault(x => x.SSGroupID == GroupID);
                    if (data != null)
                    {
                        GgName.SSGroupName = data.SSGroupName;
                    }
                    return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);

                }


                return Json(JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UpdateSelectedGroup(string GroupName, int GroupID, int GroupTypeId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Group_Update"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    if (GroupTypeId==1) {
                        GroupInfo GgName = new GroupInfo();
                        var data = _groupinfoService.All().ToList().FirstOrDefault(x => x.GroupID == GroupID);
                        if (data != null)
                        {
                            //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);

                            data.GroupName = GroupName;
                            _groupinfoService.Update(data);
                            _groupinfoService.Save();

                            transaction.Complete();
                        }
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }
                    else if (GroupTypeId == 2) {
                        SGroupInfo GgName = new SGroupInfo();
                        var data = _sgroupinfoService.All().ToList().FirstOrDefault(x => x.SGroupID == GroupID);
                        if (data != null)
                        {
                            //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);

                            data.SGroupName = GroupName;
                            _sgroupinfoService.Update(data);
                            _sgroupinfoService.Save();

                            transaction.Complete();
                        }
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }
                    else if (GroupTypeId == 3) {
                        SSGroupInfo GgName = new SSGroupInfo();
                        var data = _ssgroupinfoService.All().ToList().FirstOrDefault(x => x.SSGroupID == GroupID);
                        if (data != null)
                        {
                            //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);

                            data.SSGroupName = GroupName;
                            _ssgroupinfoService.Update(data);
                            _ssgroupinfoService.Save();

                            transaction.Complete();
                        }
                        var ecode = 1;
                        return Json(ecode, JsonRequestBehavior.AllowGet);
                    }
                    
                    return Json( JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}