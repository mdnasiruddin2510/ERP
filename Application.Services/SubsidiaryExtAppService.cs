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
    public class SubsidiaryExtAppService : AppService<AcclineERPContext>, ISubsidiaryExtAppService
    {
        private readonly ISubsidiaryExtService _service;
        public SubsidiaryExtAppService(ISubsidiaryExtService SubsidiaryExtService)
        {
            _service = SubsidiaryExtService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SubsidiaryInfo_Ext Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SubsidiaryInfo_Ext> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SubsidiaryInfo_Ext> Find(Expression<Func<SubsidiaryInfo_Ext, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SubsidiaryInfo_Ext> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SubsidiaryInfo_Ext obj)
        {
            _service.Add(obj);
        }

        public void Update(SubsidiaryInfo_Ext obj)
        {
            _service.Update(obj);
        }

        public void Delete(SubsidiaryInfo_Ext obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SubsidiaryInfo_Ext entity, SubsidiaryInfo_Ext existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
