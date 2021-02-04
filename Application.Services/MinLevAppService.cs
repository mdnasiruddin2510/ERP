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
    public class MinLevAppService : AppService<AcclineERPContext>, IMinLevAppService
    {
        private readonly IMinLevService _service;
        public MinLevAppService(IMinLevService minLevService)
        {
            _service = minLevService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MinLev Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<MinLev> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<MinLev> Find(Expression<Func<MinLev, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<MinLev> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(MinLev obj)
        {
            _service.Add(obj);
        }

        public void Update(MinLev obj)
        {
            _service.Update(obj);
        }

        public void Delete(MinLev obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(MinLev entity, MinLev existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
