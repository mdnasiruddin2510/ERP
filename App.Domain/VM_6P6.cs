using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public  class VM_6P6
    {
        public int VDSID { get; set; }
        public byte SerialNo { get; set; }
        public string CertificateNo { get; set; }
        public System.DateTime CertificateDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddr { get; set; }
        public string SupplierBIN { get; set; }
        public byte TaxChallanNo { get; set; }
        public System.DateTime IssueDate { get; set; }
        public decimal TotalValue { get; set; }
        public decimal VATAmount { get; set; }
        public decimal VDSAmount { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDesig { get; set; }
        public System.TimeSpan IssueTime { get; set; }
    }
}
