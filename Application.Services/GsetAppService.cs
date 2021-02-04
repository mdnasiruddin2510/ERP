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
    public class GsetAppService : AppService<AcclineERPContext>, IGsetAppService
    {
        private readonly IGsetService _service;
        public GsetAppService(IGsetService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Gset Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Gset> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Gset> Find(Expression<Func<Gset, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Gset> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Gset obj)
        {
            _service.Add(obj);
        }

        public void Update(Gset obj)
        {
            _service.Update(obj);
        }

        public void Delete(Gset obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Gset entity, Gset existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
