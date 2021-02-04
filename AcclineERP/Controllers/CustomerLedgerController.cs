using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    //[Authorize(Roles = "Admin, User")]
    public class CustomerLedgerController : Controller
    {
        private readonly ICustomerLedgerAppService _CustomerLedgerService;
        private readonly ISubsidiaryInfoAppService _CustomerService;

        public CustomerLedgerController(ICustomerLedgerAppService _CustomerLedgerService, ISubsidiaryInfoAppService _CustomerService)
        {
            this._CustomerLedgerService = _CustomerLedgerService;
            this._CustomerService = _CustomerService;
        }
        public ActionResult Search(string errMsg)
        {
            ViewBag.SubCode = new SelectList(_CustomerService.All().ToList(), "SubCode", "SubName");
            ViewBag.Message = errMsg;
            return View();

        }

        public ActionResult GetCustomerLedger(CLSearchVModel vmodel, string finyear)
        {
            finyear = Session["FinYear"].ToString();
            ViewBag.fDate = vmodel.fDate;
            ViewBag.tDate = vmodel.tDate;
            ViewBag.CustCode = vmodel.SubCode;
            ViewBag.Customer = _CustomerService.All().FirstOrDefault(x => x.SubCode == vmodel.SubCode.Trim()).SubName.ToString();
            string sql = string.Format(" EXEC rptCustomerLedger '" + finyear + "','" + vmodel.SubCode + "','" + Convert.ToDateTime(vmodel.fDate) + "','" + Convert.ToDateTime(vmodel.tDate) + "'");
            List<CustomerLedger> custLedger = _CustomerLedgerService.SqlQueary(sql).ToList();
            if (custLedger.Count == 0)
            {
                string errMsg = "There is no data in this combination. Please try again !!!";
                return RedirectToAction("Search", "CustomerLedger", new { errMsg });
            }
            else
            {
                return View(custLedger);
            }
        }
        [HttpPost]
        public ActionResult CustomerLedgerPdf(CLSearchVModel vmodel, string finyear)
        {
            finyear = Session["FinYear"].ToString();
            string sql = string.Format(" EXEC rptCustomerLedger '" + finyear + "','" + vmodel.SubCode + "','" + Convert.ToDateTime(vmodel.fDate) + "','" + Convert.ToDateTime(vmodel.tDate) + "'");
            List<CustomerLedger> custLedger = _CustomerLedgerService.SqlQueary(sql).ToList();
            ViewBag.fDate = vmodel.fDate;
            ViewBag.tDate = vmodel.tDate;
            ViewBag.Customer = _CustomerService.All().FirstOrDefault(x => x.SubCode == vmodel.SubCode.Trim()).SubName.ToString();
            ViewBag.PrintDate = DateTime.Now.ToShortDateString();
            return new Rotativa.ViewAsPdf("CustomerLedgerPdf", custLedger);
        }

    }
}
