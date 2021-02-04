using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class BankInfo
    {
        [Key]
        public string BankCode { set; get; }
        public string BankName { set; get; }
    }
}
