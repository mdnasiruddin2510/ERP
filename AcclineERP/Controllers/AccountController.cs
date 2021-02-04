using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Domain.ViewModel;
//using App.Domain;
using Application.Interfaces;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PoIpaWeb.Filters;
using PoIpaWeb.Models;
using App.Domain;
using System.Net;
using System.Web.Hosting;
using System.IO;
using System.Web.Services;


namespace PoIpaWeb.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
      
        //private readonly ICustomerAppService _customerService;
        private readonly IEmailConfigAppService _emailService;

        private readonly IRecPasswordAppService _recPassword;
        public AccountController( IRecPasswordAppService _recPassword, IEmailConfigAppService _emailService)
        {
          
            //this._customerService = _customerService;
            this._recPassword = _recPassword;
            this._emailService = _emailService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult SupervisorDashboard()
        {
            return View();
        }

        
        // POST: /Account/Login

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Login(VMLogin model, string returnUrl)
        //{
        //    var customer = _customerService.All().FirstOrDefault(x => x.Email.ToLower() == model.UserName.ToLower());
        //    if (customer != null && customer.IsActive != true)
        //    {
        //        ModelState.AddModelError("", "Your account is not activated. Please active your account.");
        //        return View(model);
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid && WebSecurity.Login(model.UserName.ToLower(), model.Password))
        //        {
        //            if (Roles.IsUserInRole(model.UserName, "Customer"))
        //            {

        //                Session["UserName"] = customer.CustName;
        //                Session["ImagePath"] = customer.FilePath;
        //                return RedirectToAction("CustDashBoard", "Customer");
        //            }
        //        }

        //        // If we got this far, something failed, redisplay form
        //        ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //        return View(model);
        //    }

        //}

        [Authorize]
        public ActionResult LogOff()
        {
            bool isCustomer = Roles.IsUserInRole("Customer");
            FormsAuthentication.SignOut();
            Session.Abandon();
            if(isCustomer==true)
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                return RedirectToAction("LogIn","Office");
            } 
        }
        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    var isExist = _customerService.All().FirstOrDefault(x => x.Email == model.Email.ToLower());
        //    if (isExist == null)
        //    {

        //        try
        //        {
        //            model.Thana = _thanaService.Get(model.ThanaId);
        //            model.District = _districtService.Get(model.DistrictId);
        //            model.UserName = model.Email;

        //            ModelState.Clear();
        //            UpdateModel(model);
        //        }
        //        catch
        //        {

        //        }
        //        var validate = 0;
        //        if (model.ProfilePicture.ContentType.Trim() == "image/png".Trim() || model.ProfilePicture.ContentType.Trim() == "image/jpeg".Trim())
        //        {
        //            if (model.ProfilePicture.ContentLength < 1000000)
        //            {
        //                validate = 1;
        //            }
        //        }

               
        //        if (validate == 0)
        //        {
        //            ModelState.AddModelError(string.Empty, "We only allow JPEG or PNG type image");
        //        }


        //        if (ModelState.IsValid)
        //        {
        //            // Attempt to register the user
        //            try
        //            {
        //                Customer aCustomer = new Customer();
        //                aCustomer.Address = model.Address;
        //                aCustomer.CustName = model.CustomerName;
        //                aCustomer.District = model.District;
        //                aCustomer.Email = model.Email.ToLower();
        //                aCustomer.EntryUser = "Customer";
        //                aCustomer.MobileNo = model.MobileNo;
        //                aCustomer.PostCode = model.PostCode;
        //                aCustomer.EntryDate = DateTime.Now.ToShortDateString();
        //                aCustomer.RoleId = 1;
        //                aCustomer.Thana = model.Thana;
        //                aCustomer.District = model.District;
        //                aCustomer.SignUpAuthId = Guid.NewGuid().ToString();
        //                aCustomer.IsActive = false;

        //                string path = "";
        //                if (model.ProfilePicture != null)
        //                {
        //                    path = HostingEnvironment.ApplicationPhysicalPath + @"\Images\ProfilePicture\";
        //                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePicture.FileName);
        //                    if (Directory.Exists(path) == false)
        //                    {
        //                        Directory.CreateDirectory(path);
        //                        model.ProfilePicture.SaveAs(Server.MapPath("/" + imageName));
        //                        aCustomer.FilePath = imageName;
        //                    }
        //                    else
        //                    {
        //                        model.ProfilePicture.SaveAs(Server.MapPath("/Images/ProfilePicture/" + imageName));
        //                        aCustomer.FilePath = imageName;
        //                    }
        //                }
        //                WebSecurity.CreateUserAndAccount(model.UserName.ToLower(), model.Password);
                        
        //                String strPathAndQuery = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
        //                String strUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        //                string url = "";
        //                url = strUrl.ToString() + "Account/ConfirmEmail?confirmId=" + aCustomer.SignUpAuthId;
        //                _customerService.Add(aCustomer);
        //                _customerService.Save();
        //                Roles.AddUserToRole(model.Email, "Customer");

        //                EmailConfig emailConfig = _emailService.All().FirstOrDefault(e => e.IsActive == true);
        //                EmailVModel models = new EmailVModel();
        //                models.Link = url;
        //                models.TemplateHeader = emailConfig.TemplateHeader.ToString();
        //                models.TempFixedTopDes = emailConfig.TempFixedTopDes.ToString();
        //                models.ThanksGiving = emailConfig.ThanksGiving.ToString();
        //                models.FooterThanks = emailConfig.FooterThanks.ToString();
        //                models.Department = emailConfig.Department.ToString();
        //                models.FooterConcern = emailConfig.FooterConcern.ToString();
        //                models.FooterPhone = emailConfig.FooterPhone.ToString();
        //                models.FooterFax = emailConfig.FooterFax.ToString();
        //                models.FooterEmail = emailConfig.FooterEmail.ToString();
        //                models.FooterWeb = emailConfig.FooterWeb.ToString();
        //                string bodies = EmailTemplateStringify.RenderViewToString(this, "EmailTemplate", models);

        //                //SendEmail.SendByGmail("Post office E-Commerce Account Activation", "Please click following link to activate your Bangladesh Post Office E-Commerce account. <br />'" + url + "'", aCustomer.Email);
        //                SendEmail.SendByGmail("Post office E-Commerce Account Activation", bodies, aCustomer.Email);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            catch (MembershipCreateUserException e)
        //            {
                       
        //                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
        //            }
        //        }
        //        ViewBag.DistrictId = new SelectList(_districtService.All().ToList().OrderBy(x => x.DistrictName), "Id", "DistrictName");
        //        ViewBag.ThanaId = new SelectList(_thanaService.All().ToList().OrderBy(x => x.ThanaName), "Id", "ThanaName");
        //        // If we got this far, something failed, redisplay form
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("","This email is already registered.");
        //    }
        //    ViewBag.DistrictId = new SelectList(_districtService.All().ToList().OrderBy(x => x.DistrictName), "Id", "DistrictName");
        //    ViewBag.ThanaId = new SelectList(_thanaService.All().ToList().OrderBy(x => x.ThanaName), "Id", "ThanaName");
        //    return View(model);
        //}

        //
        // POST: /Account/Disassociate
        //[Authorize(Roles="Customer")]
        //public ActionResult Profile()
        //{
        //    RegisterModel amodel = new RegisterModel();
        //    Customer aCustomer = _customerService.All().FirstOrDefault(x => x.Email == User.Identity.Name);
        //    amodel.Address = aCustomer.Address;
        //    amodel.CustomerName = aCustomer.CustName;
        //    amodel.DistrictId = aCustomer.District.Id;
        //    amodel.ThanaId = aCustomer.Thana.Id;
        //    amodel.PostCode = aCustomer.PostCode;
        //    amodel.MobileNo = aCustomer.MobileNo;
        //    amodel.FilePath = aCustomer.FilePath;
        //    ViewBag.DistrictId = new SelectList(_districtService.All().Where(x=>x.Id==amodel.DistrictId).ToList().OrderBy(x => x.DistrictName), "Id", "DistrictName",amodel.DistrictId);
        //    ViewBag.ThanaId = new SelectList(_thanaService.All().Where(x=>x.DistrictId==amodel.DistrictId).ToList().OrderBy(x => x.ThanaName), "Id", "ThanaName",amodel.ThanaId);
        //    return View(amodel);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Customer")]
        //public ActionResult Profile(RegisterModel register)
        //{
        //    Customer aCustomer = _customerService.All().FirstOrDefault(x => x.Email == User.Identity.Name);
        //    aCustomer.Email = User.Identity.Name;
        //    aCustomer.Address = register.Address;
        //    aCustomer.PostCode = register.PostCode;
        //    aCustomer.RoleId = 1;
        //    aCustomer.CustName = register.CustomerName;
        //    aCustomer.MobileNo = register.MobileNo;
        //    aCustomer.Thana = _thanaService.All().FirstOrDefault(x => x.Id==register.ThanaId);
        //    aCustomer.District = _districtService.All().FirstOrDefault(x => x.Id == register.DistrictId);

        //    string path = "";
        //    if (register.ProfilePicture != null)
        //    {
        //        path = HostingEnvironment.ApplicationPhysicalPath + @"\Images\ProfilePicture\";
        //        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(aCustomer.FilePath);
        //        if (Directory.Exists(path) == false)
        //        {
        //            Directory.CreateDirectory(path);
        //            register.ProfilePicture.SaveAs(Server.MapPath("/" + imageName));
        //            aCustomer.FilePath = imageName;
        //        }
        //        else
        //        {
        //            register.ProfilePicture.SaveAs(Server.MapPath("/Images/ProfilePicture/" + imageName));
        //            aCustomer.FilePath = imageName;
        //        }
        //    }

        //    _customerService.Update(aCustomer);
        //    _customerService.Save();
        //    ViewBag.DistrictId = new SelectList(_districtService.All().Where(x => x.Id == aCustomer.District.Id).ToList().OrderBy(x => x.DistrictName), "Id", "DistrictName", aCustomer.District.Id);
        //    ViewBag.ThanaId = new SelectList(_thanaService.All().Where(x => x.DistrictId == aCustomer.Thana.Id).ToList().OrderBy(x => x.ThanaName), "Id", "ThanaName", aCustomer.Thana.Id);
        //    return View(register);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }
        //[AllowAnonymous]
        //public ActionResult ConfirmEmail(string confirmId)
        //{

        //    var aCustomer = _customerService.All().ToList().FirstOrDefault(x => x.SignUpAuthId == confirmId);
        //    if (aCustomer != null)
        //    {
        //        aCustomer.SignUpAuthId = "";
        //        aCustomer.IsActive = true;
        //        _customerService.Update(aCustomer);
        //        _customerService.Save();
        //    }
        //    return View();
        //}
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
      
        [AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult RecoverPassword(string email)
        //{
        //    var customer = _customerService.All().ToList().FirstOrDefault(x => x.Email == email);

        //    if (customer != null)
        //    {
        //        if (Membership.GetUser(email) != null)
        //        {
        //            RecPassword recPassword = new RecPassword();
        //            recPassword.UserName = email;
        //            recPassword.MaxValidTime = DateTime.Now.AddHours(24.0);
        //            recPassword.ValidationToken = WebSecurity.GeneratePasswordResetToken(email);
        //            _recPassword.Add(recPassword);
        //            _recPassword.Save();


        //            EmailConfig emailConfig = _emailService.All().FirstOrDefault(e => e.IsActive == true);
        //            EmailVModel models = new EmailVModel();
        //            String strPathAndQuery = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
        //            String strUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        //            string url = "";
        //            url = strUrl.ToString() + "Account/RecoverPass?token=" + recPassword.ValidationToken;
        //            models.RecoverPasswordEmail = url;
        //            models.TemplateHeader = emailConfig.TemplateHeader.ToString();
        //            models.TempFixedTopDes = emailConfig.TempFixedTopDes.ToString();
        //            models.ThanksGiving = emailConfig.ThanksGiving.ToString();
        //            models.FooterThanks = emailConfig.FooterThanks.ToString();
        //            models.Department = emailConfig.Department.ToString();
        //            models.FooterConcern = emailConfig.FooterConcern.ToString();
        //            models.FooterPhone = emailConfig.FooterPhone.ToString();
        //            models.FooterFax = emailConfig.FooterFax.ToString();
        //            models.FooterEmail = emailConfig.FooterEmail.ToString();
        //            models.FooterWeb = emailConfig.FooterWeb.ToString();
        //            string bodies = EmailTemplateStringify.RenderViewToString(this, "EmailTemplate", models);
        //            SendEmail.SendByGmail("Recover Password", bodies, email);
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "We didn't find this email for reset password.";
        //    }
        //    return View();
        //}
        public ActionResult ChangePassword()
        {
            return View(new LocalPasswordModel());
        }
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                bool isUpdate=WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                if(isUpdate==true)
                {
                    if (Roles.IsUserInRole(User.Identity.Name, "Customer"))
                    {
                        return RedirectToAction("LogOff", "Account");
                    }
                    else
                    {
                        return RedirectToAction("LogOff","Office");
                    }
                }
                else
                {
                    ViewBag.Message="Your inserted current password is incorrect";
                }
            }
            return View(model);
        }
      
         [AllowAnonymous]
        [HttpGet]
        public ActionResult RecoverPass(string token)
        {
            if (string.IsNullOrEmpty(token))
            {

            }
            else
            {
                var recpssword = _recPassword.All().FirstOrDefault(x => x.ValidationToken == token);
                if (recpssword != null)
                {
                    ViewBag.token = token;
                    return View();
                }
            }
           
            return RedirectToAction("LogIn", "Account");
        }
         [HttpPost]
         [AllowAnonymous]
         public ActionResult RecoverPass(string token, LocalPasswordModel model)
         {
             model.OldPassword = "empty";
             ModelState.Clear();
             UpdateModel(model);
             if (string.IsNullOrEmpty(token))
             {
                 return RedirectToAction("Login", "Account");
             }
             else
             {
                 if (ModelState.IsValid)
                 {
                     var rec = _recPassword.All().ToList().FirstOrDefault(x => x.ValidationToken == token);
                     bool isChange = WebSecurity.ResetPassword(token, model.NewPassword);
                     if (isChange == true)
                     {

                         if (Roles.IsUserInRole(rec.UserName, "Customer"))
                         {
                             return RedirectToAction("LogIn", "Account");
                         }
                         else
                         {
                             return RedirectToAction("LogIn", "Office");
                         }
                     }
                     else
                     {
                         ViewBag.message = "Sorry!Try again";
                         return View(model);
                     }
                 }
             }
             ViewBag.token = token;
             return View(model);
         }

        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        public ActionResult EmailTemplate()
        {
            return View();
        }
    }
}
