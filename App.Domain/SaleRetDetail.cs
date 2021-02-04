using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SaleRetDetail
    {
        [Key]
        public int SaleRetDetailId { get; set; }
        public int SaleRetId { get; set; }
        //[ForeignKey("SaleRetId")]
        //public virtual SaleRetMain SaleRetMain { set; get; } 
        public string ItemCode { get; set; } 
        public string Description { get; set; }
        public string LotNo { get; set; }
        public decimal UnitPrice { get; set; }
        public int ReturnQty { get; set; }
    }
}
