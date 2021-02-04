using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CollectionStatementVM
    {
        public DateTime MRDate { get; set; }
        public string MRSL { get; set; }
        public string MRNo { get; set; }
        public string CustName { get; set; }
        public string ChqNo { get; set; }
        public string DepositBank { get; set; }
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }
        public decimal Others { get; set; }
        public decimal LC { get; set; }
        public string EnPn { get; set; }
    }
}
