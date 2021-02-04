using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class IssueVM
    {
        
        public string IssueNo { set; get; }
        public DateTime IssueDate { set; get; }
        public string FromLocCode { set; get; }
        public string ToLocCode { set; get; }
        public string DesLocCode { set; get; }
        public string Accode { set; get; }
        public string IssueToSubCode { set; get; }
        public string RefNo { set; get; }
        public DateTime RefDate { set; get; }
        public string Remarks { set; get; }
        public string IssueByCode { set; get; }
        public string AppByCode { set; get; }
        public DateTime IssTime { set; get; }
        public DateTime IssDate { set; get; }
        public double Amount { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public bool IsReceived { set; get; }
        public string GardenId { set; get; }
        public string ItemCode { set; get; }
        public string Description { set; get; }
        public double Qty { set; get; }
        public double Price { set; get; }
        public double ExQty { set; get; }
        public double LotNo { set; get; }
        public string ItemType { get; set; }

        [NotMapped]
        public string BranchCode { get; set; }
    }
}
