using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VchrDetail
    {
        [Key]
        public int VchrDetailId { set; get; }
        public string VchrNo { set; get; }
        public string FinYear { set; get; }
        public int SlNo { set; get; }
        public string UnitCode { set; get; }
        public string DeptCode { set; get; }
        public string Accode { set; get; }
        public string RefNo { set; get; }
        public string Narration { set; get; }
        public double DrAmount { set; get; }
        public double CrAmount { set; get; }
        public string Ca_Bk_Op { set; get; }
        public DateTime ChqDate { set; get; }
        public string BankName { set; get; }
        public string BankBranch { set; get; }
        public int EntryNo { set; get; }
        public int VchrMainId { set; get; }
        public string Sub_Ac { set; get; }
        public string Sub_Ac_Op { get; set; }
        public string ChqNo { get; set; }

    }
}
