using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PurchaseDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PurDetailID { get; set; }
        [Key]
        [Column(Order = 0)]
        public int PurMainID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string ItemCode { get; set; }
        [Key]
        [Column(Order = 2)]
        public string LotNo { get; set; }
        public string LocNo { get; set; }
        public decimal UPrice { get; set; }
        public decimal PurQty { get; set; }
        public decimal Amount { get; set; }
        public decimal OBQty { get; set; }
        public decimal OBAmt { get; set; }
        public decimal SupTaxAmt { get; set; }
        public string Description { get; set; }
        public decimal VATAmt { get; set; }
        public string VATType { get; set; }
        public string TaxType { get; set; }
    }
}
