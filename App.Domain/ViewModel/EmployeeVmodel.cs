using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class EmployeeVmodel
    {
        public EmployeeVmodel()
        {
            this.Employee = new Employee();
        }
        public Employee Employee { set; get; }
        public string AllRoles { set; get; }
        public int SlNo { set; get; }
        public string[] Roles { set; get; }
    }
}
