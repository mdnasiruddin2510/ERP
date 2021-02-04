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
    public class PurRetDetailAppService : AppService<AcclineERPContext>, IPurRetDetailAppService
    {
        private readonly IPurRetDetailService _service;
        public PurRetDetailAppService(IPurRetDetailService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PurRetDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PurRetDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PurRetDetail> Find(Expression<Func<PurRetDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PurRetDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PurRetDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(PurRetDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(PurRetDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PurRetDetail entity, PurRetDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
