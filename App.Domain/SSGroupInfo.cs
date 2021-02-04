using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class SSGroupInfo
    {
       [Key]
       public int SSGroupID { set; get; }

       public string SSGroupName { set; get; }
     
      // public string BranchCode { set; get; }     
    }
}
