using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ChequeArchive
    {
        [Key]
        public int ChqArcID { get; set; }
        public string ChqNo { get; set; }
        public string ChqStatus { get; set; }
        public int? UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
    }
}
