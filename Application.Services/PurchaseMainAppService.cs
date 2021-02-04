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
    public class PurchaseMainAppService : AppService<AcclineERPContext>, IPurchaseMainAppService
    {
        private readonly IPurchaseMainService _service;
        public PurchaseMainAppService(IPurchaseMainService PurchaseMainService)
        {
            _service = PurchaseMainService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PurchaseMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PurchaseMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PurchaseMain> Find(Expression<Func<PurchaseMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PurchaseMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PurchaseMain obj)
        {
            _service.Add(obj);
        }

        public void Update(PurchaseMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(PurchaseMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PurchaseMain entity, PurchaseMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
