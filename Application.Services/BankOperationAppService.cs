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
    public class BankOperationAppService : AppService<AcclineERPContext>, IBankOperationAppService
    {
        private readonly IBankOperationService _service;
        public BankOperationAppService(IBankOperationService bankOperationService)
        {
            _service = bankOperationService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public BankOperation Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<BankOperation> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<BankOperation> Find(Expression<Func<BankOperation, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<BankOperation> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(BankOperation obj)
        {
            _service.Add(obj);
        }

        public void Update(BankOperation obj)
        {
            _service.Update(obj);
        }

        public void Delete(BankOperation obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(BankOperation entity, BankOperation existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
