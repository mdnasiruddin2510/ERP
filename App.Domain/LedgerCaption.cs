using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class LedgerCaption
    {
        [Key]
        [Column(Order = 0)]
        public int Id { set; get; }
        //public string LedgerCode { set; get; }
        [Key]
        [Column(Order = 1)]
        public string LedgerTypeCode { set; get; }
        public string LedgerCap { set; get; }
        public string RptCap { set; get; }
        public string RptName { set; get; }
        public string SP_Name { set; get; }
        public string OpeningCap { set; get; }
        public string Col1Cap { set; get; }
        public string Col2Cap { set; get; }
        public string Col3Cap { set; get; }
        public string Col4Cap { set; get; }
        public string Col5Cap { set; get; }
        public string Col6Cap { set; get; }
        public string Col7Cap { set; get; }
        public string Col8Cap { set; get; }
        public string ClosingCap { set; get; }
    }
}
