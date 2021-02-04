using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class OpenBal
    {        
        [Key]
        public int Id { get; set; }
        public string Accode { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
        public double OpenBalance { get; set; } 
        public double YrDrBal { get; set; }
        public double YrCrBal { get; set; }
        public double Budget { get; set; }
        public string UnitCode { get; set; }
        [NotMapped]
        public string AccName { get; set; }

    }
}
