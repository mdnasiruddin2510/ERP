using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common
{
   public interface IAppService<TEntity> : IDisposable
       where TEntity : class 
    {
        TEntity Get(int id, bool @readonly = false);
        IEnumerable<TEntity> All(bool @readonly = false);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false);
        IEnumerable<TEntity> SqlQueary(string sql, params object[] parameters);
        void Add(TEntity obj);
       void Update(TEntity obj);
       void Delete(TEntity obj);
       void Save();
       void Setvalues(TEntity entity, TEntity existingEntity);


    }
}
