using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class GroupType
    {
        [Key]
        public int GroupTypeId { get; set; }
        public string GroupTypeName { get; set; }
    }
}