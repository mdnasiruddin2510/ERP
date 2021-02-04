using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CashReceiptSubDetails
    {
      
        [Key, Column(Order = 0)]
        [Display(Name = "CashReceipt No")]
        public int CashReceiptsubCode { set; get; }

        [Display(Name = "Subsidiary")]
        public string SubCode { set; get; }
        [Column(Order = 1), Key, ForeignKey("SubCode")]
        public virtual SubsidiaryInfo SubsidiaryInfo { set; get; }

        //No need to display 
        [Display(Name = "Purpose")]
        public string Accode { set; get; }
        [Column(Order = 2), Key, ForeignKey("Accode")]
        public virtual NewChart NewChart { set; get; }

        [Display(Name = "Amount")]
        public double Amount { set; get; }

        [Display(Name = "Description")]
        public string Description { set; get; }

        public string CashReceiptNo { set; get; }
        [ForeignKey("CashReceiptNo")]
        public virtual CashReceipt CashReceipt { set; get; }

    
    }
}
