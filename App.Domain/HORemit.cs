using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class HORemit
    {
        [Key]
        [Display(Name = "Remittance No")]
        public string RemitNo { set; get; }

        [Display(Name = "Remttance Date")]
        public DateTime RemitDate { set; get; }

        [Required]
        public double Amount { set; get; }

        [Required]
        [Display(Name = "Bank")]
      
        public string bankAccode { set; get; }
          [ForeignKey("bankAccode")]
        public virtual NewChart NewChart { set; get; }

        public string Remarks { set; get; }

        public string BranchCode { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }

        public string VoucherNo { set; get; }


    }
}
