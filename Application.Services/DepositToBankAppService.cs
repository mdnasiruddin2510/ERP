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
    public class DepositToBankAppService : AppService<AcclineERPContext>, IDepositToBankAppService
    {
        private readonly IDepositToBankService _service;
        public DepositToBankAppService(IDepositToBankService deposit)
        {
            _service = deposit;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Deposit Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Deposit> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Deposit> Find(Expression<Func<Deposit, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Deposit> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Deposit obj)
        {
            _service.Add(obj);
        }

        public void Update(Deposit obj)
        {
            _service.Update(obj);
        }

        public void Delete(Deposit obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Deposit entity, Deposit existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
