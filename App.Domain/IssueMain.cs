using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public  class IssueMain
    {
        
        [Key]
        [Display(Name = "Issue No")]
        public string IssueNo { set; get; }

        [Display(Name = "Issue Date")]
        public DateTime IssueDate { set; get; }
        
        [Display(Name = "From Location")]
        public string FromLocCode { set; get; }        
        public string ToLocCode { set; get; }
        public string StoreLocCode { set; get; }

        [Display(Name = "Destination")]
        public string DesLocCode { set; get; }

        [Display(Name = "Purpose")]
        public string Accode { set; get; }
        [ForeignKey("Accode")]
        public virtual NewChart NewChart { set; get; }
        
        [Display(Name="Issue To")]
        public string IssueToSubCode { set; get; }
        [ForeignKey("IssueToSubCode")]
        public virtual SubsidiaryInfo Subsidiary { set; get; }
        
        [Display(Name = "Reference No")]
        public string RefNo { set; get; }
        public string JobNo { set; get; }  

        [Display(Name = "Reference Date")]
        public DateTime? RefDate { set; get; }
        public string Remarks { set; get; }

        [Display(Name = "Issued By")]
        public string IssueByCode { set; get; }

        [Display(Name = "Approved By")]
        public string AppByCode { set; get; }

        [Display(Name = "Issue Time")]
        public DateTime IssTime { set; get; }

        [Display(Name = "Issue Date")]
        public DateTime IssDate { set; get; }

        public double Amount { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public bool IsReceived { set; get; }
        [NotMapped]
        public string LotNo { get; set; }

        public string BranchCode { set; get; }
        [ForeignKey("BranchCode")]
        public virtual Branch branch { set; get; }

        public string VchrNo { get; set; } 
        public bool cashReceiptStatus { set; get; }
        public string EntrySrc { get; set; }
        public string EntrySrcNo { get; set; } 
        
        [NotMapped]
        public string MakeChallanAuto { get; set; }         
        public virtual List<IssueDetails> IssueDetails { set; get; } 
        [NotMapped]
        public string DesLocCodeText { get; set; } 
    }
}
