using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    public class BackupDBController : Controller
    {
        private readonly ITransactionLogAppService _transactionLogService;
        public BackupDBController(ITransactionLogAppService _transactionLogService)
        {
            this._transactionLogService = _transactionLogService;
        }
        // GET: BackupDB
        public ActionResult BackupDB()
        {
            return View();
        }

        public ActionResult GetBackupDB()
        {
            if (Session["UserID"] != null)
            {
                // read connectionstring from config file
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // read backup folder from config file ("C:/BackupDB/")
                var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

                var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

                // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
                var backupFileName = String.Format("{0}{1}-{2}.bak", backupFolder, sqlConStrBuilder.InitialCatalog, DateTime.Now.ToString("yyyy-MM-dd"));

                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {
                    var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                        sqlConStrBuilder.InitialCatalog, backupFileName);

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                string PhyscPath = backupFileName;
                try
                {
                    byte[] fileByte = System.IO.File.ReadAllBytes(PhyscPath);
                    string DownFileName = String.Format("{0}_{1}.bak", sqlConStrBuilder.InitialCatalog,
                    DateTime.Now.ToString("yyyy-MM-dd"));
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "BackupDB", "", "01", Session["UserName"].ToString());
                    return File(fileByte, System.Net.Mime.MediaTypeNames.Application.Octet, DownFileName);
                }
                catch (Exception)
                {
                    return RedirectToAction("BackupDB", "BackupDB");
                }
                finally
                {
                    System.IO.File.Delete(backupFileName);

                }

            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

    }
}