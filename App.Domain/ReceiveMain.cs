using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ReceiveMain
    {
        [Key]
        [Display(Name = "Receive No")]
        public string RcvNo { get; set; }

        public string BranchCode { get; set; }
        [ForeignKey("BranchCode")]
        public virtual Branch branch { get; set; }

        [Display(Name = "Credit Purchase")]
        public bool CreditPur { get; set; }

        [Display(Name = "Receive Date")]
        public DateTime RcvDate { get; set; }

        public string ChallanNo { get; set; }

        [Display(Name = "From Location")]
        public string FromLocCode { get; set; }

        public string ToLocCode { get; set; }

        public string Source { get; set; }

        [Display(Name = "Purpose")]
        public string Accode { get; set; }
        [ForeignKey("Accode")]
        public virtual NewChart NewChart { get; set; }

        [Display(Name = "Reference No")]
        public string RefNo { get; set; }

        [Display(Name = "Reference Date")]
        public DateTime RefDate { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Received By")]
        public string RcvdByCode { get; set; }

        [Display(Name = "Approved By")]
        public string AppByCode { get; set; }

        [Display(Name = "Received Time")]
        public DateTime RcvdTime { get; set; }

        [Display(Name = "Received Date")]
        public DateTime RcvdDate { get; set; }
        public double Amount { get; set; }
        public string FinYear { get; set; }
        public bool GLPost { get; set; }
        public bool expenseStatus { get; set; }
        public string StoreLocCode { get; set; }
        public string RecvFromSubCode { get; set; }
        public string VchrNo { set; get; }
        public string EntrySrc { set; get; }
        public string EntrySrcNo { set; get; }
        public virtual List<ReceiveDetails> RcvDetails { get; set; }
    }
}
