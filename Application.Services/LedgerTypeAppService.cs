﻿using App.Domain;
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
    public class LedgerTypeAppService : AppService<AcclineERPContext>, ILedgerTypeAppService
    {
        private readonly ILedgerTypeService _service;
        public LedgerTypeAppService(ILedgerTypeService ldgTypeService)
        {
            _service = ldgTypeService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LedgerType Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<LedgerType> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<LedgerType> Find(Expression<Func<LedgerType, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<LedgerType> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(LedgerType obj)
        {
            _service.Add(obj);
        }

        public void Update(LedgerType obj)
        {
            _service.Update(obj);
        }

        public void Delete(LedgerType obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(LedgerType entity, LedgerType existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
