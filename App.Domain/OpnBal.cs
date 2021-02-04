using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class OpnBal
    {
        [Key]
        public decimal OpenBalance { set; get; }
        
        [NotMapped]
        public decimal Balance { set; get; }
    }
}
