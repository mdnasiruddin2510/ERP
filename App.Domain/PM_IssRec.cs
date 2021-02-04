using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class PM_IssRec
    {
       [Key]
       public int PM_IssRecId { get; set; }
       public string BatchNo { get; set; }
       public string RefNo { get; set; }
       public DateTime RefDate { get; set; }
       public string JobNo { get; set; }
       public string IssueNo { get; set; }
       public string ReceiveNo { get; set; }
       public string IssAcCode { get; set; }
       public string RecAcCode { get; set; }
       public decimal AmountR { get; set; }
       [NotMapped]
       public IssueMain IssueMain { get; set; }
       [NotMapped]
       public ReceiveMain ReceiveMain { get; set; }
    }
}
