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
    public class SaleRetDetailAppService : AppService<AcclineERPContext>, ISaleRetDetailAppService
    {
        private readonly ISaleRetDetailService _service;
        public SaleRetDetailAppService(ISaleRetDetailService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SaleRetDetail Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SaleRetDetail> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SaleRetDetail> Find(Expression<Func<SaleRetDetail, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SaleRetDetail> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SaleRetDetail obj)
        {
            _service.Add(obj);
        }

        public void Update(SaleRetDetail obj)
        {
            _service.Update(obj);
        }

        public void Delete(SaleRetDetail obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SaleRetDetail entity, SaleRetDetail existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
