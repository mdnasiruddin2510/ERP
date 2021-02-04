using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat4P3
    {
        //vm_4p3
        public string DeclarationNo { get; set; }
        public DateTime SubDate { get; set; }
        public DateTime FirstDate { get; set; }


        public string OutItemCode { get; set; } 
        public decimal ProductQty { get; set; }
        public string ProductUnit { get; set; }
        public decimal DetailQty { get; set; }
        public string DetailUnit { get; set; }

        public string RPMItemCode { get; set; }
        public string RPMUnit { get; set; }
        public decimal qtyWW { get; set; }
        public decimal WastageQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Waste_Perc { get; set; }
        public decimal Profit { get; set; }
        public decimal SellingPrice { get; set; }


        //CM_CostExp
        public string CostId { get; set; }
        public decimal Amount { get; set; }

    }
}
