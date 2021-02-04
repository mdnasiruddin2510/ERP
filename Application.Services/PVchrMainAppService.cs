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
    public class PVchrMainAppService : AppService<AcclineERPContext>, IPVchrMainAppService
    {
        private readonly IPVchrMainService _service;
        public PVchrMainAppService(IPVchrMainService PVchrMainService)
        {
            _service = PVchrMainService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PVchrMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PVchrMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PVchrMain> Find(Expression<Func<PVchrMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PVchrMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PVchrMain obj)
        {
            _service.Add(obj);
        }

        public void Update(PVchrMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(PVchrMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PVchrMain entity, PVchrMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
