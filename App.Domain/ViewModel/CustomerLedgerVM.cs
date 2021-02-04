using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CustomerLedgerVM
    {
        public int id { set; get; }
        public DateTime TrDare { set; get; }
        public string AcclineNo { set; get; }
        public string InvNo { set; get; }
        public string MrNo { set; get; }
        public string TrType { set; get; }
        public decimal SaleAmt { set; get; }
        public decimal MrAmt { set; get; }
        public decimal RetAmt { set; get; }
        public decimal AdjAmt { set; get; }
        public decimal netamt { set; get; }
        public decimal Closing { set; get; }
    }
}
