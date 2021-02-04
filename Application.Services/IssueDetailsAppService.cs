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
    public class IssueDetailsAppService : AppService<AcclineERPContext>, IIssueDetailsAppService
    {
        private readonly IIssueDetailsService _service;
        public IssueDetailsAppService(IIssueDetailsService hoRemitService)
        {
            _service = hoRemitService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IssueDetails Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<IssueDetails> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<IssueDetails> Find(Expression<Func<IssueDetails, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<IssueDetails> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(IssueDetails obj)
        {
            _service.Add(obj);
        }

        public void Update(IssueDetails obj)
        {
            _service.Update(obj);
        }

        public void Delete(IssueDetails obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(IssueDetails entity, IssueDetails existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
