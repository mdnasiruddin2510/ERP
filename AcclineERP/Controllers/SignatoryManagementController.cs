using App.Domain;
using App.Domain.Interface.Service;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class SignatoryManagementController : Controller
    {

        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeFuncService _employeefuncService;
        private readonly IFuncSLService _FuncSLService;

        public SignatoryManagementController(ISubsidiaryInfoAppService _subsidiaryInfoService,

         IEmployeeService _employeeService,
           IEmployeeFuncService _employeefuncService,
             IFuncSLService _FuncSLService)
        {
            this._employeeService = _employeeService;
            this._employeefuncService = _employeefuncService;
            this._FuncSLService = _FuncSLService;
        }
        // GET: SignatoryManagement
        public ActionResult SignatoryManagement()
        {
            return View();
        }
        class signatoryManagementWiseShow
        {
            public int FuncId { get; set; }  
            public string FormName { get; set; }
            public string FuncName { get; set; }
            public int FuncSl { get; set; } 

            //public Nullable<decimal> ReceiptAmt { get; set; }
            //public decimal Billamt { get; set; }
        }

        public ActionResult GetAllDataForSignatoryManagement()
        {
            try
            {               
                string sql = string.Format("select * from FuncSL ");
                IEnumerable<signatoryManagementWiseShow> SMRawLst;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    SMRawLst = dbContext.Database.SqlQuery<signatoryManagementWiseShow>(sql).ToList();

                }
                return Json(new { data = SMRawLst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDataForSelectedUserName(string UserName)
        {
            try
            {                
                // select * from EmployeeFunc where UserID = (select id from Employee where UserName = 'admin' )
                string sql = string.Format("select * from EmployeeFunc where EmpId = (select Id from Employee where UserName = '" + UserName + "')");
                IEnumerable<EmployeeFunc> EmployeeList;

                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    EmployeeList = dbContext.Database.SqlQuery<EmployeeFunc>(sql).ToList();

                }
                return Json(EmployeeList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveSignatoryManagement(string UserName, List<FuncSL> Check)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    //if (!rUser.HasPermission("Signatory_Insert"))
                    //{
                    //    return Json("X", JsonRequestBehavior.AllowGet);
                    //}
                    var UserId = _employeeService.All().Where(x => x.UserName == UserName).Select(x => x.Id).LastOrDefault();
                    var IsExistuser = _employeefuncService.All().Where(x => x.EmpId == UserId).ToList();
                    foreach (var data in IsExistuser)
                    {
                        _employeefuncService.Delete(data);
                        _employeefuncService.Save();
                    }

                    var IfExit = _employeeService.All().Where(x => x.UserName == UserName).LastOrDefault();
                    if (IfExit == null)
                    {

                        Employee EmployeeAdd = new Employee();
                        EmployeeAdd.FullName = UserName;
                        EmployeeAdd.UserName = UserName;
                        //EmployeeAdd.UserRank = 1;
                        EmployeeAdd.BranchCode = "001";
                        EmployeeAdd.IsActive = true;
                        _employeeService.Add(EmployeeAdd);
                        _employeeService.Save();

                    }


                    if (Check != null)
                    {
                        foreach (var data in Check)
                        {
                            //  var IfExist = _acbrServic.All().Where(x => x.Accode == data.Accode && x.BranchCode == data.BranchCode).FirstOrDefault();                      
                            if (Check.Count != 0)
                            {
                                List<EmployeeFunc> emfuncList = new List<EmployeeFunc>();
                                EmployeeFunc emfunc = new EmployeeFunc();
                                emfunc.BranchCode = "001";
                                emfunc.FormName = data.FormName;
                                emfunc.FuncName = data.FuncName;
                                emfunc.FuncSl = Convert.ToInt32( data.FuncSl);
                                emfunc.EmpId = _employeeService.All().ToList().Where(x => x.UserName == UserName).Select(x => x.Id).FirstOrDefault();
                                emfuncList.Add(emfunc);
                                _employeefuncService.Add(emfunc);
                                _employeefuncService.Save();
                            }
                        }
                    }
                    transaction.Complete();

                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}