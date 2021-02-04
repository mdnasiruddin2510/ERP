using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CurrentStock
    {
        public int Id {set; get;}

        public string LocCode { set; get; }
        [ForeignKey("LocCode")]
        public virtual Location Location { set; get; }

        public string LotNo {set; get;}
        public string ItemCode {set; get;}
        public double CurrQty { set; get; }
        public double UnitPrice { set; get; }
        // For View
        [NotMapped]
        public string UnitName { get; set; }
        [NotMapped]
        public string HSCode { get; set; }
        [NotMapped]
        public string TaxHeadingNo { get; set; }  
        
    }
}
