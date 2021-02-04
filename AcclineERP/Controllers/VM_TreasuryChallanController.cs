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
    public class VM_TreasuryChallanController : Controller
    {

        private ITransactionLogAppService _transactionLogService;
        public VM_TreasuryChallanController(
            ITransactionLogAppService _transactionLogService)
        {            
            this._transactionLogService = _transactionLogService;
        }


        public ActionResult VM_TreasuryChallan()
        {
            ViewBag.AccountCode = LoadDropDown.LoadVM_TreasuryAc();
            ViewBag.Bank = LoadDropDown.LoadBankInfo();
            ViewBag.TrDepList = GetVM_TrDep();
            ViewBag.TrDepNo = GetNewVM_TrDep(); 
            return View();
        }

        public ActionResult GetNewVM_TrDepbyView()
        {
            int TrDepNo = GetNewVM_TrDep();
            return Json(TrDepNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewVM_TrDep()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_TrDep/GetNewVM_TrDep").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_TrDep> GetVM_TrDep()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_TrDep/GetVM_TrDep", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_TrDep> TrDepList = js.Deserialize<IEnumerable<VM_TrDep>>(JsonResponse);
            return TrDepList;
        }


        public ActionResult GetVM_TrDepByTrDepNo(string TrDepNo)
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_TrDep/GetVM_TrDep?TrDepNo=" + TrDepNo.ToString()).Result;
            VM_TrDep VM_TrDep = response.Content.ReadAsAsync<VM_TrDep>().Result;
            return Json(VM_TrDep, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveVM_TrDep(VM_TrDep VM_TrDep)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("TreasuryChallan_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(VM_TrDep, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_TrDep/PostVM_TrDep", VM_TrDep).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VM Treasury Challan", "Save", VM_TrDep.TrDepNo, Session["UserName"].ToString());
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
        public ActionResult UpdateVM_TrDep(VM_TrDep VM_TrDep)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("TreasuryChallan_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(VM_TrDep, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_TrDep/PutVM_TrDep", VM_TrDep).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "VM Treasury Challan", "Update", VM_TrDep.TrDepNo, Session["UserName"].ToString());
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

