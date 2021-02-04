using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat6P10
    {
        public Nullable<int> SerialNo { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public decimal TotalValue { get; set; }
        public String Name { get; set; }
        public String Addr { get; set; }
        public String BIN_NID_No { get; set; }
        
    }
}
