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
    public class IssueMainAppService : AppService<AcclineERPContext>, IIssueMainAppService
    {
        private readonly IIssueMainService _service;
        public IssueMainAppService(IIssueMainService issueMainService)
        {
            _service = issueMainService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IssueMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<IssueMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<IssueMain> Find(Expression<Func<IssueMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<IssueMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(IssueMain obj)
        {
            _service.Add(obj);
        }

        public void Update(IssueMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(IssueMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(IssueMain entity, IssueMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
