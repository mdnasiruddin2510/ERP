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
    public class CustWiseSummSaleRptAppService : AppService<AcclineERPContext>, ICustWiseSummSaleRptAppService
    {
        private readonly ICustWiseSummSaleRptService _service;
        public CustWiseSummSaleRptAppService(ICustWiseSummSaleRptService branchService)
        {
            _service = branchService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public CustWiseSummSaleRpt Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<CustWiseSummSaleRpt> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<CustWiseSummSaleRpt> Find(Expression<Func<CustWiseSummSaleRpt, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<CustWiseSummSaleRpt> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(CustWiseSummSaleRpt obj)
        {
            _service.Add(obj);
        }

        public void Update(CustWiseSummSaleRpt obj)
        {
            _service.Update(obj);
        }

        public void Delete(CustWiseSummSaleRpt obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(CustWiseSummSaleRpt entity, CustWiseSummSaleRpt existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
