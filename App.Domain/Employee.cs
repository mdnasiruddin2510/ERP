using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Domain
{
    public class Employee
    {    
        [Key]
        public int Id { set; get; }
        [DisplayName("Employee Name")]
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsActive { set; get; }
        public string BranchCode { set; get; }
        public string FullName { set; get; }
        [NotMapped]
        public string Password { set; get; }
        [NotMapped]
        public string ConfirmPassword { set; get; }

        //public string Designation { get; set; }


    }
}

