using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_PurRegister1_6P1
    {
        public long PurRegID { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public Nullable<System.DateTime> PurDate { get; set; }
        public Nullable<decimal> OBQty { get; set; }
        public Nullable<decimal> OBValue { get; set; }
        public string Ch_BENO { get; set; }
        public Nullable<System.DateTime> Ch_BEDate { get; set; }
        public string SuppName { get; set; }
        public string SuppAddr { get; set; }
        public string R_E_N_No { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> PurQty { get; set; }
        public Nullable<decimal> PurValue { get; set; }
        public Nullable<decimal> SuppTax { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public Nullable<decimal> IssueProdQty { get; set; }
        public Nullable<decimal> IssueValue { get; set; }
        public Nullable<decimal> CloseQty { get; set; }
        public Nullable<decimal> CloseValue { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SuppID { get; set; }
        public string SuppCode { get; set; }
        public string R_E_N { get; set; }
        public string TranNo { get; set; }
        public string TranType { get; set; }
        public string ItemCode { get; set; }
        public string TaxType { get; set; }
        public string VATType { get; set; }
    }
}
