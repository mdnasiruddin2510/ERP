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
    
    public partial class CurrentStock
    {
        public int Id { get; set; }
        public string LocCode { get; set; }
        public string LotNo { get; set; }
        public string ItemCode { get; set; }
        public double CurrQty { get; set; }
        public double UnitPrice { get; set; }
    }
}