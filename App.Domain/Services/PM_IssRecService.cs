using App.Domain.Interface.Repository;
using App.Domain.Interface.Service;
using App.Domain.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Services
{
    public class PM_IssRecService : Service<PM_IssRec>, IPM_IssRecService
    {
        public PM_IssRecService(IPM_IssRecRepository repository)
            : base(repository)
        {

        }
    }
}
