using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Repository.Common;
using Data.Context;
using Data.Context.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace Data.Repository.EntityFramework.Common
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<TEntity> _dbSet;

        public Repository()
        {
            var contextManager = ServiceLocator.Current.GetInstance<IContextManager<AcclineERPContext>>()
                as ContextManager<AcclineERPContext>;
            
            _dbContext = contextManager.GetContext();
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected IDbContext Context
        {
            get { return _dbContext; }
        }

        protected IDbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }
        public void Setvalues(TEntity entity,TEntity existingEntity)
        {
              var a = new AcclineERPContext();
              _dbContext.Entry(entity).CurrentValues.SetValues(existingEntity);
        }
        public virtual void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            entry.State = EntityState.Modified;
            //DbSet.Attach(entity)
           
        }

        public virtual IEnumerable<TEntity> All(bool @readonly = false)
        {
            return @readonly
                ? DbSet.AsNoTracking().ToList()
                : DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false)
        {
            return @readonly
                ? DbSet.Where(predicate).AsNoTracking()
                : DbSet.Where(predicate);
        }

        public IEnumerable<TEntity> SqlQueary(string sql, params object[] parameters)
        {
            //using ()
            //{
               // a.Database.CommandTimeout = 1000;
            var a = new AcclineERPContext();
                return a.Database.SqlQuery<TEntity>(sql, parameters);
            //}
            //Context.Database.CommandTimeout = 1000;
           
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Context == null) return;
            Context.Dispose();
        }

        #endregion
    }
}
