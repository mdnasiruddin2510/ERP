using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_VDS_Receipt
    {
        public int VDS_RID { get; set; }
        public string VDS_ReceiptNo { get; set; }
        public string Cust_name { get; set; }
        public string Cust_Addr { get; set; }
        public string Cust_BIN { get; set; }
        public decimal TotalValue { get; set; }
        public decimal VAT_Deduct { get; set; }
        public string Challan_No { get; set; }
        public System.DateTime Challan_Dt { get; set; }
        public string AccountCode { get; set; }
        public Nullable<int> CustID { get; set; }
        public string Remarks { get; set; }
    }
}
