using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    [Table("SysSet")]
    public class SysSet
    {
        [Key]
        public int SysId { set; get; }
        public bool Budget { set; get; }
        public bool ConsolidatedCash { set; get; }
        public bool NotProfit { set; get; }
        public bool VouSlTypeWise { set; get; }
        public bool HasProject { set; get; }
        public bool HasBranch { set; get; } 
        public bool HasDept { set; get; }
        public bool HasUnit { set; get; }
        public bool HasParty { set; get; }
        public bool MultiFinYear { set; get; }
        public bool HeadWiseNarration { set; get; }
        public bool AlwaysMultiple { set; get; }
        public bool MaintOldCode { set; get; }
        public string BudgetAllocation { set; get; }
        public bool RecurringVoucher { set; get; }
        public bool ShowJobName { set; get; }
        public bool autovouaftersave { set; get; }
        public int VchrLen { set; get; }
        public string MRConv { get; set; }
        public int InvoiceFormat { get; set; }
        public bool MaintJob { get; set; }
        public bool MaintLot { get; set; }
        public bool MaintVAT { get; set; }
        public bool OnlyVAT { get; set; } 
        public bool Stockedcheck { set; get; }
        public DateTime? OpenDate { set; get; }
        public bool MakeChallanAuto { get; set; }
        public bool MakeRecvAuto { get; set; }
        public bool OnlyGL { get; set; }
        public bool HasSale { get; set; } 
        public bool HasRemittance { get; set; }
        public bool IsSubsidiaryCtrl { get; set; }
        public bool NoGrp { get; set; }
        public bool OnlyGrp { get; set; }
        public bool GrpAndSubGrp { get; set; }
        public bool SubSubGrp { get; set; }
        public bool RateChart { get; set; }
        public bool CashBasis { get; set; }
        public bool ActualBasis { get; set; }
        public bool MaintPacking { get; set; }
        public string CashRule { set; get; }

    } 
}
