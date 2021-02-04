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
    public class IssRecSrcDestAppService : AppService<AcclineERPContext>, IIssRecSrcDestAppService
    {
        private readonly IIssRecSrcDestService _service;
        public IssRecSrcDestAppService(IIssRecSrcDestService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IssRecSrcDest Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<IssRecSrcDest> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<IssRecSrcDest> Find(Expression<Func<IssRecSrcDest, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<IssRecSrcDest> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(IssRecSrcDest obj)
        {
            _service.Add(obj);
        }

        public void Update(IssRecSrcDest obj)
        {
            _service.Update(obj);
        }

        public void Delete(IssRecSrcDest obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(IssRecSrcDest entity, IssRecSrcDest existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
