using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VchrMainExt
    {
        [Key]
        public int VchrExtId { set; get; }
        public string VchrNo { set; get; }
        public string FinYear { set; get; }
        public string VchrSrc { set; get; }
        public string VchrSrcRef { set; get; }
        public string VchrAttach { set; get; }
        public int PVchrMainId { set; get; }
        public string AdjVchrNo { set; get; }
        public string PrepBy { set; get; }
        public string PrepComment { set; get; }
        public DateTime PrepDate { set; get; }
        public string ApprBy { set; get; }
        public string ApprComment { set; get; }
        public DateTime ApprDate { set; get; }
        public string RecurrVchrNo { set; get; }
        [NotMapped]
        public string VType { get; set; }
        [NotMapped]
        public string VDesc { get; set; }
    }
}
