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
    public class PurchaseDetailAppService : AppService<AcclineERPContext>, IPurchaseDetailAppService
    {
        private readonly IPurchaseDetailService _service;
        public PurchaseDetailAppService(IPurchaseDetailService PurchaseDetailService)
        {
            _service = PurchaseDetailService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PurchaseDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PurchaseDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PurchaseDetail> Find(Expression<Func<PurchaseDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PurchaseDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PurchaseDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(PurchaseDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(PurchaseDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PurchaseDetail entity, PurchaseDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
