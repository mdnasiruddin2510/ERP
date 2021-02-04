using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{

    public class RateChart
    {
        [Key]
        public int RateChartId { get; set; }
        public string CustCode { get; set; }
        public string ItemCode { get; set; }
        public Nullable<decimal> CommPerc { get; set; }
        public Nullable<decimal> ItemRate { get; set; }
        public string RC { get; set; }
        public string CustType { get; set; }
        public string CustGroup { get; set; }
    }


}
