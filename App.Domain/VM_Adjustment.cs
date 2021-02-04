using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public  class VM_Adjustment
    {
        [Key]
        public int AdjID { get; set; }
        public string AdjNo { get; set; }
        public System.DateTime AdjDate { get; set; }
        public string AdjType { get; set; }
        public string AdjHead { get; set; }
        public decimal AdjAmount { get; set; }
        public string Remarks { get; set; }
    }
}
