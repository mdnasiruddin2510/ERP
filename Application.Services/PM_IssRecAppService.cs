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
    public class PM_IssRecAppService : AppService<AcclineERPContext>, IPM_IssRecAppService
    {
        private readonly IPM_IssRecService _service;
        public PM_IssRecAppService(IPM_IssRecService pM_IssRecAppService)
        {
            _service = pM_IssRecAppService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PM_IssRec Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PM_IssRec> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PM_IssRec> Find(Expression<Func<PM_IssRec, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PM_IssRec> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PM_IssRec obj)
        {
            _service.Add(obj);
        }

        public void Update(PM_IssRec obj)
        {
            _service.Update(obj);
        }

        public void Delete(PM_IssRec obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PM_IssRec entity, PM_IssRec existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
