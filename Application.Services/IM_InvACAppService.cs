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
    public class IM_InvACAppService : AppService<AcclineERPContext>, IIM_InvACAppService
    {
        private readonly IIM_InvACService _service;
        public IM_InvACAppService(IIM_InvACService iiM_InvACService)
        {
            _service = iiM_InvACService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IM_InvAC Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<IM_InvAC> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<IM_InvAC> Find(Expression<Func<IM_InvAC, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<IM_InvAC> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(IM_InvAC obj)
        {
            _service.Add(obj);
        }

        public void Update(IM_InvAC obj)
        {
            _service.Update(obj);
        }

        public void Delete(IM_InvAC obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(IM_InvAC entity, IM_InvAC existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
