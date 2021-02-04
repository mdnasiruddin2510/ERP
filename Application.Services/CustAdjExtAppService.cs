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
    public class CustAdjExtAppService : AppService<AcclineERPContext>, ICustAdjExtAppService
    {
        private readonly ICustAdjExtService _service;
        public CustAdjExtAppService(ICustAdjExtService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CustAdjExt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CustAdjExt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CustAdjExt> Find(Expression<Func<CustAdjExt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CustAdjExt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CustAdjExt obj)
        {
            _service.Add(obj);
        }

        public void Update(CustAdjExt obj)
        {
            _service.Update(obj);
        }

        public void Delete(CustAdjExt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CustAdjExt entity, CustAdjExt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
