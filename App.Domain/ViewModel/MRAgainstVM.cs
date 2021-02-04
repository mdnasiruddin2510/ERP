using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class MRAgainstVM
    {
        [Key]
        public int MRAgainstID { get; set; }
        public string MRAgainstDesc { get; set; }
        public string AcCode { get; set; }
        public string MRAgainstType { get; set; }

    }
}
