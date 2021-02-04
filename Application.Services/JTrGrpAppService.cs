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
    public class JTrGrpAppService : AppService<AcclineERPContext>, IJTrGrpAppService
    {
        private readonly IJTrGrpService _service;
        public JTrGrpAppService(IJTrGrpService jTrGrp)
        {
            _service = jTrGrp;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public JTrGrp Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<JTrGrp> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<JTrGrp> Find(Expression<Func<JTrGrp, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<JTrGrp> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(JTrGrp obj)
        {
            _service.Add(obj);
        }

        public void Update(JTrGrp obj)
        {
            _service.Update(obj);
        }

        public void Delete(JTrGrp obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(JTrGrp entity, JTrGrp existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
