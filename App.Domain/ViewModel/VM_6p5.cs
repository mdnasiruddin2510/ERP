using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VM_6p5
    {
        public int StoreTransferID { get; set; }
        public byte SerialNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitIn { get; set; }
        public decimal ChallanQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public decimal SuppTaxAmt { get; set; }
        public decimal VATRate { get; set; }
        public decimal VATAmt { get; set; }
        public decimal NetValue { get; set; }
        public string ChallanNo { get; set; }
        public System.DateTime ChallanDate { get; set; }
        public System.TimeSpan ChallanTime { get; set; }
        public string IssueAddr { get; set; }
        public string ShipAddr { get; set; }
        public string IssueNo { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDesig { get; set; }
        public System.TimeSpan IssueTime { get; set; }
    }
}
