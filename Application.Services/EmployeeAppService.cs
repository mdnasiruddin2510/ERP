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
    public class EmployeeAppService : AppService<AcclineERPContext>, IEmployeeAppService
    {
        private readonly IEmployeeService _service;
        public EmployeeAppService(IEmployeeService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Employee Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Employee> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Employee> Find(Expression<Func<Employee, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Employee> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Employee obj)
        {
            _service.Add(obj);
        }

        public void Update(Employee obj)
        {
            _service.Update(obj);
        }

        public void Delete(Employee obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Employee entity, Employee existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
