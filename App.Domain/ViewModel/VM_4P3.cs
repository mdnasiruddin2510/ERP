using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VM_4P3
    {
        public Nullable<byte> Line_No { get; set; }
        public string HSCode { get; set; }
        public string FGItemName { get; set; }
        public string UnitName { get; set; }
        public string RPMItemName { get; set; }
        public string QtyWW { get; set; }
        public decimal PurPrice { get; set; }
        public Nullable<decimal> WastageQty { get; set; }
        public string Waste_Perc { get; set; }
        public string CostName { get; set; }
        public decimal CostAmt { get; set; }
        public string Remarks { get; set; }
        public System.DateTime SubDate { get; set; }
        public System.DateTime FirstDate { get; set; }
    }
}
