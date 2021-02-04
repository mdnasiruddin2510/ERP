using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class SGroupInfo
    {
       [Key]
       public int SGroupID { set; get; }

       public string SGroupName { set; get; }
     
      // public string BranchCode { set; get; }     
    }
}
