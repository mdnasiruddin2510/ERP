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
    public class ReportLedgerAppService : AppService<AcclineERPContext>, IReportLedgerAppService
    {
        private readonly IReportLedgerService _service;
        public ReportLedgerAppService(IReportLedgerService rptLdgService)
        {
            _service = rptLdgService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ReportLedger Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ReportLedger> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ReportLedger> Find(Expression<Func<ReportLedger, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ReportLedger> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ReportLedger obj)
        {
            _service.Add(obj);
        }

        public void Update(ReportLedger obj)
        {
            _service.Update(obj);
        }

        public void Delete(ReportLedger obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ReportLedger entity, ReportLedger existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
