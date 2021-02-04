using App.Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Xml;
using System.Text;
using System.Transactions;

namespace AcclineERP.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private readonly INewChartAppService _newChartService;
        private readonly IMinLevAppService _minLevService;
        private readonly ILevelLenAppService _levelLenService;
        private readonly IProjInfoAppService _projInfoService;
        private readonly IPVchrDetailAppService _pVchrDetailService;
        private readonly ITVchrDetailAppService _tVchrDeatailService;
        private readonly IVchrDetailAppService _VchrDeatilService;
        public ChartOfAccountsController(INewChartAppService _newChartService, IMinLevAppService _minLevService,
                                          ILevelLenAppService _levelLenService, IProjInfoAppService _projInfoService,
            IPVchrDetailAppService _pVchrDetailService, ITVchrDetailAppService _tVchrDeatailService,
            IVchrDetailAppService _VchrDeatilService)
        {
            this._newChartService = _newChartService;
            this._minLevService = _minLevService;
            this._levelLenService = _levelLenService;
            this._projInfoService = _projInfoService;
            this._pVchrDetailService = _pVchrDetailService;
            this._tVchrDeatailService = _tVchrDeatailService;
            this._VchrDeatilService = _VchrDeatilService;
        }

        public ActionResult LoadChartofAccounts(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                var sepa = _minLevService.All().FirstOrDefault();
                ViewBag.Separator = sepa.Sepa;
                ViewBag.errMsg = errMsg;
                ViewBag.ProjName = new SelectList(_projInfoService.All().ToList(), "ProjId", "ProjName");

                var plist = _newChartService.All().Where(x => x.ParentCode == 0).Select(a => new
                {
                    a.AcSyscode,
                    a.AcName
                }).ToList();
                GetHierarchy();
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public JsonResult GetHierarchy()
        {
            List<NewChart> hdList;
            List<NewChart> records;

            hdList = _newChartService.All().ToList();

            records = hdList.Where(l => l.ParentCode == 0)
                .Select(l => new NewChart
                {
                    Id = l.AcSyscode,
                    text = l.AcName,
                    perentId = l.ParentCode.ToString(),
                    children = GetChildren(hdList, l.AcSyscode)
                }).ToList();

            return this.Json(records, JsonRequestBehavior.AllowGet);
        }

        private List<NewChart> GetChildren(List<NewChart> hdList, int parentId)
        {
            return hdList.Where(l => l.ParentCode == parentId)
                .Select(l => new NewChart
                {
                    Id = l.AcSyscode,
                    text = l.AcName,
                    perentId = l.ParentCode.ToString(),
                    children = GetChildren(hdList, l.AcSyscode)
                }).ToList();
        }


        public ActionResult GetNewChartPdf()
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("ChartOfAccounts_Preview"))
            {
                string errMsg = "P";
                return RedirectToAction("LoadChartofAccounts", "ChartOfAccounts", new { errMsg });
            }
            List<NewChart> chart = _newChartService.All().ToList();
            return new Rotativa.ViewAsPdf("GetNewChartPdf", chart) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }

        [HttpPost]
        public ActionResult GetCodeName(string values)
        {
            try
            {
                var id = values.Split(',');
                int rId = Convert.ToInt32(id.FirstOrDefault());
                NewChart datas = new NewChart();

                datas = _newChartService.All().ToList().FirstOrDefault(x => x.AcSyscode == rId);
                _newChartService.Save();

                return Json(datas, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public JsonResult DeleteNode(string values, string IsTree)
        {
            try
            {
                string msg = "Deleted successfully";
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("ChartOfAccounts_Delete"))
                {
                    msg = "No Delete Permission For This User !!";
                    return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);
                }

                var id = values.Split(',');
                foreach (var item in id)
                {
                    int ID = int.Parse(item);
                    var nChart = (from nc in _newChartService.All().ToList().Where(x => x.AcSyscode == ID)
                                  select new
                                  {
                                      ParentCode = nc.ParentCode,
                                      AcSyscode = nc.AcSyscode,
                                      Accode = nc.Accode
                                  }).FirstOrDefault();

                    var isVExist = _VchrDeatilService.All().ToList().Where(x => x.Accode == nChart.Accode).FirstOrDefault();
                    var isPvExist = _tVchrDeatailService.All().ToList().Where(x => x.Accode == nChart.Accode).FirstOrDefault();
                    var isTvExist = _pVchrDetailService.All().ToList().Where(x => x.Accode == nChart.Accode).FirstOrDefault();

                    var pCode = _newChartService.All().ToList().Where(x => x.ParentCode == ID).FirstOrDefault();

                    if (nChart.ParentCode != 0 && pCode == null && isVExist == null && isPvExist == null && isTvExist == null)
                    {
                        _newChartService.All().Where(x => x.AcSyscode == ID).ToList().ForEach(u => _newChartService.Delete(u));
                        _newChartService.Save();
                    }
                    else if (nChart.ParentCode != 0 && IsTree == "btnDeleteTree" && isVExist == null && isPvExist == null && isTvExist == null)
                    {
                        _newChartService.All().Where(x => x.AcSyscode == ID).ToList().ForEach(u => _newChartService.Delete(u));
                        _newChartService.Save();
                    }
                    else
                    {
                        msg = "The Selected Account Head isn't Deleteable!";
                        return Json(new { msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex }, JsonRequestBehavior.AllowGet);
            }

        }


        private string GetAccNewCode(string ParentCode, string Separator, int LevelNo, int AcSysCode)
        {
            string AccCode = "";
            string LevelDig = _levelLenService.All().FirstOrDefault(x => x.LevelNo == LevelNo).LevelDig.ToString();
            string Code = "";
            try
            {
                var dt = _newChartService.All().ToList().OrderBy(y => y.Accode).Where(x => x.ParentCode == AcSysCode).ToList();
                if (dt.Count == 0)
                {
                    Code = "1";
                }
                else if (dt.Count > 0)
                {
                    string x = dt.LastOrDefault().Accode;
                    string[] CD = x.Split('.');
                    int Cod = Convert.ToInt16(CD[CD.Length - 1].ToString()) + 1;
                    Code = Cod.ToString();
                }

            }
            catch (Exception) { }
            string Zero = "";
            for (int i = Code.Length; i < Convert.ToInt16(LevelDig); i++)
            {
                Zero += "0";
            }

            AccCode = ParentCode + Separator + Zero + Code;
            return AccCode;
        }

        [HttpPost]
        public ActionResult AddNewNode(NewChart model)
        {
            try
            {
                string msg = "";
                if (model.IsModifyFor == "Sa")
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("ChartOfAccounts_Insert"))
                    {
                        msg = "No Add Permission For This User !!";
                        return Json(new { success = false, msg = msg }, JsonRequestBehavior.AllowGet);
                    }
                    var parChart = _newChartService.All().FirstOrDefault(x => x.AcSyscode == model.AcSyscode);
                    var parLbl = _levelLenService.All().FirstOrDefault(x => x.LevelNo == parChart.LevelNo).LevelNo + 1;
                    var sep = _minLevService.All().FirstOrDefault().Sepa;
                    var LAcSysCode = _newChartService.All().OrderByDescending(x => x.AcSyscode).Select(s => s.AcSyscode).FirstOrDefault();

                    NewChart chart = new NewChart()
                    {
                        Accode = GetAccNewCode(parChart.Accode.ToString(), sep, parLbl, Convert.ToInt32(parChart.AcSyscode)),
                        AcName = model.NewAcName,
                        ParentCode = parChart.AcSyscode,
                        LevelNo = parLbl,
                        AcSyscode = LAcSysCode + 1,
                        B_S = false,
                        I_S = false,
                        T_B = false,
                        ParentAcCode = parChart.Accode,
                        AccType = "",
                        OldCode = model.NewOldCode
                    };

                    _newChartService.Add(chart);

                    parChart.T_B = true;
                    _newChartService.Update(parChart);

                    _newChartService.Save();
                    msg = "Save successfully!";
                    return Json(new { success = true, msg = msg }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("ChartOfAccounts_Update"))
                    {
                        msg = "No Modify Permission For This User !!";
                        return Json(new { success = false, msg = msg }, JsonRequestBehavior.AllowGet);
                    }
                    var parChart = _newChartService.All().FirstOrDefault(x => x.AcSyscode == model.AcSyscode);
                    parChart.AcName = model.NodeName;
                    parChart.OldCode = model.OldCode;

                    _newChartService.Update(parChart);
                    _newChartService.Save();
                    msg = "Modify successfully!";
                    return Json(new { success = true, msg = msg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownlaodExcel()
        {
            string DownFilePath = ConfigurationManager.AppSettings["DownFilePath"];
            string FormatFileName = ConfigurationManager.AppSettings["FormatNChartFileName"];
            string PhyscPath = Path.Combine(Server.MapPath(DownFilePath), FormatFileName);
            try
            {
                byte[] fileByte = System.IO.File.ReadAllBytes(PhyscPath);
                string DownFileName = ConfigurationManager.AppSettings["DownNChartFileName"];
                return File(fileByte, System.Net.Mime.MediaTypeNames.Application.Octet, DownFileName);
            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpPost]
        public ActionResult UploadExcel()
        {
            //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            //if (!rUser.HasPermission("MonthlyContribution_Import"))
            //{
            //    return Json("I", JsonRequestBehavior.AllowGet);
            //}
            try
            {
                var UploadedFile = Request.Files["file"];
                int AcSyscode = Convert.ToInt32(Request.Form["AcSyscode"]);

                string BranchCode = "";
                if (!string.IsNullOrEmpty(Session["BranchCode"] as string))
                {
                    BranchCode = Session["BranchCode"].ToString();
                }

                string fileLocation = "";
                string filePath = "";
                string FileName = "";
                string[] fileList = null;
                string fileExtension = "";
                List<NewChart> TNChartList = new List<NewChart>();

                filePath = ConfigurationManager.AppSettings["UPFilePath"];

                DataSet ds = new DataSet();
                if (UploadedFile.ContentLength > 0)
                {
                    FileName = Path.GetFileName(UploadedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(FileName);
                    fileLocation = Path.Combine(Server.MapPath(filePath), FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {

                        fileList = System.IO.Directory.GetFiles(Server.MapPath(filePath));
                        foreach (string file in fileList)
                        {
                            System.IO.File.Delete(file);
                        }

                        UploadedFile.SaveAs(fileLocation);

                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;OLE DB Services=-4;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {

                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;OLE DB Services=-4;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }

                        //Create Connection to Excel work book and add oledb namespace
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                        {
                            excelConnection.Open();
                            DataTable dt = new DataTable();

                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (dt == null)
                            {
                                return null;
                            }

                            String[] excelSheets = new String[dt.Rows.Count];
                            int t = 0;
                            //excel data saves in temp file here.
                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[t] = row["TABLE_NAME"].ToString();
                                t++;
                            }

                            using (OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString))
                            {
                                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                                {
                                    dataAdapter.Fill(ds);
                                }
                            }
                        }

                    }
                    if (fileExtension.ToString().ToLower().Equals(".xml"))
                    {
                        fileList = System.IO.Directory.GetFiles(Server.MapPath(filePath));
                        foreach (string file in fileList)
                        {
                            System.IO.File.Delete(file);
                        }

                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();
                        ds.Dispose();
                    }
                    List<NewChart> LstImpFlie = new List<NewChart>();
                    StringBuilder ValdtMsg = new StringBuilder();
                    if (ds != null)
                    {
                        var FDT = RemoveAllNullRowsFromDataTable(ds.Tables[0]);
                        LstImpFlie = GetNChartFromDT(FDT);
                        //ValdtMsg.Append(ValidateFileFiledFormat(FDT));
                    }
                    else
                    {
                        ValdtMsg.Append("Cannot Read File Properly!!");
                    }

                    if (ValdtMsg.Length > 0)
                    {
                        return Json(ValdtMsg.ToString(), JsonRequestBehavior.AllowGet);
                    }
                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            foreach (var nItem in LstImpFlie)
                            {
                                var parChart = _newChartService.All().FirstOrDefault(x => x.AcSyscode == AcSyscode);
                                var parLbl = _levelLenService.All().FirstOrDefault(x => x.LevelNo == parChart.LevelNo).LevelNo + 1;
                                var sep = _minLevService.All().FirstOrDefault().Sepa;
                                var LAcSysCode = _newChartService.All().OrderByDescending(x => x.AcSyscode).Select(s => s.AcSyscode).FirstOrDefault();

                                NewChart chart = new NewChart()
                                {
                                    Accode = GetAccNewCode(parChart.Accode.ToString(), sep, parLbl, Convert.ToInt32(parChart.AcSyscode)),
                                    AcName = nItem.AcName,
                                    ParentCode = parChart.AcSyscode,
                                    LevelNo = parLbl,
                                    AcSyscode = LAcSysCode + 1,
                                    B_S = false,
                                    I_S = false,
                                    T_B = false,
                                    ParentAcCode = parChart.Accode,
                                    AccType = "",
                                    OldCode = nItem.OldCode,
                                    BranchCode = BranchCode
                                };

                                _newChartService.Add(chart);
                                _newChartService.Save();
                            }
                            transaction.Complete();
                            return Json("0", JsonRequestBehavior.AllowGet);

                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        finally
                        {
                            ds.Clear();
                            ds.Dispose();
                            ValdtMsg.Length = 0;
                            ValdtMsg.Clear();
                        }
                    }
                }
                else
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        public DataTable RemoveAllNullRowsFromDataTable(DataTable dt)
        {
            int columnCount = dt.Columns.Count;

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                bool allNull = true;
                for (int j = 0; j < columnCount; j++)
                {
                    if (dt.Rows[i][j] != DBNull.Value)
                    {
                        allNull = false;
                    }
                }
                if (allNull)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        public List<NewChart> GetNChartFromDT(DataTable FDT)
        {
            var TmpNChartRows = (from DataRow row in FDT.Rows
                                 select new NewChart
                                 {
                                     AcName = row["AcName"].ToString(),
                                     OldCode = row["OldCode"].ToString()

                                 }).ToList();
            return TmpNChartRows;
        }
        public string ValidateFileFiledFormat(DataTable FDT)
        {

            StringBuilder builder = new StringBuilder();
            decimal DResponse;
            int[] DClmn = { 0, 1 };
            bool HasWConv = false;

            for (int i = 0; i < FDT.Rows.Count; i++)
            {

                var DRow = FDT.Rows[i];

                for (int j = 0; j < DClmn.Count(); j++)
                {
                    if (decimal.TryParse(DRow[DClmn[j]].ToString(), out DResponse))
                    {

                    }
                    else
                    {
                        HasWConv = true;

                        builder.Append("At Row No ");
                        builder.Append(i + 2);
                        builder.Append(" and column no ");
                        builder.Append(DClmn[j] + 1);
                        builder.Append(" Has Invalid data");
                        builder.Append("\n");

                    }

                }

            }

            if (HasWConv == false)
            {
                builder.Length = 0;
            }

            return builder.ToString();

        }
    }
}
