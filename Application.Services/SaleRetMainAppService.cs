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
    public class SaleRetMainAppService : AppService<AcclineERPContext>, ISaleRetMainAppService
    {
        private readonly ISaleRetMainService _service;
        public SaleRetMainAppService(ISaleRetMainService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SaleRetMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SaleRetMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SaleRetMain> Find(Expression<Func<SaleRetMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SaleRetMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SaleRetMain obj)
        {
            _service.Add(obj);
        }

        public void Update(SaleRetMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(SaleRetMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SaleRetMain entity, SaleRetMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
