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
    public class VchrSetAppService : AppService<AcclineERPContext>, IVchrSetAppService
    {
        private readonly IVchrSetService _service;
        public VchrSetAppService(IVchrSetService VchrSetService)
        {
            _service = VchrSetService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public VchrSet Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<VchrSet> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<VchrSet> Find(Expression<Func<VchrSet, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<VchrSet> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(VchrSet obj)
        {
            _service.Add(obj);
        }

        public void Update(VchrSet obj)
        {
            _service.Update(obj);
        }

        public void Delete(VchrSet obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(VchrSet entity, VchrSet existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
