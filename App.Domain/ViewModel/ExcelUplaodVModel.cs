using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Domain.ViewModel
{
    public class ExcelUplaodVModel
    {
        public HttpPostedFileBase Cn51Rates { set; get; }
        public HttpPostedFileBase ParcelRates { set; get; }
        public HttpPostedFileBase EmsRate { set; get; }
        public HttpPostedFileBase LetterPostRate { set; get; }
    }
}
