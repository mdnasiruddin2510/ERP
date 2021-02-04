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
    public class ItemListAppService : AppService<AcclineERPContext>, IItemListAppService
    {
        private readonly IItemListService _service;
        public ItemListAppService(IItemListService itemService)
        {
            _service = itemService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ItemList Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ItemList> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ItemList> Find(Expression<Func<ItemList, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ItemList> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ItemList obj)
        {
            _service.Add(obj);
        }

        public void Update(ItemList obj)
        {
            _service.Update(obj);
        }

        public void Delete(ItemList obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ItemList entity, ItemList existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
