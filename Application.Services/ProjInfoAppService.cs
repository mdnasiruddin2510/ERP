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
    public class ProjInfoAppService : AppService<AcclineERPContext>, IProjInfoAppService
    {
        private readonly IProjInfoService _service;
        public ProjInfoAppService(IProjInfoService projInfoService)
        {
            _service = projInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ProjInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ProjInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ProjInfo> Find(Expression<Func<ProjInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ProjInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ProjInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(ProjInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(ProjInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ProjInfo entity, ProjInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
