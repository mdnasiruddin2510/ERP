using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat9P1_SF_Ka
    {
        public Nullable<int> SerialNo {get; set;}
        public string ItemName {get; set;}
        public string VATType {get; set;}
        public string HSCode { get; set; }
        public decimal PurValue {get; set;}
        public decimal SuppTax { get; set; }
        public Nullable<decimal> VATAmt {get; set;}
        public string Remarks { get; set; }
       
        						
    }
}
