using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class CRSubDetailsVModel
    {
        public CRSubDetailsVModel()
        {

        }
        public CRSubDetailsVModel(string subCode, double amount, string description, string subName)
        {
            this.SubCode = subCode;
            this.Amount = amount;
            this.Description = description;
            this.SubName = subName;
        }
        public string SubCode { set; get; }
        public double Amount { set; get; }
        public string Description { set; get; }
        public string SubName { set; get; }
        
    }
}
