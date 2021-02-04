using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class VM_DrCrNoteReason
    {
        [Key]
        public byte DrCrNoteId { get; set; }
        public string NoteReason { get; set; }
        public string DrCr { get; set; }
    }
}
