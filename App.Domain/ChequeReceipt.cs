using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ChequeReceipt
    {
        public string MRSL { get; set; }
        [Key]
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
        public decimal Amount { get; set; }
        public Nullable<DateTime> EncashDate { get; set; }
        public string Remarks { get; set; }
        public string FinYear { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string VchrNo { get; set; }
        public bool PostDated { get; set; }
        public Nullable<DateTime> SuggPlaceDate { get; set; }
        public bool GLPost { get; set; }
        public Nullable<bool> AdjWithBill { get; set; }
        public int MRTing { get; set; }
        public Nullable<DateTime> MRTingTime { get; set; }
        public string ChqStatus { get; set; }
        public int? UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string JobNo { get; set; }

        [NotMapped]
        public string InWord { get; set; }
        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string Reason { get; set; }
        [NotMapped]
        public string oldChqNo { get; set; }
    }
}
