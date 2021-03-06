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
    
    public partial class ItemInfo
    {
        public ItemInfo()
        {
            this.IssueDetails = new HashSet<IssueDetail>();
        }
    
        public string ItemCode { get; set; }
        public Nullable<int> ItemType { get; set; }
        public string ItemName { get; set; }
        public double OpenBal { get; set; }
        public string PartNo { get; set; }
        public string UnitCode { get; set; }
        public string DetUnitCode { get; set; }
        public double Price { get; set; }
        public double PackSize { get; set; }
        public string PackItemCode { get; set; }
        public string AltItemCode { get; set; }
        public string Ratio { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public string Prod_Ser { get; set; }
        public string HSCode { get; set; }
        public Nullable<decimal> ROLevel { get; set; }
        public string TaxHeadingNo { get; set; }
    
        public virtual ICollection<IssueDetail> IssueDetails { get; set; }
    }
}
