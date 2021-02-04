using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Ageing
    {
        public string custcode { get; set; }
        public string custname { get; set; }
        public Nullable<decimal> days30 { get; set; }
        public Nullable<decimal> days60 { get; set; }
        public Nullable<decimal> days90 { get; set; }
        public Nullable<decimal> above90 { get; set; }
    }
}
