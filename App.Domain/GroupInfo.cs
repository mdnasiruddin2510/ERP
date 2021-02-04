using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class GroupInfo
    {
       [Key]
       public int GroupID { set; get; }

       public string GroupName { set; get; }
     
      // public string BranchCode { set; get; }     
    }
}
