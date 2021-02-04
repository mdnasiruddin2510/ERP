using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ItemType
    {
        [Key]
        public int ItemTypeCode {set; get;}
        public string ItemTypeName { set; get; }

    }
}
