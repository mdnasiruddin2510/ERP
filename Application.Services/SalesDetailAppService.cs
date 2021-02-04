using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;

namespace Application.Services
{
    public class SalesDetailAppService : AppService<AcclineERPContext>, ISalesDetailAppService
    {
        private readonly ISalesDetailService _service;

        public SalesDetailAppService(ISalesDetailService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SalesDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SalesDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SalesDetail> Find(Expression<Func<SalesDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SalesDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SalesDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(SalesDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(SalesDetail obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SalesDetail entity, SalesDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}