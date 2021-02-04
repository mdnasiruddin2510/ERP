using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PurRetMain
    {
        [Key]
        public int PurRetId { get; set; } 
        public string PurRetNo { get; set; }
        public DateTime PurRetDate { get; set; }
        public string RefNo { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string PurNo { get; set; }
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
