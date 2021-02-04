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
    public class JarnalVoucherAppService : AppService<AcclineERPContext>, IJarnalVoucherAppService
    {
        private readonly IJarnalVoucherService _service;
        public JarnalVoucherAppService(IJarnalVoucherService JarnalVoucherService)
        {
            _service = JarnalVoucherService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public JarnalVoucher Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<JarnalVoucher> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<JarnalVoucher> Find(Expression<Func<JarnalVoucher, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<JarnalVoucher> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(JarnalVoucher obj)
        {
            _service.Add(obj);
        }

        public void Update(JarnalVoucher obj)
        {
            _service.Update(obj);
        }

        public void Delete(JarnalVoucher obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(JarnalVoucher entity, JarnalVoucher existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
