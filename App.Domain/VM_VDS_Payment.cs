using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_VDS_Payment
    {
        public int VDS_PID { get; set; }
        public string VDS_PaymentNo { get; set; }
        public string Supp_name { get; set; }
        public string Supp_Addr { get; set; }
        public string Supp_BIN { get; set; }
        public decimal TotalValue { get; set; }
        public decimal VAT_Deduct { get; set; }
        public string Challan_No { get; set; }
        public System.DateTime Challan_Dt { get; set; }
        public string AccountCode { get; set; }
        public Nullable<int> SuppID { get; set; }
        public string Remarks { get; set; }
    }
}
