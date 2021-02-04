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
    public class RecPasswordAppService : AppService<AcclineERPContext>, IRecPasswordAppService
    {
        private readonly IRecPasswordService _service;
        public RecPasswordAppService(IRecPasswordService RecPasswordService)
        {
            _service = RecPasswordService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public RecPassword Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<RecPassword> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<RecPassword> Find(Expression<Func<RecPassword, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<RecPassword> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(RecPassword obj)
        {
            _service.Add(obj);
        }

        public void Update(RecPassword obj)
        {
            _service.Update(obj);
        }

        public void Delete(RecPassword obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(RecPassword entity, RecPassword existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
