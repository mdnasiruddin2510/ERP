using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_SalRegister1_6P2
    {
        public Nullable<int> SerialNo { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<decimal> OBQty { get; set; }
        public Nullable<decimal> OBValue { get; set; }
        public Nullable<decimal> ProdQty { get; set; }
        public Nullable<decimal> ProdValue { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public string CustName { get; set; }
        public string CustAddr { get; set; }
        public string R_E_N_No { get; set; }
        public string Ch_BENO { get; set; }
        public Nullable<System.DateTime> Ch_BEDate { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> IssueQty { get; set; }
        public Nullable<decimal> IssueValue { get; set; }
        public Nullable<decimal> SuppTax { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> CloseQty { get; set; }
        public Nullable<decimal> CloseValue { get; set; }
        public string Remarks { get; set; }
        public string CustCode { get; set; }
        public string TranNo { get; set; }
        public string TranType { get; set; }
        public string ItemCode { get; set; }
        public string HSCode { get; set; }
        public string ItemDesc { get; set; }
        public string SaleType { get; set; }
        public string TaxType { get; set; }
        public string VATType { get; set; }
        public long SalRegID { get; set; }
    }
}
