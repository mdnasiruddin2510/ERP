using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SalesCollectionStat
    {

        public string custid { get; set; }
        public string custname { get; set; }
        public decimal ob { get; set; } 
        public decimal sales { get; set; }
        public decimal Collection { get; set; }
        public decimal Adjustment { get; set; }
        public decimal cb { get; set; }
        public string custgroup { get; set; }


    }
}
