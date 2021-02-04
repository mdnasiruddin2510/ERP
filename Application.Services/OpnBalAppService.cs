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
    public class OpnBalAppService : AppService<AcclineERPContext>, IOpnBalAppService
    {
        private readonly IOpnBalService _service;
        public OpnBalAppService(IOpnBalService obService)
        {
            _service = obService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public OpnBal Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<OpnBal> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<OpnBal> Find(Expression<Func<OpnBal, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<OpnBal> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(OpnBal obj)
        {
            _service.Add(obj);
        }

        public void Update(OpnBal obj)
        {
            _service.Update(obj);
        }

        public void Delete(OpnBal obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(OpnBal entity, OpnBal existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
