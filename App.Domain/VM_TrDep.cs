using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_TrDep
    {
        [Key]
        public int TrDepID { get; set; }

        [Required]
        [DisplayName("Department No")]
        public string TrDepNo { get; set; }

        [Required]
        [DisplayName("Department Date")]
        public System.DateTime TrDepDate { get; set; }

        [DisplayName("Challan Month")]
        public string TrDepMonth { get; set; }

        [DisplayName("Challan Year")]
        public string TrDepYear { get; set; }

        [Required]
        [DisplayName("Challan No")]
        public string TrChallanNo { get; set; }

        [DisplayName("Bank")]
        public string Bank { get; set; }

        [DisplayName("Branch")]
        public string Branch { get; set; }

        [Required]
        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }
    }
}
