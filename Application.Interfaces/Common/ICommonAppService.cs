using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Domain;
using App.Domain.Interface.VModel;

namespace Application.Interfaces.Common
{
    public interface ICommonAppService : IAppService<ICommonSearchModel>
    {
    }
}
