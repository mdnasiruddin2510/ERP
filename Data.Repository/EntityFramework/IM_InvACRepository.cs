using App.Domain;
using App.Domain.Interface.Repository;
using Data.Repository.EntityFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.EntityFramework
{
    public class IM_InvACRepository : Repository<IM_InvAC>, IIM_InvACRepository
    {
    }
}
