using App.Domain;
using Application.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    //off when user permission
    // [Authorize(Roles = "Admin, User")]
    public class ItemController : Controller
    {
        private readonly IUnitAppService _UnitService;
        private readonly IItemTypeAppService _ItemTypeService;
        private readonly IItemInfoAppService _ItemInfoService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly IItemListAppService _ItemListService;
        private readonly IvuItemInfoAppService _vuItemInfoAppService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly ISysSetAppService _sysSetService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly ILocationAppService _locationService;
        private readonly IBranchAppService _branchSerive;

        public ItemController(IUnitAppService _UnitService, IItemTypeAppService _ItemService,
            IItemInfoAppService _ItemInfoService, ITransactionLogAppService _transactionLogServic,
            ITransactionLogAppService _transactionLogService, IItemListAppService _ItemListService,
            IvuItemInfoAppService _vuItemInfoAppService, ICommonPVVMAppService _CommonVmService,
            ISysSetAppService _sysSetService, ICostLedgerAppService CostLedgerAppService,
            ILocationAppService _locationService, IBranchAppService _branchSerive)
        {
            this._UnitService = _UnitService;
            this._ItemTypeService = _ItemService;
            this._ItemInfoService = _ItemInfoService;
            this._transactionLogService = _transactionLogService;
            this._ItemListService = _ItemListService;
            this._vuItemInfoAppService = _vuItemInfoAppService;
            this._CommonVmService = _CommonVmService;
            this._sysSetService = _sysSetService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._locationService = _locationService;
            this._branchSerive = _branchSerive;
        }


        public ActionResult Item(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ItemCode = GenerateItemCode();
                var item = _ItemInfoService.All().ToList().LastOrDefault();
                ViewBag.ItemType = new SelectList(_ItemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.UnitCode = new SelectList(_UnitService.All().ToList(), "UnitCode", "UnitName");
                ViewBag.DetUnitCode = new SelectList(_UnitService.All().ToList(), "UnitCode", "UnitName");
                ViewBag.PackItemCode = new SelectList(_ItemInfoService.All().ToList(), "ItemCode", "ItemName");
                ViewBag.AltItemCode = new SelectList(_ItemInfoService.All().ToList(), "ItemCode", "ItemName");
                ViewBag.Items = GetItemAll();
                ViewBag.Group = LoadDropDown.LoadGroupInf(_CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupInfo(_CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadAllSSGroupInfo(_CommonVmService);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintVAT = sysSet.MaintVAT;
                Session["MaintVAT"] = sysSet.MaintVAT;
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        public ActionResult GetUnitCodeByItemCode(string itemCode)
        {
            if (!string.IsNullOrEmpty(itemCode) && itemCode.Trim() != "Select FG Item")
            {
                return Json(_ItemTypeService.All().ToList().FirstOrDefault(x => x.ItemTypeCode == Convert.ToInt32(itemCode.Trim())).ItemTypeCode, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetItemById(string ItemCode)
        {
            var item = _ItemInfoService.All().ToList().FirstOrDefault(x => x.ItemCode == ItemCode);
            ItemInfo _objitem = new ItemInfo();
            _objitem = item;
            //_objitem.ItemCode = item.ItemCode;
            //_objitem.ItemName = item.ItemName;
            //_objitem.ItemType = item.ItemType;
            //_objitem.UnitCode = item.UnitCode;
            //_objitem.RetailPrice = item.RetailPrice;
            //_objitem.DetUnitCode = item.UnitCode;
            //_objitem.PartNo = item.PartNo;
            //_objitem.Price = item.Price;
            //_objitem.Ratio = item.Ratio;
            //_objitem.PackSize = item.PackSize;
            //_objitem.PackItemCode = item.PackItemCode;
            //_objitem.AltItemCode = item.AltItemCode;
            //_objitem.Prod_Ser = item.Prod_Ser;
            //_objitem.HSCode = item.HSCode;
            //_objitem.ROLevel = item.ROLevel;

            HttpResponseMessage response = GlobalVariabls.WebApiClient.GetAsync("ItemMaps?ItemCode=" + item.ItemCode.ToString()).Result;
            ItemMap itemM = response.Content.ReadAsAsync<ItemMap>().Result;
            _objitem.Id = itemM.Id;
            _objitem.GroupID = itemM.GroupID;
            _objitem.SGroupID = itemM.SGroupID;
            _objitem.SSGroupID = itemM.SSGroupID;

            return Json(_objitem, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveItem(ItemInfo item)
        {
            //if (ModelState.IsValid)
            //{
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Item_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var items = _ItemInfoService.All().ToList().FirstOrDefault(x => x.ItemCode == item.ItemCode && x.ItemName == item.ItemName);
                    if (items == null)
                    {

                        _ItemInfoService.Add(item);
                        ItemMap iM = new ItemMap();
                        iM.ItemTypeCode = (int)item.ItemType;
                        iM.GroupID = item.GroupID;
                        iM.SGroupID = item.SGroupID;
                        iM.SSGroupID = item.SSGroupID;
                        iM.ItemCode = item.ItemCode;

                        var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                        string json = JsonConvert.SerializeObject(iM, Formatting.Indented, serializerSettings);

                        HttpResponseMessage response = GlobalVariabls.WebApiClient.PostAsJsonAsync("ItemMaps/PostItemMap", iM).Result;
                        _ItemInfoService.Save();
                        TransactionLogService.SaveTransactionLog(_transactionLogService, "Item", "Save", item.ItemCode, Session["UserName"].ToString());

                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Item already exists!!!", JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                //}
                //else
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}

            }
        }


        public List<ItemInfo> GetItemAll()
        {
            List<Unit> unit = _UnitService.All().ToList();
            List<ItemType> itemType = _ItemTypeService.All().ToList();
            List<ItemInfo> iteminfo = _ItemInfoService.All().ToList();

            var itemInfo = from ii in iteminfo
                           join i in itemType on ii.ItemType equals i.ItemTypeCode
                           join u in unit on ii.UnitCode equals u.UnitCode
                           select new
                           {
                               ItemCode = ii.ItemCode,
                               //PartNo = ii.PartNo,
                               ItemName = ii.ItemName,
                               ItemType = i.ItemTypeName,
                               Unit = u.UnitName
                           };
            List<ItemInfo> sInfos = new List<ItemInfo>();
            foreach (var item in itemInfo)
            {
                ItemInfo sInfo = new ItemInfo();
                sInfo.ItemCode = item.ItemCode;
                //sInfo.PartNo = item.PartNo;
                sInfo.ItemName = item.ItemName;
                sInfo.ItemTypeName = item.ItemType;
                sInfo.UnitCode = item.Unit;
                sInfos.Add(sInfo);
            }
            return sInfos;
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemInfo item)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Item_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    _ItemInfoService.Update(item);
                    _ItemInfoService.Save();
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Item", "Update", item.ItemCode, Session["UserName"].ToString());

                    ItemMap ItemMap = new ItemMap();
                    ItemMap.Id = item.Id;
                    ItemMap.ItemTypeCode = (int)item.ItemType;
                    ItemMap.GroupID = item.GroupID;
                    ItemMap.SGroupID = item.SGroupID;
                    ItemMap.SSGroupID = item.SSGroupID;
                    ItemMap.ItemCode = item.ItemCode;

                    var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                    string json = JsonConvert.SerializeObject(ItemMap, Formatting.Indented, serializerSettings);

                    HttpResponseMessage response = GlobalVariabls.WebApiClient.PutAsJsonAsync("ItemMaps/" + item.Id, ItemMap).Result;


                    transaction.Complete();
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }




        public ActionResult GetAllByItemCode(string itemCode)
        {
            ItemInfo item = new ItemInfo();
            item = _ItemInfoService.All().ToList().FirstOrDefault(x => x.ItemCode == itemCode.Trim());
            HttpResponseMessage response = GlobalVariabls.WebApiClient.GetAsync("ItemMaps?ItemCode=" + item.ItemCode.ToString()).Result;
            ItemMap itemM = response.Content.ReadAsAsync<ItemMap>().Result;

            item.Id = itemM.Id;
            item.GroupID = itemM.GroupID;
            item.SGroupID = itemM.SGroupID;
            item.SSGroupID = itemM.SSGroupID;

            return Json(item, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetItemCode()
        {
            return Json(GenerateItemCode(), JsonRequestBehavior.AllowGet);
        }
        public string GenerateItemCode()
        {
            string itemCode = "";
            var item = _ItemInfoService.All().ToList().LastOrDefault();
            if (item != null)
            {
                itemCode = (Convert.ToInt32(item.ItemCode) + 1).ToString().PadLeft(5, '0');
            }
            else
            {
                itemCode = "00001";
            }

            return itemCode;
        }



        [HttpPost]
        public ActionResult GetAllItems()
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Item_Preview"))
            {
                string errMsg = "P";
                return RedirectToAction("Item", "Item", new { errMsg });
            }
            string sql = string.Format(" EXEC AllItem ");
            List<ItemList> items = _ItemListService.SqlQueary(sql).ToList();
            ViewBag.PrintDate = DateTime.Now.ToShortDateString();
            return new Rotativa.ViewAsPdf("GetAllItems", items) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }

        public ActionResult SearchItem(string errMsg)
        {
            ViewBag.ItemCode = LoadDropDown.LoadItems(_ItemInfoService);
            ViewBag.Message = errMsg;
            return View();
        }

        public ActionResult GetSearchItem(string itemCode)
        {

            string sql = "";
            List<vuItemInfo> vuItemInfoList = new List<vuItemInfo>();
            if (itemCode != "")
            {
                if (Convert.ToBoolean(Session["MaintLot"]) == true)
                {
                    sql = string.Format("SELECT dbo.CurrentStock.ItemCode,dbo.ItemInfo.ItemName, dbo.Location.LocName, dbo.Branch.BranchName,");
                    sql += string.Format("dbo.CurrentStock.CurrQty  FROM  dbo.CurrentStock INNER JOIN dbo.ItemInfo ON dbo.CurrentStock.ItemCode = dbo.ItemInfo.ItemCode ");
                    sql += string.Format("INNER JOIN dbo.Location ON dbo.CurrentStock.LocCode = dbo.Location.LocCode INNER JOIN dbo.BranchLocMap ");
                    sql += string.Format("ON dbo.Location.LocCode = dbo.BranchLocMap.LocCode INNER JOIN dbo.Branch ON dbo.BranchLocMap.BranchCode = dbo.Branch.BranchCode where dbo.CurrentStock.ItemCode='" + itemCode + "' and CurrQty <> 0");
                    vuItemInfoList = _vuItemInfoAppService.SqlQueary(sql).ToList();

                }
                else
                {
                    var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == itemCode).ToList();
                    if (existCurrStoc.Count != 0)
                    {
                        var result = (from r in existCurrStoc group r by r.LocNo into g
                                      select new { LocNo = g.Key, TrDate = g.Max(a => a.TrDate) }).ToList(); 
                        foreach (var item in result)
                        {
                            var existCurrStock = CostLedgerAppService.All().Where(x => x.ItemCode == itemCode && x.TrDate == item.TrDate && x.LocNo == item.LocNo).FirstOrDefault();
                            vuItemInfo vuItemInfo = new vuItemInfo();
                            vuItemInfo.ItemCode = existCurrStock.ItemCode;
                            vuItemInfo.ItemName = (_ItemInfoService.All().Where(s => s.ItemCode == itemCode)
                                                                   .Select(c => c.ItemName)).FirstOrDefault();
                            vuItemInfo.LocName = (_locationService.All().Where(s => s.LocCode == item.LocNo)
                                                                   .Select(c => c.LocName)).FirstOrDefault();
                            vuItemInfo.BranchName = "";
                            vuItemInfo.CurrQty = existCurrStock.BalQty;
                            vuItemInfoList.Add(vuItemInfo);
                        }                       
                    }
                }
            }
            return Json(new { data = vuItemInfoList }, JsonRequestBehavior.AllowGet);

        }

    }
}
