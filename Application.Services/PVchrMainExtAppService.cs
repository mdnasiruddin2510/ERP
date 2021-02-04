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
    public class PVchrMainExtAppService : AppService<AcclineERPContext>, IPVchrMainExtAppService
    {
        private readonly IPVchrMainExtService _service;
        public PVchrMainExtAppService(IPVchrMainExtService PVchrMainExtService)
        {
            _service = PVchrMainExtService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PVchrMainExt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PVchrMainExt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PVchrMainExt> Find(Expression<Func<PVchrMainExt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PVchrMainExt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PVchrMainExt obj)
        {
            _service.Add(obj);
        }

        public void Update(PVchrMainExt obj)
        {
            _service.Update(obj);
        }

        public void Delete(PVchrMainExt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PVchrMainExt entity, PVchrMainExt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
