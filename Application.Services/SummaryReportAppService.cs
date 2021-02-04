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
    public class SummaryReportAppService : AppService<AcclineERPContext>, ISummaryReportAppService
    {
        private readonly ISummaryReportService _service;
        public SummaryReportAppService(ISummaryReportService sumRptService)
        {
            _service = sumRptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SummaryReport Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SummaryReport> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SummaryReport> Find(Expression<Func<SummaryReport, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SummaryReport> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SummaryReport obj)
        {
            _service.Add(obj);
        }

        public void Update(SummaryReport obj)
        {
            _service.Update(obj);
        }

        public void Delete(SummaryReport obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SummaryReport entity, SummaryReport existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
