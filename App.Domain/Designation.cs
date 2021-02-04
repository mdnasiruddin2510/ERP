using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    
    public class Designation
    {
        [Key]
        public int DesigID { get; set; }
        public string DesigCode { get; set; }
        public string DesigName { get; set; }
        public string DesigLocalName { get; set; }
        public string DesigDesc { get; set; }
    }
}
