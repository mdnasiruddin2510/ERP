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
    public class SalesMainAppService : AppService<AcclineERPContext>, ISalesMainAppService
    {
        private readonly ISalesMainService _service;

        public SalesMainAppService(ISalesMainService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SalesMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SalesMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SalesMain> Find(Expression<Func<SalesMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SalesMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SalesMain obj)
        {
            _service.Add(obj);
        }

        public void Update(SalesMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(SalesMain obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SalesMain entity, SalesMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}