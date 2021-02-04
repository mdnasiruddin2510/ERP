using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class IssueDetails
    {
        [Key]
        public int Id { set; get; }

        public string IssueNo { set; get; }
        [ForeignKey("IssueNo")]
        public virtual IssueMain Issues { set; get; }

        public string ItemCode { set; get; }
        [ForeignKey("ItemCode")]
        public virtual ItemInfo Items { set; get; }

        public string Description { set; get; }
        public double Qty { set; get; }
        public double Price { set; get; }
        public double ExQty { set; get; }
        public string LotNo { set; get; }
        public string ItemType { get; set; }

        public string SubCode { get; set; }
        [ForeignKey("SubCode")]
        public virtual SubsidiaryInfo Subsidiary { set; get; }

        //For VAT
        [NotMapped]
        public decimal VATAmt { get; set; }
        [NotMapped]
        public decimal SupTaxAmt { get; set; }
    }
}
