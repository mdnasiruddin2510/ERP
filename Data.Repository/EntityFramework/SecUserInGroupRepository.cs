using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.Domain;
using App.Domain.Interface.Repository;
using Data.Repository.EntityFramework.Common;

namespace Data.Repository.EntityFramework
{
    public class SecUserInGroupRepository : Repository<SecUserInGroup>, ISecUserInGroupRepository
    {
    
    }
}
