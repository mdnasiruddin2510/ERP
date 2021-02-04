using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
  public  class VMSupervisor
    {
        [Key]
        public int Id { get; set; }
        [StringLength(3)]
        [Column(TypeName = "char")]
        [Required]
        public string SupCode { get; set; }
        [Required]
        public string SupName { get; set; }
        public string MobileNo { get; set; }
        public string EntryUser { get; set; }
        public string EntryDate { get; set; }
     
        public string IMEI { get; set; }
        public bool Discontinue { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
      public string PECode { get; set; }
    }
}
