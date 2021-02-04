using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ItemWiseSaleVM
    {
        public DateTime SaleDate { get; set; }
        public string ItemName { get; set; }
        public string SaleNo { get; set; }
        public string CustName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SaleQty { get; set; }

    }
}
