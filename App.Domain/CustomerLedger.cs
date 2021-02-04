using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
   public class CustomerLedger
    {
       public string SubCode { set; get; }
        [ForeignKey("SubCode")]
        public virtual SubsidiaryInfo SubsidiaryInfo { set; get; }
        public DateTime TranDate { set; get; }
        //public DateTime FYDF {set; get;}
        public string TranNo { set; get; }
        public string TranType { set; get; }
        public decimal OpenBal { set; get; }
        public decimal Debit { set; get; }
        public decimal Credit { set; get; }
    }
}
