using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecFormRight
    {

        [Key, Column(Order = 0)]
        public int FormRightID { get; set; }

        [Display(Name = "Group")]
        public int GroupID { get; set; }
        public int FormProcessID { get; set; }
        public bool Allow { get; set; }

        [Key, Column(Order = 1), ForeignKey("GroupID")]
        public virtual SecUserGroup SecUserGroup { get; set; }

        [Key, Column(Order = 2), ForeignKey("FormProcessID")]
        public virtual SecFormProcess SecFormProcess { get; set; }

    }
}
