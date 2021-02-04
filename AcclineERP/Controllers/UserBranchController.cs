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
    public class UserBranchController : Controller
    {
        private readonly ISecUserInfoService _secuserinfoService;
        private readonly IUserBranchService _userbranchService;
        private readonly IBranchService _branchService;

        public UserBranchController(ISubsidiaryInfoAppService _subsidiaryInfoService,

         ISecUserInfoService _secuserinfoService,
           IUserBranchService _userbranchService,
             IBranchService _branchService)
        {
            this._secuserinfoService = _secuserinfoService;
            this._userbranchService = _userbranchService;
            this._branchService = _branchService;
        }
        // GET: UserBranch
        public ActionResult UserBranch()
        {
            ViewBag.UserName = LoadDropDown.LoaduserBranchType();
            return View();
        }

        class UserBranchWiseShow
        {
            public string BranchCode { get; set; }
            public string BranchName { get; set; }                  
        }
        public ActionResult GetAllDataForUserBranch()
        {
            try
            {                
                string sql = string.Format("select *from Branch");
                IEnumerable<UserBranchWiseShow> userBranchLst;
                using (AcclineERPContext dbContext = new AcclineERPContext())
                {
                    userBranchLst = dbContext.Database.SqlQuery<UserBranchWiseShow>(sql).ToList();

                }
                return Json(new { data = userBranchLst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetDataForSelectedUserBranch(int UserID)
        {
            try
            {                              
                // select * from EmployeeFunc where UserID = (select id from Employee where UserName = 'admin' )
                string sql = string.Format("select * from UserBranch where Userid ='" + UserID + "'");               
                IEnumerable<UserBranch> userBranchList;

                using (AcclineERPContext dbContext = new AcclineERPContext()) 
                {
                    userBranchList = dbContext.Database.SqlQuery<UserBranch>(sql).ToList();

                }
                return Json(userBranchList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveUserBranch(int UserID, List<Branch> Check)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("UserBranch_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    var IsExistBr = _userbranchService.All().Where(x => x.Userid == UserID.ToString()).ToList();
                    foreach (var data in IsExistBr)
                    {

                        _userbranchService.Delete(data);
                        _userbranchService.Save();
                    }
                    if (Check != null)
                    {


                        foreach (var data in Check)
                        {                                     
                            if (Check.Count != 0)
                            {
                                List<UserBranch> userbranchList = new List<UserBranch>();

                                UserBranch userBranchAdd = new UserBranch();
                                userBranchAdd.BranchCode = data.BranchCode;
                                userBranchAdd.Userid = UserID.ToString();
                             
                                userbranchList.Add(userBranchAdd);

                               _userbranchService.Add(userBranchAdd);
                                _userbranchService.Save();
                            }

                            //else
                            //{
                            //   
                            //}

                        }
                        // transaction.Dispose();
                        // return Json("3", JsonRequestBehavior.AllowGet);
                    }
                    transaction.Complete();
                    //Check.Clear();
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