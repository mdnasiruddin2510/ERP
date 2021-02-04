using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class BankOperationVModel
    {
        public BankOperationVModel()
        {

        }
        public BankOperationVModel(int SerialNo, string Purpose, double Amount, string TransactionNo, string Tag, string VoucherNo)
        {
            this.SerialNo = SerialNo;
            this.Purpose = Purpose;
            this.Amount = Amount;
            this.TransactionNo = TransactionNo;
            this.Tag = Tag;
            this.VoucherNo = VoucherNo;
        
        }
        public int SerialNo { set; get; }
        public string Purpose { set; get; }
        public double Amount { set; get; }
        public string TransactionNo { set; get; }
        public string Tag { set; get; }
        public string VoucherNo { set; get; }
    }
}
