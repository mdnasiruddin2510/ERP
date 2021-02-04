using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;

namespace Application.Services
{
    public class SecUserInfoAppService : AppService<AcclineERPContext>, ISecUserInfoAppService
    {
        private readonly ISecUserInfoService _service;

        public SecUserInfoAppService(ISecUserInfoService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecUserInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecUserInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecUserInfo> Find(Expression<Func<SecUserInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecUserInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecUserInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(SecUserInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecUserInfo obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecUserInfo entity, SecUserInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}