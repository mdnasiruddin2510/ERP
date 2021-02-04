using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CurrencyInfo
    {
        [Key]
        public byte CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }
}
