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
    public class FuncSLAppService : AppService<AcclineERPContext>, IFuncSLAppService
    {
        private readonly IFuncSLService _service;
        public FuncSLAppService(IFuncSLService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public FuncSL Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<FuncSL> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<FuncSL> Find(Expression<Func<FuncSL, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<FuncSL> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(FuncSL obj)
        {
            _service.Add(obj);
        }

        public void Update(FuncSL obj)
        {
            _service.Update(obj);
        }

        public void Delete(FuncSL obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(FuncSL entity, FuncSL existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
