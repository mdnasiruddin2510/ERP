using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class EmailConfig
    {
        public int Id { set; get; }
        [DisplayName("Template Header")]
        public string TemplateHeader { set; get; }
         [DisplayName("Temp Fixed Top Des")]
        public string TempFixedTopDes { set; get; }
         [DisplayName("Thanks Giving")]
        public string ThanksGiving { set; get; }
         [DisplayName("Footer Thanks")]
        public string FooterThanks { set; get; }
         [DisplayName("Department")]
        public string Department { set; get; }
         [DisplayName("Footer Concern")]
        public string FooterConcern { set; get; }
         [DisplayName("Footer Phone")]
        public string FooterPhone { set; get; }
         [DisplayName("Footer Fax")]
        public string FooterFax { set; get; }
         [DisplayName("Footer Email")]
        public string FooterEmail { set; get; }
         [DisplayName("Footer Website")]
        public string FooterWeb { set; get; }
         [DisplayName("Is Active")]
        public bool IsActive { set; get; }
    }
}
