using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class CustomerWiseRateChartVM
    {
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ItemRate { get; set; }

    }
}
