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
    public class ItemTypeAppService : AppService<AcclineERPContext>, IItemTypeAppService
    {
        private readonly IItemTypeService _service;
        public ItemTypeAppService(IItemTypeService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ItemType Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ItemType> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ItemType> Find(Expression<Func<ItemType, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ItemType> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ItemType obj)
        {
            _service.Add(obj);
        }

        public void Update(ItemType obj)
        {
            _service.Update(obj);
        }

        public void Delete(ItemType obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ItemType entity, ItemType existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
