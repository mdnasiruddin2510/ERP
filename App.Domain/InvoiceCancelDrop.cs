using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class InvoiceCancelDrop
    {
        [Key]
        public int CancelReceiptId { get; set; }
        public string CancelReceiptType { get; set; }
        public string CancelReceiptSPName { get; set; }
    }
}
