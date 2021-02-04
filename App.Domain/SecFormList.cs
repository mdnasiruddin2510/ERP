using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecFormList
    {
        public SecFormList()
        {
            this.SecFormProcesses = new HashSet<SecFormProcess>();
            //this.SecFormRights = new HashSet<SecFormRight>();
        }

        [Key]
        public int FormID { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }

        public virtual ICollection<SecFormProcess> SecFormProcesses { get; set; }
        //public virtual ICollection<SecFormRight> SecFormRights { get; set; }
    }
}
