using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VM_4P3VM
    {
        public Vat4P3 VM_4P3 { get; set; }      
        public IEnumerable<CM_Ingredient> CM_Ingredient { get; set; }
        public IEnumerable<CM_CostExp> CM_CostExp { get; set; }   
    } 
}
