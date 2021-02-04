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
    public class BranchAppService : AppService<AcclineERPContext>, IBranchAppService
    {
        private readonly IBranchService _service;
        public BranchAppService(IBranchService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Branch Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Branch> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Branch> Find(Expression<Func<Branch, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Branch> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Branch obj)
        {
            _service.Add(obj);
        }

        public void Update(Branch obj)
        {
            _service.Update(obj);
        }

        public void Delete(Branch obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Branch entity, Branch existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
