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
    public class CustomerLedgerAppService : AppService<AcclineERPContext>, ICustomerLedgerAppService
    {
        private readonly ICustomerLedgerService _service;
        public CustomerLedgerAppService(ICustomerLedgerService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CustomerLedger Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CustomerLedger> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CustomerLedger> Find(Expression<Func<CustomerLedger, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CustomerLedger> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CustomerLedger obj)
        {
            _service.Add(obj);
        }

        public void Update(CustomerLedger obj)
        {
            _service.Update(obj);
        }

        public void Delete(CustomerLedger obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CustomerLedger entity, CustomerLedger existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
