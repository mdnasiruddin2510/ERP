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
    public class VchrDetailAppService : AppService<AcclineERPContext>, IVchrDetailAppService
    {
        private readonly IVchrDetailService _service;
        public VchrDetailAppService(IVchrDetailService vchrDetailService)
        {
            _service = vchrDetailService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public VchrDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<VchrDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<VchrDetail> Find(Expression<Func<VchrDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<VchrDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(VchrDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(VchrDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(VchrDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(VchrDetail entity, VchrDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
