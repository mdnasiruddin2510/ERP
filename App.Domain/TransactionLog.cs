using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace App.Domain
{
    public class TransactionLog
    {
        [Key]
        public Int64 Id { get; set; }
        public string TranForm { get; set; }
        public string TranType { get; set; }
        public string TranDateTime { get; set; }
        public string TranNo { get; set; }
        public string UserName { get; set; }
       

    }
}
