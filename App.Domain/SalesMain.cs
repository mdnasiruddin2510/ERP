using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SalesMain
    {
        [Key]
        public long SalesMainID { get; set; }
        public string SaleNo { get; set; }
        public System.DateTime SaleDate { get; set; }
        public string JobNo { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string IssueNo { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string CustCode { get; set; }
        public string LocCode { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> Transport { get; set; }
        public Nullable<decimal> Labour { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public decimal TotSaleAmt { get; set; }
        public decimal NetAmount { get; set; }
        public Nullable<decimal> PrevDue { get; set; }
        public string Remarks { get; set; }
        public string VchrNo { get; set; }
        public string AppBy { get; set; }
        public string IssueBy { get; set; }
        public string ReceiptNo { get; set; }
        public string CustInvSl { get; set; }
        public Nullable<decimal> ReceiptAmt { get; set; }
        public Nullable<decimal> AdjAmt { get; set; } 
        public Nullable<bool> IsPaid { get; set; }
        public string Accode { get; set; }
        public string ca_bk_op { get; set; } 
        [NotMapped]
        public string tSaleNo { get; set; }
        [NotMapped]
        public string RegNo { get; set; }
        [NotMapped]
        public string RegType { get; set; }
    }
}
