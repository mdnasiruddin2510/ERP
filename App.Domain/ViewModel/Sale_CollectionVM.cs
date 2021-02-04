using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Sale_CollectionVM
    {

       
        public string MktName { set; get; }
        public decimal Opening { set; get; }
        public decimal Sales { set; get; }
        public decimal SalesRet { set; get; }
        public decimal SalesPerc { set; get; }
        public decimal Collection { set; get; }
        public decimal Adjustment { set; get; }
        public decimal Closing { set; get; }
        public decimal DuesPerc { set; get; }


    }
}
