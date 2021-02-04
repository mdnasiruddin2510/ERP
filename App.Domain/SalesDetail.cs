using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SalesDetail
    {
        [Key]
        public long SalesDetailID { get; set; }
        public long SalesMainID { get; set; }
        public string ItemCode { get; set; }
        public string LotNo { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> SaleQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> VATPerc { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> DiscPerc { get; set; }
        public Nullable<decimal> DiscAmt { get; set; }
        public decimal ExtQty { get; set; }
        public Nullable<decimal> ExtUPrice { get; set; } 
    }
}
