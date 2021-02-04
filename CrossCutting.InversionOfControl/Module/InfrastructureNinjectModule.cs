using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Context.Interfaces;
using Ninject.Modules;
using Application.Interfaces;

namespace CrossCutting.InversionOfControl.Module
{
    public class InfrastructureNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbContext>().To<AcclineERPContext>();
            Bind(typeof(IContextManager<>)).To(typeof(ContextManager<>));
            Bind(typeof(IUnitOfWork<>)).To((typeof(UnitOfWork<>)));
           
        }
    }
}
