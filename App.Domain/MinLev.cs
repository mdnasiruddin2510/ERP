using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class MinLev
    {
        [Key]
        public int mlId { set; get; }
        public int MinLevel { set; get; }
        public string Sepa { set; get; }
    }
}
