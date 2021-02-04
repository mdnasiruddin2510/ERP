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
    public class SecUserGroupAppService : AppService<AcclineERPContext>, ISecUserGroupAppService
    {
        private readonly ISecUserGroupService _service;

        public SecUserGroupAppService(ISecUserGroupService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecUserGroup Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecUserGroup> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecUserGroup> Find(Expression<Func<SecUserGroup, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecUserGroup> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecUserGroup obj)
        {
            _service.Add(obj);
        }

        public void Update(SecUserGroup obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecUserGroup obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecUserGroup entity, SecUserGroup existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}