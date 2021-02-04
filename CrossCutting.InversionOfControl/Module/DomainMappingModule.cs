using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using App.Domain.Interface.Domain;
using App.Domain.Interface.Service.Common;
using App.Domain.Interface.VModel;
using App.Domain.Services.Common;
using App.Domain.ViewModel;
using Application.Interfaces;
using Application.Services;
using Ninject.Modules;
using App.Domain.Interface.Service;
using App.Domain.Services;

namespace CrossCutting.InversionOfControl.Module
{
  public class DomainMappingModule : NinjectModule
    {
        public override void Load()
        {
            // Bind<ICommonSearchModel>().To<CommonSearchModel>();
             //Bind<IRegionWiseReport>().To<RegionWiseReport>();
             Bind<IUtilityService>().To<UtilityService>();
            
        }
    }
}
