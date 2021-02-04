using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class LotDT
    {
        [Key]
        public int LotDtId { get; set; }
        public string LotNo { get; set; }
        public DateTime LotDate { get; set; }
        public string RefSource { get; set; }
        public string RefNo { get; set; }
    }
}
