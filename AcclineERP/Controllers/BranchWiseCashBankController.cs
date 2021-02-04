using App.Domain;
using App.Domain.ViewModel;
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
    public class BranchWiseCashBankController : Controller
    {

        private readonly IBranchAppService _branchService;
        private readonly IAcBRAppService _acbrServic;
        private readonly IDefACAppService _defacService;
        private readonly INewChartAppService _newchartService;
        private readonly IGsetAppService _GsetService;
        public BranchWiseCashBankController(ISubsidiaryInfoAppService _subsidiaryInfoService,
          IBranchAppService _branchService,
          IAcBRAppService _acbrServic,
          IDefACAppService _defacService,
          INewChartAppService _newchartService,
            IGsetAppService _GsetService)
        {
            this._defacService = _defacService;
            this._acbrServic = _acbrServic;
            this._branchService = _branchService;
            this._newchartService = _newchartService;
            this._GsetService = _GsetService;
        }

        // GET: BranchWiseCashBank
        public ActionResult BranchWiseCashBank()
        {
            ViewBag.BranchCode = LoadDropDown.LoadBranchType();
            return View();
        }

        class branchWiseShow
        {
            public string Accode { get; set; }
            public string AcName { get; set; }
            public string AccType { get; set; }
            public bool Check { get; set; }
            public string BranchCode { get; set; }
           
        }


        public ActionResult GetAllDataForAllBranch()
        {
            //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            //if (!rUser.HasPermission("Receive_Insert"))
            //{
            //    return Json("X", JsonRequestBehavior.AllowGet);
            //}
            var Bills = default(dynamic);
            var gset = _GsetService.All().FirstOrDefault();
            string sql = string.Format("select  Accode,AcName,BranchCode,'Cash' as AccType from NewChart where Accode LIKE ('" + gset.GCa + "%')  Union select  Accode ,AcName,BranchCode,'Bank' as AccType from NewChart where( Accode LIKE ('" + gset.GBa + "%'))");
            //  string sql = string.Format("select *from NewChart");
            IEnumerable<branchWiseShow> VchrLst;
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                VchrLst = dbContext.Database.SqlQuery<branchWiseShow>(sql).ToList();
                Bills = VchrLst;
            }
            return Json(new { data = Bills }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDataForSelectedBranch(string BranchCode, string Accode)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Receive_Insert"))
            {
                return Json("X", JsonRequestBehavior.AllowGet);
            }
            string sql = string.Format("select *from AcBR where BranchCode='" + BranchCode + "'");
            IEnumerable<AcBR> BranchList;

            using (AcclineERPContext dbContext = new AcclineERPContext())
            {
                BranchList = dbContext.Database.SqlQuery<AcBR>(sql).ToList();

            }
            return Json(BranchList, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult SaveCashBank(string BranchCode, List<NewChart> Check)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("BranchWiseCashBank_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }

                    var IsExistBr = _acbrServic.All().Where(x => x.BranchCode == BranchCode).ToList();
                    foreach (var data in IsExistBr)
                    {

                        _acbrServic.Delete(data);
                        _acbrServic.Save();
                    }

                    if (Check != null)
                    {


                        foreach (var data in Check)
                        {
                            if (Check.Count != 0)
                            {
                                List<AcBR> AcBrList = new List<AcBR>();

                                AcBR acb = new AcBR();
                                acb.Accode = data.Accode;
                                //acb.BranchCode = data.BranchCode;
                                acb.BranchCode = BranchCode;
                                acb.Ca_Ba = data.AccType;

                                AcBrList.Add(acb);
                                _acbrServic.Add(acb);
                                _acbrServic.Save();
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