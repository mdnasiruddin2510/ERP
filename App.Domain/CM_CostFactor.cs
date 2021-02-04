using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class CM_CostFactor
    {
       [Key]
       public int CostId { get; set; }
       public string Cost_Type { get; set; }
       public string CostName { get; set; }
       public string Accode { get; set; }
    }
}
