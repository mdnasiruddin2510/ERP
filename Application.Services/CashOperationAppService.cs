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
    public class CashOperationAppService : AppService<AcclineERPContext>, ICashOperationAppService
    {
        private readonly ICashOperationService _service;
        public CashOperationAppService(ICashOperationService cashOperation)
        {
            _service = cashOperation;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CashOperation Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CashOperation> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CashOperation> Find(Expression<Func<CashOperation, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CashOperation> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CashOperation obj)
        {
            _service.Add(obj);
        }

        public void Update(CashOperation obj)
        {
            _service.Update(obj);
        }

        public void Delete(CashOperation obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CashOperation entity, CashOperation existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
