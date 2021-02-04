using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VoucherType
    {
        [Key]
        public int VchrTypeId { get; set; }
        public string TypeDesc { get; set; }
        public string VInitial { get; set; }
    }
}
