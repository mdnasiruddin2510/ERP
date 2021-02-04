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
    public class ExpenseAppService : AppService<AcclineERPContext>, IExpenseAppService
    {
        private readonly IExpenseService _service;
        public ExpenseAppService(IExpenseService hoRemittance)
        {
            _service = hoRemittance;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Expense Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<Expense> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<Expense> Find(Expression<Func<Expense, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<Expense> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(Expense obj)
        {
            _service.Add(obj);
        }

        public void Update(Expense obj)
        {
            _service.Update(obj);
        }

        public void Delete(Expense obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(Expense entity, Expense existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
