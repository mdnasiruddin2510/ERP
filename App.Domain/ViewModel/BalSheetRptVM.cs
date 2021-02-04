using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class BalSheetRptVM
    {
        public string IncExp { get; set; }
        public string IncExpHead { get; set; }
        public string NoteNo { get; set; }
        public decimal CurrPeriod { get; set; }
        public decimal PrevPeriod { get; set; }
        public string DCaption { get; set; }
        public string ProjName { get; set; }
        public string fDate { get; set; }
        public string tDate { get; set; }
    }
}
