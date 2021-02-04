using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class SubVModel
    {
        public SubVModel()
        {
            this.SubsidiaryInfo = new SubsidiaryInfo();
        }
        public int SubTypeExtID { get; set; }
        public SubsidiaryInfo SubsidiaryInfo { set; get; }
        [Display(Name = "Sub Name")]
        public string SubName { set; get; }
        public int SlNo { set; get; }
        public string[] Roles { set; get; }
        [Display(Name = "Subsidiary Code")]
        public string SubCode { set; get; }
        [Display(Name = "Subsidiary Type")]
        public string SubType { set; get; }
        [Display(Name = "Subsidiary Name")]
        public string Name { set; get; }
        public string SubCategory { get; set; }
        public string SuppCategory { get; set; }
        public string CustCategory { get; set; }
        public string SubAddress { get; set; }
        public string Tel { get; set; }
        [Display(Name = "Title")]
        public string SubTitle { get; set; }
        [Display(Name = "Subsidiary Email")]
        public string SubEmail { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Opening Balance")]
        public decimal OpenBal { get; set; }
        public Nullable<DateTime> OpenDate { get; set; }
        public string TIN { get; set; }
        public string BIN { get; set; }
        [Display(Name = "VAT Reg No")]
        public string VATRegNo { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Contact Person")]
        public string ContPerson { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        [Display(Name = "Email")]
        public string ContEmail { get; set; }
        [Display(Name = "Country")]
        public string CountryCode { get; set; }
        [Display(Name = "Group")]
        public string SubGrp { get; set; } 
    }
}
