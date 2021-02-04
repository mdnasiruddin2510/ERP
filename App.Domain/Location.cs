using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Location
    {
        [Key]
        public string LocCode { set; get; }
        public string LocName { set; get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocId { get; set; }
    }
}
