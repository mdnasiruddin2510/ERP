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
    public class CustAdjustmentAppService : AppService<AcclineERPContext>, ICustAdjustmentAppService
    {
        private readonly ICustAdjustmentService _service;
        public CustAdjustmentAppService(ICustAdjustmentService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CustAdjustment Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CustAdjustment> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CustAdjustment> Find(Expression<Func<CustAdjustment, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CustAdjustment> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CustAdjustment obj)
        {
            _service.Add(obj);
        }

        public void Update(CustAdjustment obj)
        {
            _service.Update(obj);
        }

        public void Delete(CustAdjustment obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CustAdjustment entity, CustAdjustment existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
