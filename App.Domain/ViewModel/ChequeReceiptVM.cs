using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ChequeReceiptVM
    {
        //For Print
        public string MRNo { get; set; }
        public DateTime MRDate { get; set; }
        public string Code { get; set; }
        public string CustName { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<DateTime> ChequeDate { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AcType { get; set; }
        public string PayMode { get; set; }
        public string MRSL { get; set; }
        //Common
        public decimal Amount { get; set; }
        public string InWord { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }

        //For chequeReceipt, chequeReceiptExt
        public int ChqReceiptId { get; set; }
        public string ChqReceiptNo { get; set; }
        public DateTime ChqReceiptDate { get; set; }
        public string MRAgainst { get; set; }
        public string SubCode { get; set; }
        public string ChqNo { get; set; }
        public DateTime ChqDate { get; set; }
        public string BankCode { get; set; }
        public string BranchName { get; set; }
        public string DepositBank { get; set; }
        public Nullable<DateTime> DepositDate { get; set; }
        public Nullable<DateTime> EncashDate { get; set; }
        public string FinYear { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string VchrNo { get; set; }
        public bool PostDated { get; set; }
        public DateTime SuggPlaceDate { get; set; }
        public bool GLPost { get; set; }
        public int MRTing { get; set; }
        public Nullable<DateTime> MRTingTime { get; set; }
        public string ChqStatus { get; set; }
        public int? UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        
        //For chequeArchive
        public int ChqArcID { get; set; }
        public string Reason { get; set; }
        public string JobNo { get; set; }
    }
}
