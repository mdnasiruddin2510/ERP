using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Domain
{

    public class SecUserInfo
    {
        public SecUserInfo()
        {
            this.SecUserInGroups = new HashSet<SecUserInGroup>();
        }

        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        [NotMapped]
        [Display(Name = "Confirm Password")]
        public string ConfPassword { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }


        public virtual ICollection<SecUserInGroup> SecUserInGroups { get; set; }


    }
}
