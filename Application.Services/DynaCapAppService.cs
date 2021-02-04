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
    public class DynaCapAppService : AppService<AcclineERPContext>, IDynaCapAppService
    {
        private readonly IDynaCapService _service;

        public DynaCapAppService(IDynaCapService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public DynaCap Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<DynaCap> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<DynaCap> Find(Expression<Func<DynaCap, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<DynaCap> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(DynaCap obj)
        {
            _service.Add(obj);
        }

        public void Update(DynaCap obj)
        {
            _service.Update(obj);
        }

        public void Delete(DynaCap obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(DynaCap entity, DynaCap existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}