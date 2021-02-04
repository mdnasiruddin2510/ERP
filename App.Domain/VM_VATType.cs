using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_VATType
    {
        [Key]
        public int VATTypeId { get; set; }
        public string VATType { get; set; }
        public int RuleNo { get; set; }
    }
}
