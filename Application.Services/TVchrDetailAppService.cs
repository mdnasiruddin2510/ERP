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
    public class TVchrDetailAppService : AppService<AcclineERPContext>, ITVchrDetailAppService
    {
        private readonly ITVchrDetailService _service;
        public TVchrDetailAppService(ITVchrDetailService TVchrDetailService)
        {
            _service = TVchrDetailService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TVchrDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<TVchrDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<TVchrDetail> Find(Expression<Func<TVchrDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<TVchrDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(TVchrDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(TVchrDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(TVchrDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(TVchrDetail entity, TVchrDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
