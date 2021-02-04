using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_6P2P1
    {
        [Key]
        public long PurSalRegID { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public Nullable<System.DateTime> TrDate { get; set; }
        public Nullable<decimal> OBQty { get; set; }
        public Nullable<decimal> OBValue { get; set; }
        public Nullable<decimal> PurQty { get; set; }
        public Nullable<decimal> PurValue { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public string SuppName { get; set; }
        public string SuppAddr { get; set; }
        public string Supp_R_E_N_No { get; set; }
        public string Ch_BENO { get; set; }
        public Nullable<System.DateTime> Ch_BEDate { get; set; }
        public string SoldItemName { get; set; }
        public Nullable<decimal> IssueProdQty { get; set; }
        public Nullable<decimal> IssueValue { get; set; }
        public Nullable<decimal> SuppTax { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public string CustName { get; set; }
        public string CustAddr { get; set; }
        public string Sale_R_E_N_No { get; set; }
        public string Sale_Ch_BENO { get; set; }
        public Nullable<System.DateTime> Sale_Ch_BEDate { get; set; }
        public Nullable<decimal> CloseQty { get; set; }
        public Nullable<decimal> CloseValue { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> SuppID { get; set; }
        public string SuppCode { get; set; }
        public string CustCode { get; set; }
        public string TranNo { get; set; }
        public string TranType { get; set; }
        public string ItemCode { get; set; }
        public string TaxType { get; set; }
        public string VATType { get; set; }
        public long PurRegID { get; set; }
        public long SalRegID { get; set; }
        public long TRSysDateTime { get; set; }
    }
}
