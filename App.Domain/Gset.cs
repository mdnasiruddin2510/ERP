using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class Gset
    {
       [Key]
       public string GCa { set; get; }
       public string GBa { set; get; }
       public string GCA_HO { set; get; } 
       public string GSa { set; get; }
       public string GSnS { set; get; }
       public string GFA { set; get; }
       public string GLnP { set; get; }
       public string GPnM { set; get; }
       public string GCWinP { set; get; }
       public string GRinT { set; get; }  
        
    }
}
