using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopUp17Web.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class AccountsReceivableController : Controller
    {
        private readonly IAccountsReceivableAppService _AccountsReceivableAppService;

        public AccountsReceivableController(IAccountsReceivableAppService _AccountsReceivableAppService)
        {
            this._AccountsReceivableAppService = _AccountsReceivableAppService;
        }
        public ActionResult Search(string errMsg)
        {
            ViewBag.Message = errMsg;
            return View();

        }

        public ActionResult GetAccReceivable(RPSearchVModel vmodel, string finyear)
        {
            finyear = Session["FinYear"].ToString();
            ViewBag.fDate = vmodel.fDate;
            ViewBag.tDate = vmodel.tDate;
            string sql = string.Format(" EXEC rptAccountsReceivable '" + finyear + "','" + Convert.ToDateTime(vmodel.fDate) + "','" + Convert.ToDateTime(vmodel.tDate) + "'");
            List<AccountsReceivable> accRcv = _AccountsReceivableAppService.SqlQueary(sql).ToList();
            if (accRcv.Count == 0)
            {
                string errMsg = "There is no data in this combination. Please try again !!!";
                return RedirectToAction("Search", "AccountsReceivable", new { errMsg });
            }
            else
            {
                return View(accRcv);
            }
        }
        [HttpPost]
        public ActionResult AccReceivablePdf(DateTime fDate, DateTime tDate, string finyear)
        {
            //finyear = "2017-17";
            finyear = Session["FinYear"].ToString();
            string sql = string.Format(" EXEC rptAccountsReceivable '" + finyear + "','" + Convert.ToDateTime(fDate) + "','" + Convert.ToDateTime(tDate) + "'");
            List<AccountsReceivable> accRcv = _AccountsReceivableAppService.SqlQueary(sql).ToList();
            ViewBag.fDate = fDate.ToShortDateString();
            ViewBag.tDate = tDate.ToShortDateString();
            ViewBag.PrintDate = DateTime.Now.ToShortDateString();
            return new Rotativa.ViewAsPdf("AccReceivablePdf", accRcv);
        }

    }
}
