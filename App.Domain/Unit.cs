using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Unit
    {
        [Key]
        public string UnitCode {set; get;}
        public string UnitName { set; get; }
    }
}
