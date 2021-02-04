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
    public class SecProcessListAppService : AppService<AcclineERPContext>, ISecProcessListAppService
    {
        private readonly ISecProcessListService _service;

        public SecProcessListAppService(ISecProcessListService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecProcessList Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecProcessList> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecProcessList> Find(Expression<Func<SecProcessList, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecProcessList> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecProcessList obj)
        {
            _service.Add(obj);
        }

        public void Update(SecProcessList obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecProcessList obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecProcessList entity, SecProcessList existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}