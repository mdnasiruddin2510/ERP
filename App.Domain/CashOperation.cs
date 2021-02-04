using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CashOperation
    {

        public CashOperation(DateTime trDate, double openBal, double recTotal,
            double payTotal, double closeBal, string closedBy, DateTime closedTime,
            bool isClosed, string finYear, bool glPost, string branchCode)
        {
            this.TrDate = trDate;
            this.OpenBal = openBal;
            this.RecTotal = recTotal;
            this.PayTotal = payTotal;
            this.CloseBal = closeBal;
            this.ClosedBy = closedBy;
            this.ClosingTime = closedTime;
            this.IsClosed = isClosed;
            this.FinYear = finYear;
            this.GLPost = glPost;
            this.BranchCode = branchCode;
        }
        public CashOperation()
        {

        }
        [Key]
        public int Id { set; get; }

        [Display(Name = "Date")]
        public DateTime TrDate { set; get; }

        [Display(Name = "Opening Balance")]
        public double OpenBal { set; get; }

        [Display(Name = "Total Received")]
        public double RecTotal { set; get; }

        [Display(Name = "Total Payment")]
        public double PayTotal { set; get; }

        [Display(Name = "Closing Balance")]
        public double CloseBal { set; get; }

        [Display(Name = "Closing Time")]
        public DateTime ClosingTime { set; get; }

        [Display(Name = "Closed By")]
        public string ClosedBy { set; get; }
        public bool IsClosed { set; get; }
        public string FinYear { set; get; }
        public bool GLPost { set; get; }
        public string BranchCode { set; get; }

        public virtual List<CashReceipt> CashReceipts { get; set; }
        public virtual List<Withdraw> Withdraws { get; set; }
        public virtual List<Expense> Expenses { set; get; }
        public virtual List<Deposit> Deposits { set; get; }                
    }
}
