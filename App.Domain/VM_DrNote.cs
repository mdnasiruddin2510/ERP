using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_DrNote
    {
        public int DrNoteId { get; set; }
        public string DrNoteNo { get; set; }
        public System.DateTime DrNoteDate { get; set; }
        public string Reason { get; set; }
        public string ChallanNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}
