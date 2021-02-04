using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class JarnalVoucher
    {
        public string ACCode { set; get; }
        public DateTime VDate { set; get; }
        public string AcName { set; get; }
        public string Narration { set; get; }
        public string SubSidiary { set; get; }
        public double CrAmount { set; get; }
        public double DrAmount { set; get; }
        public string VchrNo { get; set; }
        public bool Posted { get; set; }
        
        public string Sub_Ac { get; set; }
        [NotMapped]
        public int NO { get; set; }
        [NotMapped]
        public int VNO { get; set; }
        [NotMapped]
        public string InWords { get; set; }
        // public virtual List<JarnalVoucher> InWords { set; get; }
        [NotMapped]
        public string BranchCode { get; set; }
    }
}
