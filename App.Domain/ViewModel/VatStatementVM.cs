using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VatStatementVM
    {
        public Nullable<int> SerialNo { get; set; }
        public DateTime PurDate { get; set; }
        public Nullable<decimal> OBQty { get; set; }
        public Nullable<decimal> OBValue { get; set; }
        public string Ch_BENO { get; set; }
        public Nullable<DateTime> Ch_BEDate { get; set; }
        public string SuppName { get; set; }
        public string SuppAddr { get; set; }
        public string CustName { get; set; }
        public string CustAddr { get; set; }
        public string R_E_N_No { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> PurQty { get; set; }
        public Nullable<decimal> PurValue { get; set; }
        public Nullable<decimal> ProdQty { get; set; }
        public Nullable<decimal> ProdValue { get; set; }
        public Nullable<decimal> SuppTax { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public Nullable<decimal> IssueProdQty { get; set; }
        public Nullable<decimal> IssueValue { get; set; }
        public Nullable<decimal> CloseQty { get; set; }
        public Nullable<decimal> CloseValue { get; set; }
        public string Remarks { get; set; }

        public Nullable<decimal> IssueQty { get; set; }
        public string Sale_Ch_BENO { get; set; }
        public string Supp_R_E_N_No { get; set; } 
        public Nullable<DateTime> Sale_Ch_BEDate { get; set; }


    }
}
