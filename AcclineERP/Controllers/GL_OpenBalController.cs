using App.Domain;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class GL_OpenBalController : Controller
    {
        // GET: GL_OpenBal
        private IOpenBalAppService _pR_OpenBalanceService;
        private ITransactionLogAppService _transactionLogService;
        private IBranchAppService _BranchService;
        private readonly IProjInfoAppService _ProjInfoService;
        private readonly INewChartAppService _NewChartAppService;

        public GL_OpenBalController(IBranchAppService _BranchService, IProjInfoAppService _ProjInfoService,
            INewChartAppService _NewChartAppService, IOpenBalAppService _pR_OpenBalanceService, ITransactionLogAppService _transactionLogService)
        {
            this._pR_OpenBalanceService = _pR_OpenBalanceService;
            this._BranchService = _BranchService;
            this._ProjInfoService = _ProjInfoService;
            this._NewChartAppService = _NewChartAppService;
            this._transactionLogService = _transactionLogService;
        }
        public ActionResult GL_OpenBal()
        {
            ViewBag.ProjCode = new SelectList(_ProjInfoService.All().ToList(), "ProjCode", "ProjName");
            ViewBag.BranchCode = new SelectList(_BranchService.All().ToList(), "BranchCode", "BranchName");
            var defAc = LoadDropDown.GetDefAc();

            List<NewChart> charts = _NewChartAppService.All().ToList().Where(x => x.Accode.Substring(0, 1) == defAc.AssAc || x.Accode.Substring(0, 1) == defAc.LiaAc).ToList();
            List<NewChart> finalList = new List<NewChart>();

            foreach (var chart in charts)
            {
                bool key = true;
                foreach (var temp in charts)
                {
                    if (chart == temp)
                    {

                    }
                    else
                    {
                        if (chart.Accode == temp.ParentAcCode)
                        {
                            key = false;
                        }
                    }
                }
                if (key)
                {
                    finalList.Add(chart);
                }
            }
            ViewBag.Accode = new SelectList(finalList, "Accode", "AcName");
            return View();
        }

        [HttpPost]
        public ActionResult SaveOpenBalance(OpenBal OpnBal)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GL_OpenBal_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }

            string eCode = "";
            var defAc = LoadDropDown.GetDefAc();

            //    Checking and  operation Data for save database

            string isAsset = OpnBal.Accode.Substring(0, 1);

            if (isAsset == defAc.AssAc)
            {
                OpnBal.OpenBalance = OpnBal.OpenBalance;

            }
            else if (isAsset == defAc.LiaAc)
            {
                OpnBal.OpenBalance = OpnBal.OpenBalance * (-1);
            }
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // query   for check existing  data and then if  null go to transaction
                    var objOpnBal = _pR_OpenBalanceService.All().ToList()
                                    .Where(x => x.Accode == OpnBal.Accode && x.BranchCode == OpnBal.BranchCode && x.ProjCode == OpnBal.ProjCode).ToList();

                    if (objOpnBal.Count == 0)
                    {
                        //// should not here , in setOpnBal
                        OpnBal.FinYear = Session["FinYear"].ToString();
                        _pR_OpenBalanceService.Add(OpnBal);
                        _pR_OpenBalanceService.Save();
                        var OpenBalanceID = 1;   // not defined 

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "SaveOpnBal", "Save",
                           OpenBalanceID.ToString(), Session["UserName"].ToString());

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





        public ActionResult GetAllDataForOpenBalance(string Proj, string BrCode)
        {
            try
            {
                List<OpenBal> OBInfoList = new List<OpenBal>();

                OBInfoList = _pR_OpenBalanceService.All().Where(s => s.ProjCode == Proj && s.BranchCode == BrCode).ToList();

                // Convert the  object member  "Acccode"  to its name from newChart table

                var defAc = LoadDropDown.GetDefAc();

                foreach (var item in OBInfoList)
                {
                    item.AccName = _NewChartAppService.All().Where(x => x.Accode == item.Accode).Select(s => s.AcName).FirstOrDefault();

                    string isAsset = item.Accode.Substring(0, 1);

                    if (isAsset == defAc.AssAc)
                    {
                        item.YrCrBal = 0;
                        item.YrDrBal = item.OpenBalance;
                    }
                    else
                    {
                        item.YrDrBal = 0;
                        item.YrCrBal = item.OpenBalance * (-1);

                    }
                }
                Session["mydata"] = OBInfoList;

                if (OBInfoList != null)
                {
                    return Json(new { data = OBInfoList }, JsonRequestBehavior.AllowGet);
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




        [HttpGet]
        public ActionResult GetOpenBalForEdit(int OpenBalId)
        {
            try
            {
                OpenBal GLOB = new OpenBal();
                var defAc = LoadDropDown.GetDefAc();
                var Obal = _pR_OpenBalanceService.All().ToList().FirstOrDefault(x => x.Id == OpenBalId);
                string isAsset = Obal.Accode.Substring(0, 1);
                if (Obal != null)
                {
                    GLOB.Id = Obal.Id;
                    GLOB.ProjCode = Obal.ProjCode;
                    GLOB.BranchCode = Obal.BranchCode;
                    GLOB.Accode = Obal.Accode;

                    if (isAsset == defAc.AssAc)
                    {
                        GLOB.OpenBalance = Obal.OpenBalance;

                    }
                    else if (isAsset == defAc.LiaAc)
                    {
                        GLOB.OpenBalance = Obal.OpenBalance * (-1);
                    }
                }
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(GLOB, Formatting.Indented, serializerSettings);
                if (GLOB != null && GLOB.Id > 0)
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






        [HttpGet]
        public ActionResult DeleteOpenBalance(int OpenBalId)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GL_OpenBal_Delete"))
            {
                return Json("D", JsonRequestBehavior.AllowGet);
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var IsExist = _pR_OpenBalanceService.All().ToList().FirstOrDefault(x => x.Id == OpenBalId);
                    if (IsExist != null)
                    {
                        _pR_OpenBalanceService.Delete(IsExist);
                        _pR_OpenBalanceService.Save();
                        var HDHolidayID = IsExist.Id;
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "SaveOpnBal", "Delete",
                           HDHolidayID.ToString(), Session["UserName"].ToString());
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





        public ActionResult UpdateOpenBalance(OpenBal OpnBal)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GL_OpenBal_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }

            var defAc = LoadDropDown.GetDefAc();

            string eCode = "";
            string isAsset = OpnBal.Accode.Substring(0, 1);
            if (isAsset == defAc.AssAc)
            {
                OpnBal.OpenBalance = OpnBal.OpenBalance;
            }
            else if (isAsset == defAc.LiaAc)
            {
                OpnBal.OpenBalance = OpnBal.OpenBalance * (-1);
            }
            using (var transaction = new TransactionScope())
            {
                try
                {

                    OpenBal UpOpnBal = new OpenBal();

                    UpOpnBal = _pR_OpenBalanceService.All().ToList().FirstOrDefault(x => x.Id == OpnBal.Id);

                    if (UpOpnBal != null)
                    {
                        OpnBal.FinYear = Session["FinYear"].ToString();
                        Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);
                        _pR_OpenBalanceService.Update(UpOpnBal);
                        _pR_OpenBalanceService.Save();

                        var THolidayID = UpOpnBal.Id;

                        TransactionLogService.SaveTransactionLog(_transactionLogService, "OpenBal", "Update",
                           THolidayID.ToString(), Session["UserName"].ToString());

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


        public ActionResult GLOpenBalPreviewPdf()
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GL_OpenBal_Preview"))
            {
                string errMsg = "No Preview Prmission For This User !!";
                return RedirectToAction("GL_OpnBal", "GL_OpnBal", new { errMsg });
            }

            List<OpenBal> OpnBal = Session["mydata"] as List<OpenBal>;
            //string sql = string.Format(" Select * from OpenBal ");
            //List<OpnBal> OpnBal = _pR_OpenBalanceService.SqlQueary(sql).ToList();
            //foreach (var item in OpnBal)
            //{
            //    item.Accode = _NewChartAppService.All().Where(x => x.Accode == item.Accode).Select(s => s.AcName).FirstOrDefault();
            //    if (item.OpenBal < 0)
            //    {
            //        item.YrCrBal = item.OpenBal * (-1);
            //        item.YrDrBal = 0;
            //    }
            //    else
            //    {
            //        item.YrDrBal = item.OpenBal;
            //        item.YrCrBal = 0;

            //    }
            //}

            if (OpnBal.Count == 0)
            {
                //string errMsg = "Data not found !!!";
                //return RedirectToAction("GLOpenBal", "GLOpenBal", new { errMsg });
                return new Rotativa.ViewAsPdf("~/Views/GL_OpenBal/GLOpenBalPreviewPdf.cshtml", OpnBal)
                {
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }
            else
            {
                return new Rotativa.ViewAsPdf("~/Views/GL_OpenBal/GLOpenBalPreviewPdf.cshtml", OpnBal)
                {
                    CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\""
                };
            }

        }
    }
}