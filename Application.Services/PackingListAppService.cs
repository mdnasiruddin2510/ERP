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
    public class PackingListAppService : AppService<AcclineERPContext>, IPackingListAppService
    {
        private readonly IPackingListService _service;
        public PackingListAppService(IPackingListService PackingListService)
        {
            _service = PackingListService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PackingList Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PackingList> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PackingList> Find(Expression<Func<PackingList, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PackingList> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PackingList obj)
        {
            _service.Add(obj);
        }

        public void Update(PackingList obj)
        {
            _service.Update(obj);
        }

        public void Delete(PackingList obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PackingList entity, PackingList existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
