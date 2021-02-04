using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Domain;
using App.Domain.Interface.Domain;
using App.Domain.Interface.VModel;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using Data.Context.Interfaces;
using WebMatrix.WebData;

namespace PoIpaWeb.Controllers
{
  [Authorize]
    public class AccountManagementController : Controller
    {
      NationalTeaContext con = new NationalTeaContext();  
        private readonly ICommonSearchModel _commonSearchModel;
        private readonly IUnitOfWork<NationalTeaContext> _unit;
        private readonly IUserProfileAppService _userProfileAppService;
        public AccountManagementController(IUnitOfWork<NationalTeaContext> ofWork, ICommonSearchModel commonSearchModel, IUserProfileAppService userProfileAppService)
        {
          
            _commonSearchModel = commonSearchModel;
           
            _userProfileAppService = userProfileAppService;
            _unit = ofWork;
        }
        //
        // GET: /AccountManagement/
        #region Customer
       
        #endregion
        // Supervisor 
     
    }
}
