using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class VchrPreviewVM
    {
        public string finyear { get; set; }
        public string vchrno { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime vdate { get; set; }
        public string vtype { get; set; }
        public string branchcode { get; set; }
        public string vdesc { get; set; }
        public string accode { get; set; }
        public string acname { get; set; }
        public string sub_ac { get; set; }
        public string subname { get; set; }
        public string narration { get; set; }
        public double dramount { get; set; }
        public double cramount { get; set; }
        public int entrysl { get; set; }
        public string loginname { get; set; }
    }
}
