using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CustWiseSummSaleRpt
    {
        [Key]
        public int Id { set; get; }
        public string SubCode { set; get; }        
        public string SubName { set; get; }
        public string LocCode { set; get; }
        public decimal SaleAmount { set; get; }
        public decimal Discount { set; get; }
        public decimal Commission { set; get; }
        public decimal VAT { set; get; }
        public decimal GrossSale { set; get; }
    }
}
