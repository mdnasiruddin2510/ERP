using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class IssRcvViewModel
    {
       public PM_IssRec IssRcvInfo { get; set; }
       public ReceiveMain RcvMain { get; set; }
       public IssueMain IssMain { get; set; }
       public IEnumerable<IssueDetails> IssueDetail { get; set; }
       public IEnumerable<ReceiveDetails> ReceiveDetail { get; set; }
    }
}
