using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ProductionRptVM
    {
        public DateTime ProdDate { get; set; }
        public int ItemTypeCode { get; set; }
        public string ItemType { get; set; }
        public double Qty { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }

    }
}
