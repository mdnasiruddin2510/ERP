using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SubOpenBal
    {

        [Key]
        public int SubOpenId { set; get; }
        public string SubCode { set; get; }
        public string FinYear { set; get; }
        public Double OpenBal { set; get; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; } 

    }
}
