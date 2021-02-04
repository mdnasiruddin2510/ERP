using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CashReceipt
    {
        [Key]
        [Display(Name = "Receipt No")]
        public string ReceiptNo { set; get; }

        [Display(Name = "Receipt Date")]
        public DateTime ReceiptDate { set; get; }

        [Display(Name = "Purpose")]

        public string purAccode { set; get; }
        [ForeignKey("purAccode")]
        public virtual NewChart NewChart { set; get; }

        [Display(Name = "Reference No")]
        public string RefNo { set; get; }

        public double Amount { set; get; }
        public bool Advance { set; get; }
        public string Remarks { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public string BranchCode { set; get; }
        public string VoucherNo { set; get; }
        public string JobNo { get; set; }
        public virtual List<CashReceiptSubDetails> CashReceiptSubDetails { get; set; }


    }
}
