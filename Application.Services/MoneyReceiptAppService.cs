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
    public class MoneyReceiptAppService : AppService<AcclineERPContext>, IMoneyReceiptAppService
    {
        private readonly IMoneyReceiptService _service;
        public MoneyReceiptAppService(IMoneyReceiptService MoneyReceiptService)
        {
            _service = MoneyReceiptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MoneyReceipt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<MoneyReceipt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<MoneyReceipt> Find(Expression<Func<MoneyReceipt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<MoneyReceipt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(MoneyReceipt obj)
        {
            _service.Add(obj);
        }

        public void Update(MoneyReceipt obj)
        {
            _service.Update(obj);
        }

        public void Delete(MoneyReceipt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(MoneyReceipt entity, MoneyReceipt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
