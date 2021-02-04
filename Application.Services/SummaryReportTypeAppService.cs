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
    public class SummaryReportTypeAppService : AppService<AcclineERPContext>, ISummaryReportTypeAppService
    {
        private readonly ISummaryReportTypeService _service;
        public SummaryReportTypeAppService(ISummaryReportTypeService sumRptTypeService)
        {
            _service = sumRptTypeService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SummaryReportType Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<SummaryReportType> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<SummaryReportType> Find(Expression<Func<SummaryReportType, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<SummaryReportType> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(SummaryReportType obj)
        {
            _service.Add(obj);
        }

        public void Update(SummaryReportType obj)
        {
            _service.Update(obj);
        }

        public void Delete(SummaryReportType obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(SummaryReportType entity, SummaryReportType existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
