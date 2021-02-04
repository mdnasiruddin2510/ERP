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

    public class IssRecSrcDestController : Controller
    {
        private readonly IIssRecSrcDestAppService _issRecSrcDestService;
        private readonly ISubsidiaryTypeAppService _subsidiarytypeService;
        private readonly INewChartAppService _newchartService;
        public IssRecSrcDestController(ISubsidiaryInfoAppService _subsidiaryInfoService,

           IIssRecSrcDestAppService _issRecSrcDestService,
           ISubsidiaryTypeAppService _subsidiarytypeService,
            INewChartAppService _newchartService)
        {

            this._issRecSrcDestService = _issRecSrcDestService;
            this._subsidiarytypeService = _subsidiarytypeService;
            this._newchartService = _newchartService;
        }


        // GET: IssRecSrcDest
        public ActionResult IssRecSrcDest()
        {
            ViewBag.ActionSub = LoadDropDown.SubsidiaryItem();
            ViewBag.AcName = LoadDropDown.LoadAcCodefoMRA();
            return View();
        }

        public ActionResult SaveIssRecSrcDest(IssRecSrcDest lSrcDestId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("IssRecSrcDest_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    //var IfExit = _issRecSrcDestService.All().Where(x => x.ActionCtrl == lSrcDestId.ActionCtrl).ToList();
                    //if (IfExit.Count == 0)
                    //{

                    IssRecSrcDest issRecSrcDestAdd = new IssRecSrcDest();
                    issRecSrcDestAdd = lSrcDestId;
                     //select COUNT(srcdestId)from IssRecSrcDest                   
                    var srcDestId = _issRecSrcDestService.All().Select(x => x.SrcDestId).Count();
                    issRecSrcDestAdd.SrcDestId = (srcDestId + 1).ToString();
                    _issRecSrcDestService.Add(issRecSrcDestAdd);
                    _issRecSrcDestService.Save();

                    transaction.Complete();
                    //}
                    var ecode = 1;
                    return Json(ecode, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetAllDataForIssRecSrcDest()
        {
            try
            {

                List<IssRecSrcDest> IssRecSrcDestList = new List<IssRecSrcDest>();

                IssRecSrcDestList = LoadIssRecSrcDest();


                if (IssRecSrcDestList != null)
                {
                    return Json(new { data = IssRecSrcDestList }, JsonRequestBehavior.AllowGet);
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

        private List<IssRecSrcDest> LoadIssRecSrcDest()
        {

            List<IssRecSrcDest> issRecSrcDestList = new List<IssRecSrcDest>();

            var issRecSrcList = _issRecSrcDestService.All().ToList();

            if (issRecSrcList.Count > 0)
            {

                foreach (var value in issRecSrcList)
                {
                    IssRecSrcDest issRecSrcDestadd = new IssRecSrcDest();
                    issRecSrcDestadd.SrcDestId = value.SrcDestId;
                    issRecSrcDestadd.SrcDestName = value.SrcDestName;
                    issRecSrcDestadd.Type = value.Type;
                    if (issRecSrcDestadd.Type == "S")
                    {
                        issRecSrcDestadd.Type = "Source";
                    }
                    else if (issRecSrcDestadd.Type == "D")
                    {
                        issRecSrcDestadd.Type = "Destination";
                    }

                    issRecSrcDestadd.ActionSub = value.ActionSub;
                    issRecSrcDestadd.ActionSub = _subsidiarytypeService.All().Where(x => x.TypeCode == value.ActionSub).Select(x => x.SubType).FirstOrDefault();

                    issRecSrcDestadd.ActionCtrl = value.ActionCtrl;
                    issRecSrcDestadd.ActionCtrl = _newchartService.All().Where(x => x.Accode == value.ActionCtrl).Select(x => x.AcName).FirstOrDefault();
                    issRecSrcDestList.Add(issRecSrcDestadd);
                }

                if (issRecSrcDestList != null)
                {
                    issRecSrcDestList = issRecSrcDestList.OrderBy(x => x.ActionCtrl).ToList();
                }

                return issRecSrcDestList;
            }
            else
            {
                return null;
            }

        }

        public ActionResult GetIssRecSrcDestByIdforEdit(string SrcDestId)
        {

              RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("IssRecSrcDest_Edit"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }
        
            try
            {
                IssRecSrcDest IssRecSrcDestName = new IssRecSrcDest();
                  var data = _issRecSrcDestService.All().ToList().FirstOrDefault(x => x.SrcDestId == SrcDestId);
                    if (data != null)
                    {
                        IssRecSrcDestName.SrcDestId = data.SrcDestId;
                        IssRecSrcDestName.SrcDestName = data.SrcDestName;
                        IssRecSrcDestName.Type = data.Type;
                        IssRecSrcDestName.ActionSub = data.ActionSub;
                        IssRecSrcDestName.ActionCtrl = data.ActionCtrl;
                    }
                  //  return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
                    return Json(IssRecSrcDestName, JsonRequestBehavior.AllowGet);
                
             // return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpdateIssRecSrcDest(IssRecSrcDest SrcDest, string SrcDestId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("IssRecSrcDest_Update"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                   // IssRecSrcDest GgName = new IssRecSrcDest();
                    var data = _issRecSrcDestService.All().ToList().FirstOrDefault(x => x.SrcDestId == SrcDestId);
                        if (data != null)
                        {
                            //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);

                            data.SrcDestName = SrcDest.SrcDestName;
                            data.Type = SrcDest.Type;
                            data.ActionSub = SrcDest.ActionSub;
                            data.ActionCtrl = SrcDest.ActionCtrl;
                            _issRecSrcDestService.Update(data);
                            _issRecSrcDestService.Save();

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
        public ActionResult DeleteIssRecSrcDest(string SrcDestId)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("IssRecSrcDest_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {

                    var IsUserBr = _issRecSrcDestService.All().Where(x => x.SrcDestId == SrcDestId).FirstOrDefault();
                        if (IsUserBr != null)
                        {

                            //For user branch table by Farhad
                            _issRecSrcDestService.Delete(IsUserBr);
                            _issRecSrcDestService.Save();
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