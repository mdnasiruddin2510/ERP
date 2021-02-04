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
    public class GroupInfoAppService : AppService<AcclineERPContext>, IGroupInfoAppService
    {
        private readonly IGroupInfoService _service;
        public GroupInfoAppService(IGroupInfoService groupinfoService)
        {
            _service = groupinfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public GroupInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<GroupInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<GroupInfo> Find(Expression<Func<GroupInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<GroupInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(GroupInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(GroupInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(GroupInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(GroupInfo entity, GroupInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
