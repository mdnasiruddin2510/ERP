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
    public class UnitAppService : AppService<AcclineERPContext>, IUnitAppService
    {
        private readonly IUnitService _service;
        public UnitAppService(IUnitService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Unit Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Unit> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Unit> Find(Expression<Func<Unit, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Unit> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Unit obj)
        {
            _service.Add(obj);
        }

        public void Update(Unit obj)
        {
            _service.Update(obj);
        }

        public void Delete(Unit obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Unit entity, Unit existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
