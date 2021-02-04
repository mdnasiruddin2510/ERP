using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class ExpenseVModel
    {
        public ExpenseVModel()
        {
            this.Main = new Expense();
            this.Details = new List<EXSubDetailsVModel>();
        }
        public Expense Main;
        public List<EXSubDetailsVModel> Details;
    }
}
