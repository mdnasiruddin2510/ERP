using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class IssueDetailsVM
    {
        public IssueDetailsVM()
        {

        }
        public IssueDetailsVM(string ItemCode, string ItemName, double Qty, double Price, string IssueNo, string LotNo, double UnitPrice, string Description, double ExQty)
        {
            this.ItemCode = ItemCode;
            this.Qty = Qty;
            this.Price = Price;
            this.IssueNo = IssueNo;
            this.LotNo = LotNo;
            this.ItemName = ItemName;
            this.UnitPrice = UnitPrice;
            this.Description = Description;
            this.ExQty = ExQty;
        }
        public string ItemName { get; set; }
        public string ItemCode { set; get; }
        public string Description { set; get; }
        public double Qty { set; get; }
        public double Price { set; get; }
        public double ExQty { set; get; }
        public string LotNo { set; get; }
        public string IssueNo { get; set; }
        public double UnitPrice { get; set; }
    }
}
