using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    [Table("dbo.Payment")]
    public class Payment
    {
        [Key]
        [Display(Name = "Payment Code")]
        public string PayCode { set; get; }

        [Display(Name = "Payment Date")]
        public DateTime PayDate { set; get; }

        [Display(Name = "Puspose")]
        public string purAccode { set; get; }
        [ForeignKey("purAccode")]
        public virtual NewChart NewChartPur { set; get; }

        [Display(Name = "Banc A/C")]
        public string bankAccode { set; get; }
        [ForeignKey("bankAccode")]
        public virtual NewChart NewChartBank { set; get; }

        [Display(Name = "Cheque No")]
        public string ChequeNo { set; get; }

        [Display(Name = "Cheque Date")]
        public DateTime ChequeDate { set; get; }

        public double Amount { set; get; }

        [Display(Name = "Subsidiary")]
        public string SubCode { set; get; }
        [ForeignKey("SubCode")]
        public virtual SubsidiaryInfo Subsidiary { set; get; }

        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public string BranchCode { set; get; }
        public bool Advance { set; get; }
        public string VoucherNo { set; get; }
        public string JobNo { get; set; }
        public string RefNo { get; set; }
    }
}
