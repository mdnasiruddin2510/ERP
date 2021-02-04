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
    public class UserBranchAppService : AppService<AcclineERPContext>, IUserBranchAppService
    {
        private readonly IUserBranchService _service;

        public UserBranchAppService(IUserBranchService service)
        {
            _service = service;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public UserBranch Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<UserBranch> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<UserBranch> Find(Expression<Func<UserBranch, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<UserBranch> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(UserBranch obj)
        {
            _service.Add(obj);
        }

        public void Update(UserBranch obj)
        {
            _service.Update(obj);
        }

        public void Delete(UserBranch obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }

        public UserBranch GetByName(string uName)
        {
            return _service.All().FirstOrDefault(x => x.Userid == uName);
        }
        public void Setvalues(UserBranch entity, UserBranch existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
