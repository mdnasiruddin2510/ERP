using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Branch
    {
        [Key]
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BrLocalName { get; set; }
        public string BrAddress { get; set; }
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BrID { get; set; }
    }
}
