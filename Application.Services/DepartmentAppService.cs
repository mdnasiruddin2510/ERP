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
    public class DepartmentAppService : AppService<AcclineERPContext>, IDepartmentAppService
    {
        private readonly IDepartmentService _service;

        public DepartmentAppService(IDepartmentService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Department Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Department> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<Department> Find(Expression<Func<Department, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Department> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(Department obj)
        {
            _service.Add(obj);
        }

        public void Update(Department obj)
        {
            _service.Update(obj);
        }

        public void Delete(Department obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(Department entity, Department existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}