using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CompanyInfo
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompCode { get; set; }
        public string CompName { get; set; }
        public string CompNameBN { get; set; }
        public string ShortName { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string AddrBN { get; set; } 
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string TINNo { get; set; }
        public string BIN { get; set; }
        public string BINBN { get; set; } 
        public string VATRegNo { get; set; }
    }
}
