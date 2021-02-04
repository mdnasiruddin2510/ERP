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
   public class CurrentStockAppService : AppService<AcclineERPContext>, ICurrentStockAppService
    {
        private readonly ICurrentStockService _service;
        public CurrentStockAppService(ICurrentStockService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CurrentStock Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CurrentStock> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CurrentStock> Find(Expression<Func<CurrentStock, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CurrentStock> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CurrentStock obj)
        {
            _service.Add(obj);
        }

        public void Update(CurrentStock obj)
        {
            _service.Update(obj);
        }

        public void Delete(CurrentStock obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CurrentStock entity, CurrentStock existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
