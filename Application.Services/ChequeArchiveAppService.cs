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
    public class ChequeArchiveAppService : AppService<AcclineERPContext>, IChequeArchiveAppService
    {
        private readonly IChequeArchiveService _service;
        public ChequeArchiveAppService(IChequeArchiveService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ChequeArchive Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<ChequeArchive> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<ChequeArchive> Find(Expression<Func<ChequeArchive, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<ChequeArchive> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(ChequeArchive obj)
        {
            _service.Add(obj);
        }

        public void Update(ChequeArchive obj)
        {
            _service.Update(obj);
        }

        public void Delete(ChequeArchive obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(ChequeArchive entity, ChequeArchive existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
