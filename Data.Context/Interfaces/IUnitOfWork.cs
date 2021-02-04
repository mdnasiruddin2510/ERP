using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;

namespace Data.Context.Interfaces
{
    public interface IUnitOfWork<TContext>
        where TContext : IDbContext, new()
    {
        void BeginTransaction();
        void SaveChanges();
       // bool IsUserExits(string name, bool persistCookie = false);
    }
}
