using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class ItemMap
    {
        [Key, Column("Id", Order = 0)]
        public int Id { get; set; }
        [Key, Column("ItemTypeCode", Order = 1)]
        public int ItemTypeCode { get; set; }
        [Key, Column("GroupID", Order = 2)]
        public int GroupID { get; set; }
        [Key, Column("SGroupID", Order = 3)]
        public int SGroupID { get; set; }
        [Key, Column("SSGroupID", Order = 4)]
        public int SSGroupID { get; set; }
        [Key, Column("ItemCode", Order = 5)]
        public string ItemCode { get; set; }
    }
}
