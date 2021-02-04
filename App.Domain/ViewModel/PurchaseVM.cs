using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class PurchaseVM
    {
        public string LocNo { get; set; }
        public string PurNo { get; set; }
        public DateTime PurDate { get; set; }
        public string JobNo { get; set; }
        public string PurType { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public string  LCNo { get; set; }
        public DateTime LCOpenDate { get; set; }
        public string PurCurrency { get; set; }
        public string ConversionRate { get; set; }
        public string SupCode { get; set; }
        public string Address { get; set; }
        public string RegNo { get; set; }
        public string RegType { get; set; }
        public string B_C_No { get; set; } //SupBillNo to B_C_No
        public DateTime B_C_Date { get; set; } 
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public string LotNo { get; set; }
        public int OpenQty { get; set; }
        public decimal OpeningAmt { get; set; }
        public int PurQty { get; set; }
        public decimal UPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal SupTaxAmt { get; set; }
        public string Description { get; set; } 
        public string VATType { get; set; }
        public decimal VATAmt { get; set; }
        public decimal TotPurAmt { get; set; }
        public DateTime PayDueDate { get; set; }
        public DateTime RebateDueDate { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string Remarks { get; set; }
        public bool Posted { get; set; }
        public string VchrNo { get; set; }
        public bool vat6p1 { get; set; }
        public bool vat6p10 { get; set; }
        public bool vat9p1 { get; set; } 
        public string Accode { get; set; }
        public string Ca_Bk_Op { get; set; }
        public string SubCategory { get; set; }
        public string HSCode { get; set; }
        public string CountryCode { get; set; }
        public string CHCode { get; set; }
        public string VDSAmt { get; set; }

        //For VDS Modal
        public int VDS_PID { get; set; }
        public string VDS_PaymentNo { get; set; }
        public string Supp_name { get; set; }
        public string Supp_Addr { get; set; }
        public string Supp_BIN { get; set; }
        public decimal TotalValue { get; set; }
        public decimal VAT_Deduct { get; set; }
        public string Challan_No { get; set; }
        public System.DateTime Challan_Dt { get; set; }
        public string AccountCode { get; set; }
        public Nullable<int> SuppID { get; set; }
        public string VDSRemarks { get; set; } 


    }
}
