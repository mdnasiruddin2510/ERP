using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class JVEditMainVM
    {
        public JVEditMainVM()
        {
            this.pMainExt = new PVchrMainExt();
            this.MainExt = new VchrMainExt();
            this.Details = new List<TvchrDetailVM>();
        }
        public PVchrMainExt pMainExt;
        public VchrMainExt MainExt;
        public List<TvchrDetailVM> Details;
    }
}
