using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ReportLedger
    {
        public int Id {set; get;}
        public DateTime TrDate {set; get;}
        public string TrType {set; get;}
        public string TrNo {set; get;}
        public string Ctrl_Sub {set; get;}
        public string Narration {set; get;}
        public decimal OpenBal {set; get;}
		public decimal Debit {set; get;}
        public decimal Credit {set; get;}
        public decimal Balance { set; get; }
    }
}
