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
    public class SGroupInfoAppService : AppService<AcclineERPContext>, ISGroupInfoAppService
    {
        private readonly ISGroupInfoService _service;
        public SGroupInfoAppService(ISGroupInfoService sgroupinfoService)
        {
            _service = sgroupinfoService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SGroupInfo Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SGroupInfo> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SGroupInfo> Find(Expression<Func<SGroupInfo, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SGroupInfo> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SGroupInfo obj)
        {
            _service.Add(obj);
        }

        public void Update(SGroupInfo obj)
        {
            _service.Update(obj);
        }

        public void Delete(SGroupInfo obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SGroupInfo entity, SGroupInfo existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
