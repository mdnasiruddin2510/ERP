using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;

namespace Application.Services
{
    public class CompanyInfoAppService : AppService<AcclineERPContext>, ICompanyInfoAppService
    {
        private readonly ICompanyInfoService _service;

        public CompanyInfoAppService(ICompanyInfoService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CompanyInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CompanyInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<CompanyInfo> Find(Expression<Func<CompanyInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CompanyInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(CompanyInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(CompanyInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(CompanyInfo obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(CompanyInfo entity, CompanyInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}