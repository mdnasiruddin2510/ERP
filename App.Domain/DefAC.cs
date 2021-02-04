using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class DefAC
    {
        [Key]
        public int DefACId { get; set; }
        public string PrevBalAc { get; set; }
        public string CurrBalAc { get; set; }
        public string DiffAc { get; set; }
        public string CashAc { get; set; }
        public string BankAc { get; set; }
        public string IncAc { get; set; }
        public string ExpAc { get; set; }
        public string APL { get; set; }
        public string Sales { get; set; }
        public string Purchase { get; set; }
        public string PurAc1 { get; set; }
        public string AdminOverHead { get; set; }
        public string SellOverHead { get; set; }
        public string FinOverHead { get; set; }
        public string OtherIncm { get; set; }
        public string ProvForTax { get; set; }
        public string AdjAc { get; set; }
        public string AssAc { get; set; }
        public string LiaAc { get; set; }
        public string ARAc { get; set; } 
    }
}
