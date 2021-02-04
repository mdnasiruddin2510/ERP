using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoIpaWeb.Filters;
using PoIpaWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TopUp17Web.Models;
using WebMatrix.WebData;

namespace TopUp17Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    [InitializeSimpleMembership]
    public class AccountsController : Controller
    {
        private readonly IEmployeeAppService _employeeService;
        private readonly IEmployeeFuncAppService _employeeFuncService;
        private readonly IFuncSLAppService _funcSLService;
        private readonly IFYDDAppService _FYDDAppService;
        private readonly IBranchAppService _branchService;
        private readonly IUserBranchAppService _userbranchService;
        public AccountsController(IEmployeeAppService _employeeService, IFYDDAppService _FYDDAppService, IBranchAppService _branchService, IFuncSLAppService _funcSLService, IEmployeeFuncAppService _employeeFuncService, IUserBranchAppService _userbranchService)
        {
            this._employeeService = _employeeService;
            this._FYDDAppService = _FYDDAppService;
            this._branchService = _branchService;
            this._funcSLService = _funcSLService;
            this._employeeFuncService = _employeeFuncService;
            this._userbranchService = _userbranchService;
        }

        public ActionResult Register()
        {
            //var branch = _employeeService.All().ToList().FirstOrDefault(x => x.Email == User.Identity.Name);
            //Session["Garden"] = branch.BranchCode;

            List<string> roles = new List<string>();
            //  List<string> branch = new List<string>();
            foreach (string role in System.Web.Security.Roles.GetAllRoles().ToList())
            {
                roles.Add(role);
            }
            ViewBag.Roles = roles;
            //ViewBag.BranchCode = branch;

            ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
            ViewBag.Id = new SelectList(_employeeService.All().ToList(), "Id", "UserName");
            // ViewBag.RecvItem = LoadDropDown.LoadItem(_itemInfoService);
            return View();
        }


        public JsonResult DropDownGrouping()
        {

            var employeeCollection = _branchService.All().ToList();
            return Json(employeeCollection, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Register(Employee model, string[] EmployeesCustom, string[] roles, string returnUrl)
        {
            var isExist = _employeeService.All().FirstOrDefault(x => x.Email == model.Email.ToLower());
            if (isExist == null)
            {
                try
                {
                    model.UserName = model.Email;
                    ModelState.Clear();
                    UpdateModel(model);
                }
                catch
                {

                }
                if (ModelState.IsValid)
                {
                    if (model.Password == model.ConfirmPassword)
                    {
                        try
                        {
                            Employee employee = new Employee();
                            employee.UserName = model.UserName;
                            employee.Email = model.Email;
                            //employee.BranchCode = model.BranchCode;
                            employee.IsActive = false;

                            WebSecurity.CreateUserAndAccount(model.Email.ToLower(), model.Password);
                            Roles.AddUserToRoles(employee.Email, roles);

                            _employeeService.Add(employee);
                            _employeeService.Save();

                            int id = employee.Id;//_employeeService.All().LastOrDefault().Id;

                            List<string> userbranch = EmployeesCustom.ToList();



                            string UID = Convert.ToString(id);
                            //List<UserBranch> userList = new List<UserBranch>();
                            foreach (var BranchCode in userbranch)
                            {
                                UserBranch userbranchs = new UserBranch();
                                userbranchs.Userid = UID;
                                userbranchs.BranchCode = BranchCode;
                                //userList.Add(userbranchs);
                                _userbranchService.Add(userbranchs);
                                _userbranchService.Save();
                            }

                            // model.Branchs = userList;
                            //_employeeService.Add(model);

                            // _userbranchService.SaveChanges();

                            //Roles.AddUserToRole(model.Email, "");
                            ViewBag.Roles = roles;

                            ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                            ViewBag.Id = new SelectList(_employeeService.All().ToList(), "Id", "UserName");

                            return RedirectToAction("Register");
                        }
                        catch (MembershipCreateUserException e)
                        {
                            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Password doesn't match. Please recheck password");
                    }
                }
            }
            else
            {
                ViewBag.IsAlreadyRegistered = "This User Name is already registered.";
            }
            List<string> allRole = new List<string>();
            foreach (string role in System.Web.Security.Roles.GetAllRoles().ToList())
            {
                allRole.Add(role);
            }
            ViewBag.Roles = allRole;
            ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
            ViewBag.Id = new SelectList(_employeeService.All().ToList(), "Id", "UserName");
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult LogInWithGarden(string returnUrl)
        {
            //Session["Garden"] = model.BranchCode; call G
            ViewBag.BranchCode = GardenSelection(); //new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogInWithGarden(VMLogin model, string returnUrl)
        {
            model.UserName = User.Identity.Name;
            var employee = _employeeService.All().ToList().FirstOrDefault(x => x.Email == model.UserName);

            var userss = _userbranchService.All().ToList().FirstOrDefault(x => x.Userid == employee.Id.ToString());

            if (employee.Id.ToString() == userss.Userid)
            {
                //Session["BranchCode"] = model.BranchCode;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + "/" + Server.UrlDecode(returnUrl);
                    return Content("<script>window.location = '" + returnUrl + "';</script>");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //Session["BranchCode"] = model.BranchCode;
                ViewBag.BranchCode = new SelectList(_branchService.All().ToList(), "BranchCode", "BranchName");
                ViewBag.Message = "Please select correct Branch!!!";
                return View(model);
            }

        }

        public ActionResult GetBranchByEmployee(string Id)
        {

            return Json(ByBranch(Id), JsonRequestBehavior.AllowGet);

        }

        public SelectList ByBranch(string Id)
        {
            var items = (dynamic)null;
            var getbranch = (dynamic)null;
            List<Branch> branchList = new List<Branch>();

            if (Id != "")
            {
                items = _userbranchService.All().ToList().Where(x => x.Userid == Id).ToList();
                foreach (var item in items)
                {
                    var branch = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
                    branchList.Add(branch);
                }
                // getbranch = _branchService.All().ToList().Where(x => x.BranchCode == items.).ToList();
            }
            else
            {

                var branch = _branchService.All().ToList().FirstOrDefault();
                branchList.Add(branch);

            }

            //items.Insert(0, new ItemInfo() { ItemCode = "0", ItemName = "--- Select ---" });
            return new SelectList(branchList, "BranchCode", "BranchName");

        }

        public SelectList GardenSelection()
        {

            var UserName = User.Identity.Name;
            var brans = _employeeService.All().Where(x => x.Email == UserName).FirstOrDefault();

            //shahin

            List<Branch> branchs = _branchService.All().ToList();
            List<UserBranch> userbranch = _userbranchService.All().ToList();
            var branchInfo = (from ii in userbranch
                              join i in branchs on ii.BranchCode equals i.BranchCode
                              where ii.Userid == brans.Id.ToString()
                              select new
                              {
                                  BranchCode = ii.BranchCode,
                                  BranchName = i.BranchName
                              }).ToList();

            //shahin
            //List<Branch> branchList = new List<Branch>();
            //foreach (var item in brans)
            //{
            //    var UserId = item.Id;
            //    var branch = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
            //    branchList.Add(branch);
            //}
            ////List<Branch> BranchCode = new SelectList(branchList, "BranchCode", "BranchName");
            return new SelectList(branchInfo.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");

        }
        //public SelectList GardenSelection()
        //{

        //    var UserName = User.Identity.Name;
        //    var brans = _employeeService.All().Where(x => x.Email == UserName).ToList();
        //    List<Branch> branchList = new List<Branch>();
        //    foreach (var item in brans)
        //    {
        //        var branch = _branchService.All().ToList().FirstOrDefault(x => x.BranchCode == item.BranchCode);
        //        branchList.Add(branch);
        //    }
        //    //List<Branch> BranchCode = new SelectList(branchList, "BranchCode", "BranchName");
        //    return new SelectList(branchList.OrderBy(x => x.BranchCode), "BranchCode", "BranchName");

        //}

        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {

            ViewBag.returnUrl = returnUrl;
            ViewBag.FinYear = new SelectList(_FYDDAppService.All().OrderByDescending(x => x.FinYear), "FinYear", "FinYear");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(VMLogin model, string returnUrl)  
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, true))
            {
                //Get the token
                //var token = GetAPIToken(model.UserName, model.Password).Result;

                //Make the call
                //var apiGetPeoplePath = "login";
               // var response = GetRequest(token, apiGetPeoplePath).Result;
                string token = GetToken(ConfigurationManager.AppSettings["ApiUrl"], model.UserName, model.Password);

                Session["FinYear"] = model.FinYear;
                Session["UserName"] = _employeeService.All().FirstOrDefault(x => x.Email == model.UserName).UserName;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + "/" + Server.UrlDecode(returnUrl);
                    return Content("<script>window.location = '" + returnUrl + "';</script>");
                }
                else
                {
                    var user = _employeeService.All().ToList().FirstOrDefault(x => x.Email == model.UserName);
                    var userss = _userbranchService.All().ToList().FirstOrDefault(x => x.Userid == user.Id.ToString());
                    // if (user.BranchCode != null)
                    if (userss != null)
                    {
                        Session["token"] = token;
                        return RedirectToAction("LogInWithGarden", "Accounts");

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            else
            {
                Session["FinYear"] = model.FinYear;
                ViewBag.FinYear = new SelectList(_FYDDAppService.All().ToList(), "FinYear", "FinYear");
                ViewBag.Message = "User Name or Password doesn't match";
                return View(model);
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

        


        [Authorize]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session.Abandon();
            return RedirectToAction("LogIn");
        }
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View(new LocalPasswordModel());
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUpdate = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                if (isUpdate == true)
                {
                    return RedirectToAction("LogOff", "Accounts");
                }
                else
                {
                    ViewBag.Message = "Your inserted current password is incorrect";
                }
            }
            return View(model);
        }

        public ActionResult UserList()
        {
            var employees = _employeeService.All().ToList();
            List<EmployeeVmodel> vmodel = new List<EmployeeVmodel>();
            int i = 0;
            foreach (var item in employees)
            {
                EmployeeVmodel model = new EmployeeVmodel();
                model.Employee.UserName = item.UserName;
                model.Employee.Email = item.Email;
                model.Employee.Id = item.Id;
                model.SlNo = ++i;
                var roles = Roles.GetRolesForUser(item.Email);

                foreach (var aRole in roles)
                {
                    model.AllRoles += aRole + ", ";
                }
                vmodel.Add(model);
            }
            ViewBag.Items = vmodel;
            ViewBag.Roles = Roles.GetAllRoles();
            return View(new EmployeeVmodel());
        }
        public ActionResult GetUserById(int id)
        {
            var employee = _employeeService.All().ToList().FirstOrDefault(x => x.Id == id);
            EmployeeVmodel vmodel = new EmployeeVmodel();
            vmodel.Employee = employee;
            vmodel.Roles = Roles.GetRolesForUser(employee.Email);
            return Json(vmodel, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateUser(int id, string name, string[] roles)
        {
            var employee = _employeeService.All().ToList().FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {

                try
                {
                    if (Roles.GetRolesForUser(employee.Email.ToLower()).Count() > 0)
                    {
                        Roles.RemoveUserFromRoles(employee.Email.ToLower(), Roles.GetRolesForUser(employee.Email.ToLower()));
                    }

                    Roles.AddUserToRoles(employee.Email, roles);
                    employee.UserName = name;
                    _employeeService.Update(employee);
                    _employeeService.Save();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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


        public ActionResult EmployeeManagement()
        {
            ViewBag.EmpId = new SelectList(_employeeService.All().ToList(), "Id", "UserName");
            //  ViewBag.FuncId = new SelectList(_funcSLService.All().ToList(), "FuncId", "Function");
            return View();
        }

        public string GenerateEmpId()
        {
            var employeeFunc = _employeeFuncService.All().LastOrDefault();
            string EmpId = "";
            if (employeeFunc != null)
            {
                EmpId = (Convert.ToInt32(employeeFunc.Id) + 1).ToString().PadLeft(6, '0');
            }
            else
            {
                EmpId = "000001";
            }
            return EmpId;
        }

        //public ActionResult GetFunByEmpId( int Empid)
        //{

        //    List<EmployeeFunc> selectedFunc = _employeeFuncService.All().ToList().Where(x => x.EmpId == Empid).ToList();
        //    return Json(selectedFunc, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult GetFuncSl()
        {
            var area = _funcSLService.All().ToList();
            return Json(area, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetFunByEmpId(string Empid)
        {
            var area = _funcSLService.All().ToList();

            List<EmployeeFunc> slectedFunc = _employeeFuncService.All().ToList().Where(x => x.EmpId == Empid).ToList();
            List<EmpFuncVModel> EmpFuncVM = new List<EmpFuncVModel>();
            if (slectedFunc.Count == 0)
            {
                foreach (var func in area)
                {
                    EmpFuncVModel emp = new EmpFuncVModel();
                    emp.FuncId = func.FuncId;
                    emp.Form = func.Form;
                    emp.Function = func.Function;
                    emp.IsChecked = false;
                    EmpFuncVM.Add(emp);
                }

            }
            else
            {

                foreach (var func in area)
                {

                    int count = 0;
                    foreach (var empFunc in slectedFunc)
                    {
                        if (count < 1)
                        {
                            EmpFuncVModel emp = new EmpFuncVModel();
                            if (empFunc.Form == func.Form && empFunc.Function == func.Function)
                            {
                                emp.FuncId = func.FuncId;
                                emp.Form = func.Form;
                                emp.Function = func.Function;
                                emp.IsChecked = true;
                                EmpFuncVM.Add(emp);
                            }
                            else
                            {
                                emp.FuncId = func.FuncId;
                                emp.Form = func.Form;
                                emp.Function = func.Function;
                                emp.IsChecked = false;
                                EmpFuncVM.Add(emp);
                            }
                            count++;
                        }
                        break;
                    }
                    count = 0;

                }
            }

            return Json(EmpFuncVM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFunction(string EmpId, List<int> FuncId, string BranchCode)
        {

            if (EmpId != null)
            {
                List<FuncSL> Funcs = new List<FuncSL>();

                foreach (var id in FuncId)
                {
                    FuncSL Func = _funcSLService.All().Where(m => m.FuncId == id).FirstOrDefault();
                    Funcs.Add(Func);
                }

                foreach (var item in Funcs)
                {
                    var selectedFunc = _employeeFuncService.All().ToList().FirstOrDefault(x => x.FuncSl == item.FuncSl && x.EmpId == EmpId);
                    if (selectedFunc != null)
                    {
                        _employeeFuncService.Update(selectedFunc);
                        _employeeFuncService.Save();
                    }
                    else
                    {
                        EmployeeFunc emp = new EmployeeFunc();
                        emp.Id = GenerateEmpId();
                        emp.FuncSl = item.FuncSl;
                        emp.EmpId = EmpId;
                        emp.Form = item.Form;
                        emp.Function = item.Function;
                        Employee empo = new Employee();
                        emp.BranchCode = BranchCode;

                        //  var employee = _employeeService.All().ToList().FirstOrDefault(x => x.Id == Convert.ToInt16(EmpId));
                        //var userbranch = _userbranchService.All().ToList().FirstOrDefault(x => x.Userid == EmpId);

                        //emp.BranchCode = userbranch.BranchCode;

                        _employeeFuncService.Add(emp);
                        _employeeFuncService.Save();

                    }

                }
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            return Json("0", JsonRequestBehavior.AllowGet);


        }
    }
}
