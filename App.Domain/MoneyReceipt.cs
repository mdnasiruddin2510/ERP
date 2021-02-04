using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class MoneyReceipt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MRId { get; set; }
        public string MRSL { get; set; }
        [Key]
        public string MRNo { get; set; }
        public Nullable<System.DateTime> MRDate { get; set; }
        public string CustCode { get; set; }
        public string MRAgainst { get; set; }
        public decimal MRAmount { get; set; }
        public string Accode { get; set; }
        public string VchrNo { get; set; }
        public string PayMode { get; set; }
        public string ChqNo { get; set; }
        public Nullable<System.DateTime> ChqDate { get; set; }
        public string LCNo { get; set; }
        public Nullable<System.DateTime> LCDate { get; set; }
        public string PONo { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public string DDNo { get; set; }
        public Nullable<System.DateTime> DDDate { get; set; }
        public int BankId { get; set; }
        public string Branch { get; set; }
        public string TranId { get; set; }
        public int GetwayId { get; set; }
        public string Ca_Bk { get; set; }
        public Nullable<bool> AdjWithBill { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string FinYear { get; set; }
        public bool Posted { get; set; }
        public int CollectedBy { get; set; }
        public string Remarks { get; set; }
        public string JobNo { get; set; }
        public string BillNo { get; set; }
        public Nullable<DateTime> PayDate { get; set; }
        public string DepositBank { get; set; }
        public Nullable<DateTime> EncashDate { get; set; }
    }
}
