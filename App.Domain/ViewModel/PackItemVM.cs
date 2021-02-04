using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class PackItemVM
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string PackItem { get; set; }
        public string PackItemCode { get; set; }
        public int PackSize { get; set; }
        //public string PackPcs { get; set; }
        public int PackPcs { get; set; }
        public Nullable<int> Qty { get; set; }
        public string PacketLot { get; set; }
        public string PL_Item { get; set; }
        public int Total_Qty { set; get; }
        //saledetail
        public string SaleNo { get; set; }
        public DateTime SaleDate { get; set; }
       // public string ItemCode { get; set; }
       // public string ItemName { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string RefNo { get; set; }
        public string LocCode { get; set; }
        public Nullable<DateTime> RefDate { get; set; }
        public string IssueNo { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
       // public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public decimal TotSaleAmt { get; set; }
        public string SubAddress { get; set; }
        public string Description { get; set; }

     

    }
}
