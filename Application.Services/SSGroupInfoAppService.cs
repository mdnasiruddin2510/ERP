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
    public class SSGroupInfoAppService : AppService<AcclineERPContext>, ISSGroupInfoAppService
    {
        private readonly ISSGroupInfoService _service;
        public SSGroupInfoAppService(ISSGroupInfoService ssgroupinfoService)
        {
            _service = ssgroupinfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SSGroupInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SSGroupInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SSGroupInfo> Find(Expression<Func<SSGroupInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SSGroupInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SSGroupInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(SSGroupInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(SSGroupInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SSGroupInfo entity, SSGroupInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
