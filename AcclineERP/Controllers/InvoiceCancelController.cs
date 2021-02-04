using App.Domain;
using App.Domain.ViewModel;
using Application.Interfaces;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AcclineERP.Models;
using System.Data;

namespace AcclineERP.Controllers
{
    public class InvoiceCancelController : Controller
    {

        private readonly ISalesMainAppService _salesMainService;
        private readonly ISalesDetailAppService _salesDetailsService;
        private readonly IMoneyReceiptAppService _moneyReceiptService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoService;
        private readonly ITransactionLogAppService _transactionLogService;

        public InvoiceCancelController(ISubsidiaryInfoAppService _subsidiaryInfoService,
           ISalesMainAppService _salesMainService,
           ISalesDetailAppService _salesDetailsService,
           IMoneyReceiptAppService _moneyReceiptService,
            ITransactionLogAppService _transactionLogService)
        {

            this._salesMainService = _salesMainService;
            this._salesDetailsService = _salesDetailsService;
            this._moneyReceiptService = _moneyReceiptService;
            this._subsidiaryInfoService = _subsidiaryInfoService;
            this._transactionLogService = _transactionLogService;

        }

        // GET: InvoiceCancel
        public ActionResult InvoiceCancel()
        {
            ViewBag.CancelType = LoadDropDown.LoadCancelType();
            return View();
        }

        public ActionResult GetAllBySaleNo(string saleNo, string CancelType)
        {

            var SaleMain = (dynamic)null;
            if (CancelType == "1")
            {
                SaleMain = _salesMainService.All().ToList().FirstOrDefault(x => x.SaleNo == saleNo);
            }
            else
            {
                SaleMain = _moneyReceiptService.All().ToList().FirstOrDefault(x => x.MRSL == saleNo);
            }
            return Json(SaleMain, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PostCancel(string InvoiceNo, string CancelType)
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("MRCancel"))
            {
                return Json("C", JsonRequestBehavior.AllowGet);
            }
            string ReturnSTR = "";
            using (AcclineERPContext dbContext = new AcclineERPContext())
            {

                var resultParameter = new SqlParameter("@ReturnMSG", SqlDbType.VarChar, 300)
                {
                    Direction = ParameterDirection.Output
                };


                dbContext.Database.ExecuteSqlCommand("MRCancel @MRNo, @ReturnMSG out",
                new SqlParameter("@MRNo", InvoiceNo),
                resultParameter);

                ReturnSTR = (string)resultParameter.Value;
                TransactionLogService.SaveTransactionLog(_transactionLogService, "Cancel", "MRCancel",
                                  InvoiceNo, Session["UserName"].ToString());
            }
            return Json(ReturnSTR, JsonRequestBehavior.AllowGet);



        }
    }
}