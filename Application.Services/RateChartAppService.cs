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
    public class RateChartAppService : AppService<AcclineERPContext>, IRateChartAppService
    {
        private readonly IRateChartService _service;
        public RateChartAppService(IRateChartService RateChartService)
        {
            _service = RateChartService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public RateChart Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<RateChart> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<RateChart> Find(Expression<Func<RateChart, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<RateChart> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(RateChart obj)
        {
            _service.Add(obj);
        }

        public void Update(RateChart obj)
        {
            _service.Update(obj);
        }

        public void Delete(RateChart obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(RateChart entity, RateChart existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
