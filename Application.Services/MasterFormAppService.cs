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
    public class MasterFormAppService : AppService<AcclineERPContext>, IMasterFormAppService
    {
        private readonly IMasterFormService _service;

        public MasterFormAppService(IMasterFormService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MasterForm Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<MasterForm> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<MasterForm> Find(Expression<Func<MasterForm, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<MasterForm> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(MasterForm obj)
        {
            _service.Add(obj);
        }

        public void Update(MasterForm obj)
        {
            _service.Update(obj);
        }

        public void Delete(MasterForm obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(MasterForm entity, MasterForm existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}