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
    public class UnitInfoAppService : AppService<AcclineERPContext>, IUnitInfoAppService
    {
        private readonly IUnitInfoService _service;
        public UnitInfoAppService(IUnitInfoService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public UnitInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<UnitInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<UnitInfo> Find(Expression<Func<UnitInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<UnitInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(UnitInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(UnitInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(UnitInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(UnitInfo entity, UnitInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
