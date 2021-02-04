using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AcclineERP.Models;
using System.Globalization;
using System.Threading;
using System.Transactions;
using System.Diagnostics;
using Spire.Pdf;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data;

namespace AcclineERP.Controllers
{
    public class SalesController : Controller
    {
        private readonly ILocationAppService _locationService;
        private readonly IItemTypeAppService _itemTypeService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly IItemInfoAppService _itemInfoService;
        private readonly ICurrentStockAppService _currentStockService;
        private readonly ICommonPVVMAppService _CommonVmService;
        private readonly IItemWiseDisVatAppService _ItemWiseDisVatService;
        private readonly ISalesMainAppService _SalesMainService;
        private readonly ISalesDetailAppService _SaleDetailService;
        private readonly IIssueMainAppService _IssueMainService;
        private readonly IIssueDetailsAppService _IssueDetailService;
        private readonly IEmployeeFuncAppService _EmpFuncService;
        private readonly IEmployeeAppService _EmployeeService;
        private readonly IIssueMainAppService _issueMainService;
        private readonly IJarnalVoucherAppService _jarnalVoucherService;
        private readonly ISysSetAppService _sysSetService;
        private readonly IVchrSetAppService _vchrSetService;
        private readonly ILotDTAppService _lotDtService;
        private readonly ICostLedgerAppService CostLedgerAppService;
        private readonly ISubsidiaryExtAppService _subsidiaryInfoExtService;
        private readonly IDefACAppService _defACService;
        private readonly IUnitAppService _unitService;
        private readonly IPVchrMainAppService _pVchrmainService;
        private readonly IRateChartAppService _RateChartService;
        private readonly IPackingListAppService _PackingListService;
        private readonly ITransactionLogAppService _transactionLogService;
        public SalesController(ILocationAppService _locationService, IItemTypeAppService _itemTypeService,
             IPackingListAppService _PackingListService,
            ISubsidiaryInfoAppService _subsidiaryInfoService, IItemInfoAppService _itemInfoService,
            ICurrentStockAppService _currentStockService, ICommonPVVMAppService _CommonVmService,
            IItemWiseDisVatAppService _ItemWiseDisVatService, ISalesMainAppService _SalesMainService,
            ISalesDetailAppService _SaleDetailService, IIssueMainAppService _IssueMainService,
            IIssueDetailsAppService _IssueDetailService, IEmployeeFuncAppService _EmpFuncService,
            IEmployeeAppService _EmployeeService, IIssueMainAppService _issueMainService,
            IJarnalVoucherAppService _jarnalVoucherService, ISysSetAppService _sysSetService,
            IVchrSetAppService _vchrSetService, ILotDTAppService _lotDtService, ICostLedgerAppService CostLedgerAppService,
            ISubsidiaryExtAppService _subsidiaryInfoExtService, IDefACAppService _defACService,
            IUnitAppService _unitService, IPVchrMainAppService _pVchrmainService, IRateChartAppService _RateChartService,
            ITransactionLogAppService _transactionLogService)
        {
            this._PackingListService = _PackingListService;
            this._locationService = _locationService;
            this._itemTypeService = _itemTypeService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._itemInfoService = _itemInfoService;
            this._currentStockService = _currentStockService;
            this._CommonVmService = _CommonVmService;
            this._ItemWiseDisVatService = _ItemWiseDisVatService;
            this._SalesMainService = _SalesMainService;
            this._SaleDetailService = _SaleDetailService;
            this._IssueMainService = _IssueMainService;
            this._IssueDetailService = _IssueDetailService;
            this._EmpFuncService = _EmpFuncService;
            this._EmployeeService = _EmployeeService;
            this._issueMainService = _issueMainService;
            this._jarnalVoucherService = _jarnalVoucherService;
            this._sysSetService = _sysSetService;
            this._vchrSetService = _vchrSetService;
            this._lotDtService = _lotDtService;
            this.CostLedgerAppService = CostLedgerAppService;
            this._subsidiaryInfoExtService = _subsidiaryInfoExtService;
            this._defACService = _defACService;
            this._unitService = _unitService;
            this._RateChartService = _RateChartService;
            this._pVchrmainService = _pVchrmainService;
            this._transactionLogService = _transactionLogService;
        }
        public AcclineERPContext db = new AcclineERPContext();
        public class retvalpro
        {
            public string ARAC { get; set; }
        }
        public ActionResult Sales(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                string branchCode = Session["BranchCode"].ToString();
                ViewBag.IssueNo = GenerateIssueNo(branchCode);
                ViewBag.LocCode = new SelectList(_locationService.All().Where(s => s.LocCode != "900").ToList(), "LocCode", "LocName");
                ViewBag.CustCode = new SelectList(_subsidiaryInfoService.All().ToList().Where(x => x.SubType == "1").OrderBy(x => x.SubName), "SubCode", "SubName");
                ViewBag.ItemType = new SelectList(_itemTypeService.All().ToList(), "ItemTypeCode", "ItemTypeName");
                ViewBag.Group = LoadDropDown.LoadGroupInfoByItemType("", _CommonVmService);
                ViewBag.SubGroup = LoadDropDown.LoadSGroupByGroupId("", "", _CommonVmService);
                ViewBag.SubSubGroup = LoadDropDown.LoadSSGroupInfo("", "", "", _CommonVmService);
                ViewBag.ItemCode = LoadDropDown.LoadItemBySSGroupID("", "", "", "", _itemInfoService, _CommonVmService);
                ViewBag.GLProvition = Count("AR");
                ViewBag.GLEntries = CountEntries("AR", DateTime.Now);
                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                ViewBag.MaintJob = sysSet.MaintJob;
                Session["MaintLot"] = sysSet.MaintLot;
                Session["RateChart"] = sysSet.RateChart;
                ViewBag.MaintLot = sysSet.MaintLot;
                ViewBag.OnlyVAT = sysSet.OnlyVAT;
                Session["MaintVAT"] = sysSet.MaintVAT;
                ViewBag.MaintVAT = sysSet.MaintVAT;
                ViewBag.MakeChallanAuto = sysSet.MakeChallanAuto;
                ViewBag.JobNo = LoadDropDown.LoadJobInfo();
                var VchrConv = _vchrSetService.All().ToList().FirstOrDefault().VchrConv;
                Session["VchrConv"] = VchrConv;
                ViewBag.InvNo = GenerateInvNo("01");
                #region For item Filtering option
                ViewBag.NoGrp = sysSet.NoGrp;
                ViewBag.OnlyGrp = sysSet.OnlyGrp;
                ViewBag.GrpAndSubGrp = sysSet.GrpAndSubGrp;
                ViewBag.SubSubGrp = sysSet.SubSubGrp;
                #endregion
                ////For Packing List
                ViewBag.MaintPacking = sysSet.MaintPacking;
                ViewBag.PL_Item = LoadDropDown.LoadPL_ItemList();
                ViewBag.PackItemCode = LoadDropDown.LoadPL_PackItemList();
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }

        }

        public ActionResult GettSaleNo(DateTime SaleDate)
        {
            var tSaleNo = Session["UserID"].ToString() + SaleDate.ToString("ddMMyy");
            return Json(tSaleNo, JsonRequestBehavior.AllowGet);
        }

        #region For Packet List

        public static List<PackItemVM> PacketWiseItemList = new List<PackItemVM>();
        public ActionResult LoadDropdownItem(string items)
        {
            try
            {
                //string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems?SaleNo=" + SaleNo.ToString(), Session["token"].ToString());
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems", Session["token"].ToString());
                // Session["Response_tmpAddedItems"]=JsonResponse;
                JavaScriptSerializer js = new JavaScriptSerializer();
                IEnumerable<tempAddedItem> itemList = js.Deserialize<IEnumerable<tempAddedItem>>(JsonResponse);
                var tempAddedItemList = itemList.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                var packlistitems = new SelectList(tempAddedItemList, "ItemCode", "ItemName");
                return Json(packlistitems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("LogOff", "SecUserLogin");
            }
        }
        public ActionResult LoadDropdownItem_pack(string items)
        {
            try
            {
                //string JsonResponse;
                //if (Session["Response_tmpAddedItems"] != null)
                //{
                //    JsonResponse = Session["Response_tmpAddedItems"].ToString();
                //} 
                //else
                //{
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems", Session["token"].ToString());

                // }

                JavaScriptSerializer js = new JavaScriptSerializer();
                IEnumerable<tempAddedItem> itemList = js.Deserialize<IEnumerable<tempAddedItem>>(JsonResponse);
                Dictionary<string, string> tempAddedItemList_pack = new Dictionary<string, string>();
                foreach (var item in itemList)
                {
                    var packcode = _itemInfoService.All().Where(x => x.ItemCode == item.ItemCode).Select(x => x.PackItemCode).FirstOrDefault();
                    var packname = _itemInfoService.All().Where(x => x.ItemCode == packcode).Select(x => x.ItemName).FirstOrDefault();
                    if (!tempAddedItemList_pack.Any(s => s.Key == packcode))
                    {
                        tempAddedItemList_pack.Add(packcode, packname);
                    }
                }
                var packlistitems_pack = new SelectList(tempAddedItemList_pack, "key", "value");
                return Json(packlistitems_pack, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("LogOff", "SecUserLogin");
            }

            //var item = (from uc in db.BankInfo select new { BankCode = uc.BankCode, BankName = uc.BankName }).ToList();
            //return new SelectList(item.OrderBy(x => x.BankCode).ToList(), "BankName", "BankName");


        }

        public ActionResult Edit_PakageList(int id)
        {

            PackItemVM PackItem = new PackItemVM();
            foreach (var item in PacketWiseItemList)
            {
                if (item.ID == id)
                {
                    PackItem = item;
                    break;
                }
            }
            return Json(PackItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add_PakageList(PackItemVM PackItem)
        {

            PackItem.ID = PacketWiseItemList.Count() + 1;
            PacketWiseItemList.Add(PackItem);

            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update_PakageList(PackItemVM PackItem)
        {


            var index = 0;
            foreach (var Item in PacketWiseItemList)
            {
                if (Item.ID == PackItem.ID)
                {
                    index = PacketWiseItemList.IndexOf(Item);
                }
            }
            PacketWiseItemList.RemoveAt(index);
            PacketWiseItemList.Add(PackItem);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /* This Action has 3 part;
         * part1:(initial) :: 
         * define  how many individual  row in packet list generate from the  data of  tempAdddedTable 
         * and  packet is added to PacketWiseItemList(GLOBAL LIST) using GetAllFeatureforSinglePacket().
         * 
         * GetAllFeatureforSinglePacket make a row  with itemcode ,packete no , quantity
         */

        public ActionResult GetDataForPackListPV(string stage, int deleteCode, PackItemVM AddPackItem, string SaleNo)
        {


            // part1: (initial)

            if (stage == "initial")
            {
                var tempAddedItemList = new List<tempAddedItem>();
                //string JsonResponse;
                //if (Session["Response_tmpAddedItems"] != null)
                //{
                //    JsonResponse = Session["Response_tmpAddedItems"].ToString();
                //}
                //else
                //{
                //string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems", Session["token"].ToString());
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems?SaleNo=" + SaleNo.ToString(), Session["token"].ToString());

                //}
                JavaScriptSerializer js = new JavaScriptSerializer();
                IEnumerable<tempAddedItem> itemList = js.Deserialize<IEnumerable<tempAddedItem>>(JsonResponse);
                tempAddedItemList = itemList.ToList();

                PacketWiseItemList.Clear();
                foreach (var item in tempAddedItemList)
                {
                    var packItemCode = _itemInfoService.All().Where(x => x.ItemCode == item.ItemCode).Select(x => x.PackItemCode).FirstOrDefault();
                    var psize = _itemInfoService.All().Where(x => x.ItemCode == packItemCode).Select(x => x.PackSize).FirstOrDefault();
                    decimal packsize = (decimal)psize;
                    if (item.Qty >= packsize && packsize != 0)
                    {
                        var n = item.Qty / packsize;

                        PacketWiseItemList.Add(GetAllFeatureforSinglePacket(item.ItemCode, n, 1)); //GetAllFeatureforSinglePacket(ItemCode,no of packet,Qty)//first with n>1
                        var remaining_Qty = item.Qty % packsize;
                        if (remaining_Qty != 0)
                        {
                            PacketWiseItemList.Add(GetAllFeatureforSinglePacket(item.ItemCode, 1, remaining_Qty)); //second with n=1
                        }
                    }
                    else //quntity is smaller then pack size
                    {
                        PacketWiseItemList.Add(GetAllFeatureforSinglePacket(item.ItemCode, 1, item.Qty));
                    }
                }
                return Json(new { data = PacketWiseItemList }, JsonRequestBehavior.AllowGet);
            }



            //part 2 :delete from  packet List

            if (stage == "delete" && PacketWiseItemList.Count != 0) //delete Action in packdataTable
            {
                var index = 0;
                foreach (var Item in PacketWiseItemList)
                {
                    if (Item.ID == deleteCode)
                    {
                        index = PacketWiseItemList.IndexOf(Item);
                    }
                }

                PacketWiseItemList.RemoveAt(index);
                return Json(new { data = PacketWiseItemList }, JsonRequestBehavior.AllowGet);
            }


 //part 3 :update packet List
            else
            {
                return Json(new { data = PacketWiseItemList }, JsonRequestBehavior.AllowGet);
            }

        }
        private PackItemVM GetAllFeatureforSinglePacket(string itemcode, decimal n, decimal remaining_Qty)
        {
            PackItemVM Packetlistobject = new PackItemVM();

            Packetlistobject.ID = PacketWiseItemList.Count() + 1;
            Packetlistobject.ItemCode = itemcode;
            Packetlistobject.ItemName = _itemInfoService.All().Where(x => x.ItemCode == itemcode).Select(x => x.ItemName).FirstOrDefault();
            Packetlistobject.PackItemCode = _itemInfoService.All().Where(x => x.ItemCode == itemcode).Select(x => x.PackItemCode).FirstOrDefault();
            Packetlistobject.PackItem = _itemInfoService.All().Where(x => x.ItemCode == Packetlistobject.PackItemCode).Select(x => x.ItemName).FirstOrDefault();
            Packetlistobject.PackSize = (int)_itemInfoService.All().Where(x => x.ItemCode == Packetlistobject.PackItemCode).Select(x => x.PackSize).FirstOrDefault();
            Packetlistobject.PacketLot = " ";

            if (n != 1)
            {
                int pack = (int)n;
                Packetlistobject.PackPcs = pack;
                Packetlistobject.Qty = Packetlistobject.PackPcs * Packetlistobject.PackSize;

            }
            else
            {
                Packetlistobject.PackPcs = 1;
                Packetlistobject.Qty = (int)remaining_Qty;
            }
            return Packetlistobject;
        }
        public ActionResult GetPackSizebyPackItemCode(string PackItemCode)
        {
            var packsize = _itemInfoService.All().Where(x => x.ItemCode == PackItemCode).Select(x => x.PackSize).FirstOrDefault();
            return Json(packsize, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult GetNewInvNoandIsseueNo(string LocCode)
        {
            string InvNo = GenerateInvNo(LocCode);
            string IsseueNo = GenerateIssueNo(Session["BranchCode"].ToString());
            return Json(new { InvNo = InvNo, IsseueNo = IsseueNo }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatatableOnly(string SaleNo)
        {
            try
            {
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems?SaleNo=" + SaleNo.ToString(), Session["token"].ToString());

                JavaScriptSerializer js = new JavaScriptSerializer();
                IEnumerable<tempAddedItem> itemList = js.Deserialize<IEnumerable<tempAddedItem>>(JsonResponse);
                return Json(new { data = itemList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("LogOff", "SecUserLogin");
            }


        }

        public ActionResult GetAllBySaleNo(string SaleNo)
        {
            //packlist
            bool MaintPacking = _sysSetService.All().ToList().FirstOrDefault().MaintPacking;
            if (MaintPacking) //condition
            {
                List<PackingList> PackingList = new List<PackingList>();
                PackingList = _PackingListService.All().Where(x => x.InvoiceNo == SaleNo).ToList();


                PackItemVM PackItem = new PackItemVM();
                foreach (var item in PackingList)
                {
                    PackItem.SaleNo = item.InvoiceNo;
                    // PackItem.ChallanNo = item.ChallanNo;
                    PackItem.ItemCode = item.ItemCode;
                    PackItem.PackPcs = item.No_of_Pack;
                    PackItem.PackItemCode = item.PackItemCode;
                    PackItem.PacketLot = item.PackLotNo;
                    PackItem.PackSize = Convert.ToInt32(item.PackSize);
                    PackItem.Qty = item.Qty;
                    PackItem.Total_Qty = item.Total_Qty;

                    PacketWiseItemList.Add(PackItem);
                }
            }

            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "SaleViewModels?SaleNo=" + SaleNo.ToString(), Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            SalesMain itemList = js.Deserialize<SalesMain>(JsonResponse);
            itemList.RegNo = _subsidiaryInfoExtService.All().Where(s => s.RegNo == itemList.RegNo).Select(x => x.BIN).FirstOrDefault();

            string returnValue = "";
            if (Convert.ToBoolean(Session["MaintVAT"]) == true)
            {
                string respse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["VATApiUrl"] + "/api/VAT/" + "GetVM_SalRegister1_6P2?TransNo=" + SaleNo.ToString(), Session["token"].ToString());
                JavaScriptSerializer jsvat = new JavaScriptSerializer();
                returnValue = jsvat.Deserialize<string>(respse);
            }

            return Json(new { data = itemList, returnValue = returnValue }, JsonRequestBehavior.AllowGet);
        }

        #region For Common Item Filtering option
        public ActionResult LoadGroupInfoByItemType(string ItemType)
        {
            return Json(LoadDropDown.LoadGroupInfoByItemType(ItemType, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadSGroupByGroupId(string ItemType, string GroupId)
        {
            return Json(LoadDropDown.LoadSGroupByGroupId(ItemType, GroupId, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadSSGroupInfo(string ItemType, string GroupId, string SGroupId)
        {
            return Json(LoadDropDown.LoadSSGroupInfo(ItemType, GroupId, SGroupId, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadItemByItemType(string ItemType)
        {
            return Json(LoadDropDown.LoadItemByItemType(ItemType, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadItemByGroupId(string ItemType, string GroupId)
        {
            return Json(LoadDropDown.LoadItemByGroupId(ItemType, GroupId, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadItemBySSGroupID(string ItemType, string GroupId, string SGroupId, string SSGroupId)
        {
            return Json(LoadDropDown.LoadItemBySSGroupID(ItemType, GroupId, SGroupId, SSGroupId, _itemInfoService, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadItemBySGroupID(string ItemType, string GroupId, string SGroupId)
        {
            return Json(LoadDropDown.LoadItemBySGroupID(ItemType, GroupId, SGroupId, _itemInfoService, _CommonVmService), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetItemById(string ItemCode)
        {
            //HttpResponseMessage response = GlobalVariabls.WebApiClient.GetAsync("tmpAddedItems/" + ItemCode.ToString()).Result;
            //return Json(response.Content.ReadAsAsync<tempAddedItem>().Result, JsonRequestBehavior.AllowGet);
            //10-02-18
            string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems/" + ItemCode.ToString(), Session["token"].ToString());
            JavaScriptSerializer js = new JavaScriptSerializer();
            tempAddedItem itemList = js.Deserialize<tempAddedItem>(JsonResponse);
            return Json(itemList, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult SaveTempTbl(tempAddedItem addOnTempTbl)
        {
            try
            {
                //var SaleNo = Session["UserID"].ToString() + DateTime.Now.ToString("ddMMyy");
                //addOnTempTbl.SaleNo = SaleNo;
                if (addOnTempTbl.ID == 0)
                {
                    HttpResponseMessage response = GlobalVariabls.WebApiClient.PostAsJsonAsync("tmpAddedItems", addOnTempTbl).Result;
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                    //string json = JsonConvert.SerializeObject(addOnTempTbl, Formatting.Indented, serializerSettings);
                    HttpResponseMessage response = GlobalVariabls.WebApiClient.PutAsJsonAsync("tmpAddedItems/" + addOnTempTbl.ID, addOnTempTbl).Result;
                    return Json("1", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DelTempTbl(string Id)
        {
            HttpResponseMessage response = GlobalVariabls.WebApiClient.DeleteAsync("tmpAddedItems/" + Id.ToString()).Result;
            return Json("1", JsonRequestBehavior.AllowGet);

        }

        //class currStocks
        //{
        //    public int Id;
        //    public double CurrQty;
        //    public double UnitPrice;
        //} 

        public ActionResult GetItemByQtyPrice(string Item, string Location, DateTime SaleDate, string SubCode)
        {
            if (!string.IsNullOrEmpty(Item))
            {
                string ProdSer;
                decimal ItemWDiscPerc = 0;
                decimal UnitPrice = 0;
                var items = default(dynamic);
                var SysSet = _sysSetService.All().ToList().Select(s => new { Stockedcheck = s.Stockedcheck, RateChart = s.RateChart }).FirstOrDefault();
                if (SysSet.Stockedcheck == true)
                {
                    if (Session["MaintLot"].ToString() == "True")
                    {
                        string sql = string.Format("EXEC ExistingQty '" + Session["ProjCode"].ToString() + "', '" + Item + "', '" + Location + "'");
                        items = _currentStockService.SqlQueary(sql).Select(x => new { x.CurrQty }).FirstOrDefault();
                        string itemCatgSql = string.Format("select SubCategory from SubsidiaryInfo_Ext where SubCode ='" + SubCode + "'");
                        var itemCatgS = _ItemWiseDisVatService.SqlQueary(itemCatgSql).Select(x => new { x.SubCategory }).FirstOrDefault();

                        string itemPriceSql = string.Format("Select Price, RetailPrice from ItemInfo where ItemCode = '" + Item + "'");
                        var itemPrice = _ItemWiseDisVatService.SqlQueary(itemPriceSql).Select(x => new { x.Price, x.RetailPrice }).FirstOrDefault();

                        if (SysSet.RateChart == true)
                        {
                            var RateChart = _RateChartService.All().Where(x => x.ItemCode == Item && x.CustCode == SubCode).FirstOrDefault();
                            UnitPrice = RateChart == null ? 0 : Convert.ToDecimal(RateChart.ItemRate);
                        }
                        else if (itemCatgS.SubCategory.Trim() == "Whole Saler")
                        {
                            UnitPrice = Convert.ToDecimal(itemPrice.Price);
                        }
                        else
                        {
                            UnitPrice = Convert.ToDecimal(itemPrice.RetailPrice);
                        }
                    }
                    else
                    {
                        var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == Item && x.LocNo == Location.Trim()).ToList();
                        CurrentStock currStocks = new CurrentStock();
                        if (existCurrStoc.Count != 0)
                        {
                            var date = existCurrStoc.Max(x => x.TrDate);
                            var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == Item && x.LocNo == Location.Trim() && x.TrDate == date).FirstOrDefault();
                            currStocks.CurrQty = existCurrStock.BalQty;
                            if (SysSet.RateChart == true)
                            {
                                var RateChart = _RateChartService.All().Where(x => x.ItemCode == Item && x.CustCode == SubCode).FirstOrDefault();
                                UnitPrice = RateChart == null ? 0 : Convert.ToDecimal(RateChart.ItemRate);
                            }
                            else
                            {
                                UnitPrice = (decimal)existCurrStock.BalRate;
                            }
                            currStocks.ItemCode = existCurrStock.ItemCode;
                            items = currStocks;
                        }
                        else
                        {
                            currStocks.CurrQty = 0;
                            items = currStocks;
                        }
                    }
                }

                #region Item wise vat and discount
                //string itemVat = string.Format("select VatPerc from ItemWiseVAT where Itemcode = '" + Item + "' and FromDate <='" + SaleDate.ToString("yyyy/MM/dd") + "' and ToDate >= '" + SaleDate.ToString("yyyy/MM/dd") + "' order by ToDate desc");
                //var wVat = _ItemWiseDisVatService.SqlQueary(itemVat).Select(x => new { x.VatPerc }).FirstOrDefault();
                //if (wVat == null)
                //{
                //    ItemWVat = 0;
                //}
                //else
                //{
                //    ItemWVat = Convert.ToDecimal(wVat.VatPerc);
                //}

                //string itemDiscPerc = string.Format("select DiscPerc from ItemWiseDisc where Itemcode = '" + Item + "' and FromDate <='" + SaleDate.ToString("yyyy/MM/dd") + "' and ToDate >= '" + SaleDate.ToString("yyyy/MM/dd") + "' order by ToDate desc");
                //var wDisc = _ItemWiseDisVatService.SqlQueary(itemDiscPerc).Select(x => new { x.DiscPerc }).FirstOrDefault();
                //if (wDisc == null)
                //{
                //    ItemWDiscPerc = 0;
                //}
                //else
                //{
                //    ItemWDiscPerc = Convert.ToDecimal(wDisc.DiscPerc);
                //}

                #endregion

                var ItemUnit = (from i in _itemInfoService.All().ToList()
                                join u in _unitService.All().ToList() on i.UnitCode equals u.UnitCode
                                where i.ItemCode == Item
                                select new { u.UnitName, i.HSCode, i.Prod_Ser }).FirstOrDefault();
                string UnitName = ItemUnit.UnitName.ToString();
                string HSCode = (ItemUnit.HSCode == null) ? "" : ItemUnit.HSCode;
                ProdSer = ItemUnit.Prod_Ser;
                return Json(new { items = items, ProdSer = ProdSer, ItemWDiscPerc = ItemWDiscPerc, UnitPrice = UnitPrice, UnitName = UnitName, HSCode = HSCode }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTotalAmtVatDisc(string SaleNo)
        {
            if (!string.IsNullOrEmpty(SaleNo))
            {
                decimal totAmt = 0;
                decimal? totVat = 0;
                decimal? totDiscount = 0;
                //List<tempAddedItem> itemList;
                //HttpResponseMessage response = GlobalVariabls.WebApiClient.GetAsync("tmpAddedItems?SaleNo=" + SaleNo).Result;
                //itemList = response.Content.ReadAsAsync<List<tempAddedItem>>().Result;
                //10-02-18
                string JsonResponse = LoadDropDown.CallApi(ConfigurationManager.AppSettings["ApiUrl"] + "/api/" + "tmpAddedItems?SaleNo=" + SaleNo.ToString(), Session["token"].ToString());

                if (JsonResponse.Length == 61)
                {
                    return Json(Url.Action("LogOff", "SecUserLogin"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    IEnumerable<tempAddedItem> itemList = js.Deserialize<IEnumerable<tempAddedItem>>(JsonResponse);
                    foreach (var item in itemList)
                    {
                        totAmt += item.Amount;
                        totVat += item.Vat;
                        totDiscount += item.Discount;
                    }
                    return Json(new { totAmt = totAmt, totVat = totVat, totDiscount = totDiscount }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveSale(SalesMain SalesMain, IssueMain IssueMain)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("Sales_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }
                bool IsAutoLot = false;
                var todayDate = DateTime.Now;
                SaleViewModel model = new SaleViewModel();
                List<SalesDetail> SalesDetailList = new List<SalesDetail>();
                List<IssueDetails> IssueDetailList = new List<IssueDetails>();
                List<ItemWiseDisVatVM> itemList;
                List<VM_SalRegister1_6P2> VMSalReg1List = new List<VM_SalRegister1_6P2>();
                List<RateChart> RateChartList = new List<RateChart>();

                string tmpAddedItem = string.Format("select SaleNo, ItemCode, Description,LotNo, Qty, ExtQty,  UnitPrice, ExtUPrice, Vat, Discount, Amount, VatPerc, DiscPerc from tmpAddedItem where SaleNo = '" + SalesMain.tSaleNo + "'");
                itemList = _ItemWiseDisVatService.SqlQueary(tmpAddedItem).ToList();
                foreach (var item in itemList)
                {
                    RateChart RaChart = new RateChart();
                    SalesDetail sd = new SalesDetail();
                    sd.ItemCode = item.ItemCode;
                    sd.LotNo = item.LotNo;
                    sd.Description = item.Description;
                    sd.SaleQty = item.Qty;
                    sd.UnitPrice = item.UnitPrice;
                    sd.VATAmt = item.Vat;
                    sd.VATPerc = item.VatPerc;
                    sd.DiscAmt = item.Discount;
                    sd.DiscPerc = item.DiscPerc;
                    sd.ExtQty = item.ExtQty;
                    sd.ExtUPrice = item.ExtUPrice;
                    //sd.SalesMainID = sMainId;
                    SalesDetailList.Add(sd);

                    // For Rate Chart 
                    if (Session["RateChart"].ToString() == "True")
                    {
                        RaChart.ItemCode = item.ItemCode;
                        RaChart.ItemRate = item.UnitPrice;
                        RaChart.CustCode = SalesMain.CustCode;
                        RateChartList.Add(RaChart);
                    }

                    if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                    {
                        IssueDetails IssDetls = new IssueDetails();
                        IssDetls.IssueNo = IssueMain.IssueNo;
                        IssDetls.ItemCode = item.ItemCode;
                        IssDetls.LotNo = item.LotNo;
                        IssDetls.Description = item.Description;
                        IssDetls.Qty = Convert.ToDouble(item.Qty);
                        IssDetls.Price = Convert.ToDouble(item.UnitPrice);
                        IssDetls.ExQty = Convert.ToDouble(item.ExtQty);
                        IssueDetailList.Add(IssDetls);
                    }

                    #region For VAT VM_SalRegister1_6P2
                    if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                    {
                        VM_SalRegister1_6P2 VMSalReg1 = new VM_SalRegister1_6P2();
                        VMSalReg1.SerialNo = 0;
                        VMSalReg1.SaleDate = SalesMain.SaleDate;
                        VMSalReg1.OBQty = item.ExtQty;
                        VMSalReg1.OBValue = item.ExtUPrice;
                        VMSalReg1.Ch_BENO = "";
                        if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                        {
                            VMSalReg1.Ch_BENO = IssueMain.IssueNo;
                            VMSalReg1.Ch_BEDate = IssueMain.IssueDate;
                        }
                        VMSalReg1.CustCode = SalesMain.CustCode;
                        VMSalReg1.CustName = _subsidiaryInfoService.All().Where(s => s.SubCode == SalesMain.CustCode).Select(s => s.SubName).FirstOrDefault();
                        //VMSalReg1.SuppAddr = "";
                        //VMSalReg1.R_E_N_No = "";
                        VMSalReg1.ItemName = _itemInfoService.All().Where(s => s.ItemCode == item.ItemCode).Select(s => s.ItemName).FirstOrDefault();
                        VMSalReg1.ProdQty = item.Qty;
                        VMSalReg1.ProdValue = item.UnitPrice;
                        VMSalReg1.IssueQty = item.Qty;
                        VMSalReg1.IssueValue = item.UnitPrice;
                        VMSalReg1.CloseQty = 0;
                        VMSalReg1.CloseValue = 0;
                        VMSalReg1.VATAmt = 0;
                        VMSalReg1.SuppTax = 0;
                        VMSalReg1.Remarks = SalesMain.Remarks;
                        VMSalReg1.TranNo = SalesMain.SaleNo;
                        VMSalReg1.TranType = "Sales";
                        VMSalReg1.ItemCode = item.ItemCode;
                        VMSalReg1List.Add(VMSalReg1);
                    }
                    #endregion

                }
                //packlist
                bool MaintPacking = _sysSetService.All().ToList().FirstOrDefault().MaintPacking;
                if (MaintPacking) //condition
                {
                    List<IssueDetails> IssDetls_Pack_List = PacketWiseItemList.GroupBy(a => a.PackItemCode)
                        .Select(x => new { PackItemCode = x.FirstOrDefault().PackItemCode, PackPcs = x.Sum(t => t.PackPcs) }).AsEnumerable()
                        .Select(x => new IssueDetails
                        {
                            ItemCode = x.PackItemCode,
                            Qty = x.PackPcs,
                            ItemType = "P",

                            IssueNo = IssueMain.IssueNo,
                            LotNo = "",
                            Description = "",
                            Price = 0,
                            ExQty = 0
                        }).ToList();
                    IssueDetailList.AddRange(IssDetls_Pack_List);

                    //end packlist

                    var eCode = "0";

                    var objPackList = _PackingListService.All().ToList()
                                    .Where(x => x.InvoiceNo == SalesMain.SaleNo).ToList();

                    if (objPackList.Count == 0)
                    {
                        PackingList PackItem = new PackingList();
                        foreach (var item in PacketWiseItemList)
                        {
                            PackItem.InvoiceNo = SalesMain.SaleNo;
                            PackItem.ChallanNo = SalesMain.SaleNo;
                            PackItem.ItemCode = item.ItemCode;
                            PackItem.No_of_Pack = item.PackPcs;
                            PackItem.PackItemCode = item.PackItemCode;
                            PackItem.PackLotNo = item.PacketLot;
                            PackItem.PackSize = item.PackSize;
                            PackItem.Qty = item.Qty;
                            PackItem.Total_Qty = (item.PackPcs * Convert.ToInt32(item.Qty));
                            _PackingListService.Add(PackItem);
                            _PackingListService.Save();


                        }


                        eCode = "1";
                    }
                    else
                    {
                        eCode = "2";
                        return Json(eCode, JsonRequestBehavior.AllowGet);
                    }

                }
                #region run sp for Accode
                string sql = string.Format("EXEC GetARAc '" + SalesMain.CustCode + "'");
                string Accode;
                IEnumerable<retvalpro> VchrLst;
                VchrLst = db.Database.SqlQuery<retvalpro>(sql).ToList();
                Accode = VchrLst.Select(x => x.ARAC).Single();
                #endregion
                SalesMain.Accode = Accode;
                SalesMain.ca_bk_op = _defACService.All().Select(s => s.Sales).FirstOrDefault();
                SalesMain.BranchCode = Session["BranchCode"].ToString();
                SalesMain.ProjCode = Session["ProjCode"].ToString();
                SalesMain.IsPaid = false;
                IssueMain IssueMainObj = new IssueMain();

                var isExistSNo = _SalesMainService.All().ToList().Where(s => s.SaleNo == SalesMain.SaleNo).FirstOrDefault();
                if (isExistSNo != null)
                {
                    SalesMain.SaleNo = GenerateInvNo(SalesMain.LocCode);
                    if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                    {
                        IssueMainObj.IssueNo = GenerateIssueNo(Session["BranchCode"].ToString());
                    }
                }
                if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                {
                    IssueMainObj.IssueNo = IssueMain.IssueNo;
                    IssueMainObj.FromLocCode = IssueMain.FromLocCode;
                    IssueMainObj.IssueDate = IssueMain.IssueDate;
                    IssueMainObj.IssDate = IssueMain.IssueDate;
                    IssueMainObj.IssTime = DateTime.Now;
                    IssueMainObj.RefNo = IssueMain.RefNo;
                    IssueMainObj.JobNo = IssueMain.JobNo;
                    IssueMainObj.RefDate = (IssueMain.RefDate != null) ? IssueMain.RefDate : DateTime.Now;
                    IssueMainObj.Amount = Convert.ToDouble(SalesMain.TotSaleAmt);
                    IssueMainObj.GLPost = false;
                    IssueMainObj.IsReceived = false;
                    IssueMainObj.cashReceiptStatus = false;
                    IssueMainObj.BranchCode = Session["BranchCode"].ToString();
                    IssueMainObj.FinYear = Session["FinYear"].ToString();
                    IssueMainObj.VchrNo = IssueMain.VchrNo;
                    IssueMainObj.Remarks = SalesMain.Remarks;
                    //packlist

                    IssueMainObj.EntrySrc = "AR";
                    IssueMainObj.EntrySrcNo = SalesMain.SaleNo;

                    //end packlist
                    model.IssueMain = IssueMainObj;
                    model.IssueDetail = IssueDetailList;
                }
                model.SalesMain = SalesMain;
                model.SalesDetail = SalesDetailList;

                List<IssueDetails> issuDetailsList = new List<IssueDetails>();
                List<CurrentStock> CurrentStockList = new List<CurrentStock>();
                List<CostLedger> CostLedgerList = new List<CostLedger>();

                bool Stockched = _sysSetService.All().ToList().FirstOrDefault().Stockedcheck;
                if (Stockched == true)
                {
                    #region For LOT OR AVP
                    if (Session["MaintLot"].ToString() == "True")
                    {
                        if (IsAutoLot == true)
                        {
                            #region AutoLot by issueDetail

                            //foreach (var item in itemList)
                            //{
                            //    List<LotDT> itemLots;
                            //    double Qty = 0.0;
                            //    var _currStockSrc = (from old in _currentStockService.All()
                            //                         join newID in itemList
                            //                         on old.ItemCode equals newID.ItemCode
                            //                         where old.CurrQty > 0
                            //                         select old).ToList();
                            //    CurrentStock currStock = new CurrentStock();
                            //    List<CurrentStock> CurrStockItems = new List<CurrentStock>();

                            //    itemLots = new List<LotDT>();

                            //    Qty = (double)item.Qty;
                            //    CurrStockItems = _currStockSrc.Where(i => i.ItemCode == item.ItemCode).ToList();
                            //    foreach (var itemct in CurrStockItems)
                            //    {
                            //        var lotdt = _lotDtService.All().FirstOrDefault(i => i.LotNo == itemct.LotNo);
                            //        itemLots.Add(lotdt);
                            //    }
                            //    itemLots = itemLots.OrderBy(d => d.LotDate).ToList();
                            //    foreach (var lot in itemLots)
                            //    {
                            //        currStock = CurrStockItems.FirstOrDefault(m => m.ItemCode == item.ItemCode && m.LocCode == SalesMain.LocCode && m.LotNo == lot.LotNo && m.CurrQty > 0);
                            //        if (currStock == null)
                            //        { continue; }
                            //        var Lotwisestockqty = currStock.CurrQty;
                            //        if (Lotwisestockqty > Qty && Qty > 0)
                            //        {
                            //            IssueDetails issueDetailsInfo = new IssueDetails();
                            //            issueDetailsInfo.IssueNo = item.SaleNo;
                            //            issueDetailsInfo.ItemCode = item.ItemCode;
                            //            //issueDetailsInfo.SubCode = item.SubCategory;
                            //            issueDetailsInfo.Description = item.Description;
                            //            issueDetailsInfo.Qty = Qty;
                            //            issueDetailsInfo.Price = item.Price;
                            //            issueDetailsInfo.ExQty = Lotwisestockqty;
                            //            issueDetailsInfo.LotNo = lot.LotNo;
                            //            amount = amount + (item.Qty * item.Price);
                            //            issuDetailsList.Add(issueDetailsInfo);

                            //            currStock.CurrQty -= Qty;
                            //            Qty -= Qty;
                            //            _currentStockService.Update(currStock);
                            //            break;
                            //        }
                            //        else if (Qty > 0 && Lotwisestockqty <= Qty && Lotwisestockqty > 0)
                            //        {

                            //            IssueDetails issueDetailsInfo = new IssueDetails();
                            //            issueDetailsInfo.IssueNo = item.SaleNo;
                            //            issueDetailsInfo.ItemCode = item.ItemCode;
                            //            //issueDetailsInfo.SubCode = item.SubCode;
                            //            issueDetailsInfo.Description = item.Description;
                            //            issueDetailsInfo.Qty = Lotwisestockqty;
                            //            issueDetailsInfo.Price = item.Price;
                            //            issueDetailsInfo.ExQty = Lotwisestockqty;
                            //            issueDetailsInfo.LotNo = lot.LotNo;
                            //            amount = amount + (item.Qty * item.Price);
                            //            issuDetailsList.Add(issueDetailsInfo);

                            //            currStock.CurrQty = 0;
                            //            Qty -= (int)Lotwisestockqty;
                            //            _currentStockService.Update(currStock);
                            //        }
                            //        else if (Qty == 0)
                            //        {
                            //            break;
                            //        }

                            //    }
                            //}
                            #endregion Not autolot end
                        }
                        else
                        {
                            #region Not AutoLot by SalesDetail
                            foreach (var currentItem in SalesDetailList)
                            {
                                var currentStocks = _currentStockService.All().ToList().FirstOrDefault(m => m.ItemCode == currentItem.ItemCode &&
                                    m.LocCode == SalesMain.LocCode && m.LotNo == currentItem.LotNo);
                                if (currentStocks != null)
                                {
                                    currentStocks.CurrQty = currentStocks.CurrQty - (double)currentItem.SaleQty;
                                    currentStocks.UnitPrice = (double)currentItem.UnitPrice;
                                    CurrentStockList.Add(currentStocks);
                                }
                                else
                                {
                                    CurrentStock currStock = new CurrentStock();
                                    currStock.LocCode = SalesMain.LocCode;
                                    currStock.LotNo = currentItem.LotNo;
                                    currStock.ItemCode = currentItem.ItemCode;
                                    currStock.CurrQty = 0 - (double)currentItem.SaleQty;
                                    currStock.UnitPrice = (double)currentItem.UnitPrice;
                                    CurrentStockList.Add(currStock);
                                }
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        #region For AVP
                        foreach (var costLedger in SalesDetailList)
                        {
                            CostLedger cLedger = new CostLedger();
                            cLedger.IssQty = (double)costLedger.SaleQty;
                            cLedger.IssRate = (double)costLedger.ExtUPrice;
                            cLedger.IssTotal = cLedger.IssQty * cLedger.IssRate;

                            cLedger.RecQty = 0;
                            cLedger.RecRate = 0;
                            cLedger.RecTotal = 0;

                            var existCurrStoc = CostLedgerAppService.All().Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == SalesMain.LocCode.Trim()).ToList();
                            if (existCurrStoc.Count != 0)
                            {
                                var date = existCurrStoc.Max(x => x.TrDate);
                                var existCurrStock = CostLedgerAppService.All().OrderByDescending(s => s.RecId).Where(x => x.ItemCode == costLedger.ItemCode && x.LocNo == SalesMain.LocCode.Trim() && x.TrDate == date).FirstOrDefault();

                                cLedger.BalQty = existCurrStock.BalQty - cLedger.IssQty;
                                cLedger.BalRate = System.Math.Round(existCurrStock.BalRate, 2);
                                cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                cLedger.LocNo = SalesMain.LocCode;
                                cLedger.ItemCode = costLedger.ItemCode;
                                cLedger.TrDate = SalesMain.SaleDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                cLedger.UpdSrcNo = SalesMain.SaleNo;
                                cLedger.UpdSrc = "AR";
                                CostLedgerList.Add(cLedger);
                            }
                            else if (existCurrStoc.Count == 0)
                            {
                                cLedger.CurrQty = 0;
                                cLedger.UnitPrice = 0;
                                cLedger.BalTotal = 0;

                                cLedger.BalQty = cLedger.BalQty - cLedger.IssQty;
                                cLedger.BalRate = cLedger.IssRate;
                                cLedger.BalRate = System.Math.Round(cLedger.BalRate, 2);
                                cLedger.BalTotal = System.Math.Round(cLedger.BalQty * cLedger.BalRate, 2);

                                cLedger.LocNo = SalesMain.LocCode;
                                cLedger.ItemCode = costLedger.ItemCode;
                                cLedger.TrDate = SalesMain.SaleDate.AddHours(todayDate.Hour).AddMinutes(todayDate.Minute).AddSeconds(todayDate.Second).AddMilliseconds(todayDate.Millisecond);

                                cLedger.UpdSrcNo = SalesMain.SaleNo;
                                cLedger.UpdSrc = "AR";
                                CostLedgerList.Add(cLedger);
                            }
                        }
                        #endregion
                    }
                    #endregion
                }


                model.CostLedger = CostLedgerList;
                model.CurrentStock = CurrentStockList;



                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented, serializerSettings);
                string content = "Please add the item.";
                string savedNo = "Save Successfully. The Saved Invoice Number is " + SalesMain.SaleNo;
                if (SalesDetailList.Count != 0)
                {
                    HttpResponseMessage response = GlobalVariabls.WebApiClient.PostAsJsonAsync("SaleViewModels", model).Result;
                    content = response.StatusCode.ToString();
                    if (content == "OK")
                    {
                        var CaBkOp = "";
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        if (sysSet.CashBasis == true)
                        {
                            CaBkOp = _defACService.All().FirstOrDefault().Sales;
                        }
                        else
                        {
                            CaBkOp = _defACService.All().FirstOrDefault().ARAc;
                        }
                        LoadDropDown.ProvitionSalesSave("AR", "I", SalesMain.SaleNo, SalesMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), SalesMain.SaleDate.ToString("yyyy-MM-dd"), CaBkOp, Session["UserName"].ToString(), SalesMain.JobNo);

                        //For Bin No
                        var subExt = _subsidiaryInfoExtService.All().ToList().Where(s => s.SubCode == SalesMain.CustCode).FirstOrDefault();
                        subExt.BIN = SalesMain.RegNo;
                        subExt.RegNo = SalesMain.RegNo;
                        subExt.RegType = SalesMain.RegType;
                        _subsidiaryInfoExtService.Update(subExt);
                        _subsidiaryInfoExtService.Save();

                        //For Rate Chart
                        if (Session["RateChart"].ToString() == "True")
                        {
                            foreach (var RChartItem in RateChartList)
                            {
                                var rChart = _RateChartService.All().Where(s => s.CustCode == RChartItem.CustCode && s.ItemCode == RChartItem.ItemCode).FirstOrDefault();
                                if (rChart == null)
                                {
                                    _RateChartService.Add(RChartItem);
                                    _RateChartService.Save();
                                }
                                else
                                {
                                    rChart.ItemRate = RChartItem.ItemRate;
                                    _RateChartService.Update(rChart);
                                    _RateChartService.Save();
                                }
                            }
                        }


                        //For VAT api 
                        string returnValue = "";
                        if (Convert.ToBoolean(Session["MaintVAT"]) == true)
                        {
                            var serSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                            string jsonCov = JsonConvert.SerializeObject(VMSalReg1List, Formatting.Indented, serSettings);
                            var respse = GlobalVariabls.VatApiClient.PostAsJsonAsync("VAT/SaveVM_SalRegister1_6P2", VMSalReg1List).Result;
                            returnValue = respse.Content.ReadAsAsync<string>().Result;
                        }
                        return Json(new { status = 1, savedNo, returnValue = returnValue }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(content, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(content, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpdateSale(SalesMain SalesMain, IssueMain IssueMain)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("Sales_Update"))
                {
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                var VchrExist = _pVchrmainService.All().FirstOrDefault(s => s.VchrNo == SalesMain.VchrNo);
                if (VchrExist == null)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

                bool MaintLot = _sysSetService.All().Select(s => s.MaintLot).FirstOrDefault(); //Pending

                var SaleM = _SalesMainService.All().Where(s => s.SaleNo == SalesMain.SaleNo).FirstOrDefault();
                SalesMain.SalesMainID = SaleM.SalesMainID;


                SaleViewModel model = new SaleViewModel();
                List<SalesDetail> SalesDetailList = new List<SalesDetail>();
                List<IssueDetails> IssueDetailList = new List<IssueDetails>();
                List<ItemWiseDisVatVM> itemList;
                string TotAmtVatDis = string.Format("select ItemCode, Description,LotNo, Qty, ExtQty, ExtUPrice,  UnitPrice, Vat, Discount, Amount, VatPerc, DiscPerc from tmpAddedItem where SaleNo = '" + SalesMain.SaleNo + "'");
                itemList = _ItemWiseDisVatService.SqlQueary(TotAmtVatDis).ToList();
                foreach (var item in itemList)
                {
                    SalesDetail sd = new SalesDetail();
                    sd.ItemCode = item.ItemCode;
                    sd.LotNo = item.LotNo;
                    sd.SaleQty = item.Qty;
                    sd.UnitPrice = item.UnitPrice;
                    sd.VATAmt = item.Vat;
                    sd.VATPerc = item.VatPerc;
                    sd.DiscAmt = item.Discount;
                    sd.DiscPerc = item.DiscPerc;
                    sd.ExtQty = item.ExtQty;
                    sd.ExtUPrice = item.ExtUPrice;
                    sd.SalesMainID = SalesMain.SalesMainID;
                    sd.Description = item.Description;
                    SalesDetailList.Add(sd);
                    if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                    {
                        IssueDetails IssDetls = new IssueDetails();
                        IssDetls.IssueNo = IssueMain.IssueNo;
                        IssDetls.ItemCode = item.ItemCode;
                        IssDetls.LotNo = item.LotNo;
                        IssDetls.Qty = Convert.ToDouble(item.Qty);
                        IssDetls.Price = Convert.ToDouble(item.UnitPrice);
                        IssDetls.ExQty = Convert.ToDouble(item.ExtQty);
                        IssueDetailList.Add(IssDetls);
                        //packlist
                        bool MaintPacking = _sysSetService.All().ToList().FirstOrDefault().MaintPacking;
                        if (MaintPacking) //condition
                        {
                            List<IssueDetails> IssDetls_Pack_List = PacketWiseItemList.GroupBy(a => a.PackItemCode)
                                .Select(x => new { PackItemCode = x.FirstOrDefault().PackItemCode, PackPcs = x.Sum(t => t.PackPcs) }).AsEnumerable()
                                .Select(x => new IssueDetails
                                {
                                    ItemCode = x.PackItemCode,
                                    Qty = x.PackPcs,
                                    ItemType = "P",

                                    IssueNo = IssueMain.IssueNo,
                                    // IssDetls.ItemCode = item.ItemCode;
                                    LotNo = "",
                                    Description = "",
                                    //IssDetls.Qty = Convert.ToDouble(item.Qty);
                                    Price = 0,
                                    ExQty = 0
                                }).ToList();
                            IssueDetailList.AddRange(IssDetls_Pack_List);
                        }
                    }
                }

                if (IssueMain.MakeChallanAuto == "MakeChallanAuto")
                {
                    IssueMain IssueMainObj = new IssueMain();
                    IssueMainObj.IssueNo = IssueMain.IssueNo;
                    IssueMainObj.FromLocCode = IssueMain.FromLocCode;
                    IssueMainObj.IssueDate = IssueMain.IssueDate;
                    IssueMainObj.IssDate = IssueMain.IssueDate;
                    IssueMainObj.IssTime = DateTime.Now;
                    IssueMainObj.JobNo = IssueMain.JobNo;
                    IssueMainObj.RefNo = IssueMain.RefNo;
                    IssueMainObj.RefDate = (IssueMain.RefDate != null) ? IssueMain.RefDate : DateTime.Now; ;
                    IssueMainObj.Amount = Convert.ToDouble(SalesMain.TotSaleAmt);
                    IssueMainObj.GLPost = false;
                    IssueMainObj.IsReceived = false;
                    IssueMainObj.cashReceiptStatus = false;
                    IssueMainObj.BranchCode = Session["BranchCode"].ToString();
                    IssueMainObj.FinYear = Session["FinYear"].ToString();
                    IssueMainObj.VchrNo = IssueMain.VchrNo;
                    IssueMainObj.Remarks = SalesMain.Remarks;

                    //packlist
                    IssueMainObj.EntrySrc = "AR";
                    IssueMainObj.EntrySrcNo = SalesMain.SaleNo;

                    //end packlist
                    model.IssueMain = IssueMainObj;
                    model.IssueDetail = IssueDetailList;

                    //end packlist

                    bool MaintPacking = _sysSetService.All().ToList().FirstOrDefault().MaintPacking;
                    if (MaintPacking) //condition
                    {
                        var objPackingList = _PackingListService.All().ToList()
                                        .Where(x => x.InvoiceNo == SalesMain.SaleNo).ToList();
                        foreach (var item_pack in objPackingList)
                        {
                            _PackingListService.Delete(item_pack);
                        }
                        PackingList PackItem = new PackingList();
                        foreach (var item_pack in PacketWiseItemList)
                        {
                            PackItem.InvoiceNo = SalesMain.SaleNo;
                            PackItem.ChallanNo = SalesMain.SaleNo;
                            PackItem.ItemCode = item_pack.ItemCode;
                            PackItem.No_of_Pack = item_pack.PackPcs;
                            PackItem.PackItemCode = item_pack.PackItemCode;
                            PackItem.PackLotNo = item_pack.PacketLot;
                            PackItem.PackSize = item_pack.PackSize;
                            PackItem.Qty = item_pack.Qty;
                            PackItem.Total_Qty = (item_pack.PackPcs * Convert.ToInt32(item_pack.Qty));
                            _PackingListService.Add(PackItem);

                        }
                        _PackingListService.Save();
                    }
                    //packList                    
                }

                #region run sp for Accode
                string sql = string.Format("EXEC GetARAc '" + SalesMain.CustCode + "'");
                string Accode;
                IEnumerable<retvalpro> VchrLst;
                VchrLst = db.Database.SqlQuery<retvalpro>(sql).ToList();
                Accode = VchrLst.Select(x => x.ARAC).Single();
                #endregion
                SalesMain.Accode = Accode;
                SalesMain.ca_bk_op = _defACService.All().Select(s => s.Sales).FirstOrDefault();
                SalesMain.BranchCode = Session["BranchCode"].ToString();
                SalesMain.ProjCode = Session["ProjCode"].ToString();
                SalesMain.IsPaid = SaleM.IsPaid;
                model.SalesMain = SalesMain;
                model.SalesDetail = SalesDetailList;

                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented, serializerSettings);

                HttpResponseMessage response = GlobalVariabls.WebApiClient.PutAsJsonAsync("SaleViewModels", model).Result;
                string content = response.StatusCode.ToString();
                if (content == "OK")
                {
                    LoadDropDown.journalVoucherRemoval("AR", SalesMain.SaleNo, SalesMain.VchrNo, Session["FinYear"].ToString());
                    var CaBkOp = _defACService.All().FirstOrDefault().Sales;
                    LoadDropDown.ProvitionSalesSave("AR", "I", SalesMain.SaleNo, SalesMain.VchrNo, Session["FinYear"].ToString(), Session["ProjCode"].ToString(), Session["BranchCode"].ToString(), SalesMain.SaleDate.ToString("yyyy-MM-dd"), CaBkOp, Session["UserName"].ToString(), SalesMain.JobNo);

                    //For Bin No
                    var subExt = _subsidiaryInfoExtService.All().ToList().Where(s => s.SubCode == SalesMain.CustCode).FirstOrDefault();
                    if (subExt != null)
                    {
                        subExt.BIN = SalesMain.RegNo;
                        subExt.RegNo = SalesMain.RegNo;
                        subExt.RegType = SalesMain.RegType;
                        _subsidiaryInfoExtService.Update(subExt);
                        _subsidiaryInfoExtService.Save();
                    }                  

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(content, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json("Update Failed..!", JsonRequestBehavior.AllowGet);
            }

        }

        public void SalePreviewPdfPrint()
        {
            try
            {
                List<PrinterSet> PrinterSet = LoadDropDown.GetPrinterInfo();
                var dir = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
                string fileName = dir + @"\SalePreviewPdf" + ".pdf";
                PdfDocument pdfdocument = new PdfDocument();
                if (System.IO.File.Exists(fileName))
                {
                    pdfdocument.LoadFromFile(fileName);
                    pdfdocument.PrinterName = PrinterSet.Where(s => s.PP == "BILL").Select(x => x.PN).FirstOrDefault();
                    pdfdocument.PrintDocument.PrinterSettings.Copies = 1;
                    pdfdocument.PrintFromPage = 1;
                    pdfdocument.PrintToPage = 1;
                    pdfdocument.PrintDocument.Print();


                    pdfdocument.LoadFromFile(fileName);
                    pdfdocument.PrinterName = PrinterSet.Where(s => s.PP == "CHALLAN").Select(x => x.PN).FirstOrDefault();
                    pdfdocument.PrintDocument.PrinterSettings.Copies = 1;
                    pdfdocument.PrintFromPage = 2;
                    pdfdocument.PrintToPage = 2;
                    pdfdocument.PrintDocument.Print();

                    System.IO.File.Delete(fileName); // for delete current downloded file
                }
                pdfdocument.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult InvoiceCancel(string InvNo)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("InvoiceCancel"))
            {
                return Json("C", JsonRequestBehavior.AllowGet);
            }
            string ReturnSTR = "";
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {

                var resultParameter = new SqlParameter("@ReturnMSG", SqlDbType.VarChar, 300)
                {
                    Direction = ParameterDirection.Output
                };
                                

                dbContext.Database.ExecuteSqlCommand("InvoiceCancel @InvoiceNo, @ReturnMSG out",
                new SqlParameter("@InvoiceNo", InvNo),                
                resultParameter);

                ReturnSTR = (string)resultParameter.Value;
                TransactionLogService.SaveTransactionLog(_transactionLogService, "Sales", "InvoiceCancel",
                                  InvNo, Session["UserName"].ToString());
            }

            return Json(ReturnSTR, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SalePreviewPdf(int CopyQty, int CopyType, string sNo)
        {

            List<SalePreviewVM> SaleDetail = (from sm in _SalesMainService.All().ToList()
                                              join sd in _SaleDetailService.All().ToList() on sm.SalesMainID equals sd.SalesMainID
                                              where sm.SaleNo == sNo
                                              select new SalePreviewVM
                                              {
                                                  SaleNo = sm.SaleNo,
                                                  SaleDate = sm.SaleDate,
                                                  ItemCode = sd.ItemCode,
                                                  ItemName = (_itemInfoService.All().Where(s => s.ItemCode == sd.ItemCode)
                                                                   .Select(c => c.ItemName)).FirstOrDefault(),
                                                  CustName = (_subsidiaryInfoService.All().Where(s => s.SubCode == sm.CustCode)
                                                                   .Select(c => c.SubName)).FirstOrDefault(),
                                                  CustCode = sm.CustCode,
                                                  RefNo = sm.RefNo,
                                                  RefDate = sm.RefDate,
                                                  IssueNo = sm.IssueNo,
                                                  IssueDate = sm.IssueDate,
                                                  LocCode = (_locationService.All().Where(s => s.LocCode == sm.LocCode.Trim())
                                                                   .Select(c => c.LocName)).FirstOrDefault(),
                                                  Qty = sd.SaleQty,
                                                  UnitPrice = sd.UnitPrice,
                                                  Vat = sm.VATAmt,
                                                  Discount = sm.Discount,
                                                  NetAmount = sm.NetAmount,
                                                  TotSaleAmt = sm.TotSaleAmt,
                                                  SubAddress = db.SubsidiaryInfo_Ext.Where(s => s.SubCode == sm.CustCode).Select(s => s.SubAddress).FirstOrDefault()
                                              }).ToList();

            //var due = SaleDetail.
            string eId = _EmployeeService.All().ToList().Where(e => e.UserName == Session["UserName"].ToString()).FirstOrDefault().Id.ToString();
            ViewBag.signatory = _EmpFuncService.All().ToList().Where(s => s.BranchCode == Session["BranchCode"].ToString() && s.EmpId == Convert.ToInt32(eId) && s.FormName == "Sales").Select(s => s.FuncName).ToList();


            if (SaleDetail.Count == 0)
            {
                string errMsg = "NP";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            else
            {
                ViewBag.CopyQty = CopyQty;
                ViewBag.CopyType = CopyType;
                ViewBag.sNo = sNo;

                ViewBag.InWordsAmt = InWord.ConvertToWords((SaleDetail.FirstOrDefault().NetAmount + SaleDetail.FirstOrDefault().Vat).ToString());
                ViewBag.InvFormat = _sysSetService.All().FirstOrDefault().InvoiceFormat;
                decimal totQty = 0;
                foreach (var item in SaleDetail)
                {
                    totQty += Convert.ToDecimal(item.Qty);
                }
                ViewBag.InWordstotQty = InWord.ConvertToWords(totQty.ToString());
                //For us Culture Ex: 0.00
                const string culture = "en-US";
                CultureInfo ci = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;

                var sysSet = _sysSetService.All().ToList().FirstOrDefault();
                if (CopyType == 4) //gatepass
                {
                    var SaleM = _SalesMainService.All().Where(s => s.SaleNo == sNo).FirstOrDefault();
                    ViewBag.signatory = _EmpFuncService.All().ToList().Where(s => s.BranchCode == Session["BranchCode"].ToString() && s.EmpId == Convert.ToInt32(eId) && s.FormName == "Sales").Select(s => s.FuncName).ToList();
                    ViewBag.SaleDate = SaleM.SaleDate.ToShortDateString();
                    ViewBag.LocCode = SaleM.LocCode;
                    ViewBag.JobNo = SaleM.JobNo;
                    ViewBag.LocName = (_locationService.All().Where(s => s.LocCode == SaleM.LocCode).Select(c => c.LocName)).FirstOrDefault();
                    return View("~/Views/Sales/GatePassPdf.cshtml", PacketWiseItemList);

                }
                else //invoice & challan,invoice , challan
                {

                    return new Rotativa.ViewAsPdf("~/Views/Sales/SaleChallanPdf.cshtml", SaleDetail) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \" \"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };

                }

            }
        }

        public ActionResult GetCustCodeByAddReg(string CustCode)
        {
            var subExt = _subsidiaryInfoExtService.All().ToList().Where(s => s.SubCode == CustCode).Select(x => new { SubAddress = x.SubAddress, BIN = x.BIN, SubCategory = x.SubCategory }).FirstOrDefault();
            return Json(subExt, JsonRequestBehavior.AllowGet);
        }

        public string GenerateInvNo(string LocCode)
        {
            string invSql = string.Format("exec GetNewInvoiceNo '" + Session["VchrConv"].ToString() + "','" + LocCode + "','" + Session["ProjCode"].ToString() + "','" + Session["BranchCode"].ToString() + "',6");
            string InvNo = _ItemWiseDisVatService.SqlQueary(invSql).ToList().FirstOrDefault().InvoiceNo;
            return InvNo;
        }

        public string GenerateIssueNo(string branchCode)
        {
            branchCode = Session["BranchCode"].ToString();
            string issueNo = "";
            var rcvNo = _issueMainService.All().ToList().LastOrDefault(x => x.BranchCode.Trim() == branchCode);

            if (string.IsNullOrEmpty(branchCode))
            {
                var recvNo = _issueMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    issueNo = "00" + (Convert.ToInt64(recvNo.IssueNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    issueNo = "00000001";
                }
            }
            else
            {
                if (rcvNo != null)
                {
                    issueNo = branchCode + (Convert.ToInt64(rcvNo.IssueNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    issueNo = branchCode + "000001";
                }

            }

            return issueNo;

        }

        public ActionResult GetJournalVoucher(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Sales_VchrWaitingForPosting"))
            {
                string errMsg = "VWP";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "' "
                + "group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac");

            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not Found !!!";
                //ViewBag.msg = errMsg;
                return RedirectToAction("Sales", "Sales", new { errMsg });

            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/JournalVoucher/JournalVoucher.cshtml", glReport);
            }
        }

        public ActionResult GetGLEntries(DateTime date, string pageType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Sales_VchrList"))
            {
                string errMsg = "VL";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            string branchCode = Session["BranchCode"].ToString();
            string sql = string.Format("EXEC GLEntriesByDate '" + pageType + "', '" + date.ToString("MM-dd-yyyy") + "', '" + branchCode + "'");
            List<JarnalVoucher> glReport = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (glReport.Count == 0)
            {
                string errMsg = "Data not found !!!";
                return RedirectToAction("Sales", "Sales", new { errMsg });
            }
            else
            {
                ViewBag.glDate = date;
                return View("~/Views/CashOperation/GLEntres.cshtml", glReport);
            }
        }

        public ActionResult GetGlECountN(string VType, DateTime TrDate)
        {
            return Json(CountEntries(VType, TrDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGlECount(string VType)
        {
            return Json(Count(VType), JsonRequestBehavior.AllowGet);
        }

        public string CountEntries(string pageType, DateTime TrDate)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM VchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'and pvm.VDate='" + TrDate.ToString("MM/dd/yyyy") + "'");
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }

        public string Count(string pageType)
        {
            string countNo = "";
            string sql = string.Format("SELECT COUNT(*) as NO FROM PVchrMain"
                + " as pvm INNER JOIN JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup ='" + pageType + "'");
            List<JarnalVoucher> Number = _jarnalVoucherService.SqlQueary(sql).ToList();
            if (Number.Count == 0)
            {
                countNo = "0";
            }
            else
            {
                countNo = Number.FirstOrDefault().NO.ToString();
            }
            return countNo;
        }

        public ActionResult LoadandSaveJob(string nJobNo)
        {
            return Json(LoadDropDown.LoadandSaveJob(nJobNo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadCustSelfInvoiceNo(string CustCode)
        {
            string SubName = _subsidiaryInfoService.All().ToList().Where(x => x.SubCode == CustCode).Select(x => x.SubName).FirstOrDefault();
            String firstCode = SubName.Substring(0, 3);
            var CustSelfInvoiceNo = "";

            int Sl = 0;
            CustSelfInvoiceNo = _SalesMainService.All().ToList().Where(x => x.CustCode == CustCode).Select(x => x.CustInvSl).FirstOrDefault();
            if (String.IsNullOrEmpty(CustSelfInvoiceNo))//first CustSelfInvoiceNo 
            {
                Sl = 1;

            }
            else // serialize  no
            {
                //Get Sl from CustInvSl

                string[] Info_CustSInvoice = CustSelfInvoiceNo.Split('/');


                Sl = Int32.Parse(Info_CustSInvoice[1]) + 1;

            }



            CustSelfInvoiceNo = Convert.ToString(firstCode) + "/" + Sl.ToString().PadLeft(3, '0') + "/" + DateTime.Now.Year.ToString();

            return Json(CustSelfInvoiceNo, JsonRequestBehavior.AllowGet);
        }

    }
}
