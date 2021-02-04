using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Expense
    {

        [Key]
        [Display(Name = "Expense No")]
        public string ExpenseNo { set; get; }

        [Display(Name = "Expense Date")]
        public DateTime ExpenseDate { set; get; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { set; get; }

        [Display(Name = "Ref No")]
        public string RefNo { get; set; }

        [Display(Name = "Purpose")]
        public string expPurAccode { set; get; }
        [ForeignKey("expPurAccode")]
        public virtual NewChart NewChart { set; get; }

        public double Amount { set; get; }

        public bool Advance { set; get; }
        public string Remarks { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public string BranchCode { set; get; }
        public string JobNo { set; get; } 

        public virtual List<ExpenseSubDetails> ExpenseSubDetails { get; set; }
    }
}
