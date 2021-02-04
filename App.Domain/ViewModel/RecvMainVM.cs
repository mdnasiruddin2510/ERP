using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class RecvMainVM
    {
        public RecvMainVM()
        {
            //this.Main = new ReceiveMain();
            this.Details = new List<RecvDetailsVM>();
        }

        public ReceiveMain Main{get;set;}
        public List<RecvDetailsVM> Details{get;set;}
    }
}
