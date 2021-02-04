using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public  class Deposit
   {
       [Key]
       [Display(Name = "Deposit No")]
       public string DepositNo { set; get; }

       [Display(Name = "Deposit Date")]
       public DateTime DepositDate { set; get; }

       [Display(Name = "Purpose")]

       public string bankAccode { set; get; }
       [ForeignKey("bankAccode")]
       public virtual NewChart NewChart { set; get; }

       public double Amount { set; get; }

       [Display(Name = "Deposit Slip No")]
       public string DSlipNo { set; get; }

       public string FinYear { set; get; }
       public bool GLPost { set; get; }
       public string BranchCode { set; get; }
       public string VoucherNo { set; get; }
       public string JobNo { get; set; }

    }
}
