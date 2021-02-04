using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CostLedger
    {
        [Key]
        public long RecId { set; get; }
        public string LocNo { set; get; }
        public string ItemCode { set; get; }
        public DateTime TrDate { set; get; }
        public double RecQty { set; get; }
        public double RecRate { set; get; }
        public double RecTotal { set; get; }
        public double IssQty { set; get; }
        public double IssRate { set; get; }
        public double IssTotal { set; get; }
        public double BalQty { set; get; }
        public double BalRate { set; get; }
        public double BalTotal { set; get; }
        public string UpdSrc { set; get; }
        public string UpdSrcNo { set; get; }

        [NotMapped]
        public double CurrQty { get; set; }
        [NotMapped]
        public double UnitPrice { get; set; }
        [NotMapped]
        public string UnitName { get; set; }
        [NotMapped]
        public string LotNo { set; get; }
    }
}
