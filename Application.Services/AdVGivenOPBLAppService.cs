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
    public class AdVGivenOPBLAppService : AppService<AcclineERPContext>, IAdVGivenOPBLAppService
    {
        private readonly IAdVGivenOPBLService _service;
        public AdVGivenOPBLAppService(IAdVGivenOPBLService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public AdVGivenOPBL Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<AdVGivenOPBL> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<AdVGivenOPBL> Find(Expression<Func<AdVGivenOPBL, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<AdVGivenOPBL> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(AdVGivenOPBL obj)
        {
            _service.Add(obj);
        }

        public void Update(AdVGivenOPBL obj)
        {
            _service.Update(obj);
        }

        public void Delete(AdVGivenOPBL obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(AdVGivenOPBL entity, AdVGivenOPBL existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
