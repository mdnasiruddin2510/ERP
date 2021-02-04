using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class UserAccess
    {
        public int Id { set; get; }
        public virtual Role Role { set; get; }
        public string FormName { set; get; }
        public string ProcessName { set; get; }

    }
}
