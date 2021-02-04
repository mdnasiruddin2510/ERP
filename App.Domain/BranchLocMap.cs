using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class BranchLocMap
    {
        public int Id { set; get; }


        public string BranchCode { set; get; }
        [ForeignKey("BranchCode")]
        public virtual Branch Branch { set; get; }

        public string LocCode { set; get; }
        [ForeignKey("LocCode")]
        public virtual Location Location { set; get; }

    }
}
