using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class TVchrMain
    {
        [Key]
        public int TVchrMainId { get; set; }
        public string VchrNo { get; set; }
        public string FinYear { get; set; } 
        public DateTime VDate { get; set; }
        public string VType { get; set; }
        public string VDesc { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public bool Posted { get; set; }
        public string LoginName { get; set; }
        [NotMapped]
        public string TVchrNo { get; set; }
    }
}
