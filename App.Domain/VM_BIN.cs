using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    [Table("VM_BIN")]
    public class VM_BIN
    {
        [Key]
        public int Id { get; set; }
        public string ProjCode { get; set; }
        public string BranchCode { get; set; }
        public string BINNo { get; set; }
    }
}
