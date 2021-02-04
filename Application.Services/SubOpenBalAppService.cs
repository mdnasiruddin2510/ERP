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
    public class SubOpenBalAppService : AppService<AcclineERPContext>, ISubOpenBalAppService
    {
        private readonly ISubOpenBalService _service;
        public SubOpenBalAppService(ISubOpenBalService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SubOpenBal Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SubOpenBal> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SubOpenBal> Find(Expression<Func<SubOpenBal, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SubOpenBal> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SubOpenBal obj)
        {
            _service.Add(obj);
        }

        public void Update(SubOpenBal obj)
        {
            _service.Update(obj);
        }

        public void Delete(SubOpenBal obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SubOpenBal entity, SubOpenBal existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
