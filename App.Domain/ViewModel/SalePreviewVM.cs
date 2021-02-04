using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SalePreviewVM
    {      
        public string  SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string RefNo { get; set; }
        public string LocCode { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string IssueNo { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> NetAmount { get; set; } 
        public decimal TotSaleAmt { get; set; }
        public string SubAddress { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Transport { get; set; }
        public Nullable<decimal> Labour { get; set; }
        public Nullable<decimal> ReceiptAmt { get; set; }
        public Nullable<decimal> SPDiscount { get; set; }
        public Nullable<decimal> Comission { get; set; }
        public Nullable<decimal> Others { get; set; }
    }
}
