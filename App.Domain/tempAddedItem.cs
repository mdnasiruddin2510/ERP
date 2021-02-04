using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class tempAddedItem
    {
        [Key]
        public long ID { get; set; }
        public string SaleNo { get; set; }
        public string ItemCode { get; set; }
        public string LotNo { get; set; } 
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public decimal ExtQty { get; set; }
        public Nullable<decimal> ExtUPrice { get; set; } 
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> VatPerc { get; set; } 
        public Nullable<decimal> DiscPerc { get; set; } 
        public decimal Amount { get; set; }
        public Nullable<byte> EntrySl { get; set; }
    }
}
