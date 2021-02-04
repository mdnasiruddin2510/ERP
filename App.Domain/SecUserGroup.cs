using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecUserGroup
    {
        public SecUserGroup()
        {
            this.SecFormRights = new HashSet<SecFormRight>();
            this.SecUserInGroups = new HashSet<SecUserInGroup>();
        }

        [Key]
        public int GroupID { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual ICollection<SecFormRight> SecFormRights { get; set; }
        public virtual ICollection<SecUserInGroup> SecUserInGroups { get; set; }
    }
}
