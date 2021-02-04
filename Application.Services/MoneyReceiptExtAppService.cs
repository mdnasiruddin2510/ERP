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
    public class MoneyReceiptExtAppService : AppService<AcclineERPContext>, IMoneyReceiptExtAppService
    {
        private readonly IMoneyReceiptExtService _service;
        public MoneyReceiptExtAppService(IMoneyReceiptExtService MoneyReceiptExtService)
        {
            _service = MoneyReceiptExtService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MoneyReceiptExt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<MoneyReceiptExt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<MoneyReceiptExt> Find(Expression<Func<MoneyReceiptExt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<MoneyReceiptExt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(MoneyReceiptExt obj)
        {
            _service.Add(obj);
        }

        public void Update(MoneyReceiptExt obj)
        {
            _service.Update(obj);
        }

        public void Delete(MoneyReceiptExt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(MoneyReceiptExt entity, MoneyReceiptExt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
