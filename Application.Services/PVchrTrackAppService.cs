using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;

namespace Application.Services
{
    public class PVchrTrackAppService : AppService<AcclineERPContext>, IPVchrTrackAppService
    {
        private readonly IPVchrTrackService _service;

        public PVchrTrackAppService(IPVchrTrackService commonSearchService)
        {
            _service = commonSearchService;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PVchrTrack Get(int id, bool @readonly = false)
        {
            return _service.Get(id, @readonly);
        }

        public IEnumerable<PVchrTrack> All(bool @readonly = false)
        {
            return _service.All(@readonly);
        }

        public IEnumerable<PVchrTrack> Find(Expression<Func<PVchrTrack, bool>> predicate, bool @readonly = false)
        {
            return _service.Find(predicate, @readonly);
        }

        public IEnumerable<PVchrTrack> SqlQueary(string sql, params object[] parameters)
        {
            return _service.SqlQueary(sql, parameters);
        }



        public void Add(PVchrTrack obj)
        {
            _service.Add(obj);
        }

        public void Update(PVchrTrack obj)
        {
            _service.Update(obj);
        }

        public void Delete(PVchrTrack obj)
        {
           _service.Delete(obj);
        }

        public void Save()
        {
          _service.Save();
        }
        public void Setvalues(PVchrTrack entity, PVchrTrack existingEntity)
        {
            _service.Setvalues(entity, existingEntity);
        }
    
    }
}