using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_CrNote
    {
        public int CrNoteId { get; set; }
        public string CrNoteNo { get; set; }
        public System.DateTime CrNoteDate { get; set; }
        public string Reason { get; set; }
        public string ChallanNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}
