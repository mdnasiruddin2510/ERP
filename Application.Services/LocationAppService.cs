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
    public class LocationAppService : AppService<AcclineERPContext>, ILocationAppService
    {
        private readonly ILocationService _service;
        public LocationAppService(ILocationService newChart)
        {
            _service = newChart;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Location Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Location> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Location> Find(Expression<Func<Location, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Location> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Location obj)
        {
            _service.Add(obj);
        }

        public void Update(Location obj)
        {
            _service.Update(obj);
        }

        public void Delete(Location obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Location entity, Location existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
