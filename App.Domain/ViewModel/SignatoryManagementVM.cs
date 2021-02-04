using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class SignatoryManagementVM
    {
        public int Id { set; get; }
        [DisplayName("Employee Name")]
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsActive { set; get; }      
        public string Password { set; get; }       
        public string ConfirmPassword { set; get; }        
        public string BranchCode { set; get; }
        
        [ForeignKey("BranchCode")]
        public virtual Branch Branchs { set; get; }

        public string FormName { set; get; }
        [Display(Name = "Function")]
        public string FuncName { set; get; }
        [Display(Name = "Emplopyee")]
        public string EmpId { set; get; }
        public int FuncSl { set; get; }
        public string FullName { set; get; }
        public string Designation { get; set; }
        public string UserRank { get; set; }
        public int UserID { get; set; }
    }
}
