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
    public class SubsidiaryTypeAppService : AppService<AcclineERPContext>, ISubsidiaryTypeAppService
    {
        private readonly ISubsidiaryTypeService _service;
        public SubsidiaryTypeAppService(ISubsidiaryTypeService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SubsidiaryType Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SubsidiaryType> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SubsidiaryType> Find(Expression<Func<SubsidiaryType, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SubsidiaryType> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SubsidiaryType obj)
        {
            _service.Add(obj);
        }

        public void Update(SubsidiaryType obj)
        {
            _service.Update(obj);
        }

        public void Delete(SubsidiaryType obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SubsidiaryType entity, SubsidiaryType existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
