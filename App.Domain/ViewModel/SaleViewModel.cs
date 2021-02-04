using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SaleViewModel
    {
        public SalesMain SalesMain { get; set; }
        public IEnumerable<SalesDetail> SalesDetail { get; set; }
        public IssueMain IssueMain { get; set; }
        public IEnumerable<IssueDetails> IssueDetail { get; set; }
        public IEnumerable<CurrentStock> CurrentStock { get; set; }
        public IEnumerable<CostLedger> CostLedger { get; set; }
    }
}
