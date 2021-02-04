using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class EmailVModel
    {
        public string Title { get; set; }
        public String Email { get; set; }
        public String PassWord { get; set; }
        public String TransactionNo { get; set; }
        public string DocNo { get; set; }
        public string TempoRaryId { get; set; }
        public string Link { get; set; }
        public string OrderPreviewLink { set; get; }
       
        public string RecoverPasswordEmail { set; get; }


        public string TemplateHeader { set; get; }
        public string TempFixedTopDes { set; get; }
        public string ThanksGiving { set; get; }
        public string FooterThanks { set; get; }
        public string Department { set; get; }
        public string FooterConcern { set; get; }
        public string FooterPhone { set; get; }
        public string FooterFax { set; get; }
        public string FooterEmail { set; get; }
        public string FooterWeb { set; get; }
    }
}
