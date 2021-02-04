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
    public class AcBRAppService : AppService<AcclineERPContext>, IAcBRAppService
    {
        private readonly IAcBRService _service;
        public AcBRAppService(IAcBRService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public AcBR Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<AcBR> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<AcBR> Find(Expression<Func<AcBR, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<AcBR> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(AcBR obj)
        {
            _service.Add(obj);
        }

        public void Update(AcBR obj)
        {
            _service.Update(obj);
        }

        public void Delete(AcBR obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(AcBR entity, AcBR existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
