
using App.Domain;
using Application.Interfaces;
using Newtonsoft.Json;
using AcclineERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{

    public class RateChartController : Controller
    {
        private readonly ISubsidiaryInfoAppService _ISubsidiaryInfoService;
        private readonly ISubsidiaryTypeAppService ISubsidiaryTypeService;
        private readonly IItemInfoAppService _IItemInfoAppService;
        private readonly IRateChartAppService _RateChartService;
        private readonly IFYDDAppService _FYDDService;
        private readonly IProjInfoAppService _ProjInfoService;
        private ITransactionLogAppService _transactionLogService;
        public RateChartController(ISubsidiaryTypeAppService ISubsidiaryTypeService, ITransactionLogAppService _transactionLogService, IItemInfoAppService _IItemInfoAppService, ISubsidiaryInfoAppService _ISubsidiaryInfoService, IRateChartAppService _RateChartService, IFYDDAppService _FYDDService, IProjInfoAppService _ProjInfoService)
        {
            this._RateChartService = _RateChartService;
            this._FYDDService = _FYDDService;
            this._ProjInfoService = _ProjInfoService;
            this._ISubsidiaryInfoService = _ISubsidiaryInfoService;
            this._IItemInfoAppService = _IItemInfoAppService;
            this._transactionLogService = _transactionLogService;
            this.ISubsidiaryTypeService = ISubsidiaryTypeService;
        }

        public ActionResult RateChart(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.CustGroup = LoadDropDown.LoadSubGrpDDL();
                ViewBag.CustCode = LoadDropDown.LoadSubsidiary(_ISubsidiaryInfoService);
                ViewBag.ItemCode = LoadDropDown.LoadItem(_IItemInfoAppService);
                ViewBag.CustType = new SelectList(ISubsidiaryTypeService.All().ToList(), "TypeCode", "SubType");
                var Fydd = _FYDDService.All().FirstOrDefault(s => s.FinYear == Session["FinYear"].ToString());
                ViewBag.FyddFDate = Fydd.FYDF;
                ViewBag.FyddTDate = Fydd.FYDT;
                ViewBag.Message = errMsg;
                return View();

            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        [HttpPost]
        public ActionResult Save(RateChart RateChart)
        {
            //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            //if (!rUser.HasPermission(""))
            //{
            //    return Json("X", JsonRequestBehavior.AllowGet);
            //}

            string eCode = "";
            using (var transaction = new TransactionScope())
            {

                try
                {
                    // query   for check existing  data and then if  null go to transaction

                    var objRateChart = _RateChartService.All().ToList()
                                    .Where(x => x.ItemCode == RateChart.ItemCode && x.CustCode == RateChart.CustCode).ToList();

                    //if (objRateChart.Count == 0)

                    if (objRateChart.Count == 0)
                    {

                        RateChart SaveRateChart = new RateChart();

                        Misclns.CopyPropertiesTo(RateChart, SaveRateChart);
                        _RateChartService.Add(SaveRateChart);
                        _RateChartService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "RateChart", "Save",
                           DateTime.Now.ToString(), Session["UserName"].ToString());

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






        public ActionResult GetAllDataForRateChart(string CustCode, string ItemCode)
        {
            List<RateChart> OBInfoList = new List<RateChart>();
            try
            {


                if (ItemCode == "")
                {
                    OBInfoList = _RateChartService.All().Where(s => s.CustCode == CustCode).ToList();
                }
                else
                {
                    OBInfoList = _RateChartService.All().Where(s => s.CustCode == CustCode && s.ItemCode == ItemCode).ToList();
                }

                foreach (var item in OBInfoList)
                {
                    //item.CustGroup = "No Group";
                    var iCode = _IItemInfoAppService.All().Where(y => y.ItemCode == item.ItemCode).Select(x => x.ItemName).FirstOrDefault();
                    if (iCode != null) { item.ItemCode = iCode; }
                    var sCode = _ISubsidiaryInfoService.All().Where(y => y.SubCode == item.CustCode).Select(x => x.SubName).FirstOrDefault();
                    if (sCode != null) { item.CustCode = sCode; }
                }
                return Json(new { data = OBInfoList }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetItemForEdit(int RateChartId)
        {
            try
            {
                RateChart rateChart = new RateChart();

                var RateChartOb = _RateChartService.All().ToList().FirstOrDefault(x => x.RateChartId == RateChartId);

                if (RateChartOb != null)
                {
                    rateChart.RateChartId = RateChartOb.RateChartId;
                    rateChart.CustCode = RateChartOb.CustCode;
                    rateChart.ItemCode = RateChartOb.ItemCode;
                    rateChart.CommPerc = RateChartOb.CommPerc;
                    rateChart.ItemRate = RateChartOb.ItemRate;
                    rateChart.RC = RateChartOb.RC;
                    rateChart.CustType = RateChartOb.CustType;
                    rateChart.CustGroup = RateChartOb.CustGroup;


                }
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(rateChart, Formatting.Indented, serializerSettings);
                if (rateChart != null && rateChart.RateChartId > 0)
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
        public ActionResult UpdateRateChart(RateChart RateChart)
        {

            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("GL_OpenBal_Update"))
            {
                return Json("U", JsonRequestBehavior.AllowGet);
            }
            string eCode = "";
            //    Checking and  operation Data for save database           
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RateChart UpRateChart = new RateChart();

                    UpRateChart = _RateChartService.All().ToList().FirstOrDefault(x => x.RateChartId == RateChart.RateChartId);

                    if (UpRateChart != null)
                    {

                        Misclns.CopyPropertiesTo(RateChart, UpRateChart);
                        _RateChartService.Update(UpRateChart);
                        _RateChartService.Save();
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
        public ActionResult DeleteRateChart(int Id)
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
                    var IsExist = _RateChartService.All().ToList().FirstOrDefault(x => x.RateChartId == Id);
                    if (IsExist != null)
                    {
                        _RateChartService.Delete(IsExist);
                        _RateChartService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "RateChart", "Delete",
                         Session["UserName"].ToString(), Session["FinYear"].ToString());
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



        public ActionResult GetItemByCustCode(string CustCode)
        {
            if (CustCode != "")
            {
                var itemcodelist = _RateChartService.All().Where(s => s.CustCode == CustCode).ToList();
                List<SelectListItem> itemSelectlist = new List<SelectListItem>();

                foreach (var item in itemcodelist)
                {
                    var itemname = _IItemInfoAppService.All().Where(x => x.ItemCode == item.ItemCode).Select(y => y.ItemName).FirstOrDefault();
                    SelectListItem selListItem = new SelectListItem() { Value = item.ItemCode, Text = itemname };
                    itemSelectlist.Add(selListItem);
                }
                itemSelectlist.Insert(0, new SelectListItem() { Value = "", Text = "All" });
                var data = itemSelectlist;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

    }
}
