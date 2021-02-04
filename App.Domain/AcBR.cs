using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class AcBR
    {
        [Key]
        public string AcBRCode { set; get; }
        public string Accode { set; get; }
        public string BranchCode { set; get; }
        public string Ca_Ba { get; set; }
    }
}
