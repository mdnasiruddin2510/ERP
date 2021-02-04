using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Department
    {        
        [Key]
        public int DeptID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string DeptLocalName { get; set; }
        public string DeptDesc { get; set; }

    }
}
