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
    public class WithdrawAppService : AppService<AcclineERPContext>, IWithdrawAppService
    {
        private readonly IWithdrawService _service;
        public WithdrawAppService(IWithdrawService withdrawService)
        {
            _service = withdrawService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Withdraw Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Withdraw> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Withdraw> Find(Expression<Func<Withdraw, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Withdraw> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Withdraw obj)
        {
            _service.Add(obj);
        }

        public void Update(Withdraw obj)
        {
            _service.Update(obj);
        }

        public void Delete(Withdraw obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Withdraw entity, Withdraw existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
