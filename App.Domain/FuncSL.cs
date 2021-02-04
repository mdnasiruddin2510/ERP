using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class FuncSL
    {

        [Key]
        public int FuncId { set; get; }

        [Display(Name = "From")]
        public string FormName { set; get; }
        [Display(Name = "Function")]
        public string FuncName { set; get; }
        public int FuncSl { set; get; }

    }
}
