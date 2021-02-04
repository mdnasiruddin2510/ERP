using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Application.Interfaces.Common;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailConfigAppService : AppService<AcclineERPContext>, IEmailConfigAppService
    {
         private readonly IEmailConfigService _service;
        public EmailConfigAppService(IEmailConfigService emailConfig)
        {
            _service = emailConfig;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public EmailConfig Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<EmailConfig> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<EmailConfig> Find(Expression<Func<EmailConfig, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<EmailConfig> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(EmailConfig obj)
        {
            _service.Add(obj);
        }

        public void Update(EmailConfig obj)
        {
            _service.Update(obj);
        }

        public void Delete(EmailConfig obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
           _service.Save();
        }
        public void Setvalues(EmailConfig entity, EmailConfig existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
