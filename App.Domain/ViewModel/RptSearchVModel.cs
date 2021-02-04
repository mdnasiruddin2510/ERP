using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class RptSearchVModel
    {
        public string LedgerTypeCode { set; get; }
        public string AccountCode { set; get; }
        public string BookType { set; get; }
        public string projCode { set; get; }
        public string BranchCode { set; get; }
        public DateTime fDate { get; set; }
        public DateTime toDate { get; set; }
        public DateTime tDate { get; set; }
        public string JTrGrpId { set; get; }
        public string LevelNo { set; get; }
        public string TrDate { set; get; }
        public int DisControl { get; set; }
        public int DisTransection { get; set; }
        public int Id { get; set; }
        public string VchrNo { get; set; }
        public string FinYear { get; set; }
        public string UnitCode { get; set; }
        public string DeptCode { get; set; }
        public string SubCode { get; set; }
        public string isCheck { get; set; }
        public string CustomerName { set; get; }
        public string Location { set; get; }
        public string DesignationCode { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string Source { get; set; }
    }
}
