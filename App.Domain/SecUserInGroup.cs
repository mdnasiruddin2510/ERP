using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class SecUserInGroup
    {
        [Key, Column(Order = 0)]
        public int UserGroupID { get; set; }
        
        public int GroupID { get; set; }
        public int UserID { get; set; }

        [Key, Column(Order = 1), ForeignKey("GroupID")]
        public virtual SecUserGroup SecUserGroup { get; set; }

        [Key, Column(Order = 2), ForeignKey("UserID")]
        public virtual SecUserInfo SecUserInfo { get; set; }
    }
}
