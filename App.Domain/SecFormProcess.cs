using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecFormProcess
    {
        [Key, Column(Order = 0)]
        public int FormProcessID { get; set; }

        public int FormID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDescription { get; set; }


        [Key, Column(Order = 1), ForeignKey("FormID")]
        public virtual SecFormList SecFormList { get; set; }

    }
}
