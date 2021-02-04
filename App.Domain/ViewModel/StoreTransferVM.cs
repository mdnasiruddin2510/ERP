using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class StoreTransferVM
    {
        public string StoreType { get; set; }
        public string IssueNo { set; get; }
        public string ReceiveNo { get; set; }
        public DateTime IssueDate { set; get; }
        public string FromLocCode { set; get; }
        public string FromLocCodeAll { set; get; }
        public string ToLocCode { set; get; }
        public string RefNo { set; get; }
        public DateTime RefDate { set; get; }
        public string ItemCode { set; get; }
        public string Description { set; get; }
        public int Qty { set; get; }
        public int ExQty { set; get; }
        public double Price { get; set; } 
        public decimal Amount { get; set; }
        public decimal SupTaxAmt { get; set; }
        public decimal VATAmt { get; set; }
        public decimal TotAmt { get; set; } 
        public string LotNo { set; get; }
        public string Remarks { set; get; }
        public string IssueAppByCode { set; get; }
        public string ReceiveByCode { set; get; }
        public string IssueByCode { get; set; }
        public string AppByCode { set; get; }
        public DateTime Time { set; get; }
        public DateTime AppDate { set; get; }
        public string VchrNo { get; set; }
        public string ItemType { get; set; }
        public string BranchCode { get; set; }
        public decimal TotSupTaxAmt { get; set; }
        public decimal TotVATAmt { get; set; }
        public string HSCode { get; set; }
    }
}
