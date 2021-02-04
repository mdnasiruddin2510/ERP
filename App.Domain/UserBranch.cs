using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class UserBranch
    {
        [Key]
        public int Id { get; set; }
        public string Userid { get; set; }
        public string BranchCode { get; set; }

    }
}
