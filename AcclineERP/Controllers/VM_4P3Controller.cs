using AcclineERP.Models;
using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    public class VM_4P3Controller : Controller
    {
        private readonly IItemInfoAppService _itemInfoService;
        private readonly IUnitAppService _unitService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISysSetAppService _sysSetService;
        public VM_4P3Controller(IItemInfoAppService _itemInfoService, IUnitAppService _unitService, ITransactionLogAppService _transactionLogService, ISysSetAppService _sysSetService)
        {
            this._itemInfoService = _itemInfoService;
            this._unitService = _unitService;
            this._transactionLogService = _transactionLogService;
            this._sysSetService = _sysSetService;
        }
        public ActionResult Vat4P3(string errMsg)
        {
            ViewBag.OutItemCode = new SelectList(_itemInfoService.All().ToList().Where(x => x.ItemType == 1).OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            ViewBag.RPMItemCode = new SelectList(_itemInfoService.All().ToList().Where(x => x.ItemType == 3 || x.ItemType == 4).OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            ViewBag.CostId = LoadDropDown.LoadCostFactor();
            ViewBag.DeclarationNo = GetDeclarationNo();
            return View();
        }

        [HttpPost]
        public ActionResult SaveVat4P3(Vat4P3 Vat4P3, List<CM_Ingredient> CMIngredient, List<CM_CostExp> CMCostExp)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("CostSheet_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                #region For VAT VM_4P3

                VM_4P3VM VM_4P3VM = new VM_4P3VM();
                VM_4P3VM.VM_4P3 = Vat4P3;                
                VM_4P3VM.CM_CostExp = CMCostExp;
                VM_4P3VM.CM_Ingredient = CMIngredient;
               
                string content = "An error occured during the save.";
                var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string jsonCov = JsonConvert.SerializeObject(VM_4P3VM, Formatting.Indented, serSettings);
                HttpResponseMessage response = GlobalVariabls.VatApiClient.PostAsJsonAsync("VM_4P3/PostVM_4P3", VM_4P3VM).Result;
                content = response.StatusCode.ToString();

                if (content == "OK")
                {
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Vat 4.3", "Save", Vat4P3.DeclarationNo, Session["UserName"].ToString());
                    return Json(new { Msg = "1", returnValue = "4.3;" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetItemByItemCode(string Item)
        {

            if (!string.IsNullOrEmpty(Item))
            {
                var data = _itemInfoService.All().Where(x => x.ItemCode == Item).ToList();
                var ItemUnit = (from i in _itemInfoService.All().ToList()
                                join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                join ud in _unitService.All().ToList() on i.DetUnitCode equals ud.UnitCode
                                let DUnitName = ud.UnitName
                                where i.ItemCode == Item
                                select new { u.UnitName, DUnitName, i.PackSize }).FirstOrDefault();
                string UnitName = ItemUnit.UnitName.ToString();
                string PackSize = ItemUnit.PackSize.ToString();
                return Json(new { item = Item, UnitName = UnitName, PackSize = PackSize, DUnitName = ItemUnit.DUnitName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDeclarationNoView()
        {
            int DeclaNo = GetDeclarationNo();
            return Json(DeclaNo, JsonRequestBehavior.AllowGet);
        }

        public int GetDeclarationNo() 
        {
            HttpResponseMessage response = GlobalVariabls.VatApiClient.GetAsync("VM_4P3/GetDeclarationNo").Result;
            int returnValue = response.Content.ReadAsAsync<int>().Result;
            return returnValue;
        }
	}
}