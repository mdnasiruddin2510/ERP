using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class OpeningStock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { set; get; }

        [Key, Column(Order = 1)]
        public string LocCode { set; get; }
        [Key, Column(Order = 2)]
        [ForeignKey("LocCode")]
        public virtual Location Location { set; get; }
        public string BranchCode { set; get; }
        [Key, Column(Order = 3)]
        public string FinYear { set; get; }
        [Key, Column(Order = 4)]
        public string LotNo { set; get; }
        [Key, Column(Order = 5)]
        public string ItemCode { set; get; }
        public double UnitPrice { set; get; }
        public double OpenQty { set; get; }
        public double QtyIndetail { set; get; }
        public DateTime OpenDate { set; get; }
        public DateTime ExpDate { set; get; }

        [NotMapped]
        public string ItemType { get; set; }
        [NotMapped]
        public string ItemName { get; set; }

        public double TotalPrice { get; set; }
        public string Remarks { get; set; }
    }
}
