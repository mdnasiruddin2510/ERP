using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class JTrGrp
    {
        public int JTrGrpId { get; set; }
        public string TranGroup { get; set; }
        public string VType { get; set; }
        public string TypeDesc { get; set; }
        public string TrType { get; set; } 

    }
}
