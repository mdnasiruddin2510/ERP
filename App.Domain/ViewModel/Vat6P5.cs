using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat6_5
    {
        public string FromLoc { get; set; }
        public string ToLoc { get; set; }
        public string ChallanNo { get; set; }
        public DateTime Challandate { get; set; }
        public TimeSpan ChallanTime { get; set; }
        public byte SLNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalValue { get; set; }
        public decimal VATax { get; set; }
        public string Remarks { get; set; }
        public string CustName { get; set; }

        public string IssuedDesig { get; set; }
        public string Cust_BIN { get; set; }
        public TimeSpan issueTime { get; set; }
        public string IssueAddr { get; set; }

    }
}
