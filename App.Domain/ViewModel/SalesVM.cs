using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SalesVM
    {

        public long SalesMainID { get; set; }
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string IssueNo { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string CustCode { get; set; }
        public string LocCode { get; set; }
        public string BranchCode { get; set; }
        public Nullable<decimal> Transport { get; set; }
        public Nullable<decimal> Labour { get; set; }
        public decimal TotSaleAmt { get; set; }
        public decimal NetAmount { get; set; }
        public Nullable<decimal> PrevDue { get; set; }
        public string Remarks { get; set; }
        public string VchrNo { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Discount { get; set; }        
        public string AppBy { get; set; }
        public string IssueBy { get; set; }

        public long SalesDetailID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> SaleQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> VATPerc { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public decimal DiscPerc { get; set; }
        public Nullable<decimal> DiscAmt { get; set; }
        public decimal ExtQty { get; set; }
        public decimal ExtUPrice { get; set; } 
        public string ItemType { get; set; }        
        public double AdditionalAmt { get; set; }
        public int CopyQty { get; set; }
        public string CopyType { get; set; }
        public string Description { get; set; }
        public string LotNo { get; set; }
        public string tSaleNo { get; set; }
        public string JobNo { get; set; }
        public string HSCode { get; set; } 
        public string ProdSer { get; set; }
        public string RegNo { get; set; }
        public string RegType { get; set; }

        //packet list
        public int PL_ID { get; set; }
        public string PL_Item { get; set; }
        public string PackItemCode { set; get; }
        public double PL_PackSize { set; get; }
        public int PL_PackPcs { get; set; }
        public Nullable<int> PL_TotalQty { get; set; }
    }

}
