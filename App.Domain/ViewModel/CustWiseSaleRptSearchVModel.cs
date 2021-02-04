using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CustWiseSaleRptSearchVModel
    {
        public string LocCode { set; get; }
        public DateTime fDate { get; set; }
        public DateTime toDate { get; set; }
        public int RptType { get; set; }
    }
}
