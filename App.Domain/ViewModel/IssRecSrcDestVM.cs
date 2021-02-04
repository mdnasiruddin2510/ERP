using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class IssRecSrcDestVM
    {
        public string SrcDestId { set; get; }
        public string SrcDestName { set; get; }
        public string Type { set; get; }
        public string AcName { get; set; }
        public string AcCode { get; set; }
        public string ActionSub { set; get; }
        public string SubType { get; set; }
        public string TypeCode { get; set; }
        public string ActionCtrl { set; get; }
    }
}
