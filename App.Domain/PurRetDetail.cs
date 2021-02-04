using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PurRetDetail
    {
        [Key]
        public int PurRetDetailId { get; set; }
        public int PurRetId { get; set; }
        //[ForeignKey("PurRetId")]
        //public virtual PurRetMain PurRetMain { set; get; } 
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string LotNo { get; set; }
        public decimal UnitPrice { get; set; }
        public int ReturnQty { get; set; }
    }
}
