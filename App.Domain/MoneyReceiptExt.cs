using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class MoneyReceiptExt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MRExtId { get; set; }
        [Key, Column(Order = 0)]
        public int MRId { get; set; }
        [Key, Column(Order = 1)]
        public string SaleNo { get; set; }
        public string OrderNo { get; set; }
        public int InstallNo { get; set; }
        public Nullable<System.DateTime> InstallDate { get; set; }
        public decimal Amount { get; set; }
        [NotMapped]
        public Boolean IsPaid_mre { get; set; }
        [NotMapped]
        public Boolean IsAdjust_mre { get; set; }
    }
}
