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
    public class SecFormListAppService : AppService<AcclineERPContext>, ISecFormListAppService
    {
        private readonly ISecFormListService _service;

        public SecFormListAppService(ISecFormListService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecFormList Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecFormList> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecFormList> Find(Expression<Func<SecFormList, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecFormList> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecFormList obj)
        {
            _service.Add(obj);
        }

        public void Update(SecFormList obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecFormList obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecFormList entity, SecFormList existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}