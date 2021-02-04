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
    public class PayInvRecvAppService : AppService<AcclineERPContext>, IPayInvRecvAppService
    {
        private readonly IPayInvRecvService _service;
        public PayInvRecvAppService(IPayInvRecvService PayInvRecvService)
        {
            _service = PayInvRecvService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PayInvRecv Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PayInvRecv> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<PayInvRecv> Find(Expression<Func<PayInvRecv, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PayInvRecv> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(PayInvRecv obj)
        {
            _service.Add(obj);
        }

        public void Update(PayInvRecv obj)
        {
            _service.Update(obj);
        }

        public void Delete(PayInvRecv obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(PayInvRecv entity, PayInvRecv existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
