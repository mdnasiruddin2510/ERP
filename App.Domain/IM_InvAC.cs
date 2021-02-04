using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class IM_InvAC
    {
       [Key]
        public int InvAcId  { get; set; }
        public string InvType { get; set; }
        public string Accode { get; set; }
        public int ItemType { get; set; }


    }
}
