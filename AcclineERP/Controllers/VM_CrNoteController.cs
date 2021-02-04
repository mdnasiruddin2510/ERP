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
    public class VM_CrNoteController : Controller
    {

        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISysSetAppService _sysSetService;

        public VM_CrNoteController(ITransactionLogAppService _transactionLogService, ISysSetAppService _sysSetService)
        {
            this._transactionLogService = _transactionLogService;
            this._sysSetService = _sysSetService;
        }

        // GET: VM_CrNote
        public ActionResult VM_CrNote()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.Reason = LoadDropDown.LoadCrNoteReason();
                ViewBag.NoteList = GetCrNote();
                ViewBag.CrNoteNo = GetNewCrNote();
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintVAT = sysSet.MaintVAT;
                Session["MaintVAT"] = sysSet.MaintVAT;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public ActionResult GetNewCrNotebyView()
        {
            int CrNoteNo = GetNewCrNote();
            return Json(CrNoteNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewCrNote()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_CrNote/GetNewCrNote").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_CrNote> GetCrNote()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_CrNote/GetVM_CrNote", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_CrNote> NoteList = js.Deserialize<IEnumerable<VM_CrNote>>(JsonResponse);
            return NoteList;
        }


        public ActionResult GetCrNoteByCrNoteNo(string CrNoteNo)
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_CrNote/GetVM_CrNote?CrNoteNo=" + CrNoteNo.ToString()).Result;
            VM_CrNote VM_CrNote = response.Content.ReadAsAsync<VM_CrNote>().Result;


            string returnValue = "";
            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
            {
                string respse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetVM_6P7?TransNo=" + CrNoteNo.ToString(), Session["token"].ToString());
                JavaScriptSerializer js = new JavaScriptSerializer();
                returnValue = js.Deserialize<string>(respse);
            }

            return Json(new { VM_CrNote = VM_CrNote, returnValue = returnValue}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCrNote(VM_CrNote CrNote)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CrNote_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(CrNote, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_CrNote/PostVM_CrNote", CrNote).Result;
                content = response.StatusCode.ToString();
                

                #region For VAT VM_6p7
                VM_6P7 VM6p7 = new VM_6P7();         
                string returnValue = "";
                if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                {
                    VM6p7.SRID = 0;
                    VM6p7.OrigChallanNo = CrNote.ChallanNo;
                    VM6p7.OrigChallanDate = DateTime.Now;
                    VM6p7.UnitIn = "";
                    VM6p7.ChallanTime = new TimeSpan();
                    VM6p7.ReturnFrom = "";
                    VM6p7.ReturnFromAddr = "";
                    VM6p7.ReturnFromBIN = "";
                    VM6p7.ReturnTo = "";
                    VM6p7.ReturnToAddr = "";
                    VM6p7.ReturnToBIN = "";
                    VM6p7.CrNoteNo = CrNote.CrNoteNo;
                    VM6p7.CrNoteDate = CrNote.CrNoteDate;
                    VM6p7.CrNoteTime = new TimeSpan();
                    VM6p7.SerialNo = 0;
                    VM6p7.ItemCode = "";
                    VM6p7.ItemName = "";
                    VM6p7.ReturnQty = 0;
                    VM6p7.UPriceIncVatSD = 0;
                    VM6p7.DeductAmount = 0;
                    VM6p7.TotalValue = 0;
                    VM6p7.AmtInclVAT = 0;
                    VM6p7.VATAmount = 0;
                    VM6p7.SDAmount = 0;
                    VM6p7.TotTaxAmt = 0;
                    VM6p7.HeadingNo = "";
                    VM6p7.HSCode = "";
                    VM6p7.ReturnReason = CrNote.Reason;
                    VM6p7.ReceivedBy = "";
                    VM6p7.ReceivedDesig = "";                
                    var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                    string json = JsonConvert.SerializeObject(VM6p7, Formatting.Indented, serializerSettings);
                    var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_6p7", VM6p7).Result;
                    returnValue = respse.Content.ReadAsAsync<string>().Result;
                }
                
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Cr Note", "Save", CrNote.CrNoteNo, Session["UserName"].ToString());
                    return Json(new { Msg = "1", returnValue = returnValue }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                #endregion



            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult UpdateCrNote(VM_CrNote CrNote)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CrNote_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(CrNote, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_CrNote/PutVM_CrNote", CrNote).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Cr Note", "Update", CrNote.CrNoteNo, Session["UserName"].ToString());
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