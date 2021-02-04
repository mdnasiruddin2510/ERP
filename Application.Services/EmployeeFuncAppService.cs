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
    public class EmployeeFuncAppService : AppService<AcclineERPContext>, IEmployeeFuncAppService
    {
        private readonly IEmployeeFuncService _service;
        public EmployeeFuncAppService(IEmployeeFuncService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EmployeeFunc Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<EmployeeFunc> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<EmployeeFunc> Find(Expression<Func<EmployeeFunc, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<EmployeeFunc> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(EmployeeFunc obj)
        {
            _service.Add(obj);
        }

        public void Update(EmployeeFunc obj)
        {
            _service.Update(obj);
        }

        public void Delete(EmployeeFunc obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(EmployeeFunc entity, EmployeeFunc existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
