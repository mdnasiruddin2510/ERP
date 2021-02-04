using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{     
    public  class VM_TreasuryAc
    {
        [Key]
        public byte AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }


}
