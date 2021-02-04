using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class IssueMainVM
    {
        public IssueMainVM()
        {
            this.Main = new IssueMain();
            this.Details = new List<IssueDetailsVM>();
        }
        public IssueMain Main;
        public List<IssueDetailsVM> Details;
        
    }
}
