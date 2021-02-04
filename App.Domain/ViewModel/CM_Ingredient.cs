using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CM_Ingredient
    {
        public string DeclarationNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Ingr_Type { get; set; }
        public string QtyWW { get; set; }
        public string UnitIn { get; set; }
        public Nullable<decimal> WastageQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Waste_Perc { get; set; }
        public Nullable<decimal> PurPrice { get; set; }
    }
}
