using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ItemList
    {
        [Key]
        public string ItemCode { set; get; }
        public string PartNo { set; get; }
        public string ItemName { set; get; }
        public string ItemType { set; get; }
        public string UnitCode { set; get; }
    }
}
