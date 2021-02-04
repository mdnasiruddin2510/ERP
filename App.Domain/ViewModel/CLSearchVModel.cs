using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CLSearchVModel
    {
        public string SubCode { set; get; }
        [ForeignKey("SubCode")]
        public virtual SubsidiaryInfo SubsidiaryInfo { set; get; }

        public string fDate { set; get; }
        public string tDate { set; get; }
    }
}
