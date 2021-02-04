using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class AccountsReceivable
    {
        public string CustCode {get; set;}
        public string CustName {get; set;}
        public decimal OpenBal {get; set;}
        public decimal SoldAmt { get; set; }
        public decimal RcvdAmt { get; set; }
        public decimal CloseBal { get; set; }
    }
}
