using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class BankOperation
    {
        public BankOperation(DateTime trDate,double openBal,double recTotal,double payTotal,
            double closeBal,string branchCode, string finYear, bool glPost, string bankAccode)
        {            
            this.TrDate = trDate;
            this.OpenBal = openBal;
            this.RecTotal = recTotal;
            this.PayTotal = payTotal;
            this.CloseBal = closeBal;
            this.BranchCode = branchCode;
            this.FinYear = finYear;
            this.GLPost = glPost;
            this.bankAccode = bankAccode;
        }
        public BankOperation()
        {

        }
        [Key]
        public int Id { set; get; }
        
        [Display(Name = "Date")]
        public DateTime TrDate {set; get;}
        
        [Display(Name = "Opening Balance")]
        public double OpenBal {set; get;}
        
        [Display(Name = "Total Received")]
        public double RecTotal {set; get;}
        
        [Display(Name = "Total Payment")]
        public double PayTotal {set; get;}
        
        [Display(Name = "Closing Balance")]
        public double CloseBal {set; get;}

        public string BranchCode { set; get; }
        public string FinYear {set; get;}

        public bool GLPost { set; get; }

        public string bankAccode { set; get; }
        [ForeignKey("bankAccode")]
        public virtual NewChart NewChart { set; get; }

        public virtual List<HORemit> HORemittances { get; set; }
        public virtual List<Payment> Payments { get; set; }
    }
}
