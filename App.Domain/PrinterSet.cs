using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PrinterSet
    {
        [Key]
        public int PSetId { get; set; }
        public string PP { get; set; }
        public string PN { get; set; }
        public string PD { get; set; }
        public string PT { get; set; }

    }
}
