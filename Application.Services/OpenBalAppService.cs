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
    public class OpenBalAppService : AppService<AcclineERPContext>, IOpenBalAppService
    {
        private readonly IOpenBalService _service;
        public OpenBalAppService(IOpenBalService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public OpenBal Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<OpenBal> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<OpenBal> Find(Expression<Func<OpenBal, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<OpenBal> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(OpenBal obj)
        {
            _service.Add(obj);
        }

        public void Update(OpenBal obj)
        {
            _service.Update(obj);
        }

        public void Delete(OpenBal obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(OpenBal entity, OpenBal existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
