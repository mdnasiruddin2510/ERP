using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class IssRecSrcDest
    {

        [Key]
        public string SrcDestId { set; get; }
        public string SrcDestName { set; get; }
        public string Type { set; get; }
        public string ActionSub { set; get; }
        public string ActionCtrl { set; get; }
    }
}
