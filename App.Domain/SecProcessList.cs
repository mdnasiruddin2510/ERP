using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecProcessList
    {
        //public SecProcessList()
        //{
        //    this.SecFormProcesses = new HashSet<SecFormProcess>();
        //    this.SecFormRights = new HashSet<SecFormRight>();
        //}

        [Key]    
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDescription { get; set; }

        //public virtual ICollection<SecFormProcess> SecFormProcesses { get; set; }
        //public virtual ICollection<SecFormRight> SecFormRights { get; set; }
    }
}
