using AcclineERP.Models;
using App.Domain;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace AcclineERP.Controllers 
{
    public class VM_AdjustmentController : Controller
    {

        private ITransactionLogAppService _transactionLogService;
        public VM_AdjustmentController(ITransactionLogAppService _transactionLogService)
        {
            this._transactionLogService = _transactionLogService;
        }

        public ActionResult VM_Adjustment()
        {

            if (Session["UserID"] != null)
            {
                ViewBag.AdjHead = LoadDropDown.LoadVM_AdjHead();
                ViewBag.AdjustmentList = GetVM_Adjustment(); 
                ViewBag.VM_AdjustmentNo = GetNewVM_Adjustment();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }


        public ActionResult GetNewVM_AdjustmentbyView()
        {
            int VM_AdjustmentNo = GetNewVM_Adjustment();
            return Json(VM_AdjustmentNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewVM_Adjustment()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_Adjustment/GetNewVM_Adjustment").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_Adjustment> GetVM_Adjustment()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_Adjustment/GetVM_Adjustment", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_Adjustment> VM_AdjustmentList = js.Deserialize<IEnumerable<VM_Adjustment>>(JsonResponse);
            return VM_AdjustmentList;
        }


        public ActionResult GetVM_AdjustmentByAdjNo(string AdjNo)  
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_Adjustment/GetVM_Adjustment?AdjNo=" + AdjNo.ToString()).Result;
            VM_Adjustment VM_Adjustment = response.Content.ReadAsAsync<VM_Adjustment>().Result;
            return Json(VM_Adjustment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveVM_Adjustment(VM_Adjustment VM_Adjustment)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VMAdjustment_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(VM_Adjustment, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_Adjustment/PostVM_Adjustment", VM_Adjustment).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Save", VM_Adjustment.AdjNo, Session["UserName"].ToString());
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult UpdateVM_Adjustment(VM_Adjustment VM_Adjustment)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VMAdjustment_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(VM_Adjustment, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_Adjustment/PutVM_Adjustment", VM_Adjustment).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Update", VM_Adjustment.AdjNo, Session["UserName"].ToString());
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }


    }
}

