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
    
    public partial class CustWiseSummSaleRpt
    {
        public int Id { get; set; }
        public string SubCode { get; set; }
        public string SubName { get; set; }
        public string LocCode { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Commission { get; set; }
        public decimal VAT { get; set; }
        public decimal GrossSale { get; set; }
    }
}
