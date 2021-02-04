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
    public class BankInfoAppService : AppService<AcclineERPContext>, IBankInfoAppService
    {
        private readonly IBankInfoService _service;
        public BankInfoAppService(IBankInfoService bankInfoService)
        {
            _service = bankInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public BankInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<BankInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<BankInfo> Find(Expression<Func<BankInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<BankInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(BankInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(BankInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(BankInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(BankInfo entity, BankInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
