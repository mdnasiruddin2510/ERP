using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class AdVGivenOPBL
    {

        [Key]
        public int AdVGOBId { set; get; }
        public string SubCode { set; get; }
        public string Accode { set; get; }
        public DateTime FinYear { set; get; }
        public Double OpenBal { set; get; }
    }
}
