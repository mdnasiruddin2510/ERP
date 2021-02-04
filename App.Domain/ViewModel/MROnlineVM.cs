using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class MROnlineVM
    {
        public int MRId { get; set; }
        public string MRSL { get; set; }
        public string MRNo { get; set; }
        public Nullable<System.DateTime> MRDate { get; set; }
        public string MRAgainst { get; set; }
        public decimal MRAmount { get; set; }
        public string Accode { get; set; }
        public string CustCode { get; set; }  
        public string JobNo { get; set; }
        public int GetwayId { get; set; }
        public string VchrNo { get; set; }
        public string DepositBank { get; set; }
        public Nullable<DateTime> DepositDate { get; set; }
        public Nullable<DateTime> EntryDate { get; set; } 
        public int CollectedBy { get; set; }
        public string Remarks { get; set; }
        public string BranchCode { get; set; }
        public string ProjCode { get; set; }
        public bool Posted { get; set; }
    }
}
