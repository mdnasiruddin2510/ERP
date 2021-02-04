using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class DynaCap
    {
        [Key]
        public byte Id { get; set; }
        public string Emp { get; set; }
        public string Code { get; set; }
        public string Proj { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Desig { get; set; }
        public string Dept { get; set; }
        public string Dist { get; set; }
        public string Division { get; set; }
        public string Post { get; set; }
        public string Loc { get; set; }
        public string ItemGrp { get; set; }
        public string ItemSGrp { get; set; }
        public string ItemSSGrp { get; set; } 

    }
}
