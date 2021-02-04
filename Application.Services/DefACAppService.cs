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
    public class DefACAppService : AppService<AcclineERPContext>, IDefACAppService
    {
        private readonly IDefACService _service;
        public DefACAppService(IDefACService DefACService)
        {
            _service = DefACService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public DefAC Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<DefAC> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<DefAC> Find(Expression<Func<DefAC, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<DefAC> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(DefAC obj)
        {
            _service.Add(obj);
        }

        public void Update(DefAC obj)
        {
            _service.Update(obj);
        }

        public void Delete(DefAC obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(DefAC entity, DefAC existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
