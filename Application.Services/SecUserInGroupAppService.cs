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
    public class SecUserInGroupAppService : AppService<AcclineERPContext>, ISecUserInGroupAppService
    {
        private readonly ISecUserInGroupService _service;

        public SecUserInGroupAppService(ISecUserInGroupService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecUserInGroup Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecUserInGroup> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecUserInGroup> Find(Expression<Func<SecUserInGroup, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecUserInGroup> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecUserInGroup obj)
        {
            _service.Add(obj);
        }

        public void Update(SecUserInGroup obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecUserInGroup obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecUserInGroup entity, SecUserInGroup existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}