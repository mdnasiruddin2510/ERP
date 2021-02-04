using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CustAdjustmentVM
    {
        public string AdjNo { get; set; }
        public Nullable<DateTime> AdjDate { get; set; }
        public string AdjType { get; set; }
        public string VchrType { get; set; }
        public int CustCode { get; set; } 
        public string InvNo { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal RecvAmt { get; set; }
        public decimal ReceivableAmt { get; set; }
        public decimal AdjAmt { get; set; }
        public decimal TotAdjAmt { get; set; }
        public string AdjReason { get; set; } 
        public string RefNo { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string Remarks { get; set; }
        public string ApprBy { get; set; }
        public DateTime ApprDate { get; set; }
        public string JobNo { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
    }
}
