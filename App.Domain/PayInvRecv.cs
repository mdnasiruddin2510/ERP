using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PayInvRecv
    {
        [Key]
        public string TranNo { get; set; }
        public string RefNo { get; set; }
        public string PType { get; set; }

    }
}
