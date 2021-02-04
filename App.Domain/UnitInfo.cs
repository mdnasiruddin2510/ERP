using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class UnitInfo
    {
        [Key]
        public int UnitID { set; get; }
        public string UnitCode { set; get; }
        public string UnitName { set; get; }
        public string UnitLocalName { set; get; }
        public string UnitDesc { set; get; }
        public string BrCode { get; set; }
    }
}
