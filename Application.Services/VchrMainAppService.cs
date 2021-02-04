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
    public class VchrMainAppService : AppService<AcclineERPContext>, IVchrMainAppService
    {
        private readonly IVchrMainService _service;
        public VchrMainAppService(IVchrMainService vchrMainService)
        {
            _service = vchrMainService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public VchrMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<VchrMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<VchrMain> Find(Expression<Func<VchrMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<VchrMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(VchrMain obj)
        {
            _service.Add(obj);
        }

        public void Update(VchrMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(VchrMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(VchrMain entity, VchrMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
