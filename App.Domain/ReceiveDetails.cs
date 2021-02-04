using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ReceiveDetails
    {
        [Key]
        public int Id { get; set; }

        public string RcvNo { get; set; }
        [ForeignKey("RcvNo")]
        public virtual ReceiveMain Receives { get; set; }

        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual ItemInfo Items { get; set; }

        public string Description { get; set; }
        public double Qty { get; set; }
        public double Price { get; set; }
        public double ExQty { get; set; }
        public string LotNo { get; set; }
        public double BadQty { get; set; }
        public string SubCode { get; set; }

    }
}
