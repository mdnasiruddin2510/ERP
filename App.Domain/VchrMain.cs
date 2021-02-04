using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VchrMain
    {
        [Key]
        public int VchrMainId { set; get; }
        public string VchrNo { set; get; }
        public string FinYear { set; get; }
        public DateTime VDate { set; get; }
        public string VType { set; get; }
        public string VDesc { set; get; }
        public string ProjCode { set; get; }
        public string BranchCode { set; get; }
        public bool Posted { set; get; }
        public string LoginName { set; get; }
    }
}
