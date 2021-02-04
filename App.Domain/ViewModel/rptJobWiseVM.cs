using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class rptJobWiseVM
    {
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
        public DateTime fDate { get; set; } 
        public DateTime tDate { get; set; }
        public bool Posted { get; set; }

        public string JobNo { get; set; }
        public double BillAmount { get; set; }
        public double ReceivedAmount { get; set; }
        public double PaidAmount { get; set; } 
        public string VchrNo { get; set; }
        public DateTime VDate { get; set; }
        public string Accode { get; set; } 
        public string AcName { get; set; }
        public string SubName { get; set; } 
        public string Narration { get; set; }
        public double CrAmount { get; set; }
        public double DrAmount { get; set; }
        public string RP { get; set; }
    }
}
