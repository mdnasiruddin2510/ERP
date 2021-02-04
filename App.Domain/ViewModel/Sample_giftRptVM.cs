using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Sample_giftRptVM
    {
        public string No { get; set; }
        public string date { get; set; }
        public string Type { get; set; }
        public string Item { get; set; }
        public string Lot { get; set; }
        public double Quantity { get; set; }
        public double U_Price { get; set; }
        public double Amount { get; set; }
        public string Given_To{ get; set; }
    }
}
