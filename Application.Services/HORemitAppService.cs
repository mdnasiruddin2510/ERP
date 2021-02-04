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
    public class HORemitAppService : AppService<AcclineERPContext>, IHORemitAppService
    {
        private readonly IHORemitService _service;
        public HORemitAppService(IHORemitService hoRemitService)
        {
            _service = hoRemitService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public HORemit Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<HORemit> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<HORemit> Find(Expression<Func<HORemit, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<HORemit> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(HORemit obj)
        {
            _service.Add(obj);
        }

        public void Update(HORemit obj)
        {
            _service.Update(obj);
        }

        public void Delete(HORemit obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(HORemit entity, HORemit existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
