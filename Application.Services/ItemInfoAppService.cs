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
    public class ItemInfoAppService : AppService<AcclineERPContext>, IItemInfoAppService
    {
        private readonly IItemInfoService _service;
        public ItemInfoAppService(IItemInfoService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ItemInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ItemInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ItemInfo> Find(Expression<Func<ItemInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ItemInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ItemInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(ItemInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(ItemInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ItemInfo entity, ItemInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
