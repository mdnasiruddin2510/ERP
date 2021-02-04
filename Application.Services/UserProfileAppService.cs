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
    public class UserProfileAppService : AppService<AcclineERPContext>, IUserProfileAppService
    {
        private readonly IUserProfileService _service;

        public UserProfileAppService(IUserProfileService service)
        {
            _service = service;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public UserProfileSup Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<UserProfileSup> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<UserProfileSup> Find(Expression<Func<UserProfileSup, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<UserProfileSup> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(UserProfileSup obj)
        {
            _service.Add(obj);
        }

        public void Update(UserProfileSup obj)
        {
            _service.Update(obj);
        }

        public void Delete(UserProfileSup obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }

        public UserProfileSup GetByName(string uName)
        {
           return _service.All().FirstOrDefault(x => x.UserName == uName);
        }
        public void Setvalues(UserProfileSup entity, UserProfileSup existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
