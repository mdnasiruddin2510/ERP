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
    public class VM_VDS_ReceiptController : Controller
    {
        private readonly ITransactionLogAppService _transactionLogService;
        public VM_VDS_ReceiptController(ITransactionLogAppService _transactionLogService)
        {
            this._transactionLogService = _transactionLogService;
        }

        // GET: VM_VDS_Receipt
        public ActionResult VM_VDS_Receipt() 
        {
            if (Session["UserID"] != null)
            {
                ViewBag.AccountCode = LoadDropDown.LoadVM_TreasuryAc();
                ViewBag.ReceiptList = GetVDS_Receipt();
                ViewBag.VDS_ReceiptNo = GetNewVDS_Receipt();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult GetNewVDS_ReceiptbyView()
        {
            int VDS_ReceiptNo = GetNewVDS_Receipt();
            return Json(VDS_ReceiptNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewVDS_Receipt()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_VDS_Receipt/GetNewVDS_Receipt").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_VDS_Receipt> GetVDS_Receipt()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_VDS_Receipt/GetVM_VDS_Receipt", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_VDS_Receipt> VDS_ReceiptList = js.Deserialize<IEnumerable<VM_VDS_Receipt>>(JsonResponse);
            return VDS_ReceiptList;
        }


        public ActionResult GetVDS_ReceiptByVDS_ReceiptNo(string VDS_ReceiptNo)
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_VDS_Receipt/GetVM_VDS_Receipt?VDS_ReceiptNo=" + VDS_ReceiptNo.ToString()).Result;
            VM_VDS_Receipt VM_VDS_Receipt = response.Content.ReadAsAsync<VM_VDS_Receipt>().Result;
            return Json(VM_VDS_Receipt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveVDS_Receipt(VM_VDS_Receipt VDS_Receipt)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VDSReceipt_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(VDS_Receipt, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_VDS_Receipt/PostVM_VDS_Receipt", VDS_Receipt).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Save", VDS_Receipt.VDS_ReceiptNo, Session["UserName"].ToString());
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
        public ActionResult UpdateVDS_Receipt(VM_VDS_Receipt VDS_Receipt)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("VDSReceipt_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(VDS_Receipt, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_VDS_Receipt/PutVM_VDS_Receipt", VDS_Receipt).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VDS Payment", "Update", VDS_Receipt.VDS_ReceiptNo, Session["UserName"].ToString());
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