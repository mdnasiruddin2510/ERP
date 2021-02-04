using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class GlVoucherVM
    {
        public string RecurrVchrNo { set; get; }
        public string AdjJournal { set; get; }
        public string VchrNo { set; get; }
        public string FinYear { set; get; }
        public DateTime VDate { set; get; }
        public string VType { set; get; }
        public string VDesc { set; get; }
        public string ProjCode { set; get; }
        public string BranchCode { set; get; }
        public bool Posted { set; get; }
        public string LoginName { set; get; }
        //PvchrDetail
        public float Amount { get; set; }
        public string Description { get; set; }
        public int SlNo { set; get; }
        public string UnitCode { set; get; }
        public string DeptCode { set; get; }
        public string Accode { set; get; }
        public string RefNo { set; get; }
        public string Narration { set; get; }
        public double DrAmount { set; get; }
        public double CrAmount { set; get; }
        public string Ca_Bk_Op { set; get; }
        public string Sub_Ac { set; get; }
        public string Sub_Ac_Op { set; get; }
        public string ChqNo { set; get; }
        public DateTime ChqDate { set; get; }
        public string BankName { set; get; }
        public string BankBranch { set; get; }
        public int EntryNo { set; get; }
        public int PVMId { set; get; }
        //PVchrMainExt
        public string VchrSrc { set; get; }
        public string VchrSrcRef { set; get; }
        public string VchrAttach { set; get; }

        public string PrepBy { set; get; }
        public string PrepComment { set; get; }
        public DateTime PrepDate { set; get; }
        public string ApprBy { set; get; }
        public string ApprComment { set; get; }
        public DateTime ApprDate { set; get; }
        public string JobNo { get; set; }
    }
}
