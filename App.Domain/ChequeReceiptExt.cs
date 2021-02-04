using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ChequeReceiptExt
    {
        [Key]
        public int ChqReceiptExtID { get; set; }
        public int ChqReceiptID { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public decimal BillAmount { get; set; }
        public decimal AdjAmount { get; set; }
        [NotMapped]
        public Boolean IsPaid_mre { get; set; }
        [NotMapped]
        public Boolean IsAdjust_mre { get; set; }
    }
}
