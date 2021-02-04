using App.Domain;
using App.Domain.Interface.Service;
using App.Domain.ViewModel;
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
    public class vuItemInfoAppService : AppService<AcclineERPContext>, IvuItemInfoAppService
    {
        private readonly IvuItemInfoService _service;
        public vuItemInfoAppService(IvuItemInfoService vuItemInfoService)
        {
            _service = vuItemInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public vuItemInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<vuItemInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<vuItemInfo> Find(Expression<Func<vuItemInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<vuItemInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(vuItemInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(vuItemInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(vuItemInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(vuItemInfo entity, vuItemInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
