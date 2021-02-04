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
    public class PVchrDetailAppService : AppService<AcclineERPContext>, IPVchrDetailAppService
    {
        private readonly IPVchrDetailService _service;
        public PVchrDetailAppService(IPVchrDetailService PVchrDetailService)
        {
            _service = PVchrDetailService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PVchrDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PVchrDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PVchrDetail> Find(Expression<Func<PVchrDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PVchrDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PVchrDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(PVchrDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(PVchrDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PVchrDetail entity, PVchrDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
