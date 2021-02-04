using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{

    public class ItemInfo
    {
        [Required]
        [Display(Name = "Item Type")]
        public Nullable<int> ItemType { set; get; }
        [ForeignKey("ItemType")]
        public virtual ItemType ItemTypes { set; get; }

        [Key]
        [Display(Name = "Item Code")]
        public string ItemCode { set; get; }
        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { set; get; }

        [Display(Name = "Opening Balance")]
        public double OpenBal { set; get; }

        [Display(Name = "Part No")]
        public string PartNo { set; get; }

        public string Prod_Ser { set; get; }

        public string TaxHeadingNo { get; set; } 

        [Display(Name = "HS Code")]
        public string HSCode { set; get; }
        [Display(Name = "Reorder Label")]
        public Nullable<decimal> ROLevel { set; get; }

        [Required]
        [Display(Name = "Unit")]
        public string UnitCode { set; get; }
        [ForeignKey("UnitCode")]
        public virtual Unit Unit { set; get; }


        [Display(Name = "Detail Unit")]
        public string DetUnitCode { set; get; }
        [ForeignKey("DetUnitCode")]
        public virtual Unit DetUnit { set; get; }

        [Display(Name = "Trade Price")]
        public double Price { set; get; }

        [Display(Name = "Retail Price")]
        public decimal RetailPrice { get; set; }

        [Display(Name = "Ratio")]
        public string Ratio { set; get; }

        [Display(Name = "Pack Size")]
        public double PackSize { set; get; }


        [Display(Name = "Pack Item")]
        public string PackItemCode { set; get; }


        [Display(Name = "Alternate Item")]
        public string AltItemCode { set; get; }
        [NotMapped]
        public string ItemTypeName { get; set; }
        [NotMapped]
        public int GroupID { get; set; }

        [NotMapped]
        public int SGroupID { get; set; }
        
        [NotMapped]
        public int SSGroupID { get; set; }

        [NotMapped]
        public int Id { get; set; }

    }
}
