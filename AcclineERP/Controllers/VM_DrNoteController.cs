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
    public class VM_DrNoteController : Controller
    {
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISysSetAppService _sysSetService;
        public VM_DrNoteController(ITransactionLogAppService _transactionLogService, ISysSetAppService _sysSetService)
        {
            this._transactionLogService = _transactionLogService;
            this._sysSetService = _sysSetService;
        }
        // GET: VM_DrNote
        public ActionResult VM_DrNote()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.Reason = LoadDropDown.LoadDrNoteReason();
                ViewBag.NoteList = GetDrNote();
                ViewBag.DrNoteNo = GetNewDrNote();
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


        public ActionResult GetNewDrNotebyView()
        {
            int DrNoteNo = GetNewDrNote();
            return Json(DrNoteNo, JsonRequestBehavior.AllowGet);
        }

        public int GetNewDrNote()
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_DrNote/GetNewDrNote").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }


        public IEnumerable<VM_DrNote> GetDrNote()
        {
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/" + "VM_DrNote/GetVM_DrNote", Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            IEnumerable<VM_DrNote> NoteList = js.Deserialize<IEnumerable<VM_DrNote>>(JsonResponse);
            return NoteList;
        }


        public ActionResult GetDrNoteByDrNoteNo(string DrNoteNo)
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_DrNote/GetVM_DrNote?DrNoteNo=" + DrNoteNo.ToString()).Result;
            VM_DrNote VM_DrNote = response.Content.ReadAsAsync<VM_DrNote>().Result;

            string returnValue = "";
            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
            {
                string respse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetVM_6P8?TransNo=" + DrNoteNo.ToString(), Session["token"].ToString());
                JavaScriptSerializer js = new JavaScriptSerializer();
                returnValue = js.Deserialize<string>(respse);
            }

            return Json(new { VM_DrNote = VM_DrNote, returnValue = returnValue }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDrNote(VM_DrNote DrNote)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("DrNote_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(DrNote, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_DrNote/PostVM_DrNote", DrNote).Result;
                content = response.StatusCode.ToString();

                #region For VAT VM_6P8
                VM_6P8 VM_6P8 = new VM_6P8();
                string returnValue = "";
                if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                {
                    VM_6P8.SerialNo = 0;
                    VM_6P8.OrigChallanNo = DrNote.ChallanNo;
                    VM_6P8.OrigChallanDate = DateTime.Now;
                    VM_6P8.ChallanTime = new TimeSpan();
                    VM_6P8.ReturnFrom = "";
                    VM_6P8.ReturnFromAddr = "";
                    VM_6P8.ReturnFromBIN = "";
                    VM_6P8.ReturnTo = ""; // _subsidiaryInfoService.All().Where(s => s.SubCode == PurRet.CustCode).Select(s => s.SubName).FirstOrDefault();
                    VM_6P8.ReturnToAddr = "";
                    VM_6P8.ReturnToBIN = "";
                    VM_6P8.DrNoteNo = DrNote.DrNoteNo;
                    VM_6P8.DrNoteDate = DrNote.DrNoteDate;
                    VM_6P8.DrNoteTime = new TimeSpan();
                    VM_6P8.ItemName = ""; // _itemInfoService.All().Where(s => s.ItemCode == issuDetailsItem.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                    //VM_6P8.UnitIn = (from i in _itemInfoService.All().ToList()
                    //                 join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                    //                 where i.ItemCode == issuDetailsItem.ItemCode
                    //                 select u.UnitName).FirstOrDefault();
                    VM_6P8.ReturnQty = 0;
                    VM_6P8.UPriceIncVatSD = 0;
                    VM_6P8.TotalValue = 0;
                    VM_6P8.DeductAmount = 0;
                    VM_6P8.AmtInclVAT = 0;
                    VM_6P8.VATAmount = 0;
                    VM_6P8.SDAmount = 0;
                    VM_6P8.TotTaxAmt = 0;
                    VM_6P8.ItemCode = "";//issuDetailsItem.ItemCode;
                    VM_6P8.HeadingNo = ""; //_itemInfoService.All().Where(s => s.ItemCode == issuDetailsItem.ItemCode).Select(s => s.TaxHeadingNo).FirstOrDefault();
                    VM_6P8.HSCode = ""; //_itemInfoService.All().Where(s => s.ItemCode == issuDetailsItem.ItemCode).Select(s => s.HSCode).FirstOrDefault();
                    VM_6P8.ReturnReason = DrNote.Reason; // PurRet.Reason;
                    VM_6P8.ReturnedBy = ""; //PurRet.ApprBy;
                    VM_6P8.ReturnedDesig = "";

                    var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                    string json = JsonConvert.SerializeObject(VM_6P8, Formatting.Indented, serializerSettings);
                    var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_6p8", VM_6P8).Result;
                    returnValue = respse.Content.ReadAsAsync<string>().Result;

                }
                #endregion
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Dr Note", "Save", DrNote.DrNoteNo, Session["UserName"].ToString());
                    return Json(new { Msg = "1", returnValue = returnValue }, JsonRequestBehavior.AllowGet);
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
        public ActionResult UpdateDrNote(VM_DrNote DrNote)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("DrNote_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }

                string content = "An error occured during the update.";
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(DrNote, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.VatApiClient.PutAsJsonAsync("VM_DrNote/PutVM_DrNote", DrNote).Result;
                content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Dr Note", "Update", DrNote.DrNoteNo, Session["UserName"].ToString());
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