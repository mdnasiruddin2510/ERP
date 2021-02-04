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
    public class CashReceiptAppService : AppService<AcclineERPContext>, ICashReceiptAppService
    {
        private readonly ICashReceiptService _service;
        public CashReceiptAppService(ICashReceiptService cashReceiptService)
        {
            _service = cashReceiptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CashReceipt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CashReceipt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CashReceipt> Find(Expression<Func<CashReceipt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CashReceipt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CashReceipt obj)
        {
            _service.Add(obj);
        }

        public void Update(CashReceipt obj)
        {
            _service.Update(obj);
        }

        public void Delete(CashReceipt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CashReceipt entity, CashReceipt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
