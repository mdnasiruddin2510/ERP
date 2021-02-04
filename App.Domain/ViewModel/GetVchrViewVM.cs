using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class GetVchrViewVM
    {
        public string TranType { get; set; }
        public string TranNo { get; set; }
        public DateTime TrDate { get; set; }
        public string FromLocation { get; set; }
        public string Accode { get; set; }
        public string AcName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
