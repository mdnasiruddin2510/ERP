using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class PendingNotEncashRptVM 
    {

        public string MRSL { get; set; }


        public string MRNo { get; set; }


        public DateTime? MRDate { get; set; }

        //public DateTime  MRDate { get; set; } 

        public string CustCode { get; set; }


        public decimal ChkAmount { get; set; }


        public decimal MRAmount { get; set; }


        public string Remarks { get; set; }
    }
}
