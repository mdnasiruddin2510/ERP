using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SubsidiaryCtrl
    {
        [Key]
        public int SubCtrlId { set; get; }
        public string Accode { set; get; }
        public string SubType { set; get; }
    }
}
