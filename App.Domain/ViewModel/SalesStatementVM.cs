using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class SalesStatementVM
    {
        public DateTime SaleDate { get; set; }
        public string SaleNo { get; set; }
        public string Customer { get; set; }
        public decimal SaleAmount { get; set; }
        public Nullable<decimal> Discount{ get; set; }
        public decimal VAT { get; set; }
        public decimal Transport { get; set; }
        public decimal Labour { get; set; }
        public decimal NetAmount { get; set; }

    }
}
