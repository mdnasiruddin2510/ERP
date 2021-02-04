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
    public class LotDTAppService : AppService<AcclineERPContext>, ILotDTAppService
    {
        private readonly ILotDTService _service;
        public LotDTAppService(ILotDTService LotDTService)
        {
            _service = LotDTService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LotDT Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<LotDT> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<LotDT> Find(Expression<Func<LotDT, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<LotDT> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(LotDT obj)
        {
            _service.Add(obj);
        }

        public void Update(LotDT obj)
        {
            _service.Update(obj);
        }

        public void Delete(LotDT obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(LotDT entity, LotDT existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
