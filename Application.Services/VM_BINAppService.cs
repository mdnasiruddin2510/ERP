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
    public class VM_BINAppService : AppService<AcclineERPContext>, IVM_BINAppService
    {
        private readonly IVM_BINService _service;
        public VM_BINAppService(IVM_BINService VM_BINService)
        {
            _service = VM_BINService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public VM_BIN Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<VM_BIN> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<VM_BIN> Find(Expression<Func<VM_BIN, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<VM_BIN> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(VM_BIN obj)
        {
            _service.Add(obj);
        }

        public void Update(VM_BIN obj)
        {
            _service.Update(obj);
        }

        public void Delete(VM_BIN obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(VM_BIN entity, VM_BIN existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
