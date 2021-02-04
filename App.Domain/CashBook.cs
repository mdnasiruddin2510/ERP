using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CashBook
    {
        //public string TranNo { get; set; }
        //public string AcName { get; set; }
        //public string Detail { get; set; }
        //public decimal? Amount { get; set; }
        //public string TranType { get; set; }
        //public string RPType { get; set; }
        //public decimal? OpenBal { get; set; }
        //public decimal? CloseBal { get; set; }
        //public string ClosedBy { get; set; }
        //public TimeSpan ClosingTime { get; set; }


        public int id { get; set; }
        public DateTime TrDate { get; set; }
        public string VType { get; set; }
        public string TrGroup { get; set; }
        public string TrNo { get; set; }
        public string ca_bk_op { get; set; }
        public string Narration { get; set; }
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }


    }
}
