using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SummaryReport
    {
        public int Id {set; get;}
        public string Ctrl_SubCode  {set; get;}
        public string Ctrl_SubName  {set; get;}
        public decimal OBDebit { set; get; }
        public decimal OBCredit { set; get; }
        public decimal Debit  {set; get;}
        public decimal Credit  {set; get;}
        public decimal CBDebit  {set; get;}
        public decimal CBCredit  {set; get;}
        public string SummTotals { set; get; } 
    }
}
