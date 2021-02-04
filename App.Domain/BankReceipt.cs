using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class BankReceipt
    {
        [Key]
        [Display(Name = "Receipt No")]
        public string BReceiptNo { set; get; }

        [Display(Name = "Receipt Date")]
        public DateTime BReceiptDate { set; get; }

        [Display(Name = "Purpose")]

        public string purAccode { set; get; }
        [ForeignKey("purAccode")]
        public virtual NewChart NewChart { set; get; }

        public string SubCode { set; get; }
        [ForeignKey("SubCode")]
        public virtual SubsidiaryInfo Subsidiary { set; get; }

        [Display(Name = "Reference No")]
        public string RefNo { set; get; }

        public string bankAccode { set; get; }
        [ForeignKey("bankAccode")]
        public virtual NewChart bankNewChart { set; get; }

        public string bankCode { set; get; }
        [ForeignKey("bankCode")]
        public virtual BankInfo BankInfo { set; get; }

        [Required]
        public string ChequeNo { set; get; }

        [Required]
        public DateTime ChequeDate { set; get; }
        public double Amount { set; get; }
        public bool Advance { set; get; }
        public string Remarks { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public string BranchCode { set; get; }
        public string VoucherNo { set; get; }
        public string JobNo { get; set; }
    }
}
