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
    public class SubsidiaryInfoAppService : AppService<AcclineERPContext>, ISubsidiaryInfoAppService
    {
        private readonly ISubsidiaryInfoService _service;
        public SubsidiaryInfoAppService(ISubsidiaryInfoService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SubsidiaryInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SubsidiaryInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SubsidiaryInfo> Find(Expression<Func<SubsidiaryInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SubsidiaryInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SubsidiaryInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(SubsidiaryInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(SubsidiaryInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SubsidiaryInfo entity, SubsidiaryInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
