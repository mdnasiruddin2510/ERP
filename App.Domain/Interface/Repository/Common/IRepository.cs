using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interface.Repository.Common
{
    public interface IRepository<TEntity>
      where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Save();
        TEntity Get(int id);
        IEnumerable<TEntity> All(bool @readonly = false);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false);
        IEnumerable<TEntity> SqlQueary(string sql, params object[] parameters);
        void Setvalues(TEntity entity,TEntity existingEntity);
    }
}
