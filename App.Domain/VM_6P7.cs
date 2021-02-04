using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_6P7
    {
        public int SRID { get; set; }
        public string OrigChallanNo { get; set; }
        public System.DateTime OrigChallanDate { get; set; }
        public System.TimeSpan ChallanTime { get; set; }
        public string ReturnFrom { get; set; }
        public string ReturnFromAddr { get; set; }
        public string ReturnFromBIN { get; set; }
        public string ReturnTo { get; set; }
        public string ReturnToAddr { get; set; }
        public string ReturnToBIN { get; set; }
        public string CrNoteNo { get; set; }
        public System.DateTime CrNoteDate { get; set; }
        public System.TimeSpan CrNoteTime { get; set; }
        public byte SerialNo { get; set; }
        public string ItemName { get; set; }
        public string UnitIn { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal UPriceIncVatSD { get; set; }
        public decimal TotalValue { get; set; }
        public decimal DeductAmount { get; set; }
        public decimal AmtInclVAT { get; set; }
        public decimal VATAmount { get; set; }
        public decimal SDAmount { get; set; }
        public decimal TotTaxAmt { get; set; }
        public string ItemCode { get; set; }
        public string HeadingNo { get; set; }
        public string HSCode { get; set; }
        public string ReturnReason { get; set; }
        public string ReceivedBy { get; set; }
        public string ReceivedDesig { get; set; }
    }
}
