using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class SubsidiaryType
    {
       [Key]
       [Display(Name = "Type Code")]
       public string TypeCode { set; get; }

       [Display(Name = "Item Type")]
       public string SubType { set; get; }
        

    }
}
