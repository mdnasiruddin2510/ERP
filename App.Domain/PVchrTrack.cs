using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PVchrTrack
    {
        [Key]
        public int VchrTrackId { get; set; }
        public string EntryNo { get; set; }
        public string EntrySource { get; set; }
        public int VchrMainId { get; set; }
        public int VchrDetailId { get; set; }
    }
}
