using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CustAdjExt
    {
        [Key]
        public int Id { get; set; }
        public string AdjNo { get; set; }
        public int CustCode { get; set; } 
        public string AdjOn { get; set; }  
        public int  AdjReason { get; set; }
        public decimal AdjAmt { get; set; }
    }
}
