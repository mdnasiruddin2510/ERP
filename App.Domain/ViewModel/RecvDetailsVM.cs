using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class RecvDetailsVM
    {
        public RecvDetailsVM()
        {

        }
        public RecvDetailsVM(string ItemCode, string ItemName, double Qty, double CurrQty, double Price, string LotNo, string RcvNo, double UnitPrice, string Description)
        {
            this.ItemName = ItemName;
            this.ItemCode = ItemCode;
            this.Qty = Qty;
            this.Price = Price;
            this.LotNo = LotNo;
            this.RcvNo = RcvNo;
            this.CurrQty = CurrQty;
            this.UnitPrice = UnitPrice;
            this.Description = Description;
        }
        public double CurrQty { get; set; }
        public double Qty { get; set; }
        public string RcvNo { get; set; }
        public double Price { get; set; }
        public double UnitPrice { get; set; }
        public string LotNo { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }

    }
}
