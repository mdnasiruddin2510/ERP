using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{

    public class VMMasterForm
    {
        
        [DisplayName("Type")]
        public string FormCode { get; set; }

        public string FormName { get; set; }

        public int ID { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Local Name")]
        public string LocalName { get; set; }

        [DisplayName("Description")]
        public string Descr { get; set; }

    }

}
