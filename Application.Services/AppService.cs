using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Common;
using Data.Context.Interfaces;

namespace Application.Services
{
    public class AppService<TContext> : ITransactionAppService<TContext>
        where TContext : IDbContext, new()
    {
        private IUnitOfWork<TContext> _uow;

        public AppService()
        {
            //ValidationResult = new ValidationResult();
        }

        //protected ValidationResult ValidationResult { get; private set; }

        public virtual void BeginTransaction()
        {
           // _uow = ServiceLocator.Current.GetInstance<IUnitOfWork<TContext>>();
            _uow.BeginTransaction();
        }

        public virtual void Commit()
        {
            _uow.SaveChanges();
        }
    }
}
