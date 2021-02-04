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
    public class DesignationAppService : AppService<AcclineERPContext>, IDesignationAppService
    {
        private readonly IDesignationService _service;

        public DesignationAppService(IDesignationService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Designation Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Designation> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<Designation> Find(Expression<Func<Designation, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Designation> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(Designation obj)
        {
            _service.Add(obj);
        }

        public void Update(Designation obj)
        {
            _service.Update(obj);
        }

        public void Delete(Designation obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(Designation entity, Designation existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}