using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ReturnVM 
    {
        public string LocCode { get; set; }
        public string JobNo { get; set; }
        public string SaleRetNo { get; set; }
        public Nullable<DateTime> SaleRetDate { get; set; } 
        public string ChallanNo { get; set; }
        public Nullable<DateTime> ChallanDate { get; set; } 
        public int CustCode { get; set; }
        public string SaleNo { get; set; }
        public Nullable<DateTime> SaleDate { get; set; } 
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; } 
        public double Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public string LotNo { get; set; }
        public string ApprBy { get; set; }
        public DateTime ApprDate { get; set; }
        public string Remarks { get; set; }
        public decimal RetableAmt { get; set; } 
        public decimal RetAmt { get; set; }  
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
        public string Reason { get; set; }

        public string PurRetNo { get; set; }
        public Nullable<DateTime> PurRetDate { get; set; }
        public string PurNo { get; set; }
        public Nullable<DateTime> PurDate { get; set; }  
    }
}
