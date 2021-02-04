using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VM_TaxChart
    {
        public int TaxChartId { get; set; }
        public string HeadingNo { get; set; }
        public string HeadingCode { get; set; }
        public string HS_S_Code { get; set; }
        public string Type_P_S { get; set; }
        public string Origin { get; set; }
        public string VATRate { get; set; }
        public string SDRate { get; set; }
        public Nullable<decimal> FixedRate { get; set; }
        public Nullable<decimal> LLimit { get; set; }
        public Nullable<decimal> ULimit { get; set; }
        public string WeightUnit { get; set; }
        public string Schedule { get; set; }
        public string RulesApp { get; set; }
        public string ExceptHSCode { get; set; }
    }
}
