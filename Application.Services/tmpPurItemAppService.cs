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
    public class tmpPurItemAppService : AppService<AcclineERPContext>, ItmpPurItemAppService
    {
        private readonly ItmpPurItemService _service;
        public tmpPurItemAppService(ItmpPurItemService tmpPurItemService)
        {
            _service = tmpPurItemService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public tmpPurItem Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<tmpPurItem> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<tmpPurItem> Find(Expression<Func<tmpPurItem, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<tmpPurItem> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(tmpPurItem obj)
        {
            _service.Add(obj);
        }

        public void Update(tmpPurItem obj)
        {
            _service.Update(obj);
        }

        public void Delete(tmpPurItem obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(tmpPurItem entity, tmpPurItem existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
