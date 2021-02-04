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
    public class SecFormRightAppService : AppService<AcclineERPContext>, ISecFormRightAppService
    {
        private readonly ISecFormRightService _service;

        public SecFormRightAppService(ISecFormRightService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SecFormRight Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SecFormRight> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<SecFormRight> Find(Expression<Func<SecFormRight, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SecFormRight> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(SecFormRight obj)
        {
            _service.Add(obj);
        }

        public void Update(SecFormRight obj)
        {
            _service.Update(obj);
        }

        public void Delete(SecFormRight obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(SecFormRight entity, SecFormRight existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}