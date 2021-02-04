using App.Domain;
using App.Domain.Interface.Service;
using App.Domain.ViewModel;
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
    public class BalSheetRptAppService : AppService<AcclineERPContext>, IBalSheetRptAppService
    {
        private readonly IBalSheetRptService _service;
        public BalSheetRptAppService(IBalSheetRptService BalSheetRptService)
        {
            _service = BalSheetRptService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public BalSheetRptVM Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<BalSheetRptVM> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<BalSheetRptVM> Find(Expression<Func<BalSheetRptVM, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<BalSheetRptVM> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(BalSheetRptVM obj)
        {
            _service.Add(obj);
        }

        public void Update(BalSheetRptVM obj)
        {
            _service.Update(obj);
        }

        public void Delete(BalSheetRptVM obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(BalSheetRptVM entity, BalSheetRptVM existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
