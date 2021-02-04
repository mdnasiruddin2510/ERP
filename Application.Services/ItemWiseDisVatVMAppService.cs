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
    public class ItemWiseDisVatVMAppService : AppService<AcclineERPContext>, IItemWiseDisVatAppService
    {
        private readonly IItemWiseDisVatVMService _service;
        public ItemWiseDisVatVMAppService(IItemWiseDisVatVMService ItemWiseDisVatVMService)
        {
            _service = ItemWiseDisVatVMService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ItemWiseDisVatVM Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ItemWiseDisVatVM> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ItemWiseDisVatVM> Find(Expression<Func<ItemWiseDisVatVM, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ItemWiseDisVatVM> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ItemWiseDisVatVM obj)
        {
            _service.Add(obj);
        }

        public void Update(ItemWiseDisVatVM obj)
        {
            _service.Update(obj);
        }

        public void Delete(ItemWiseDisVatVM obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ItemWiseDisVatVM entity, ItemWiseDisVatVM existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
