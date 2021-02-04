using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;

namespace Application.Services
{
    public class TransactionLogAppService : AppService<AcclineERPContext>, ITransactionLogAppService
    {
        private readonly ITransactionLogService _service;

        public TransactionLogAppService(ITransactionLogService transactionLogService)
        {
            _service = transactionLogService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TransactionLog Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<TransactionLog> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<TransactionLog> Find(Expression<Func<TransactionLog, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<TransactionLog> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }
        public void Add(TransactionLog obj)
        {
            _service.Add(obj);
        }

        public void Update(TransactionLog obj)
        {
            _service.Update(obj);
        }

        public void Delete(TransactionLog obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
           _service.Save();
        }
        public void Setvalues(TransactionLog entity, TransactionLog existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
