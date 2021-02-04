using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class tmpPurItem
    {
        public tmpPurItem()
        {

        }
        public tmpPurItem(int tPurId, string LocNo, string PurNo, string ItemName, string LotNo,
            decimal OBQty, decimal OBAmt, decimal PurQty, decimal UPrice, decimal Amount, decimal SupTaxAmt,
            string Description, string VATType, decimal VATAmt, decimal SubTotal, string VATEx, decimal TaxEx, decimal? VATRate, decimal? SDRate)
        {
            this.tPurId = tPurId;
            this.LocNo = LocNo;
            this.PurNo = PurNo;
            this.ItemName = ItemName;
            this.LotNo = LotNo;
            this.OBQty = OBQty;
            this.OBAmt = OBAmt;
            this.PurQty = PurQty;
            this.UPrice = UPrice;
            this.Amount = Amount;
            this.SupTaxAmt = SupTaxAmt;
            this.Description = Description;
            this.VATType = VATType;
            this.VATAmt = VATAmt;
            this.SubTotal = SubTotal;
            this.VATEx = VATEx;
            this.TaxEx = TaxEx;
            this.VATRate = VATRate;
            this.SDRate = SDRate;
        }
        [Key]
        public int tPurId { get; set; }
        public string LocNo { get; set; }
        public string PurNo { get; set; }
        public string ItemName { get; set; }
        public string LotNo { get; set; }
        public decimal OBQty { get; set; }
        public decimal OBAmt { get; set; }
        public decimal PurQty { get; set; }
        public decimal UPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal SupTaxAmt { get; set; }
        public string Description { get; set; }
        public string VATType { get; set; }
        public decimal VATAmt { get; set; }
        public decimal SubTotal { get; set; }
        public string VATEx { get; set; }
        public decimal TaxEx { get; set; }
        public string FinYear { get; set; }
        public string LoginName { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? SDRate { get; set; }
    }
}
