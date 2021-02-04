using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using App.Domain.Interface.Domain;
using App.Domain.Interface.Repository;
using App.Domain.Interface.Repository.Common;
using App.Domain.Interface.VModel;

namespace Data.Repository.EntityFramework.Common
{
    public class CommonSearchRepo : Repository<ICommonSearchModel>, ICommonSearchRepo
    {
    }
}
