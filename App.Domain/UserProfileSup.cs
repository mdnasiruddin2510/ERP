using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class UserProfileSup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20)]
        public string Password { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        public string IMEI { get; set; }
        [StringLength(3)]
        [Column(TypeName = "char")]
        public string Supcode { get; set; }

        public string SyncDate { get; set; }
        public bool Discontinue { get; set; }
        public bool RememberMe { get; set; }
    }
}
