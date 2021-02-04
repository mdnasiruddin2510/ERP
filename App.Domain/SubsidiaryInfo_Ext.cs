using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SubsidiaryInfo_Ext
    {
        [Key]
        public int SubTypeExtID { get; set; }
        public string SubCode { get; set; }
        public string SubCategory { get; set; }
        public string SubAddress { get; set; }
        public string Tel { get; set; }
        public string SubEmail { get; set; }
        public string Fax { get; set; }
        public Nullable<decimal> OpenBal { get; set; }
        public Nullable<DateTime> OpenDate { get; set; }
        public string TIN { get; set; }
        public string BIN { get; set; }
        public string VATRegNo { get; set; }
        public string PostCode { get; set; }
        public string ContPerson { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string ContEmail { get; set; }
        public string CountryCode { get; set; } 
        public string RegNo { get; set; }
        public string RegType { get; set; }
    }
}
