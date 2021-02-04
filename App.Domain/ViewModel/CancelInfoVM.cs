using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CancelInfoVM
    {
        public string CancelType {get; set;}
        public int TransectionNo { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
