using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class PackingList
    {
        [Key]
        public int Id { set; get; }
        public string InvoiceNo { set; get; }
        public string ChallanNo { set; get; }
        public string ItemCode { set; get; }
        public string PackItemCode { set; get; }
        public decimal PackSize { set; get; }
        public int? Qty { set; get; }
        public int No_of_Pack { set; get; }
        public int Total_Qty { set; get; }
        public string PackLotNo { set; get; }
        
    }
}
