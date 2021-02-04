using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Models
{
    public static class LoadDropDown
    {
        static AcclineERPContext db = new AcclineERPContext();
        public static string CallApi(string url, string token)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var t = JsonConvert.DeserializeObject<TokenResponse>(token);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + t.access_token);

                }
                var response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;

            }
        }
        public static SelectList LoadCancelType()
        {
            var item = (from uc in db.InvoiceCancelDrop
                        select new { CancelReceiptId = uc.CancelReceiptId, CancelReceiptType = uc.CancelReceiptType }).ToList();
            return new SelectList(item.OrderBy(x => x.CancelReceiptId).ToList(), "CancelReceiptId", "CancelReceiptType");
        }
        //public static SelectList LoadTempAddedItem_ItemList()
        //{
        //    var item = (from uc in db.tempAddedItem
        //                select new { ItemCode = uc.ItemCode, ItemName = uc.ItemName }).ToList();
        //    return new SelectList(item.OrderByDescending(x => x.ItemCode).ToList(), "ItemCode", "ItemName");
        //}
        public static SelectList SubsidiaryItem()
        {
            var item = (from uc in db.SubsidiaryType
                        select new { TypeCode = uc.TypeCode, SubType = uc.SubType }).ToList();
            return new SelectList(item.OrderBy(x => x.TypeCode).ToList(), "TypeCode", "SubType");
        }


        public static SelectList LoadAcCodefoMRA()
        {
            //select accode,acname from newchart where accode not in(select parentaccode from newchart)
            var nChart = (from p in db.NewChart select p.ParentAcCode);
            var item = (from uc in db.NewChart
                        where !nChart.Contains(uc.Accode)
                        select new { Accode = uc.Accode, AcName = uc.AcName }).ToList();
            return new SelectList(item.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
        }
        public static SelectList LoadGroupType()
        {
            var item = (from uc in db.GroupType
                        select new { GroupTypeId = uc.GroupTypeId, GroupTypeName = uc.GroupTypeName }).ToList();
            return new SelectList(item.OrderBy(x => x.GroupTypeId).ToList(), "GroupTypeId", "GroupTypeName");
        }
        public static SelectList LoaduserBranchType()
        {
            var item = (from uc in db.SecUserInfo
                        select new { UserID = uc.UserID, UserName = uc.UserName }).ToList();
            return new SelectList(item.OrderBy(x => x.UserID).ToList(), "UserID", "UserName");
        }

        public static SelectList LoadBranchType()
        {
            var item = (from uc in db.Branch
                        select new { BranchCode = uc.BranchCode, BranchName = uc.BranchName }).ToList();
            return new SelectList(item.OrderBy(x => x.BranchCode).ToList(), "BranchCode", "BranchName");
        }

        public static List<PrinterSet> GetPrinterInfo()
        {
            return db.PrinterSet.ToList();
        }

        public static SelectList LoadDepositBankDDL()
        {
            var bankCode = db.Gset.FirstOrDefault().GBa;
            //var item = db.NewChart.SqlQuery("select * from NewChart where Accode like '" + bankCode + "%' and Accode != '" + bankCode + "'").ToList();
            var item = (from uc in db.NewChart
                        where uc.Accode.Contains(bankCode) && uc.Accode != bankCode
                        select new { Accode = uc.Accode, AcName = uc.AcName }).ToList();
            item.Insert(0, new { Accode = "", AcName = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
        }

        public static SelectList LoadVATType()
        {
            var item = (from uc in db.VM_VATType
                        select new { VATType = uc.VATType }).ToList();
            return new SelectList(item.OrderBy(x => x.VATType).ToList(), "VATType", "VATType");
        }

        public static SelectList LoadJobInfo()
        {
            var item = (from uc in db.JobInfo
                        where uc.IsActive == true
                        select new { JobNo = uc.JobNo }).ToList();
            return new SelectList(item.OrderBy(x => x.JobNo).ToList(), "JobNo", "JobNo");
        }

        public static SelectList LoadGetwayProvider()
        {
            var item = (from uc in db.GetwayProvider
                        select new { GetwayId = uc.GetwayId, GetwayName = uc.GetwayName }).ToList();
            return new SelectList(item.OrderBy(x => x.GetwayId).ToList(), "GetwayId", "GetwayName");
        }

        public static SelectList LoadMRAgainstDDL()
        {
            var item = (from uc in db.MRAgainst select new { Accode = uc.Accode, MRAgainstDesc = uc.MRAgainstDesc }).ToList();
            item.Insert(0, new { Accode = "", MRAgainstDesc = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.Accode).ToList(), "Accode", "MRAgainstDesc");
        }

        public static string GetMrAgainstType(string MRAgainst)
        {
            var item = (from uc in db.MRAgainst where uc.Accode == MRAgainst select uc.MRAgainstType).FirstOrDefault();
            return item;
        }

        public static SelectList LoadCountryDDL()
        {
            var item = (from uc in db.Country select new { CountryCode = uc.CountryCode, CountryName = uc.CountryName }).ToList();
            item.Insert(0, new { CountryCode = "0", CountryName = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.CountryCode).ToList(), "CountryCode", "CountryName", "BGD");
        }

        public static SelectList LoadPL_ItemList()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            //items.Add("", "---- Select ----");
            return new SelectList(items, "key", "value");
        }

        public static SelectList LoadPL_PackItemList()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            //items.Add("", "---- Select ----");
            return new SelectList(items, "key", "value");

        }

        public static SelectList LoadSubGrpDDL()
        {
            var item = (from uc in db.SubsidiaryGrp select new { SubGrpId = uc.SubGrpId, SubGrp = uc.SubGrp }).ToList();
            return new SelectList(item.OrderBy(x => x.SubGrpId).ToList(), "SubGrpId", "SubGrp");
        }

        public static SelectList LoadCustomHouseDDL()
        {
            var item = (from uc in db.CustomHouse select new { CHCode = uc.CHCode, CHName = uc.CHName }).ToList();
            item.Insert(0, new { CHCode = "0", CHName = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.CHCode).ToList(), "CHCode", "CHName");
        }
        public static SelectList LoadCurrInfo()
        {
            var item = (from ci in db.CurrencyInfo select new { CurrencyID = ci.CurrencyID, CurrencyName = ci.CurrencyName }).ToList();
            return new SelectList(item.OrderBy(x => x.CurrencyID).ToList(), "CurrencyID", "CurrencyName");
        }

        public static string GetMrAgainst(string Accode)
        {
            var mrADesc = db.MRAgainst.FirstOrDefault(s => s.Accode == Accode).MRAgainstDesc;
            return mrADesc;
        }


        public static void CashRecalculate(string ProjCode, string BrCode, string FY, decimal Amt, DateTime TrDate)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;
            try
            {
                sqlQuery = string.Format("Exec CashRecalculate '" + TrDate.ToString("yyyy/MM/dd") + "','" + Amt + "','" + ProjCode + "','" + BrCode + "','" + FY + "'");

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@TrDate",  Value = TrDate , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Amt",  Value =Amt , Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@ProjCode",  Value =ProjCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@BrCode",  Value =BrCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},
                            };
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void BankRecalculate(DateTime TrDate, decimal Amt, string ProjCode, string BrCode, string FY, string BankAccode)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;
            try
            {
                sqlQuery = string.Format("Exec BankRecalculate '" + TrDate.ToString("yyyy/MM/dd") + "','" + Amt + "','" + ProjCode + "','" + BrCode + "','" + FY + "','" + BankAccode + "'");

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@TrDate",  Value = TrDate , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Amt",  Value =Amt , Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@ProjCode",  Value =ProjCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@BrCode",  Value =BrCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@BankAccode",  Value =BankAccode , Direction = System.Data.ParameterDirection.Input},
                            };
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenerateRecvSlNo(IMoneyReceiptAppService _moneyReceiptService, string branchCode, string sessionBrCode, string ProjCode, string VchrConv)
        {
            string RecptNo = "";
            if (VchrConv == "B")
            {
                var brCode = (branchCode == "") ? sessionBrCode : branchCode;
                var rcvNo = _moneyReceiptService.All().ToList().OrderBy(s => s.MRId).LastOrDefault(x => x.BranchCode == brCode);

                if (string.IsNullOrEmpty(brCode))
                {
                    var recvNo = _moneyReceiptService.All().ToList().OrderBy(s => s.MRId).LastOrDefault(x => x.BranchCode == null);
                    if (recvNo != null)
                    {

                        RecptNo = "00" + (Convert.ToInt64(recvNo.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        RecptNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        RecptNo = brCode + (Convert.ToInt64(rcvNo.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        RecptNo = brCode + "000001";
                    }

                }
            }
            else if (VchrConv == "P")
            {
                //Session["ProjCode"].ToString()
                var PrCode = (ProjCode == "") ? "01" : ProjCode;
                var rcvNo = _moneyReceiptService.All().ToList().OrderBy(s => s.MRId).LastOrDefault(x => x.ProjCode == PrCode);

                if (string.IsNullOrEmpty(PrCode))
                {
                    var recvNo = _moneyReceiptService.All().ToList().OrderBy(s => s.MRId).LastOrDefault(x => x.ProjCode == null);
                    if (recvNo != null)
                    {

                        RecptNo = "00" + (Convert.ToInt64(recvNo.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        RecptNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        RecptNo = PrCode + (Convert.ToInt64(rcvNo.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        RecptNo = PrCode + "000001";
                    }

                }
            }
            else if (VchrConv == "U")
            {
                var recvNo = _moneyReceiptService.All().ToList().OrderBy(s => s.MRId).LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    RecptNo = "00" + (Convert.ToInt64(recvNo.MRSL.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    RecptNo = "00000001";
                }
            }
            return RecptNo;

        }
        public static string GenerateAdjustmentNo(ICustAdjustmentAppService _custAdjustmentService, string branchCode, string sessionBrCode, string ProjCode, string VchrConv)
        {
            string AdjNo = "";
            if (VchrConv == "B")
            {
                var brCode = (branchCode == "") ? sessionBrCode : branchCode;
                var rcvNo = _custAdjustmentService.All().ToList().LastOrDefault(x => x.BranchCode == brCode);

                if (string.IsNullOrEmpty(brCode))
                {
                    var cAdjNo = _custAdjustmentService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                    if (cAdjNo != null)
                    {

                        AdjNo = "00" + (Convert.ToInt64(cAdjNo.AdjNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        AdjNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        AdjNo = brCode + (Convert.ToInt64(rcvNo.AdjNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        AdjNo = brCode + "000001";
                    }

                }
            }
            else if (VchrConv == "P")
            {
                //Session["ProjCode"].ToString()
                var PrCode = (ProjCode == "") ? "01" : ProjCode;
                var rcvNo = _custAdjustmentService.All().ToList().LastOrDefault(x => x.ProjCode == PrCode);

                if (string.IsNullOrEmpty(PrCode))
                {
                    var recvNo = _custAdjustmentService.All().ToList().LastOrDefault(x => x.ProjCode == null);
                    if (recvNo != null)
                    {

                        AdjNo = "00" + (Convert.ToInt64(recvNo.AdjNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        AdjNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        AdjNo = PrCode + (Convert.ToInt64(rcvNo.AdjNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        AdjNo = PrCode + "000001";
                    }

                }
            }
            else if (VchrConv == "U")
            {
                var recvNo = _custAdjustmentService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    AdjNo = "00" + (Convert.ToInt64(recvNo.AdjNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    AdjNo = "00000001";
                }
            }
            return AdjNo;

        }
        public static string GenerateSaleRetNo(ISaleRetMainAppService _saleRetMainService, string branchCode, string sessionBrCode, string ProjCode, string VchrConv)
        {
            string SaleRetNo = "";
            if (VchrConv == "B")
            {
                var brCode = (branchCode == "") ? sessionBrCode : branchCode;
                var rcvNo = _saleRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == brCode);

                if (string.IsNullOrEmpty(brCode))
                {
                    var cAdjNo = _saleRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                    if (cAdjNo != null)
                    {

                        SaleRetNo = "00" + (Convert.ToInt64(cAdjNo.SaleRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        SaleRetNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        SaleRetNo = brCode + (Convert.ToInt64(rcvNo.SaleRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        SaleRetNo = brCode + "000001";
                    }

                }
            }
            else if (VchrConv == "P")
            {
                //Session["ProjCode"].ToString()
                var PrCode = (ProjCode == "") ? "01" : ProjCode;
                var rcvNo = _saleRetMainService.All().ToList().LastOrDefault(x => x.ProjCode == PrCode);

                if (string.IsNullOrEmpty(PrCode))
                {
                    var recvNo = _saleRetMainService.All().ToList().LastOrDefault(x => x.ProjCode == null);
                    if (recvNo != null)
                    {

                        SaleRetNo = "00" + (Convert.ToInt64(recvNo.SaleRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        SaleRetNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        SaleRetNo = PrCode + (Convert.ToInt64(rcvNo.SaleRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        SaleRetNo = PrCode + "000001";
                    }

                }
            }
            else if (VchrConv == "U")
            {
                var recvNo = _saleRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    SaleRetNo = "00" + (Convert.ToInt64(recvNo.SaleRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    SaleRetNo = "00000001";
                }
            }
            return SaleRetNo;

        }
        public static string GeneratePurRetNo(IPurRetMainAppService _PurRetMainService, string branchCode, string sessionBrCode, string ProjCode, string VchrConv)
        {
            string PurRetNo = "";
            if (VchrConv == "B")
            {
                var brCode = (branchCode == "") ? sessionBrCode : branchCode;
                var rcvNo = _PurRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == brCode);

                if (string.IsNullOrEmpty(brCode))
                {
                    var cAdjNo = _PurRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                    if (cAdjNo != null)
                    {

                        PurRetNo = "00" + (Convert.ToInt64(cAdjNo.PurRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        PurRetNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        PurRetNo = brCode + (Convert.ToInt64(rcvNo.PurRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        PurRetNo = brCode + "000001";
                    }

                }
            }
            else if (VchrConv == "P")
            {
                //Session["ProjCode"].ToString()
                var PrCode = (ProjCode == "") ? "01" : ProjCode;
                var rcvNo = _PurRetMainService.All().ToList().LastOrDefault(x => x.ProjCode == PrCode);

                if (string.IsNullOrEmpty(PrCode))
                {
                    var recvNo = _PurRetMainService.All().ToList().LastOrDefault(x => x.ProjCode == null);
                    if (recvNo != null)
                    {

                        PurRetNo = "00" + (Convert.ToInt64(recvNo.PurRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        PurRetNo = "00000001";
                    }
                }
                else
                {
                    if (rcvNo != null)
                    {
                        PurRetNo = PrCode + (Convert.ToInt64(rcvNo.PurRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                    }
                    else
                    {
                        PurRetNo = PrCode + "000001";
                    }

                }
            }
            else if (VchrConv == "U")
            {
                var recvNo = _PurRetMainService.All().ToList().LastOrDefault(x => x.BranchCode == null);
                if (recvNo != null)
                {

                    PurRetNo = "00" + (Convert.ToInt64(recvNo.PurRetNo.Substring(2, 6)) + 1).ToString().PadLeft(6, '0');
                }
                else
                {
                    PurRetNo = "00000001";
                }
            }
            return PurRetNo;

        }

        public static SelectList LoadAllSecUserGroups(ISecUserGroupAppService _secUserGroupService)
        {
            var items = _secUserGroupService.All().ToList()
                .Select(x => new { x.GroupID, x.GroupName }).ToList();
            items.Insert(0, new { GroupID = 0, GroupName = "---- Select ----" });
            return new SelectList(items, "GroupID", "GroupName");
        }

        public static SelectList LoadAllFinYears(IFYDDAppService _fYDDService)
        {
            var items = _fYDDService.All().ToList()
                .Select(x => new { x.Id, x.FinYear }).OrderByDescending(x => x.Id).ToList();
            //items.Insert(0, new { ID = 0, FinYear = "---- Select ----" });
            return new SelectList(items, "FinYear", "FinYear");
        }

        #region For Common Item Filtering option
        public static SelectList LoadItemBySSGroupID(string ItemType, string GroupId, string SGroupId, string SSGroupId, IItemInfoAppService _itemInfoService, ICommonPVVMAppService _CommonVmService)
        {
            if (SSGroupId != "")
            {
                string sql = string.Format("SELECT dbo.ItemInfo.ItemName,dbo.ItemMap.ItemCode "
                      + " FROM dbo.GroupInfo INNER JOIN "
                     + " dbo.ItemMap ON dbo.GroupInfo.GroupID = dbo.ItemMap.GroupID left JOIN "
                     + " dbo.SGroupInfo ON dbo.ItemMap.SGroupID = dbo.SGroupInfo.SGroupID left JOIN "
                     + " dbo.SSGroupInfo ON dbo.ItemMap.SSGroupID = dbo.SSGroupInfo.SSGroupID INNER JOIN "
                     + " dbo.ItemInfo ON dbo.ItemMap.ItemCode = dbo.ItemInfo.ItemCode " +
                     " where ItemMap.ItemTypeCode ='" + ItemType + "' and ItemMap.GroupID  ='" + GroupId + "' and ItemMap.SGroupID ='" + SGroupId + "' and ItemMap.SSGroupID ='" + SSGroupId + "' group by ItemInfo.ItemName, ItemMap.ItemCode");


                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }
            else
            {
                var items = _itemInfoService.All().ToList()
                .Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }

        }
        public static SelectList LoadItemBySGroupID(string ItemType, string GroupId, string SGroupId, IItemInfoAppService _itemInfoService, ICommonPVVMAppService _CommonVmService)
        {
            if (SGroupId != "")
            {
                string sql = string.Format("SELECT dbo.ItemInfo.ItemName,dbo.ItemMap.ItemCode "
                      + " FROM dbo.GroupInfo INNER JOIN "
                     + " dbo.ItemMap ON dbo.GroupInfo.GroupID = dbo.ItemMap.GroupID INNER JOIN "
                     + " dbo.SGroupInfo ON dbo.ItemMap.SGroupID = dbo.SGroupInfo.SGroupID INNER JOIN "
                     + " dbo.ItemInfo ON dbo.ItemMap.ItemCode = dbo.ItemInfo.ItemCode " +
                     " where ItemMap.ItemTypeCode ='" + ItemType + "' and ItemMap.GroupID  ='" + GroupId + "' and ItemMap.SGroupID ='" + SGroupId + "' group by ItemInfo.ItemName, ItemMap.ItemCode");


                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }
            else
            {
                var items = _itemInfoService.All().ToList()
                .Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }

        }

        public static SelectList LoadSSGroupInfo(string ItemType, string GroupId, string SGroupId, ICommonPVVMAppService _CommonVmService)
        {
            if (SGroupId != "")
            {
                string sql = string.Format("SELECT dbo.SSGroupInfo.SSGroupName,dbo.ItemMap.SSGroupID "
                      + " FROM dbo.GroupInfo INNER JOIN "
                     + " dbo.ItemMap ON dbo.GroupInfo.GroupID = dbo.ItemMap.GroupID left JOIN "
                     + " dbo.SGroupInfo ON dbo.ItemMap.SGroupID = dbo.SGroupInfo.SGroupID left JOIN "
                     + " dbo.SSGroupInfo ON dbo.ItemMap.SSGroupID = dbo.SSGroupInfo.SSGroupID INNER JOIN "
                     + " dbo.ItemInfo ON dbo.ItemMap.ItemCode = dbo.ItemInfo.ItemCode " +
                     " where ItemMap.ItemTypeCode ='" + ItemType + "' and ItemMap.GroupID  ='" + GroupId + "' and ItemMap.SGroupID ='" + SGroupId + "' group by SSGroupInfo.SSGroupName, ItemMap.SSGroupID");

                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.SSGroupID, x.SSGroupName }).ToList();
                items.Insert(0, new { SSGroupID = 0, SSGroupName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.SSGroupName), "SSGroupID", "SSGroupName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }

        public static SelectList LoadSGroupByGroupId(string ItemType, string GroupId, ICommonPVVMAppService _CommonVmService)
        {
            if (GroupId != "")
            {
                string sql = string.Format("SELECT dbo.SGroupInfo.SGroupName, dbo.ItemMap.SGroupID "
                      + " FROM dbo.GroupInfo INNER JOIN "
                     + " dbo.ItemMap ON dbo.GroupInfo.GroupID = dbo.ItemMap.GroupID left JOIN "
                     + " dbo.SGroupInfo ON dbo.ItemMap.SGroupID = dbo.SGroupInfo.SGroupID left JOIN "
                     + " dbo.SSGroupInfo ON dbo.ItemMap.SSGroupID = dbo.SSGroupInfo.SSGroupID INNER JOIN "
                     + " dbo.ItemInfo ON dbo.ItemMap.ItemCode = dbo.ItemInfo.ItemCode " +
                     " where ItemMap.ItemTypeCode ='" + ItemType + "' and ItemMap.GroupID  ='" + GroupId + "' group by SGroupInfo.SGroupName, ItemMap.SGroupID");

                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.SGroupID, x.SGroupName }).ToList();
                items.Insert(0, new { SGroupID = 0, SGroupName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.SGroupName), "SGroupID", "SGroupName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }

        public static SelectList LoadGroupInfoByItemType(string ItemType, ICommonPVVMAppService _CommonVmService)
        {
            if (ItemType != "")
            {
                string sql = string.Format("SELECT dbo.GroupInfo.GroupName, dbo.ItemMap.GroupID "
                      + " FROM dbo.GroupInfo INNER JOIN "
                     + " dbo.ItemMap ON dbo.GroupInfo.GroupID = dbo.ItemMap.GroupID left JOIN "
                     + " dbo.SGroupInfo ON dbo.ItemMap.SGroupID = dbo.SGroupInfo.SGroupID left JOIN "
                     + " dbo.SSGroupInfo ON dbo.ItemMap.SSGroupID = dbo.SSGroupInfo.SSGroupID INNER JOIN "
                     + " dbo.ItemInfo ON dbo.ItemMap.ItemCode = dbo.ItemInfo.ItemCode where ItemMap.ItemTypeCode ='" + ItemType + "' group by GroupInfo.GroupName, ItemMap.GroupID");

                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.GroupID, x.GroupName }).ToList();
                items.Insert(0, new { GroupID = 0, GroupName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.GroupName), "GroupID", "GroupName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }

        public static SelectList LoadItemByItemType(string ItemType, ICommonPVVMAppService _CommonVmService)
        {
            if (ItemType != "")
            {
                string sql = string.Format("select ItemInfo.ItemCode, ItemInfo.ItemName from ItemMap "
                    + "inner join ItemInfo on ItemMap.ItemCode  = ItemInfo.ItemCode where ItemMap.ItemTypeCode ='" + ItemType + "'");

                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }
        public static SelectList LoadItemByGroupId(string ItemType, string GroupId, ICommonPVVMAppService _CommonVmService)
        {
            if (GroupId != "")
            {
                string sql = string.Format("select ItemInfo.ItemCode, ItemInfo.ItemName from ItemMap "
                    + "inner join ItemInfo on ItemMap.ItemCode  = ItemInfo.ItemCode where ItemMap.ItemTypeCode ='" + ItemType + "' and ItemMap.GroupID ='" + GroupId + "' group by ItemInfo.ItemCode, ItemInfo.ItemName");

                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }

        public static SelectList LoadAllSSGroupInfo(ICommonPVVMAppService _CommonVmService)
        {
            string sql = string.Format("select SSGroupID, SSGroupName from SSGroupInfo order by SSGroupID");
            List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
            var items = CommonPVVMs.Select(x => new { x.SSGroupID, x.SSGroupName }).ToList();
            items.Insert(0, new { SSGroupID = 0, SSGroupName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.SSGroupID), "SSGroupID", "SSGroupName");
        }
        public static SelectList LoadSGroupInfo(ICommonPVVMAppService _CommonVmService)
        {
            string sql = string.Format("select SGroupID, SGroupName from SGroupInfo order by SGroupID");
            List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
            var items = CommonPVVMs.Select(x => new { x.SGroupID, x.SGroupName }).ToList();
            items.Insert(0, new { SGroupID = 0, SGroupName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.SGroupID), "SGroupID", "SGroupName");
        }
        public static SelectList LoadGroupInf(ICommonPVVMAppService _CommonVmService)
        {
            string sql = string.Format("select GroupID, GroupName from GroupInfo order by GroupID");
            List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
            var items = CommonPVVMs.Select(x => new { x.GroupID, x.GroupName }).ToList();
            items.Insert(0, new { GroupID = 0, GroupName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.GroupID), "GroupID", "GroupName");
        }
        #endregion

        public static SelectList LoadandSaveJob(string nJobNo)
        {
            JobInfo job = new JobInfo();
            job.JobNo = nJobNo;
            job.IsActive = true;
            db.JobInfo.Add(job);
            db.SaveChanges();

            var item = (from uc in db.JobInfo
                        where uc.IsActive == true
                        select new { JobId = uc.JobId, JobNo = uc.JobNo }).ToList();
            return new SelectList(item.OrderByDescending(x => x.JobId).ToList(), "JobNo", "JobNo");

        }
        public static DefAC GetDefAc()
        {
            return db.DefAC.FirstOrDefault();
        }
        public static SelectList LoadVchrNoByVchrType(string VchrType, IVchrMainAppService _VchrMainService)
        {
            if (!string.IsNullOrEmpty(VchrType))
            {
                //var items = _pVchrMainService.All().ToList().Where(x => x.VType == VchrType)
                //    .Select(x => new { x.VchrNo }).ToList();

                string sql = string.Format("Select * from VchrMain where VType='" + VchrType + "'");
                List<VchrMain> items = _VchrMainService.SqlQueary(sql).ToList();
                return new SelectList(items.OrderBy(x => x.VchrNo), "VchrNo", "VchrNo");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }
        public static SelectList onlyPurpose(INewChartAppService _newChartService)
        {
            string sql = string.Format("Select Id,Accode,AcSyscode, AcName, ParentCode, LevelNo, B_S, I_S, T_B, ParentAcCode, AccType, OldCode, CorpCode, Stock, Subsidiary, BranchCode from NewChart where (parentaccode in (select accode from storeglac)) or (accode in (select accode from storeglac where accode not in (select parentaccode from newchart) ))");
            List<NewChart> newchartList = _newChartService.SqlQueary(sql).ToList();
            if (newchartList.Count == 0)
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");

            }
            else
            {
                //newchartList.Insert(0, new { Accode = " ", AcName = "---- Select ---" });
                return new SelectList(newchartList.OrderBy(x => x.Accode), "Accode", "AcName");
            }
        }
        public static SelectList LoadPurpose(INewChartAppService _orderInfoService)
        {
            var items = _orderInfoService.All().ToList()
                .Select(x => new { x.Accode, x.AcName }).ToList();
            items.Insert(0, new { Accode = "", AcName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.AcName), "Accode", "AcName");
        }

        public static SelectList LoadItem(IItemInfoAppService _itemInfoService)
        {
            var items = _itemInfoService.All().ToList()
                .Select(x => new { x.ItemCode, x.ItemName }).ToList();
            items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
        }

        public static SelectList LoadItemStore(IItemInfoAppService _itemInfoService)
        {
            var items = _itemInfoService.All().ToList()
                .Select(x => new { x.ItemCode, x.ItemName }).ToList();
            items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
        }

        public static SelectList LoadItems(IItemInfoAppService _itemInfoService)
        {
            var items = _itemInfoService.All().ToList()
                .Select(x => new
                {
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemCode + " " + x.ItemName
                }).ToList();
            //items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.ItemName), "ItemCode", "ItemName");
        }
        public static SelectList Subsidiary(ISubsidiaryInfoAppService _subsidiaryService)
        {
            var items = _subsidiaryService.All().ToList()
                .Select(x => new { x.SubCode, x.SubName }).ToList();
            items.Insert(0, new { SubCode = " ", SubName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.SubName), "SubCode", "SubName");
        }

        public static SelectList LoadRecFrom(ISubsidiaryInfoAppService _subsidiaryInfoService)
        {
            var items = _subsidiaryInfoService.All().ToList()
                .Select(x => new { x.SubCode, x.SubName }).ToList();
            items.Insert(0, new { SubCode = " ", SubName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.SubName), "SubCode", "SubName");
        }

        public static SelectList LoadAllLocation(string StoreType, ILocationAppService _locationInfoService)
        {
            if (!string.IsNullOrEmpty(StoreType))
            {
                var items = _locationInfoService.All().ToList()
                    .Select(x => new { x.LocCode, x.LocName }).ToList();
                items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
                return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public static SelectList LoadGLVAccode(string srcDest, INewChartAppService _NewChartService, IIssRecSrcDestAppService _issRecvSrcDestService)
        {
            if (!string.IsNullOrEmpty(srcDest))
            {
                var PAcCodes = _NewChartService.All().Select(x => x.ParentAcCode).ToArray();
                var efinalList = _NewChartService.All().ToList().Where(x => !PAcCodes.Contains(x.Accode)).ToList();
                //efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public static SelectList LoadGLAc(string CashAc, INewChartAppService _NewChartService)
        {
            if (!string.IsNullOrEmpty(CashAc))
            {
                var efinalList = (from uc in _NewChartService.All().ToList()
                                  where uc.ParentAcCode.StartsWith(CashAc)
                                  select new { Accode = uc.Accode, AcName = uc.AcName }).ToList();
                efinalList.Insert(0, new { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public static SelectList ByItmeType(string ItemType, IItemInfoAppService _itemInfoService)
        {
            var items = (dynamic)null;
            if (ItemType != "")
            {
                items = _itemInfoService.All().ToList().Where(x => x.ItemType == Convert.ToInt32(ItemType)).ToList();
            }
            else
            {
                items = _itemInfoService.All().ToList().Select(x => new { x.ItemCode, x.ItemName }).ToList();
            }

            //items.Insert(0, new ItemInfo() { ItemCode = "0", ItemName = "--- Select ---" });
            return new SelectList(items, "ItemCode", "ItemName");

        }

        public static SelectList ByBranch(string Id, IUserBranchAppService _userbranchService, IBranchAppService _branchService)
        {
            var items = (dynamic)null;
            var getbranch = (dynamic)null;
            if (Id != "")
            {
                items = _userbranchService.All().ToList().Where(x => x.Userid == Id).ToList();
                // List<Branch> branchList = new List<Branch>();
                foreach (var item in items)
                {
                    var branch = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
                    getbranch.Add(branch);
                }
                // getbranch = _branchService.All().ToList().Where(x => x.BranchCode == items.).ToList();
            }
            else
            {
                items = _userbranchService.All().ToList().Select(x => new { x.BranchCode }).ToList();
                getbranch = _branchService.All().ToList().Select(x => new { x.BranchCode, x.BranchName }).ToList();
            }

            //items.Insert(0, new ItemInfo() { ItemCode = "0", ItemName = "--- Select ---" });
            return new SelectList(getbranch, "BranchCode", "BranchName");

        }
        public static SelectList LoadAreaPurpuse(string srcDest, INewChartAppService _NewChartService, IIssRecSrcDestAppService _issRecvSrcDestService)
        {
            if (srcDest != "LoadAllIsEdit")
            {
                if (!string.IsNullOrEmpty(srcDest))
                {
                    var srcPurpose = _issRecvSrcDestService.All().ToList().FirstOrDefault(x => x.SrcDestId == srcDest);
                    if (srcPurpose.ActionCtrl != null)
                    {
                        List<NewChart> echarts = _NewChartService.All().ToList().Where(x => x.Accode == srcPurpose.ActionCtrl).ToList();
                        List<NewChart> epCharts = _NewChartService.All().ToList().Where(x => x.ParentAcCode == srcPurpose.ActionCtrl).ToList();
                        List<NewChart> efinalList = new List<NewChart>();
                        if (epCharts.Count != 0)
                        {
                            foreach (var chart in epCharts)
                            {
                                bool key = true;
                                foreach (var temp in epCharts)
                                {
                                    if (chart == temp)
                                    {

                                    }
                                    else
                                    {
                                        if (chart.Accode == temp.ParentAcCode)
                                        {
                                            key = false;
                                        }
                                    }
                                }
                                if (key)
                                {
                                    efinalList.Add(chart);
                                }
                            }
                            efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                            return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
                        }
                        else if (echarts.Count != 0)
                        {
                            foreach (var chart in echarts)
                            {
                                bool key = true;
                                foreach (var temp in echarts)
                                {
                                    if (chart == temp)
                                    {

                                    }
                                    else
                                    {
                                        if (chart.Accode == temp.ParentAcCode)
                                        {
                                            key = false;
                                        }
                                    }
                                }
                                if (key)
                                {
                                    efinalList.Add(chart);
                                }
                            }
                            efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                            return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
                        }
                        else
                        {
                            Dictionary<string, string> items = new Dictionary<string, string>();
                            items.Add("", "---- Select ----");
                            return new SelectList(items, "Key", "Value");
                        }

                    }
                    else
                    {
                        Dictionary<string, string> items = new Dictionary<string, string>();
                        items.Add("", "---- Select ----");
                        return new SelectList(items, "Key", "Value");
                    }

                }
                else
                {
                    Dictionary<string, string> items = new Dictionary<string, string>();
                    items.Add("", "---- Select ----");
                    return new SelectList(items, "Key", "Value");
                }
            }
            else
            {
                var efinalList = _NewChartService.All().ToList();
                efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");

                //var PAcCodes = _NewChartService.All().Select(x => x.ParentAcCode).ToArray();
                //var efinalList = _NewChartService.All().ToList().Where(x => !PAcCodes.Contains(x.Accode)).ToList();
                //efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                //return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
            }
        }

        public static SelectList LoadBookType()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("0", "---- Select ----");
            items.Add("1", "Cash");
            items.Add("2", "Bank");
            items.Add("3", "Cash & Bank)");
            return new SelectList(items, "key", "value");
        }

        public static SelectList LoadSubsidiary(ISubsidiaryInfoAppService _subsidiaryInfoService)
        {
            var recvFroms = _subsidiaryInfoService.All().ToList();
            recvFroms.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
            return new SelectList(recvFroms.OrderBy(x => x.SubType).ToList(), "SubCode", "SubName");
        }

        public static SelectList LoadSubsidiary(string srcDest, ISubsidiaryInfoAppService _subsidiaryInfoService, IIssRecSrcDestAppService _issRecvSrcDestService)
        {
            if (srcDest != "LoadAllIsEdit")
            {
                if (!string.IsNullOrEmpty(srcDest))
                {
                    var srcSub = _issRecvSrcDestService.All().ToList().FirstOrDefault(x => x.SrcDestId == srcDest);
                    if (srcSub != null)
                    {
                        var recvFroms = _subsidiaryInfoService.All().ToList().Where(x => x.SubType == srcSub.ActionSub).ToList();
                        recvFroms.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
                        return new SelectList(recvFroms.OrderBy(x => x.SubType).ToList(), "SubCode", "SubName");
                    }
                    else
                    {
                        Dictionary<string, string> items = new Dictionary<string, string>();
                        items.Add("0", "---- Select ----");
                        return new SelectList(items, "Key", "Value");
                    }

                }
                else
                {

                    Dictionary<string, string> items = new Dictionary<string, string>();
                    items.Add("", "---- Select ----");
                    return new SelectList(items, "Key", "Value");
                }

            }
            else
            {
                var recvFroms = _subsidiaryInfoService.All().ToList();
                recvFroms.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
                return new SelectList(recvFroms.OrderBy(x => x.SubType).ToList(), "SubCode", "SubName");
            }
        }

        public static SelectList GetInvMrNoByVchrType(string VchrType, string CustCode, ISalesMainAppService _SalesMainService, IMoneyReceiptAppService _moneyReceiptService, IFYDDAppService _FYDDAppService)
        {

            if (!string.IsNullOrEmpty(VchrType))
            {
                if (VchrType == "Money Receipt")
                {
                    var MRs = _moneyReceiptService.All().ToList().Where(x => x.CustCode == CustCode).ToList();
                    MRs.Insert(0, new MoneyReceipt() { MRId = 0, MRNo = "--- Select ---" });
                    return new SelectList(MRs.OrderBy(x => x.MRNo).ToList(), "MRNo", "MRNo");
                }
                else if (VchrType == "Invoice")
                {
                    var FYear = _FYDDAppService.All().FirstOrDefault().FYDF.Year;
                    var Sales = _SalesMainService.All().ToList().Where(x => x.CustCode == CustCode && x.SaleDate.Year >= FYear).ToList();
                    Sales.Insert(0, new SalesMain() { SalesMainID = 0, SaleNo = "--- Select ---" });
                    return new SelectList(Sales.OrderBy(x => x.SaleNo).ToList(), "SaleNo", "SaleNo");
                }
                else
                {
                    Dictionary<string, string> items = new Dictionary<string, string>();
                    items.Add("0", "---- Select ----");
                    return new SelectList(items, "Key", "Value");
                }

            }
            else
            {

                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }


        public static SelectList LoadSubsidiaryByPurpuse(string purAccode, ISubsidiaryInfoAppService _subsidiaryService, ISubsidiaryCtrlAppService _subsidiaryCtrlService)
        {
            if (!string.IsNullOrEmpty(purAccode))
            {
                string pAccode = purAccode.Substring(0, 7);
                var getSub = _subsidiaryCtrlService.All().ToList().FirstOrDefault(x => x.Accode == pAccode.Trim());
                var getSubType = _subsidiaryCtrlService.All().ToList().FirstOrDefault(x => x.Accode == purAccode.Trim());

                if (getSubType != null)
                {
                    var subsidiary = _subsidiaryService.All().ToList().Where(x => x.SubType == getSubType.SubType).ToList();
                    subsidiary.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
                    return new SelectList(subsidiary.OrderBy(x => x.SubType).ToList(), "SubCode", "SubName");
                }
                else if (getSub != null)
                {
                    var subsidiary = _subsidiaryService.All().ToList().Where(x => x.SubType == getSub.SubType).ToList();
                    subsidiary.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
                    return new SelectList(subsidiary.OrderBy(x => x.SubType).ToList(), "SubCode", "SubName");
                }
                else
                {
                    Dictionary<string, string> items = new Dictionary<string, string>();
                    items.Add("", "---- Select ----");
                    return new SelectList(items, "Key", "Value");
                }

            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public static SelectList LoadSrcDestByPurpose(string purAccode, INewChartAppService _NewChartService, IIssRecSrcDestAppService _issRecvSrcDestService)
        {
            if (!string.IsNullOrEmpty(purAccode))
            {
                string eAccode = purAccode.Substring(0, 7);

                string iAccode = purAccode.Substring(0, 3);

                var srcDestE = _issRecvSrcDestService.All().ToList().Where(x => x.ActionCtrl == eAccode).ToList();

                var srcDestI = _issRecvSrcDestService.All().ToList().Where(x => x.ActionCtrl == iAccode).ToList();

                if (srcDestE.Count != 0 && srcDestI.Count == 0)
                {
                    srcDestI.Insert(0, new IssRecSrcDest() { SrcDestId = "0", SrcDestName = "--- Select ---" });
                    return new SelectList(srcDestE.OrderBy(x => x.SrcDestName).ToList(), "SrcDestId", "SrcDestName");
                }
                else if (srcDestE.Count == 0 && srcDestI.Count != 0)
                {
                    srcDestI.Insert(0, new IssRecSrcDest() { SrcDestId = "0", SrcDestName = "--- Select ---" });
                    return new SelectList(srcDestI.OrderBy(x => x.SrcDestName).ToList(), "SrcDestId", "SrcDestName");
                }
                else
                {
                    Dictionary<string, string> items = new Dictionary<string, string>();
                    items.Add("", "---- Select ----");
                    return new SelectList(items, "Key", "Value");
                }
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }

        public static SelectList LoadPurposeCO(string purAccode, INewChartAppService _NewChartService)
        {
            if (!string.IsNullOrEmpty(purAccode))
            {
                var purpose = _NewChartService.All().ToList().Where(x => x.Accode == purAccode).ToList();
                //purpose.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(purpose.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }
        public static SelectList LoadUnit()
        {
            var item = (from uc in db.UnitInfo select new { UnitCode = uc.UnitCode, UnitName = uc.UnitName }).ToList();
            item.Insert(0, new { UnitCode = "", UnitName = "All" });
            return new SelectList(item.OrderBy(x => x.UnitCode).ToList(), "UnitCode", "UnitName");
        }
        public static string LoadUnitInfo(string UnitCode)
        {
            string uName = "";
            if (UnitCode == "")
            {
                uName = "All";
            }
            else
            {
                uName = db.UnitInfo.FirstOrDefault(s => s.UnitCode == UnitCode).UnitName;
            }
            return uName;
        }
        public static string LoadDeptInfo(string DeptCode)
        {
            string dName = "";
            if (DeptCode == "")
            {
                dName = "All";
            }
            else
            {
                dName = db.Department.FirstOrDefault(s => s.DeptCode == DeptCode).DeptDesc;
            }
            return dName;
        }
        public static SelectList LoadDept()
        {
            var item = (from uc in db.Department select new { DeptCode = uc.DeptCode, DeptDesc = uc.DeptDesc }).ToList();
            item.Insert(0, new { DeptCode = "", DeptDesc = "All" });
            return new SelectList(item.OrderBy(x => x.DeptCode).ToList(), "DeptCode", "DeptDesc");
        }
        public static SelectList LoadAccount(string ledgTypeCode, IItemInfoAppService _ItemService, ISubsidiaryInfoAppService _SubsidiaryService, INewChartAppService _NewChartService)
        {
            if (ledgTypeCode == "11")
            {
                List<NewChart> charts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "3" || x.Accode.Substring(0, 1) == "4").ToList();
                List<NewChart> finalList = new List<NewChart>();

                foreach (var chart in charts)
                {
                    bool key = true;
                    foreach (var temp in charts)
                    {
                        if (chart == temp)
                        {

                        }
                        else
                        {
                            if (chart.Accode == temp.ParentAcCode)
                            {
                                key = false;
                            }
                        }
                    }
                    if (key)
                    {
                        finalList.Add(chart);
                    }
                }
                finalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(finalList.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
            }

            else if (ledgTypeCode == "02")
            {
                var subsidiary = _SubsidiaryService.All().ToList();
                subsidiary.Insert(0, new SubsidiaryInfo() { SubCode = "0", SubName = "--- Select ---" });
                return new SelectList(subsidiary.OrderBy(x => x.SubCode).ToList(), "SubCode", "SubName");
            }
            else if (ledgTypeCode == "03")
            {
                var item = _ItemService.All().ToList();
                item.Insert(0, new ItemInfo() { ItemCode = "0", ItemName = "--- Select ---" });
                return new SelectList(item.OrderBy(x => x.ItemCode).ToList(), "ItemCode", "ItemName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }

        public class FinalChartList
        {
            public string Accode { get; set; } 
            public string AcName { get; set; }
        }
        public static SelectList LoadIssuetype(string srcDest, INewChartAppService _NewChartService, IIssRecSrcDestAppService _issRecvSrcDestService)
        {
            if (!string.IsNullOrEmpty(srcDest))
            {
                var PAcCodes = _issRecvSrcDestService.All().Where(x => x.Type == srcDest).Select(x => x.ActionCtrl).ToArray();
                var efinalList = _NewChartService.All().ToList().Where(x => !PAcCodes.Contains(x.Accode)).ToList();
                // efinalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(efinalList.OrderBy(x => x.AcName).ToList(), "Accode", "AcName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }
        }
        public static SelectList LoadItemByChallanNo(string ChallanNo, ICommonPVVMAppService _CommonVmService)
        {
            if (ChallanNo != "")
            {
                string sql = string.Format("select ii.ItemCode,ii.ItemName from ItemInfo ii "
                    + " inner join SalesDetail sd on ii.ItemCode=sd.ItemCode "
                + " inner join SalesMain sm on sd.SalesMainID=sm.SalesMainID where sm.IssueNo='" + ChallanNo + "'");
                List<CommonPVVM> CommonPVVMs = _CommonVmService.SqlQueary(sql).ToList();
                var items = CommonPVVMs.Select(x => new { x.ItemCode, x.ItemName }).ToList();
                //items.Insert(0, new { ItemCode = "", ItemName = "---Select---" });
                return new SelectList(items.OrderBy(x => x.ItemCode), "ItemCode", "ItemName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---Select---");
                return new SelectList(items, "Key", "Value");
            }

        }
        public static SelectList LoadContlAccount(string Transection, IItemInfoAppService _ItemService, ISubsidiaryInfoAppService _SubsidiaryService, INewChartAppService _NewChartService)
        {
            if (Transection == "1")
            {
                List<NewChart> charts = _NewChartService.All().ToList().Where(x => x.Accode.Substring(0, 1) == "1" || x.Accode.Substring(0, 1) == "2" || x.Accode.Substring(0, 1) == "3" || x.Accode.Substring(0, 1) == "4").ToList();
                List<NewChart> finalList = new List<NewChart>();

                foreach (var chart in charts)
                {
                    bool key = true;
                    foreach (var temp in charts)
                    {
                        if (chart == temp)
                        {

                        }
                        else
                        {
                            if (chart.Accode == temp.ParentAcCode)
                            {
                                key = false;
                            }
                        }
                    }
                    if (key)
                    {
                        finalList.Add(chart);
                    }
                }
                finalList.Insert(0, new NewChart() { Accode = "0", AcName = "--- Select ---" });
                return new SelectList(finalList.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
            }
            else if (Transection == "2")
            {
                string[] PAcCode = _NewChartService.All().ToList().Select(x => x.ParentAcCode).ToArray();
                var charts = (from nc in _NewChartService.All().ToList()
                              where PAcCode.Contains(nc.Accode) //|| nc.Subsidiary == true
                              select new { nc.Accode, nc.AcName, nc.ParentAcCode }).ToList();

                return new SelectList(charts.OrderBy(x => x.Accode).ToList(), "Accode", "AcName");
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                items.Add("", "---- Select ----");
                return new SelectList(items, "Key", "Value");
            }

        }
        public static void journalVoucherRemoval(string Caller, string EntryNo, string VNo, string FY)
        {

            string sqlQuery;
            SqlParameter[] sqlParams;
            try
            {
                sqlQuery = "JPostRemoval @Caller,@EntryNo, @VNo,@FY";
                sqlParams = new SqlParameter[]
                    {
                        new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                        new SqlParameter { ParameterName = "@EntryNo",  Value =EntryNo, Direction = System.Data.ParameterDirection.Input },
                        new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                        new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},
                                
                    };
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);

                }

            }
            catch (Exception)
            {
                throw;
                //handle, Log or throw exception 
            }


            //return result;
        }

        public static void journalVoucherSave(string Caller, string DML, string TNo, string VNo, string FY, string PC, string BC, DateTime VDate, string ca_bk_op, string Login)
        {
            //var result = (dynamic)null;

            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPost @Caller, @DML, @TNo,@VNo,@FY,@PC,@BC,@VDate,@ca_bk_op,@Login ";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@PC",  Value =PC, Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter { ParameterName = "@BC",  Value =BC, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VDate",  Value = VDate, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@ca_bk_op",  Value =ca_bk_op , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception)
            {
                throw;
                //handle, Log or throw exception 
            }


            //return result;
        }

        public static void JPostJVSave(DateTime VDate)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostJV @VDate";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@VDate",  Value = VDate, Direction = System.Data.ParameterDirection.Input },                          
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void ProvitionSalesSave(string Caller, string DML, string TNo, string VNo, string FY, string PC, string BC, string vDate, string Ca_bk_Op, string Login, string JobNo)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostAR @Caller, @DML, @TNo,@VNo,@FY,@PC,@BC,@VDate,@ca_bk_op,@Login, @JobNo ";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input}, 
                                new SqlParameter { ParameterName = "@PC",  Value =PC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@BC",  Value =BC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@VDate",  Value = vDate, Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@ca_bk_op",  Value = Ca_bk_Op, Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@JobNo",  Value =(JobNo==null)? "": JobNo, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void ProvitionPurchase(string Caller, string DML, string TNo, string VNo, string FY, string PC, string BC, string vDate, string Ca_bk_Op, string Login, string JobNo)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostAP @Caller, @DML, @TNo,@VNo,@FY,@PC,@BC,@VDate,@ca_bk_op,@Login,@JobNo ";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input}, 
                                new SqlParameter { ParameterName = "@PC",  Value =PC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@BC",  Value =BC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@VDate",  Value = vDate, Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@ca_bk_op",  Value = Ca_bk_Op, Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@JobNo",  Value =JobNo, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void ProvitionInvRSave(string Caller, string DML, string TNo, string VNo, string FY, string PC, string BC, string vDate, string Login)
        {
            //var result = (dynamic)null;

            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostInvRI @Caller, @DML, @TNo,@VNo,@FY,@PC,@BC,@VDate,@Login ";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input}, 
                                new SqlParameter { ParameterName = "@PC",  Value =PC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@BC",  Value =BC , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@VDate",  Value = vDate, Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    var x = dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void ProvitionInvRSave(string Caller, string DML, string TNo, string VNo, string FY, string Login)
        {
            //var result = (dynamic)null;

            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostInvRI @Caller, @DML, @TNo,@VNo,@FY,@Login ";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},                            
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }

        public static void ProvitionSTSave(string Caller, string DML, string TNo, string VNo, string FY, string Login)
        {
            //var result = (dynamic)null;

            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostInvRI @Caller, @DML, @TNo,@VNo,@FY,@Login";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@Caller",  Value = Caller , Direction = System.Data.ParameterDirection.Input},
                                new SqlParameter { ParameterName = "@DML",  Value =DML, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@TNo",  Value =TNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@VNo",  Value =VNo, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},                            
                                new SqlParameter { ParameterName = "@Login",  Value =Login, Direction = System.Data.ParameterDirection.Input },
                               
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }

        //public static void PurchaseVATSPExce(string VATNo, string PurNo, string ProjCode, string BrCode, string FY)
        //{
        //    string SpName = "";
        //    if (VATNo == "vat6p1")
        //    {
        //        SpName = "VM_VAT6P1";
        //    }
        //    else if (VATNo == "vat6p10")
        //    {
        //        SpName = "VM_VAT6P10";
        //    }
        //    else
        //    {
        //        SpName = "VM_VAT9P1";
        //    }
        //    string sqlQuery;
        //    SqlParameter[] sqlParams;

        //    try
        //    {
        //        sqlQuery = SpName + " @PurNo, @ProjCode, @BrCode, @FY";

        //        sqlParams = new SqlParameter[]
        //                    {
        //                        new SqlParameter { ParameterName = "@PurNo",  Value = PurNo , Direction = System.Data.ParameterDirection.Input},
        //                        new SqlParameter { ParameterName = "@ProjCode",  Value =ProjCode, Direction = System.Data.ParameterDirection.Input },
        //                        new SqlParameter { ParameterName = "@BrCode",  Value =BrCode, Direction = System.Data.ParameterDirection.Input },
        //                        new SqlParameter { ParameterName = "@FY",  Value =FY , Direction = System.Data.ParameterDirection.Input},                            
        //                                                   };

        //        using (AcclineERPContext dbContext = new AcclineERPContext())
        //        {
        //            dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //handle, Log or throw exception 
        //    }


        //    //return result;
        //}

        public static void JPostInvR(DateTime VDate)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostJV @VDate";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@VDate",  Value = VDate, Direction = System.Data.ParameterDirection.Input },                          
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void JPostInvI(DateTime VDate)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostInvI @VDate";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@VDate",  Value = VDate, Direction = System.Data.ParameterDirection.Input },                          
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static void JPostInvST(DateTime VDate)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "JPostInvST @VDate";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@VDate",  Value = VDate, Direction = System.Data.ParameterDirection.Input },                          
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }


            //return result;
        }
        public static SelectList LoadVType(bool OnlyGL)
        {
            if (OnlyGL == true)
            {
                var item = (from vt in db.VoucherType
                            join jtg in db.JTrGrp on vt.VInitial equals jtg.TrType
                            select new { VInitial = vt.VInitial.TrimEnd(), TypeDesc = vt.TypeDesc }).ToList();
                //item.Insert(0, new { AppNo = "--- Select ---" });
                return new SelectList(item.OrderBy(x => x.VInitial).ToList(), "VInitial", "TypeDesc");
            }
            else
            {
                var item = (from uc in db.VoucherType select new { VInitial = uc.VInitial.TrimEnd(), TypeDesc = uc.TypeDesc }).ToList();
                //item.Insert(0, new { AppNo = "--- Select ---" });
                return new SelectList(item.OrderBy(x => x.VInitial).ToList(), "VInitial", "TypeDesc");
            }
        }
        public static SelectList LoadAdjReason()
        {
            var item = (from uc in db.AdjReason select new { Id = uc.Id, Name = uc.Name }).ToList();
            item.Insert(0, new { Id = 0, Name = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.Name).ToList(), "Id", "Name");
        }

        public static SelectList LoadCrNoteReason()
        {
            var item = (from uc in db.VM_DrCrNoteReason
                        where uc.DrCr == "Cr"
                        select new { DrCrNoteId = uc.DrCrNoteId, NoteReason = uc.NoteReason }
                        ).ToList();
            return new SelectList(item.OrderBy(x => x.NoteReason).ToList(), "NoteReason", "NoteReason");
        }
        public static SelectList LoadDrNoteReason()
        {
            var item = (from uc in db.VM_DrCrNoteReason
                        where uc.DrCr == "Dr"
                        select new { DrCrNoteId = uc.DrCrNoteId, NoteReason = uc.NoteReason }
                        ).ToList();
            return new SelectList(item.OrderBy(x => x.NoteReason).ToList(), "NoteReason", "NoteReason");
        }

        public static SelectList LoadVM_TreasuryAc()
        {
            var item = (from uc in db.VM_TreasuryAc select new { AccountCode = uc.AccountCode, AccountName = uc.AccountName }).ToList();
            return new SelectList(item.OrderBy(x => x.AccountCode).ToList(), "AccountName", "AccountName");
        }

        public static SelectList LoadCostFactor()
        {
            var item = (from uc in db.CM_CostFactor
                        select new { CostId = uc.CostId, CostName = uc.CostName }).ToList();
            return new SelectList(item.OrderBy(x => x.CostId).ToList(), "CostId", "CostName");
        }
        public static SelectList LoadBankInfo()
        {

            var item = (from uc in db.BankInfo select new { BankCode = uc.BankCode, BankName = uc.BankName }).ToList();
            return new SelectList(item.OrderBy(x => x.BankCode).ToList(), "BankName", "BankName");
        }
        public static SelectList LoadPItems(IItemInfoAppService _itemInfoService)
        {
            var items = _itemInfoService.All().ToList().Where(x => x.ItemType == 1 || x.ItemType == 2) //x=>x.ItemType==2

                .Select(x => new
                {
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemCode + " " + x.ItemName
                }).ToList();
            //items.Insert(0, new { ItemCode = "", ItemName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.ItemCode), "ItemCode", "ItemName");
        }
        public static SelectList LoadItemType()
        {
            var item = (from uc in db.IM_InvAC
                        select new { ItemType = uc.ItemType, InvType = uc.InvType }).ToList();
            return new SelectList(item.OrderBy(x => x.ItemType).ToList(), "ItemType", "InvType");
        }
        public static SelectList LoadFromLocation(ILocationAppService _locationInfoService)
        {
            var items = _locationInfoService.All().ToList()
                .Select(x => new { x.LocCode, x.LocName }).ToList();
            items.Insert(0, new { LocCode = " ", LocName = "---- Select ----" });
            return new SelectList(items.OrderBy(x => x.LocName), "LocCode", "LocName");
        }

        public static SelectList LoadEmpDlList()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("", "---- Select ----");
            return new SelectList(items, "Key", "Value");
        }

        public static SelectList LoadAllMasterForms(IMasterFormAppService _masterFormService, IDynaCapAppService _dynaCapService)
        {

            var mform = _masterFormService.All().ToList();
            var DCap = _dynaCapService.All().ToList().FirstOrDefault();
            List<MasterForm> ListMF = new List<MasterForm>();

            //PropertyInfo[] props = mform.GetType().GetProperties();

            foreach (var item in mform)
            {
                PropertyInfo[] props = DCap.GetType().GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name == item.FormName)
                    {
                        if (p.GetValue(DCap, null) != null)
                        {
                            MasterForm objMF = new MasterForm();
                            string value = (string)p.GetValue(DCap, null);
                            objMF.FormID = item.FormID;
                            objMF.FormName = value;
                            ListMF.Add(objMF);
                            break;
                        }
                    }

                }
            }

            var ListFDdl = (from lmf in ListMF
                            join mf in _masterFormService.All().ToList() on lmf.FormID equals mf.FormID
                            select new
                            {
                                FormID = lmf.FormID,
                                FormName = lmf.FormName,
                                FormCode = mf.FormName
                            }).ToList();


            var items = ListFDdl.Select(x => new { x.FormCode, x.FormName }).ToList();
            items.Insert(0, new { FormCode = "", FormName = "---- Select ----" });
            return new SelectList(items, "FormCode", "FormName");
        }
        public static SelectList LoadVM_AdjHead()
        {

            var item = (from uc in db.VM_AdjHead select new { AdjHeadId = uc.AdjHeadId, AdjHead = uc.AdjHead }).ToList();
            //item.Insert(0, new { BankCode = 0, BankName = "--- Select ---" });
            return new SelectList(item.OrderBy(x => x.AdjHeadId).ToList(), "AdjHead", "AdjHead");
        }


        public static void InvoiceCancel(string InvNov)
        {
            string sqlQuery;
            SqlParameter[] sqlParams;

            try
            {
                sqlQuery = "InvoiceCancel @InvoiceNo";

                sqlParams = new SqlParameter[]
                            {
                                new SqlParameter { ParameterName = "@InvoiceNo",  Value = InvNov, Direction = System.Data.ParameterDirection.Input },                          
                            };

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    dbContext.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
                }
            }
            catch (Exception ex)
            {
                //handle, Log or throw exception 
            }
        }
    }
}