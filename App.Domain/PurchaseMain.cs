using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PurchaseMain
    {        
        public int PurMainID { get; set; }
        [Key]
        public string PurNo { get; set; }
        public DateTime PurDate { get; set; }
        public string FinYear { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string LocNo { get; set; }
        public string PurType { get; set; }
        public string JobNo { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string LCNo { get; set; }
        public DateTime? LCOpenDate { get; set; }
        public string SupCode { get; set; }
        public string B_C_No { get; set; } // Bill of entry No or Challan No
        public DateTime? B_C_Date { get; set; } 
        public string PurCurrency { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal TotPurAmt { get; set; }
        public DateTime? PayDueDate { get; set; }
        public DateTime? RebateDueDate { get; set; }
        public string VchrNo { get; set; }
        public string Remarks { get; set; }
        public int EntryBy { get; set; }
        public bool Posted { get; set; }
        public byte RevisionNo { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string Accode { get; set; }
        public string Ca_bk_Op { get; set; }
        public bool VatChPro { get; set; }
        public string CountryCode { get; set; }
        public string CHCode { get; set; }
        public Nullable<decimal> VDSAmt { get; set; }

        [NotMapped]
        public string MakeRecvAuto { get; set; }
        [NotMapped]
        public bool vat6p1 { get; set; }
        [NotMapped]
        public bool vat6p10 { get; set; }
        [NotMapped]
        public bool vat9p1 { get; set; }
        [NotMapped]
        public string RegNo { get; set; }
        [NotMapped]
        public string RegType { get; set; }        
    }
}
