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
    public class ReceiveAppService : AppService<AcclineERPContext>, IReceiveAppService
    {
        private readonly IReceiveService _service;
        public ReceiveAppService(IReceiveService beatInfoService)
        {
            _service = beatInfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ReceiveMain Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ReceiveMain> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<ReceiveMain> Find(Expression<Func<ReceiveMain, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }
        public IEnumerable<ReceiveMain> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ReceiveMain obj)
        {
            _service.Add(obj);
        }

        public void Update(ReceiveMain obj)
        {
            _service.Update(obj);
        }

        public void Delete(ReceiveMain obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ReceiveMain entity, ReceiveMain existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
