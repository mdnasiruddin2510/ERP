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
    public class CostLedgerAppService : AppService<AcclineERPContext>, ICostLedgerAppService
    {
        private readonly ICostLedgerService _service;
        public CostLedgerAppService(ICostLedgerService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CostLedger Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CostLedger> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CostLedger> Find(Expression<Func<CostLedger, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CostLedger> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CostLedger obj)
        {
            _service.Add(obj);
        }

        public void Update(CostLedger obj)
        {
            _service.Update(obj);
        }

        public void Delete(CostLedger obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CostLedger entity, CostLedger existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
