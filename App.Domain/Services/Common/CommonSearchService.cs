using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Domain;
using App.Domain.Interface.Repository.Common;
using App.Domain.Interface.Service;
using App.Domain.Interface.Service.Common;
using App.Domain.Interface.VModel;

namespace App.Domain.Services.Common
{
    public class CommonSearchService : Service<ICommonSearchModel>, ICommonSearchService
    {
        public CommonSearchService(ICommonSearchRepo repository)
            : base(repository)
        {
        }

        
    }
}
