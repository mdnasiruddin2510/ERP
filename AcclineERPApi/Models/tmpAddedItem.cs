//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AcclineERPApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tmpAddedItem
    {
        public long ID { get; set; }
        public string SaleNo { get; set; }
        public string ItemCode { get; set; }
        public string LotNo { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public decimal ExtQty { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> VatPerc { get; set; }
        public Nullable<decimal> DiscPerc { get; set; }
        public decimal Amount { get; set; }
        public Nullable<byte> EntrySl { get; set; }
        public Nullable<decimal> ExtUPrice { get; set; }
    }
}