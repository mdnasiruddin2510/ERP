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
    public class NewChartAppService : AppService<AcclineERPContext>, INewChartAppService
    {
        private readonly INewChartService _service;
        public NewChartAppService(INewChartService newChart)
        {
            _service = newChart;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public NewChart Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<NewChart> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<NewChart> Find(Expression<Func<NewChart, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<NewChart> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(NewChart obj)
        {
            _service.Add(obj);
        }

        public void Update(NewChart obj)
        {
            _service.Update(obj);
        }

        public void Delete(NewChart obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(NewChart entity, NewChart existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
