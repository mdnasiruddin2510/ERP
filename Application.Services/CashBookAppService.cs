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
    public class CashBookAppService : AppService<AcclineERPContext>, ICashBookAppService
    {
        private readonly ICashBookService _service;
        public CashBookAppService(ICashBookService cashBook)
        {
            _service = cashBook;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CashBook Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CashBook> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CashBook> Find(Expression<Func<CashBook, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CashBook> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CashBook obj)
        {
            _service.Add(obj);
        }

        public void Update(CashBook obj)
        {
            _service.Update(obj);
        }

        public void Delete(CashBook obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CashBook entity, CashBook existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
