using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountsReceivableAppService : AppService<AcclineERPContext>, IAccountsReceivableAppService
    {
        private readonly IAccountsReceivableService _service;
        public AccountsReceivableAppService(IAccountsReceivableService accReceivable)
        {
            _service = accReceivable;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public AccountsReceivable Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<AccountsReceivable> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<AccountsReceivable> Find(Expression<Func<AccountsReceivable, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<AccountsReceivable> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(AccountsReceivable obj)
        {
            _service.Add(obj);
        }

        public void Update(AccountsReceivable obj)
        {
            _service.Update(obj);
        }

        public void Delete(AccountsReceivable obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(AccountsReceivable entity, AccountsReceivable existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
