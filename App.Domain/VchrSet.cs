using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VchrSet
    {
        [Key]
        public int VchrSetId { get; set; }
        public string RefCap { get; set; }
        public bool MultiEntry { get; set; }
        public string VchrConv { get; set; }
        public byte VchrLen { get; set; }
        public bool MultiProjInVchr { get; set; }
        public bool BothSideMultiple { get; set; }
        public bool Chkvlen { get; set; }
        public string TopsheetRefCap { get; set; }
    }
}
