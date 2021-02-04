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
    public class SubsidiaryCtrlAppService : AppService<AcclineERPContext>, ISubsidiaryCtrlAppService
    {
        private readonly ISubsidiaryCtrlService _service;
        public SubsidiaryCtrlAppService(ISubsidiaryCtrlService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SubsidiaryCtrl Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SubsidiaryCtrl> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SubsidiaryCtrl> Find(Expression<Func<SubsidiaryCtrl, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SubsidiaryCtrl> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SubsidiaryCtrl obj)
        {
            _service.Add(obj);
        }

        public void Update(SubsidiaryCtrl obj)
        {
            _service.Update(obj);
        }

        public void Delete(SubsidiaryCtrl obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SubsidiaryCtrl entity, SubsidiaryCtrl existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
