using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class RequsitionVM
    {
        public string Location { get; set; }
        public string ReqNo { get; set; }
        public string User { get; set; }
        public DateTime ReqDate { get; set; }
        public string ItemType { get; set; }
        public string Group { get; set; }
        public long ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unitin { get; set; }
        public string CurStock { get; set; }
        public Nullable<System.DateTime> LastPurDate { get; set; }
        public decimal OrderQty { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Last3mis { get; set; }
        public decimal LastPurQty { get; set; }
    }
}
