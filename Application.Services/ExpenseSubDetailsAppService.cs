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
    public class ExpenseSubDetailsAppService : AppService<AcclineERPContext>, IExpenseSubDetailsAppService
    {
        private readonly IExpenseSubDetailsService _service;
        public ExpenseSubDetailsAppService(IExpenseSubDetailsService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ExpenseSubDetails Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ExpenseSubDetails> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ExpenseSubDetails> Find(Expression<Func<ExpenseSubDetails, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ExpenseSubDetails> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ExpenseSubDetails obj)
        {
            _service.Add(obj);
        }

        public void Update(ExpenseSubDetails obj)
        {
            _service.Update(obj);
        }

        public void Delete(ExpenseSubDetails obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ExpenseSubDetails entity, ExpenseSubDetails existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
