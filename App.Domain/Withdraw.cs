using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Withdraw
    {

        [Key]
        [Display(Name = "Withdraw No")]
        public string WithdrawNo { set; get; }

        [Display(Name = "Withdraw Date")]
        public DateTime WithdrawDate { set; get; }
        public double Amount { set; get; }

        [Display(Name = "Bank A/C")]

        public string bankAccode { set; get; }
        [ForeignKey("bankAccode")]
        public virtual NewChart NewChart { set; get; }

        [Required]
        public string ChequeNo { set; get; }

        public string FinYear { set; get; }
        public bool GLPost { set; get; }

        public string BranchCode { set; get; }
        public string VoucherNo { set; get; }
        public string JobNo { get; set; }

    }
}
