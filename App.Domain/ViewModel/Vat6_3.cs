using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat6_3
    {
        public Nullable<byte> SerialNo {get; set;}
        public string ItemName {get; set;}
        public string UnitIn {get; set;}
        public decimal ChallanQty { get; set; }
        public decimal UnitPrice {get; set;}
        public decimal TotalValue { get; set; }
        public decimal SuppTaxAmt{get; set;}
        public Nullable<decimal> VATRate { get; set; }
        public decimal VATAmt { get; set; }
        public decimal NetValue { get; set; }
        public string CustName { get; set; }
        public string Cust_BIN { get; set; }
        public string ChallanNo { get; set; }
        public string ShipAddr { get; set; }
        public DateTime ChallanDate { get; set; }
        public string IssueAddr { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDesig { get; set; }
        public Nullable<TimeSpan> issueTime { get; set; }
    }
}
