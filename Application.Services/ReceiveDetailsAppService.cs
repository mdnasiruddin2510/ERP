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
    public class ReceiveDetailsAppService : AppService<AcclineERPContext>, IReceiveDetailsAppService
    {
        private readonly IReceiveDetailsService _service;
        public ReceiveDetailsAppService(IReceiveDetailsService newChart)
        {
            _service = newChart;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ReceiveDetails Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ReceiveDetails> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ReceiveDetails> Find(Expression<Func<ReceiveDetails, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ReceiveDetails> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ReceiveDetails obj)
        {
            _service.Add(obj);
        }

        public void Update(ReceiveDetails obj)
        {
            _service.Update(obj);
        }

        public void Delete(ReceiveDetails obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ReceiveDetails entity, ReceiveDetails existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
    
}
