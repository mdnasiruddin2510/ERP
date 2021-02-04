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
    public class LedgerCaptionAppService : AppService<AcclineERPContext>, ILedgerCaptionAppService
    {
        private readonly ILedgerCaptionService _service;
        public LedgerCaptionAppService(ILedgerCaptionService ldgCapService)
        {
            _service = ldgCapService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LedgerCaption Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<LedgerCaption> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<LedgerCaption> Find(Expression<Func<LedgerCaption, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<LedgerCaption> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(LedgerCaption obj)
        {
            _service.Add(obj);
        }

        public void Update(LedgerCaption obj)
        {
            _service.Update(obj);
        }

        public void Delete(LedgerCaption obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(LedgerCaption entity, LedgerCaption existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
