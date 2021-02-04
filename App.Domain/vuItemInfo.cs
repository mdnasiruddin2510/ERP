using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class vuItemInfo
    {
        [Key]
        //public string ItemCode { get; set; }
        //public string GroupingName { get; set; }
        //public string ItemType { get; set; }
  
         public string ItemCode { get; set; }
         public string ItemName { get; set; }
         public string LocName { get; set; }
         public string BranchName { get; set; }
         public double CurrQty { get; set; }
    }
}
