using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using AcclineERP.Models;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AcclineERP.Controllers
{
    public class VMBINController : Controller
    {
        // GET: /VMBIN/
        private IVM_BINAppService _pR_VM_BINService;
        private ITransactionLogAppService _transactionLogService;
        private IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly INewChartAppService _NewChartAppService;

        public VMBINController(IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService, 
            INewChartAppService _NewChartAppService, IVM_BINAppService _pR_VM_BINService)
        {

            this._pR_VM_BINService = _pR_VM_BINService;
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._NewChartAppService = _NewChartAppService;

        }

        public ActionResult VMBIN()
        {
            var itemProj = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
            List<SelectListItem> _list = itemProj.ToList();
            _list.Insert(0, new SelectListItem() { Value = "-1", Text = "---select---" });
            itemProj = new SelectList((IEnumerable<SelectListItem>)_list, "Value", "Text");
            ViewBag.ProjCode = itemProj;



            var itemBranch = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
            List<SelectListItem> _listBranch = itemBranch.ToList();
            _listBranch.Insert(0, new SelectListItem() { Value = "-1", Text = "---select---" });
            itemBranch = new SelectList((IEnumerable<SelectListItem>)_listBranch, "Value", "Text");
            ViewBag.BranchCode = itemBranch;

            return View();
        }


        public ActionResult GetAllDataForVM_BIN()
        {
            try
            {
                List<VM_BIN> objVM_BIN = new List<VM_BIN>();

                objVM_BIN = _pR_VM_BINService.All().ToList();


                foreach (var item in objVM_BIN)
                {
                    if(item.ProjCode!="1")
                    item.ProjCode = _ProjInfoService.All().Where(x => x.ProjCode == item.ProjCode).Select(s => s.ProjName).FirstOrDefault();
                    if (item.BranchCode != "1")
                        item.BranchCode = _BranchService.All().Where(x => x.BranchCode == item.BranchCode).Select(s => s.BranchName).FirstOrDefault();

                }


                Session["mydata"] = objVM_BIN;


                if (objVM_BIN != null)
                {




                    return Json(new { data = objVM_BIN }, JsonRequestBehavior.AllowGet);
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







        [HttpPost]
        public ActionResult SaveVM_BIN(VM_BIN VM_BIN)
        {
            //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            //if (!rUser.HasPermission("GL_VM_BIN_Insert"))
            //{
            //    return Json("X", JsonRequestBehavior.AllowGet);
            //}


            string eCode = "";

            if (VM_BIN.ProjCode == "-1") VM_BIN.ProjCode = "1";
            if (VM_BIN.BranchCode == "-1") VM_BIN.BranchCode = "1";

            //string proj = VM_BIN.ProjCode.Substring(0, 1);


            using (var transaction = new TransactionScope())
            {

                try
                {
                    // query   for check existing  data and then if  null go to transaction

                    var objVM_BIN = _pR_VM_BINService.All()
                                    .Where(x => x.ProjCode == VM_BIN.ProjCode && x.BranchCode == VM_BIN.BranchCode).ToList();

                  

                    if (objVM_BIN.Count == 0)
                    {

                        VM_BIN SaveVM_BIN = new VM_BIN();

                        Misclns.CopyPropertiesTo(VM_BIN, SaveVM_BIN);

                        if (SaveVM_BIN.ProjCode == "-1") SaveVM_BIN.ProjCode ="1";


                        if (SaveVM_BIN.BranchCode == "-1") SaveVM_BIN.BranchCode = "1";


                        _pR_VM_BINService.Add(SaveVM_BIN);
                        _pR_VM_BINService.Save();


                        var VM_BINID = 1;   // not defined 

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "SaveVM_BIN", "Save", VM_BINID.ToString(), Session["UserName"].ToString());

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








        [HttpGet]
        public ActionResult DeleteVM_BIN(int Id)
        {

            //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            //if (!rUser.HasPermission("GL_VM_BIN_Delete"))
            //{
            //    return Json("D", JsonRequestBehavior.AllowGet);
            //}

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var IsExist = _pR_VM_BINService.All().ToList().FirstOrDefault(x => x.Id == Id);
                    if (IsExist != null)
                    {
                        _pR_VM_BINService.Delete(IsExist);
                        _pR_VM_BINService.Save();

                        var HDHolidayID = 1;

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "SaveVM_BIN", "Delete", HDHolidayID.ToString(), Session["UserName"].ToString());

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
