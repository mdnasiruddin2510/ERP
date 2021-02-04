using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public partial class VM_6P4
    {
        public int CPID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public System.TimeSpan ChallanTime { get; set; }
        public string IssuedTo { get; set; }
        public string IssuedToAddr { get; set; }
        public string IssuedToBIN { get; set; }
        public byte SerialNo { get; set; }
        public string ItemName { get; set; }
        public string UnitIn { get; set; }
        public decimal ChallanQty { get; set; }
        public string ItemCode { get; set; }
        public string HeadingNo { get; set; }
        public string HSCode { get; set; }
        public string IssueNo { get; set; }
        public string IssuePurpose { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDesig { get; set; }
        public TimeSpan IssueTime { get; set; }
    }
}
