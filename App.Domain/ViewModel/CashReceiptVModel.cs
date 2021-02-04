using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CashReceiptVModel
    {
        public CashReceiptVModel()
        {
            this.Main = new CashReceipt();
            this.Details = new List<CRSubDetailsVModel>();
        }
        public CashReceipt Main;
        public List<CRSubDetailsVModel> Details;
    }
}
