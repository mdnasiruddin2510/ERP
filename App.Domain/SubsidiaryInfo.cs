using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SubsidiaryInfo
    {
        [Key]
        [Display(Name = "Subsidiary Code")]
        public string SubCode { set; get; }

        [Display(Name = "Subsidiary Name")]
        public string SubName { set; get; }
        
        [Display(Name = "Subsidiary Type")]
        public string SubType { set; get; }
        [ForeignKey("SubType")]
        public virtual SubsidiaryType SubsidiaryTypes { set; get; }
          
        [Display(Name="Title")]
        public string SubTitle { get; set; }
        
        [Display(Name="Group")]
        public string SubGrp { get; set; } 

        [NotMapped]
        public string SubCategory { get; set; }
    }
}
