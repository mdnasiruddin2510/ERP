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
    public class LevelLenAppService : AppService<AcclineERPContext>, ILevelLenAppService
    {
        private readonly ILevelLenService _service;
        public LevelLenAppService(ILevelLenService levelLenService)
        {
            _service = levelLenService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LevelLen Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<LevelLen> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }
        public IEnumerable<LevelLen> Find(Expression<Func<LevelLen, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<LevelLen> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }

        public void Add(LevelLen obj)
        {
            _service.Add(obj);
        }

        public void Update(LevelLen obj)
        {
            _service.Update(obj);
        }

        public void Delete(LevelLen obj)
        {
            _service.Delete(obj);
        }

        public void Save()
        {
            _service.Save();
        }
        public void Setvalues(LevelLen entity, LevelLen existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    }
}
