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
    
    public partial class SalesDetail
    {
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
