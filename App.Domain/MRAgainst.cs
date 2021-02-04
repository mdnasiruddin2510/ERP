using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class MRAgainst
    {
        [Key]
        public int MRAgainstID { get; set; }
        public string MRAgainstDesc { get; set; }
        public string Accode { get; set; }
        public string MRAgainstType { get; set; }
    }
}
