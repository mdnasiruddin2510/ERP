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
    public class CashReceiptSubDetailsAppService : AppService<AcclineERPContext>, ICashReceiptSubDetailsAppService
    {
        private readonly ICashReceiptSubDetailsService _service;
        public CashReceiptSubDetailsAppService(ICashReceiptSubDetailsService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CashReceiptSubDetails Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CashReceiptSubDetails> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CashReceiptSubDetails> Find(Expression<Func<CashReceiptSubDetails, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CashReceiptSubDetails> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CashReceiptSubDetails obj)
        {
            _service.Add(obj);
        }

        public void Update(CashReceiptSubDetails obj)
        {
            _service.Update(obj);
        }

        public void Delete(CashReceiptSubDetails obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CashReceiptSubDetails entity, CashReceiptSubDetails existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
