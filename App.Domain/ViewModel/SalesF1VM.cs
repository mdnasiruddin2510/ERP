using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SalesF1VM
    {
        public bool AutoChallan { get; set; }
        public bool WithoutInv { get; set; }
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string LocCode { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public string CustCode { get; set; }
        public bool AddInCust { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> SaleQty { get; set; }
        public decimal ExtQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Description { get; set; }
        public bool AutoLot { get; set; }
        public bool ManualLot { get; set; }
        public Nullable<decimal> Labour { get; set; }
        public Nullable<decimal> Transport { get; set; }
        public string Remarks { get; set; }
        public int CopyQty { get; set; }
        public string CopyType { get; set; }
        public decimal TotSaleAmt { get; set; }
        public decimal Vat { get; set; }
        public decimal Discount { get; set; }
        public decimal AdditionalAmt { get; set; }
        public decimal NetAmount { get; set; }
        public Nullable<decimal> PrevDue { get; set; }
        public Nullable<decimal> SaleReturn { get; set; }
        public decimal NetPayable { get; set; }

        //For Packing List
        public string PackItemCode { get; set; }
        public string PackSize { get; set; }
        public string PackPcs { get; set; }
        public string PackItem { get; set; }

        //Others
        public string BranchCode { get; set; }
        public string VchrNo { get; set; }
        public string AppBy { get; set; }
        public string IssueBy { get; set; }
        public long SalesMainID { get; set; }
        public long SalesDetailID { get; set; }
        public Nullable<decimal> VATPerc { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public decimal DiscPerc { get; set; }
        public Nullable<decimal> DiscAmt { get; set; }
    }
}
