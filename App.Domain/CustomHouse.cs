using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class CustomHouse
    {
        [Key]
        public int CHId { get; set; } 
        public string CHName { get; set; }
        public string CHCode { get; set; }
    }
}
