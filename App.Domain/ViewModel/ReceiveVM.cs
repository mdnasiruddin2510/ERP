using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ReceiveVM
    {
        public string RcvNo { get; set; }
        public string FromLocCode { get; set; }
        public string ToLocCode { get; set; }
        public string Source { get; set; }
        public string RefNo { get; set; }
        public string Purpose { get; set; }
        public string RecvItem { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public int ExQty { get; set; }
        public double Price { get; set; }
        public string LotNo { get; set; }
        public DateTime RcvDate { get; set; }
        public DateTime RefDate { get; set; }
        public string Remarks { get; set; }
        public string RcvdByCode { get; set; }
        public string AppByCode { get; set; }
        public DateTime RcvdDate { get; set; }
        public DateTime RcvdTime { get; set; }
        public string GardenId { get; set; }
        public bool CreditPur { get; set; }
        public string ItemType { get; set; }

        [NotMapped]
        public string BranchCode { get; set; }

    }
}
