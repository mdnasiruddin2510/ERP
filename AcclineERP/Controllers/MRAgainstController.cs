using App.Domain;
using App.Domain.Interface.Service;
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
    public class MRAgainstController : Controller
    {

        private readonly IMRAgainstService _mrAgainstService;

        public MRAgainstController(IMRAgainstService _mrAgainstService)
        {

            this._mrAgainstService = _mrAgainstService;
        }
        // GET: MRAgainst
        public ActionResult MRAgainst()
        {
            ViewBag.AcCode = LoadDropDown.LoadAcCodefoMRA();
            
            return View();
        }

        public ActionResult GetAllDataForMRAgainst()
        {
            try
            {

                List<MRAgainst> MRAgainstList = new List<MRAgainst>();

                MRAgainstList = LoadMRAgainst();


                if (MRAgainstList != null)
                {
                    return Json(new { data = MRAgainstList }, JsonRequestBehavior.AllowGet);
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

        private List<MRAgainst> LoadMRAgainst()
        {

            List<MRAgainst> MRAgainstList = new List<MRAgainst>();

            var mrLoList = _mrAgainstService.All().ToList();

            if (mrLoList.Count > 0)
            {

                foreach (var value in mrLoList)
                {
                    MRAgainst mrAgainstAdd = new MRAgainst();
                    mrAgainstAdd.MRAgainstID = value.MRAgainstID;
                    mrAgainstAdd.MRAgainstDesc = value.MRAgainstDesc;
                    mrAgainstAdd.Accode = value.Accode;
                    mrAgainstAdd.MRAgainstType = value.MRAgainstType;
                    //locadd.Password = uObj.Password;


                    MRAgainstList.Add(mrAgainstAdd);
                }

                if (MRAgainstList != null)
                {
                    MRAgainstList = MRAgainstList.OrderBy(x => x.MRAgainstID).ToList();
                }

                return MRAgainstList;
            }
            else
            {
                return null;
            }

        }

        public ActionResult SaveMRAgainst(MRAgainst AcCode)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("MRAgainst_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var IfExit = _mrAgainstService.All().Where(x => x.Accode == AcCode.Accode).LastOrDefault();
                    if (IfExit==null)
                    {

                        MRAgainst mrAgainstadd = new MRAgainst();
                        mrAgainstadd = AcCode;
                        //branch.BranchCode = _branchService.All().Select(x => x.BranchCode).LastOrDefault() + 1;
                        _mrAgainstService.Add(mrAgainstadd);
                        _mrAgainstService.Save();

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
        //public ActionResult SaveSelectedMRAgainst(List<MRAgainst> Check)
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {
        //            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
        //            if (!rUser.HasPermission("Receive_Insert"))
        //            {
        //                return Json("X", JsonRequestBehavior.AllowGet);
        //            }

        //            //var IsExistBr = _acbrServic.All().Where(x => x.BranchCode == BranchCode).ToList();
        //            //foreach (var data in IsExistBr)
        //            //{

        //            //    _acbrServic.Delete(data);
        //            //    _acbrServic.Save();
        //            //}


        //            // transaction.Complete();


        //            //if (Check != null)
        //            //{


        //            foreach (var data in Check)
        //            {
        //                //  var IfExist = _acbrServic.All().Where(x => x.Accode == data.Accode && x.BranchCode == data.BranchCode).FirstOrDefault();                      
        //                if (Check.Count != 0)
        //                {
        //                    List<MRAgainst> AcBrList = new List<MRAgainst>();

        //                    MRAgainst mra = new MRAgainst();
        //                    mra.MRAgainstDesc = data.MRAgainstDesc;
        //                    //acb.BranchCode = data.BranchCode;
        //                    mra.AcCode = data.AcCode;
        //                    mra.MRAgainstType = data.MRAgainstType;

        //                    AcBrList.Add(mra);
        //                    _mrAgainstService.Add(mra);
        //                    _mrAgainstService.Save();
        //                }
        //            }

        //            transaction.Complete();
        //            //Check.Clear();
        //            return Json("1", JsonRequestBehavior.AllowGet);

        //        }
        //        catch (Exception)
        //        {
        //            transaction.Dispose();
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

        public ActionResult DeleteMRAgainst(string AcCode)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MRAgainst_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    //var IsExist = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == BranchCode);
                    var IsUserBr = _mrAgainstService.All().Where(x => x.Accode == AcCode).FirstOrDefault();
                    if (IsUserBr != null)
                    {

                        //For user branch table by Farhad
                        _mrAgainstService.Delete(IsUserBr);
                        _mrAgainstService.Save();
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
    }
}