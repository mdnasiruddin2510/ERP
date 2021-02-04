using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SalesCollectionComparisionVM
    {
        public byte monthid { get; set; }
        public string monthname { get; set; }
        public decimal csale { get; set; }
        public decimal cmperc { get; set; }
        public decimal ccol { get; set; }
        public decimal ex_st { get; set; }
        public decimal psale { get; set; }
        public decimal pmperc { get; set; }
        public decimal pcol { get; set; }
        public decimal incdec { get; set; }
        public decimal incdecperc { get; set; }
        public decimal TotalQty_P { get; set; }
        public decimal TotalQty_C { get; set; }
        public decimal AverageTK_C { get; set; }
        public decimal AverageTK_P { get; set; }
        public decimal AverageQty_P { get; set; }
        public decimal AverageQty_C { get; set; }

    }
}
