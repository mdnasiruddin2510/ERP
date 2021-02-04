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
    public class rptJobWiseVMAppService : AppService<AcclineERPContext>, IrptJobWiseVMAppService
    {
        private readonly IrptJobWiseVMService _service;
        public rptJobWiseVMAppService(IrptJobWiseVMService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public rptJobWiseVM Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<rptJobWiseVM> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<rptJobWiseVM> Find(Expression<Func<rptJobWiseVM, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<rptJobWiseVM> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(rptJobWiseVM obj)
        {
            _service.Add(obj);
        }

        public void Update(rptJobWiseVM obj)
        {
            _service.Update(obj);
        }

        public void Delete(rptJobWiseVM obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(rptJobWiseVM entity, rptJobWiseVM existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
