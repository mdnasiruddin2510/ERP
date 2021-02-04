using App.Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers 
{
    public class DynamicCaptionController : Controller
    {
        private readonly IDynaCapAppService _dynamiccapService;

        public DynamicCaptionController(ISubsidiaryInfoAppService _subsidiaryInfoService,

         IDynaCapAppService _dynamicService)
        {

            this._dynamiccapService = _dynamicService;
        }
        // GET: DynamicCaption
        public ActionResult DynamicCaption()
        {
            return View();
        }

        public ActionResult GetAllDataForDynamicCaption()
        {
            try
            {

                List<DynaCap> dynacapList = new List<DynaCap>();

                dynacapList = DynamicCaptionDest();


                if (dynacapList != null)
                {
                    return Json(new { data = dynacapList }, JsonRequestBehavior.AllowGet);
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
        private List<DynaCap> DynamicCaptionDest()
        {

            List<DynaCap> dynamicinfoList = new List<DynaCap>();

            var dynamicinfocheck = _dynamiccapService.All().ToList();

            if (dynamicinfocheck.Count > 0)
            {

                foreach (var value in dynamicinfocheck)
                {
                    DynaCap dynamiccaptiontadd = new DynaCap();
                    dynamiccaptiontadd.Id = 1;
                    dynamiccaptiontadd.Emp = value.Emp;
                    dynamiccaptiontadd.Code = value.Code;
                    dynamiccaptiontadd.Proj = value.Proj;
                    dynamiccaptiontadd.Branch = value.Branch;
                    dynamiccaptiontadd.Unit = value.Unit;
                    dynamiccaptiontadd.Dept = value.Dept;
                    dynamiccaptiontadd.Desig = value.Desig;
                    dynamiccaptiontadd.Dist = value.Dist;
                    dynamiccaptiontadd.Division = value.Division;
                    dynamiccaptiontadd.Post = value.Post;
                    dynamiccaptiontadd.Loc = value.Loc;
                    dynamiccaptiontadd.ItemGrp = value.ItemGrp;
                    dynamiccaptiontadd.ItemSGrp = value.ItemSGrp;
                    dynamiccaptiontadd.ItemSSGrp = value.ItemSSGrp;
                    dynamicinfoList.Add(dynamiccaptiontadd);
                }

                if (dynamicinfoList != null)
                {
                    dynamicinfoList = dynamicinfoList.OrderBy(x => x.Emp).ToList();
                }

                return dynamicinfoList;
            }
            else
            {
                return null;
            }

        }
        public ActionResult SaveDynamicCaption(DynaCap DynamicCaption)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("DynamicCaption_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    DynaCap IsExistDynaCap = _dynamiccapService.All().FirstOrDefault();
                    if (IsExistDynaCap == null)
                    {
                        _dynamiccapService.Add(DynamicCaption);
                        _dynamiccapService.Save();
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
                       
                    }
                    else
                    {
                        IsExistDynaCap.Emp = DynamicCaption.Emp;
                        IsExistDynaCap.Code = DynamicCaption.Code;
                        IsExistDynaCap.Proj = DynamicCaption.Proj;
                        IsExistDynaCap.Branch = DynamicCaption.Branch;
                        IsExistDynaCap.Unit = DynamicCaption.Unit;
                        IsExistDynaCap.Dept = DynamicCaption.Dept;
                        IsExistDynaCap.Desig = DynamicCaption.Desig;
                        IsExistDynaCap.Dist = DynamicCaption.Dist;
                        IsExistDynaCap.Division = DynamicCaption.Division;
                        IsExistDynaCap.Post = DynamicCaption.Post;
                        IsExistDynaCap.Loc = DynamicCaption.Loc;
                        IsExistDynaCap.ItemGrp = DynamicCaption.ItemGrp;
                        IsExistDynaCap.ItemSGrp = DynamicCaption.ItemSGrp;
                        IsExistDynaCap.ItemSSGrp = DynamicCaption.ItemSSGrp;
                        _dynamiccapService.Update(IsExistDynaCap);
                        _dynamiccapService.Save();
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
                      
                    }

                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult DynamicCaptionforEdit(byte Id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("DynamicCaption_Edit"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    DynaCap DynaCaplist = new DynaCap();
                    var DynamicCaption = _dynamiccapService.All().ToList().FirstOrDefault(x => x.Id == Id);
                    if (DynamicCaption != null)
                    {
                        DynaCaplist.Emp = DynamicCaption.Emp;
                        DynaCaplist.Code = DynamicCaption.Code;
                        DynaCaplist.Proj = DynamicCaption.Proj;
                        DynaCaplist.Branch = DynamicCaption.Branch;

                        DynaCaplist.Unit = DynamicCaption.Unit;
                        DynaCaplist.Dept = DynamicCaption.Dept;
                        DynaCaplist.Desig = DynamicCaption.Desig;
                        DynaCaplist.Dist = DynamicCaption.Dist;
                        DynaCaplist.Division = DynamicCaption.Division;
                        DynaCaplist.Post = DynamicCaption.Post;
                        DynaCaplist.Loc = DynamicCaption.Loc;
                        DynaCaplist.ItemGrp = DynamicCaption.ItemGrp;
                        DynaCaplist.ItemSGrp = DynamicCaption.ItemSGrp;
                        DynaCaplist.ItemSSGrp = DynamicCaption.ItemSSGrp;

                    }
                    //  return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
                    return Json(DynaCaplist, JsonRequestBehavior.AllowGet);

                    // return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }          
        }
        public ActionResult UpdateDynamicCaption(DynaCap DynamicCaption, byte Id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("DynamicCaption_Update"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    // IssRecSrcDest GgName = new IssRecSrcDest();
                    var DynaCaplist = _dynamiccapService.All().ToList().FirstOrDefault(x => x.Id == Id);
                    if (DynaCaplist != null)
                    {
                        //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);
                        DynaCaplist.Id = _dynamiccapService.All().Select(x => x.Id).LastOrDefault();
                        DynaCaplist.Emp = DynamicCaption.Emp;
                        DynaCaplist.Code = DynamicCaption.Code;
                        DynaCaplist.Proj = DynamicCaption.Proj;
                        DynaCaplist.Branch = DynamicCaption.Branch;

                        DynaCaplist.Unit = DynamicCaption.Unit;
                        DynaCaplist.Dept = DynamicCaption.Dept;
                        DynaCaplist.Desig = DynamicCaption.Desig;
                        DynaCaplist.Dist = DynamicCaption.Dist;
                        DynaCaplist.Division = DynamicCaption.Division;
                        DynaCaplist.Post = DynamicCaption.Post;
                        DynaCaplist.Loc = DynamicCaption.Loc;
                        DynaCaplist.ItemGrp = DynamicCaption.ItemGrp;
                        DynaCaplist.ItemSGrp = DynamicCaption.ItemSGrp;
                        DynaCaplist.ItemSSGrp = DynamicCaption.ItemSSGrp;

                        _dynamiccapService.Update(DynaCaplist);
                        _dynamiccapService.Save();

                        transaction.Complete();
                    }
                    var ecode = 1;
                    return Json(ecode, JsonRequestBehavior.AllowGet);
                }
                //return Json(JsonRequestBehavior.AllowGet);

                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult DeleteDynamicCaption(byte Id)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("DynamicCaption_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {

                    var IsDynaCap = _dynamiccapService.All().Where(x => x.Id == Id).FirstOrDefault();
                    if (IsDynaCap != null)
                    {

                        //For user branch table by Farhad
                        _dynamiccapService.Delete(IsDynaCap);
                        _dynamiccapService.Save();
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
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
    }
}