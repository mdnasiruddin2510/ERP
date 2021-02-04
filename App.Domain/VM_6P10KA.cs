using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_6P10KA
    {
        [Key]
        public long VAT6P10KaID { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public Nullable<System.DateTime> TrDate { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public string SuppName { get; set; }
        public string SuppAddr { get; set; }
        public string Supp_BIN_NID_No { get; set; }
        public string Supp_BIN_NID_Type { get; set; }
    }
}
