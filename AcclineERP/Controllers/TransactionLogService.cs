using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.Interfaces;
using App.Domain;

namespace AcclineERP.Controllers
{
    public static class TransactionLogService
    {
        public static bool SaveTransactionLog(ITransactionLogAppService _tranService,TransactionLog tranLog)
        {
            try
            {
                _tranService.Add(tranLog);
                _tranService.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public static bool SaveTransactionLog(ITransactionLogAppService _tranService, string tranForm, string tranType,string id,string userName)
        {
            try
            {
                TransactionLog tranLog = new TransactionLog();
                tranLog.TranForm = tranForm;
                tranLog.TranType = tranType;
                tranLog.TranDateTime = DateTime.Now.ToString();
                tranLog.TranNo = id;
                tranLog.UserName = userName;
                _tranService.Add(tranLog);
                _tranService.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}