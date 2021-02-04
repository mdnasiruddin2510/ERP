using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ProjInfo
    {
        [Key]
        public int ProjID { get; set; }
        public string ProjCode { get; set; }
        public string ProjName { get; set; }
        public string ProjLocalName { get; set; }
        public string ProjDescrip { get; set; }
        public string ProjInitial { set; get; }
        public bool AutoVouNo { set; get; }
    }
}
