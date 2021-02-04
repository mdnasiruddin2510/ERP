using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public  class VM_AdjHead
    {
        [Key]
        public int AdjHeadId { get; set; }
        public string AdjHead { get; set; }
    }
}
