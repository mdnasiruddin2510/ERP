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
    public class FYDDAppService : AppService<AcclineERPContext>, IFYDDAppService
    {
        private readonly IFYDDService _service;
        public FYDDAppService(IFYDDService finyear)
        {
            _service = finyear;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public FYDD Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<FYDD> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<FYDD> Find(Expression<Func<FYDD, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<FYDD> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(FYDD obj)
        {
            _service.Add(obj);
        }

        public void Update(FYDD obj)
        {
            _service.Update(obj);
        }

        public void Delete(FYDD obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(FYDD entity, FYDD existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
