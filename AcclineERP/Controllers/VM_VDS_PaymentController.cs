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
    public class VM_VDS_PaymentController : Controller
    {
        private readonly ITransactionLogAppService _transactionLogService;
        public VM_VDS_PaymentController(ITransactionLogAppService _transactionLogService)
        {
            this._transactionLogService = _transactionLogService;
        }

        // GET: VM_VDS_Payment
        public ActionResult VM_VDS_Payment()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.AccountCode = LoadDropDown.LoadVM_TreasuryAc(); 
                ViewBag.PaymentList = GetVDS_Payment();
                ViewBag.VDS_PaymentNo = GetNewVDS_Payment();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult GetNewVDS_PaymentbyView()
        {
            int VDS_PaymentNo = GetNewVDS_Payment();
            return Json(VDS_PaymentNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewVDS_Payment()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_VDS_Payment/GetNewVDS_Payment").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_VDS_Payment> GetVDS_Payment()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_VDS_Payment/GetVM_VDS_Payment", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_VDS_Payment> VDS_PaymentList = js.Deserialize<IEnumerable<VM_VDS_Payment>>(JsonResponse);
            return VDS_PaymentList;
        }


        public ActionResult GetVDS_PaymentByVDS_PaymentNo(string VDS_PaymentNo)
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_VDS_Payment/GetVM_VDS_Payment?VDS_PaymentNo=" + VDS_PaymentNo.ToString()).Result;
            VM_VDS_Payment VM_VDS_Payment = response.Content.ReadAsAsync<VM_VDS_Payment>().Result;
            return Json(VM_VDS_Payment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveVDS_Payment(VM_VDS_Payment VDS_Payment)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VDSPayment_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(VDS_Payment, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_VDS_Payment/PostVM_VDS_Payment", VDS_Payment).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Save", VDS_Payment.VDS_PaymentNo, Session["UserName"].ToString());
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
        public ActionResult UpdateVDS_Payment(VM_VDS_Payment VDS_Payment)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VDSPayment_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(VDS_Payment, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_VDS_Payment/PutVM_VDS_Payment", VDS_Payment).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Update", VDS_Payment.VDS_PaymentNo, Session["UserName"].ToString());
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