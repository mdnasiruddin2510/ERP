using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class GetwayProvider
    {
        [Key]
        public int GetwayId { get; set; }
        public string GetwayName { get; set; }
    }
}
