using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SaleRetMain
    {
        [Key]
        public int SaleRetId { get; set; }
        public string SaleRetNo { get; set; }
        public DateTime SaleRetDate { get; set; }
        public string RefNo { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string SaleNo { get; set; }
        public string CustCode { get; set; }
        public string LocNo { get; set; }
        public decimal RetAmt { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<DateTime> PostLedgerDate { get; set; }
        public decimal RetableAmt { get; set; }
        public string ApprBy { get; set; }
        public DateTime ApprDate { get; set; }
        public string JobNo { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
        public string VchrNo { get; set; }
        public bool Posted { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public DateTime EntryDateTime { get; set; }

    }
}
