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
    public class ChequeReceiptAppService : AppService<AcclineERPContext>, IChequeReceiptAppService
    {
        private readonly IChequeReceiptService _service;
        public ChequeReceiptAppService(IChequeReceiptService ChequeReceiptService)
        {
            _service = ChequeReceiptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ChequeReceipt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ChequeReceipt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ChequeReceipt> Find(Expression<Func<ChequeReceipt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ChequeReceipt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ChequeReceipt obj)
        {
            _service.Add(obj);
        }

        public void Update(ChequeReceipt obj)
        {
            _service.Update(obj);
        }

        public void Delete(ChequeReceipt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ChequeReceipt entity, ChequeReceipt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
