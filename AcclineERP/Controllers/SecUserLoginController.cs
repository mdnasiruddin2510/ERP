using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using App.Domain;
using Application.Interfaces;
using Data.Context;
using System.Data.SqlClient;
using System.Data;
using AcclineERP.Models;
using System.Web.Security;
using App.Domain.ViewModel;
using System.Transactions;
using PoIpaWeb.Models;
using System.Configuration;
using System.Net.Http;
using System.Net;

namespace AcclineERP.Controllers
{
    public class SecUserLoginController : Controller
    {
        //
        // GET: /SecUserLogin/

        private ISecUserInGroupAppService _secUserInGroupService;
        private ISecFormRightAppService _secFormRightService;
        private ISecUserInfoAppService _secUserInfoService;
        private ISecFormProcessAppService _secFormProcessService;
        private ISecFormListAppService _secFormListService;
        private IFYDDAppService _FYDDService;
        private IBranchAppService _branchService;
        private ISysSetAppService _sysSetService;
        private IUserBranchAppService _userBranchService;

        public SecUserLoginController(ISecUserInGroupAppService _secUserInGroupService, ISecFormRightAppService _secFormRightService,
               ISecUserInfoAppService _secUserInfoService, ISecFormProcessAppService _secFormProcessService,
               ISecFormListAppService _secFormListService, IFYDDAppService _FYDDService, IBranchAppService _branchService,
               ISysSetAppService _sysSetService, IUserBranchAppService _userBranchService)
        {
            this._secUserInGroupService = _secUserInGroupService;
            this._secFormRightService = _secFormRightService;
            this._secUserInfoService = _secUserInfoService;
            this._secFormProcessService = _secFormProcessService;
            this._secFormListService = _secFormListService;
            this._FYDDService = _FYDDService;
            this._branchService = _branchService;
            this._sysSetService = _sysSetService;
            this._userBranchService = _userBranchService;
        }

        public ActionResult SecUserLogin()
        {
            //ViewBag.FYDD = new SelectList(_FYDDService.All().ToList().OrderBy(x => x.FinYear).ToList(), "FinYear", "FinYear");

            ViewBag.FinYear = LoadDropDown.LoadAllFinYears(_FYDDService);

            return View("~/Views/Security/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(VMLogin user)
        {
            try
            {
                string Pass = SHA1.Encode(user.Password);

                var UserInfo = (from mi in _secUserInfoService.All().ToList()
                                where mi.UserName == user.UserName && mi.Password == SHA1.Encode(user.Password)
                                select new
                                {
                                    UserID = mi.UserID,
                                    UserName = mi.UserName,
                                    Password = mi.Password,
                                    Email = mi.Email

                                }).FirstOrDefault();

                var HasBranch = _sysSetService.All().FirstOrDefault().HasBranch;
                if (HasBranch == true)
                {
                    if (UserInfo != null)
                    {
                        Session["ProjCode"] = "01";
                        Session["UserID"] = UserInfo.UserID;
                        Session["UserName"] = UserInfo.UserName;
                        Session["FinYear"] = user.FinYear;
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        if (sysSet.HasSale == true)
                        {
                            string token = GetToken(ConfigurationManager.AppSettings["ApiUrl"], UserInfo.UserName, UserInfo.Password);
                            Session["token"] = token;
                        }

                        return RedirectToAction("LogInWithBranch", "SecUserLogin");
                    }
                    else
                    {
                        ViewBag.Message = "Login data is incorrect!";
                        return RedirectToAction("SecUserLogin", "SecUserLogin");
                    }
                }
                else
                {
                    if (UserInfo != null)
                    {
                        Session["ProjCode"] = "01";
                        Session["UserID"] = UserInfo.UserID;
                        Session["UserName"] = UserInfo.UserName;
                        Session["FinYear"] = user.FinYear;
                        Session["BranchCode"] = _branchService.All().FirstOrDefault().BranchCode;
                        var sysSet = _sysSetService.All().FirstOrDefault();
                        if (sysSet.HasSale == true)
                        {
                            string token = GetToken(ConfigurationManager.AppSettings["ApiUrl"], UserInfo.UserName, UserInfo.Password);
                            Session["token"] = token;
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Login data is incorrect!";
                        return RedirectToAction("SecUserLogin", "SecUserLogin");
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public ActionResult LogInWithBranch()
        {
            ViewBag.BranchCode = PermittedBranch();
            return View("~/Views/Security/LogInWithBranch.cshtml");
        }

        public SelectList PermittedBranch()
        {
            var UserId = _secUserInfoService.All().Where(x => x.UserName == Session["UserName"].ToString()).FirstOrDefault().UserID;

            List<Branch> branchs = _branchService.All().ToList();
            List<UserBranch> userbranch = _userBranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == UserId.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();
            return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");
        }


        public ActionResult LogInWithSelectedBranch(string BranchCode)
        {
            var User = _secUserInfoService.All().Where(x => x.UserName == Session["UserName"].ToString()).FirstOrDefault();

            var UserBranch = _userBranchService.All().ToList().FirstOrDefault(x => x.Userid == User.UserID.ToString());

            if (User.UserID.ToString() == UserBranch.Userid)
            {
                Session["BranchCode"] = BranchCode;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.Message = "Please Select Correct Branch!!!";
                return View();
            }

        }

        static string GetToken(string url, string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ), 
                        new KeyValuePair<string, string>( "username", userName ), 
                        new KeyValuePair<string, string> ( "Password", password )
                    };
            var content = new FormUrlEncodedContent(pairs);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "/login", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public ActionResult LogOff()
        {
            //WebSecurity.Logout();

            Session.Abandon();
            Session.RemoveAll();

            return RedirectToAction("SecUserLogin", "SecUserLogin");
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserID"] != null)
            {
                int UserID = Convert.ToInt32(Session["UserID"].ToString());

                var UserInfo = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == UserID);

                if (UserInfo != null)
                {
                    ViewBag.OldPassword = UserInfo.Password;
                }
            }
            return View("~/Views/Security/ChangePassword.cshtml");
        }

        public ActionResult UserPasswordChange(LocalPasswordModel Lpm)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string eCode = "";

                    if (Session["UserID"] != null)
                    {
                        int UserID = Convert.ToInt32(Session["UserID"].ToString());

                        var UserInfo = _secUserInfoService.All().ToList().FirstOrDefault(x => x.UserID == UserID);

                        if (UserInfo != null)
                        {

                            UserInfo.Password = SHA1.Encode(Lpm.NewPassword);

                            _secUserInfoService.Update(UserInfo);
                            _secUserInfoService.Save();

                            eCode = "1";

                        }
                        else
                        {
                            eCode = "2";
                        }
                    }
                    else
                    {
                        return RedirectToAction("SecUserLogin", "SecUserLogin");
                    }
                    @ViewBag.Message = eCode;

                    transaction.Complete();

                    return View("~/Views/Security/ChangePassword.cshtml");
                }
                catch (System.Exception)
                {
                    transaction.Dispose();
                    @ViewBag.Message = "0";
                    return View("~/Views/Security/ChangePassword.cshtml");
                }
            }
        }
    }
}
