using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class EmployeeFunc
    {
        [Key]
        public int Id { set; get; }
        public string BranchCode { set; get; }
        [ForeignKey("BranchCode")]
        public virtual Branch Branchs { set; get; }

        [Display(Name = "From")]
        public string FormName { set; get; }
        [Display(Name = "Function")]
        public string FuncName { set; get; } 

        [Display(Name = "Emplopyee")]
        public int EmpId { set; get; }
        //[ForeignKey("EmpId")]
        //public virtual Employee Employees { set; get; }

        public int FuncSl { set; get; }
        public IList<FuncSL> FuncList { get; set; }
    }
}
