using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SubsidiaryGrp
    {
        [Key]
        public int SubGrpId { get; set; }
        public string SubGrp { get; set; }
        public string SubType { get; set; }
    }
}
