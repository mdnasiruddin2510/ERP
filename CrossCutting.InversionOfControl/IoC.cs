using CommonServiceLocator.NinjectAdapter.Unofficial;
using CrossCutting.InversionOfControl.Module;
using Microsoft.Practices.ServiceLocation;
using CrossCutting.InversionOfControl.Module;
using Ninject;

namespace CrossCutting.InversionOfControl
{
    public class IoC
    {
        public IKernel Kernel { get; private set; }

        public IoC()
        {
            Kernel = GetNinjectModules();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(Kernel));
        }

        public StandardKernel GetNinjectModules()
        {
            return new StandardKernel(
                new ServiceNinjectModule(),
                new InfrastructureNinjectModule(),
                new RepositoryNinjectModule(),
                new ApplicationNinjectModule(),
            new DomainMappingModule());
        }
    }
}
