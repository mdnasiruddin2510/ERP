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
    public class TVchrMainAppService : AppService<AcclineERPContext>, ITVchrMainAppService
    {
        private readonly ITVchrMainService _service;
        public TVchrMainAppService(ITVchrMainService TVchrMainService)
        {
            _service = TVchrMainService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TVchrMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<TVchrMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<TVchrMain> Find(Expression<Func<TVchrMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<TVchrMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(TVchrMain obj)
        {
            _service.Add(obj);
        }

        public void Update(TVchrMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(TVchrMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(TVchrMain entity, TVchrMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
