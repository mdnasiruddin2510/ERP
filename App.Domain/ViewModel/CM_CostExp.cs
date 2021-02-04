using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CM_CostExp
    {
        public string Cost_Type { get; set; }
        public string CostName { get; set; }
        public Nullable<decimal> CostAmt { get; set; }
        public string DeclarationNo { get; set; }
        public int CostId { get; set; }
    }
}
