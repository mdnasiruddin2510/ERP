using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class LevelLen
    {
        [Key]
        public int LevelNo { set; get; }
        public int LevelLength { set; get; }
        public int LevelDig { set; get; }
    }
}
