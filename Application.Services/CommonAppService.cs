using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Domain;
using App.Domain.Interface.Service.Common;
using App.Domain.Interface.VModel;
using Application.Interfaces;
using Application.Interfaces.Common;
using Data.Context;

namespace Application.Services
{
    public class CommonAppService : AppService<AcclineERPContext>, ICommonAppService
    {
        private readonly ICommonSearchService _service;

        public CommonAppService(ICommonSearchService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ICommonSearchModel Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ICommonSearchModel> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<ICommonSearchModel> Find(Expression<Func<ICommonSearchModel, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ICommonSearchModel> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(ICommonSearchModel obj)
        {
            _service.Add(obj);
        }

        public void Update(ICommonSearchModel obj)
        {
            _service.Update(obj);
        }

        public void Delete(ICommonSearchModel obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(ICommonSearchModel entity, ICommonSearchModel existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
