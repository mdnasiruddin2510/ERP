using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ItemWiseDisVatVM
    {        
        public string  ItemCode { get; set; }
        public string Description { get; set; }
        public string LotNo { get; set; }
        public Nullable<decimal> DiscPerc { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string HSCode { get; set; }
        public Nullable<decimal> VatPerc { get; set; }
        public string SaleNo { get; set; }
        public decimal Amount { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public decimal ExtQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> ExtUPrice { get; set; }
        public string SubCategory { get; set; }
        public double Price { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string InvoiceNo { get; set; }

    }
}
