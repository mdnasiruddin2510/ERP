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
    public class BankReceiptAppService : AppService<AcclineERPContext>, IBankReceiptAppService
    {
        private readonly IBankReceiptService _service;
        public BankReceiptAppService(IBankReceiptService bankReceiptService)
        {
            _service = bankReceiptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public BankReceipt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<BankReceipt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<BankReceipt> Find(Expression<Func<BankReceipt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<BankReceipt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(BankReceipt obj)
        {
            _service.Add(obj);
        }

        public void Update(BankReceipt obj)
        {
            _service.Update(obj);
        }

        public void Delete(BankReceipt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(BankReceipt entity, BankReceipt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
