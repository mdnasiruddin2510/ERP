using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CustAdjustment
    {
        [Key]
        public int CustAdjId { get; set; }
        public string AdjNo { get; set; }
        public Nullable<DateTime> AdjDate { get; set; }
        public int CustCode { get; set; } 
        public decimal AdjAmt { get; set; } 
        public int AdjReason { get; set; } 
        public string ProjCode { get; set; }
        public string BranchCode { get; set; } 
        public string RefNo { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string DrCr { get; set; }         
        public string ApprBy { get; set; }
        public Nullable<DateTime> ApprDate { get; set; }  
        public bool Posted { get; set; }
        public string VchrNo { get; set; } 
        public string FinYear { get; set; }
        public Nullable<DateTime> EntryDateTime { get; set; }
        public string VATType { get; set; }
        public string AdjType { get; set; }
        public string JobNo { get; set; }
        public string Remarks { get; set; }
    }
}
