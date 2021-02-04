using App.Domain;
using App.Domain.Interface.Service;
using App.Domain.ViewModel;
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
    public class CommonPVVMAppService : AppService<AcclineERPContext>, ICommonPVVMAppService
    {
        private readonly ICommonPVVMService _service;
        public CommonPVVMAppService(ICommonPVVMService CommonPVVMService)
        {
            _service = CommonPVVMService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CommonPVVM Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CommonPVVM> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CommonPVVM> Find(Expression<Func<CommonPVVM, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CommonPVVM> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CommonPVVM obj)
        {
            _service.Add(obj);
        }

        public void Update(CommonPVVM obj)
        {
            _service.Update(obj);
        }

        public void Delete(CommonPVVM obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CommonPVVM entity, CommonPVVM existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
