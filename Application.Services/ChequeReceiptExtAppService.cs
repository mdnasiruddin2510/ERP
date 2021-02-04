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
    public class ChequeReceiptExtAppService : AppService<AcclineERPContext>, IChequeReceiptExtAppService
    {
        private readonly IChequeReceiptExtService _service;
        public ChequeReceiptExtAppService(IChequeReceiptExtService ChequeReceiptExtService)
        {
            _service = ChequeReceiptExtService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ChequeReceiptExt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ChequeReceiptExt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ChequeReceiptExt> Find(Expression<Func<ChequeReceiptExt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ChequeReceiptExt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ChequeReceiptExt obj)
        {
            _service.Add(obj);
        }

        public void Update(ChequeReceiptExt obj)
        {
            _service.Update(obj);
        }

        public void Delete(ChequeReceiptExt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ChequeReceiptExt entity, ChequeReceiptExt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
