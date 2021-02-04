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
    public class SubsidiaryInfoService : Service<SubsidiaryInfo>, ISubsidiaryInfoService
    {
        public SubsidiaryInfoService(ISubsidiaryInfoRepository repository)
            : base(repository)
        {

        }
    }
}
