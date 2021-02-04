using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Domain.Interface.Service.Common;
using App.Domain.Interface.VModel;
using App.Domain.Services.Common;
using App.Domain.ViewModel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;

namespace TopUp17Web.Report
{
    public partial class RepotView : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        //protected void Page_Load(object sender, EventArgs e)
        //{http://localhost:60342/Report/RepotView.aspx.cs

        //}
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    //SearchVModel vmodel = new SearchVModel();
                    //vmodel.Search = (Search)HttpContext.Current.Session["ReportParam"];

                    ICommonSearchModel Search= new CommonSearchModel();
                    Search = (ICommonSearchModel) HttpContext.Current.Session["ReportParam"];

                    string hiddenValue = Search.ReportTitle;
                    hiddenFieldTextBox.Text = hiddenValue;
                    databaseFileTextBox.Text = Search.RPTFileName;

                    Session[Search.ReportTitle] = Search;
                    Session[Search.RPTFileName] = Session["rptSource"];

                    Session.Remove("rptSource");
                    Session.Remove("ReportParam");
                    hiddenFieldTextBox.Style.Add("visible", "false");
                    databaseFileTextBox.Style.Add("visible", "false");
                    LoadReport(hiddenValue, Search.RPTFileName);


                }
                else
                {

                    var getVmodel = Request.Form["hiddenFieldTextBox"].ToString();
                    var getDatasource = Request.Form["databaseFileTextBox"].ToString();
                    LoadReport(getVmodel, getDatasource);


                }



            }
            catch (Exception ex)
            {
                Response.Write("Nothing found/ No report found");
            }


        }



        private void LoadReport(string getVmodel, string getDatasource)
        {

            try
            {

                //SearchVModel vmodel = new SearchVModel();
                ICommonSearchModel vmodel = new CommonSearchModel();
                IUtilityService utilityService = new UtilityService();

                bool isValid = true;
                vmodel = (ICommonSearchModel)HttpContext.Current.Session[getVmodel];
                Page.Title = "Report | " + vmodel.ReportTitle;

                // Setting Report Data Source

                var rptSource = System.Web.HttpContext.Current.Session[getDatasource];
                if (string.IsNullOrEmpty(vmodel.RPTFileName)) // Checking is Report name provided or not
                {
                    isValid = false;
                }

                if (isValid) // If Report Name provided then do other operation
                {

                    //string strRptPath = Server.MapPath(@"Report\"+vmodel.Search.RPTFileName);
                    string strRptPath = Server.MapPath("~/") + @"Report\" + vmodel.RPTFileName;


                    //Loading Report

                    rd.Load(strRptPath);

                    // Setting report data source
                    rd.Refresh();
                    switch (vmodel.ReportTitle)
                    {
                        case "Region Wise Report":
                            {

                                string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                                var sqlStringBuilder = new SqlConnectionStringBuilder(connectionstring);
                                string server = sqlStringBuilder.DataSource;
                                string database = sqlStringBuilder.InitialCatalog;
                                string userid = sqlStringBuilder.UserID;
                                string password = sqlStringBuilder.Password;


                                //may be you need to set the integrated security to false, first.
                                rd.DataSourceConnections[0].IntegratedSecurity = false;

                                rd.DataSourceConnections[0].SetConnection(server, database, userid, password);
                                // rd.SetDatabaseLogon("Dev_3", "", "192.168.1.22", "Ep_IAS");
                                break;
                            }
                        //case "Collection Statement":
                        //    {
                        //        string connectionstring = ConfigurationManager.ConnectionStrings["AccLineConectionString"].ConnectionString;
                        //        var sqlStringBuilder = new SqlConnectionStringBuilder(connectionstring);
                        //        string server = sqlStringBuilder.DataSource;
                        //        string database = sqlStringBuilder.InitialCatalog;
                        //        string userid = sqlStringBuilder.UserID;
                        //        string password = sqlStringBuilder.Password;


                        //        //may be you need to set the integrated security to false, first.
                        //        rd.DataSourceConnections[0].IntegratedSecurity = false;

                        //        rd.DataSourceConnections[0].SetConnection(server, database, userid, password);
                        //        // rd.SetDatabaseLogon("Dev_3", "", "192.168.1.22", "Ep_IAS");
                        //        break;
                        //    }
                        //case "Payble statement":
                        //    {
                        //        string connectionstring = ConfigurationManager.ConnectionStrings["AccLineConectionString"].ConnectionString;
                        //        var sqlStringBuilder = new SqlConnectionStringBuilder(connectionstring);
                        //        string server = sqlStringBuilder.DataSource;
                        //        string database = sqlStringBuilder.InitialCatalog;
                        //        string userid = sqlStringBuilder.UserID;
                        //        string password = sqlStringBuilder.Password;


                        //        //may be you need to set the integrated security to false, first.
                        //        rd.DataSourceConnections[0].IntegratedSecurity = false;

                        //        rd.DataSourceConnections[0].SetConnection(server, database, userid, password);
                        //        // rd.SetDatabaseLogon("Dev_3", "", "192.168.1.22", "Ep_IAS");
                        //        break;
                        //    }
                        //case "Vendor Ledger":
                        //    {
                        //        string connectionstring = ConfigurationManager.ConnectionStrings["AccLineConectionString"].ConnectionString;
                        //        var sqlStringBuilder = new SqlConnectionStringBuilder(connectionstring);
                        //        string server = sqlStringBuilder.DataSource;
                        //        string database = sqlStringBuilder.InitialCatalog;
                        //        string userid = sqlStringBuilder.UserID;
                        //        string password = sqlStringBuilder.Password;


                        //        //may be you need to set the integrated security to false, first.
                        //        rd.DataSourceConnections[0].IntegratedSecurity = false;

                        //        rd.DataSourceConnections[0].SetConnection(server, database, userid, password);
                        //        // rd.SetDatabaseLogon("Dev_3", "", "192.168.1.22", "Ep_IAS");
                        //        break;
                        //    }
                        default:
                            {
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                {

                                    rd.SetDataSource(rptSource);
                                }

                                break;
                            }
                    }

                    //Set Parameter
                    //Methods aMethod = new Methods();
                    //if (vmodel.Search != null)
                    //{
                    //    switch (vmodel.Search.ReportTitle)
                    //    {
                    //        case "Inventory Item":
                    //            {
                    //                new ParamCompanyInformation(ref rd, vmodel.Search);
                    //                break;
                    //            }
                    //        case "Transaction":
                    //            {
                    //                break;
                    //            }
                    //        case "Item Reorder Level":
                    //            {
                    //                new ParamCompanyInformation(ref rd, vmodel.Search);
                    //                break;
                    //            }
                    //        default:
                    //            {
                    //                new ParamCompanyInformation(ref rd, vmodel.Search);
                    //                var a = Convert.ToDateTime(aMethod.DateTimeFormat(vmodel.Search.FDate, false, 1)).ToString("dd-MMM-yyyy");
                    //                if (!string.IsNullOrEmpty(vmodel.Search.FDate))
                    //                    rd.SetParameterValue("FDate", Convert.ToDateTime(aMethod.DateTimeFormat(vmodel.Search.FDate, false, 1)).ToString("dd-MMM-yyyy"));
                    //                if (!string.IsNullOrEmpty(vmodel.Search.TDate))
                    //                    rd.SetParameterValue("tdate", Convert.ToDateTime(aMethod.DateTimeFormat(vmodel.Search.TDate, false, 1)).ToString("dd-MMM-yyyy"));

                    //                rd.SetParameterValue("Title", vmodel.Search.ReportTitle);
                    //                break;
                    //            }
                    //    }
                    //}


                    //setting stocksummary param

                    //if (vmodel.Search.ReportTitle == "Stock Summary Report" || vmodel.Search.ReportTitle == "Group Wise Stock Summary" || vmodel.Search.ReportTitle == "Store Ledger Report" || vmodel.Search.ReportTitle == "Inventory Item" || vmodel.Search.ReportTitle == "Stock Balance Report" || vmodel.Search.ReportTitle == "Details Store Ledger Report" || vmodel.Search.ReportTitle == "Invoice Wise Profit Ratio")
                    //{

                    //    if (vmodel.Search.ReportTitle == "Stock Summary Report")
                    //    {
                    //        string hostdet = Request.ServerVariables["HTTP_HOST"].ToString();
                    //        string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    //        var url = "http://" + hostdet + "/" + applicationPath + "/StoreLedger/GetStoreLedger/";
                    //        rd.SetParameterValue("WithPrice", vmodel.Search.WithPrice.ToString());
                    //        rd.SetParameterValue("Url", url);
                    //        rd.SetParameterValue("AllLocation", vmodel.Search.AllLocation.ToString());

                    //    }
                    //    if (vmodel.Search.ReportTitle == "Group Wise Stock Summary")
                    //    {

                    //        if (vmodel.Search.Summarized == true)
                    //        {
                    //            string hostdet = Request.ServerVariables["HTTP_HOST"].ToString();
                    //            string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    //            var url = "http://" + hostdet + "/" + applicationPath + "/StockSummary/GetStockSummary/";
                    //            rd.SetParameterValue("WithPrice", vmodel.Search.WithPrice.ToString());
                    //            rd.SetParameterValue("Url", url);
                    //            rd.SetParameterValue("AllLocation", vmodel.Search.AllLocation.ToString());
                    //            if (vmodel.Search.AllGroupId == true)
                    //            {
                    //                rd.SetParameterValue("AllGroup", "true");
                    //            }
                    //            else
                    //            {
                    //                rd.SetParameterValue("AllGroup", "false");
                    //            }
                    //            if (vmodel.Search.AllSubGroup == true)
                    //            {
                    //                rd.SetParameterValue("AllSubGroup", "true");
                    //            }
                    //            else
                    //            {
                    //                rd.SetParameterValue("AllSubGroup", "false");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            string hostdet = Request.ServerVariables["HTTP_HOST"].ToString();
                    //            string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    //            var url = "http://" + hostdet + "/" + applicationPath + "/StoreLedger/GetStoreLedger/";
                    //            rd.SetParameterValue("WithPrice", vmodel.Search.WithPrice.ToString());
                    //            rd.SetParameterValue("Url", url);
                    //            rd.SetParameterValue("AllLocation", vmodel.Search.AllLocation.ToString());
                    //        }
                    //    }
                    //    if (string.IsNullOrEmpty(vmodel.Search.LocationId))
                    //    {
                    //        rd.SetParameterValue("LocNo", string.Empty);
                    //    }
                    //    else
                    //    {
                    //        rd.SetParameterValue("LocNo", vmodel.Search.LocationId);
                    //    }
                    //    if (vmodel.Search.ReportTitle == "Inventory Item" == true)
                    //    {
                    //        if (vmodel.Search.AllLocation == true)
                    //        {
                    //            rd.SetParameterValue("ReportCond", 1);
                    //        }
                    //        else if (vmodel.Search.LocationId != "")
                    //        {
                    //            rd.SetParameterValue("ReportCond", 2);
                    //        }
                    //    }
                    //    if (vmodel.Search.ReportTitle == "Store Ledger Report")
                    //    {
                    //        string hostdet = Request.ServerVariables["HTTP_HOST"].ToString();
                    //        string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    //        var url = "http://" + hostdet + "/" + applicationPath + "/TReportStoreToTran/Report/";
                    //        rd.SetParameterValue("Url", url);
                    //        if (vmodel.Search.AllLocation == true)
                    //        {
                    //            rd.SetParameterValue("ReportCond", 1);

                    //        }
                    //        else if (vmodel.Search.LocationId != "")
                    //        {
                    //            rd.SetParameterValue("ReportCond", 2);

                    //        }
                    //        if (vmodel.Search.LineId == 0)
                    //        {
                    //            rd.SetParameterValue("LineName", "All Line");
                    //        }
                    //        else
                    //        {
                    //            rd.SetParameterValue("LineName", "Sigle Line");
                    //        }

                    //    }

                    //    new ParamStockSummary(ref rd, vmodel.Search);
                    //}

                    //if (vmodel.Search.ReportTitle == "Sales Return Summary")
                    //{
                    //    new ParamSalesReturnSumamry(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Payble statement" || vmodel.Search.ReportTitle == "Vendor Ledger" || vmodel.Search.ReportTitle == "Accounts Receivables Statement" || vmodel.Search.ReportTitle == "Advance From Customer Summary" || vmodel.Search.ReportTitle == "Advance From Customer")
                    //{
                    //    new ParamAccountReceivable(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Collection Statement")
                    //{
                    //    new ParamSalesAndCollectionSatement(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Line Wise Statement")
                    //{

                    //    new ParamSalesAndCollectionSatement(ref rd, vmodel.Search);
                    //}


                    //if (vmodel.Search.ReportTitle == "Customer ledger" || vmodel.Search.ReportTitle == "Customer ledger Details")
                    //{
                    //    rd.SetParameterValue("CustName", vmodel.Search.CustomerName);
                    //    rd.SetParameterValue("CustAddress", vmodel.Search.CustomerAddress);

                    //}
                    //if (vmodel.Search.ReportTitle == "Line And Location Wise Invoice Report" || vmodel.Search.ReportTitle == "Statement of Sale" || vmodel.Search.ReportTitle == "Line Wise Financial Statement" || vmodel.Search.ReportTitle == "Line Wise Sales Statement" || vmodel.Search.ReportTitle == "Line & Sale Type Wise Revenue" || vmodel.Search.ReportTitle == "Invoice Wise Sale Details" || vmodel.Search.ReportTitle == "Purschase Status" || vmodel.Search.ReportTitle == "Sales And COGS Statement" || vmodel.Search.ReportTitle == "Customer Wise Sale" || vmodel.Search.ReportTitle == "Item Wise Profit Status" || vmodel.Search.ReportTitle == "Item Wise Sales Report")
                    //{
                    //    new ParamSaleStatement(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "DivWiseWarranty" || vmodel.Search.ReportTitle == "Line Wise Warranty" || vmodel.Search.ReportTitle == "Div Wise Sample" || vmodel.Search.ReportTitle == "Line Wise Sample" || vmodel.Search.ReportTitle == "Warrenty Details" || vmodel.Search.ReportTitle == "Sample Details" || vmodel.Search.ReportTitle == "Sample Details Group SubGroup Wise" || vmodel.Search.ReportTitle == "Sample Details Group SubGroup Wise Summary" || vmodel.Search.ReportTitle == "Warrenty Details Group SubGroup Wise" || vmodel.Search.ReportTitle == "Warrenty Details Group SubGroup Wise Summary" || vmodel.Search.ReportTitle == "Sample Ledger Customer Wise" || vmodel.Search.ReportTitle == "Sample Ledger Dealer Wise")
                    //{
                    //    new ParamSampleAndWarrenty(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Transaction")
                    //{
                    //    rd.SetParameterValue("UserName", User.Identity.Name);
                    //}
                    //if (vmodel.Search.ReportTitle == "AR Ageing Report" || vmodel.Search.ReportTitle == "AR Ageing Summary Report" || vmodel.Search.ReportTitle == "Stock Ageing Report" || vmodel.Search.ReportTitle == "Stock Ageing Report- Item Wise Summary" || vmodel.Search.ReportTitle == "Stock Ageing Report- Group Wise")
                    //{
                    //    new ParamAgeingReport(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Bill Wise Outstanding Report")
                    //{
                    //    new ParamAgeingReport(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Purchase Statement Summary")
                    //{
                    //    new ParamPurStatementSummary(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Purchase Statement Details")
                    //{
                    //    new ParamPurStatementDetails(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Purchase Statement Group SubGroup Wise")
                    //{
                    //    new ParamPurStatementGrpSubGrpWise(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Bill Wise Supplier Ledger")
                    //{
                    //    new ParamBillWiseSupLedger(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Accounts Payable By Supplier")
                    //{
                    //    new ParamAPBySupp(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Accounts Payable By Supplier With Vat Tax")
                    //{
                    //    new ParamAPBySupp(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Accounts Payable Ageing")
                    //{
                    //    new ParamAPAgeing(ref rd, vmodel.Search);
                    //}

                    //if (vmodel.Search.ReportTitle == "Accounts Payable Supplier Bill Ageing")
                    //{
                    //    new ParamAPAgeing(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Advanced Ageing Summary")
                    //{
                    //    new ParamAdvancedAgeing(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Advanced Ageing Details")
                    //{
                    //    new ParamAdvancedAgeing(ref rd, vmodel.Search);
                    //}
                    //if (vmodel.Search.ReportTitle == "Person Wise Collection Report")
                    //{
                    //    new ParamPersonWiseCol(ref rd, vmodel.Search);
                    //}

                   // CrystalReportViewer.ReportSource = rd;
                   // var a = Convert.ToDateTime(aMethod.DateTimeFormat(vmodel.FDate, false, 1)).ToString("dd-MMM-yyyy");
                    //if (!string.IsNullOrEmpty(vmodel.FDate))
                    //    rd.SetParameterValue("@FrDate", Convert.ToDateTime(vmodel.FDate));
                    //if (!string.IsNullOrEmpty(vmodel.TDate))
                    //    rd.SetParameterValue("@ToDate", Convert.ToDateTime(vmodel.TDate));
                    if (!string.IsNullOrEmpty(vmodel.FDate))
                        //rd.SetParameterValue("@FrDate", Convert.ToDateTime(vmodel.FDate));
                        rd.SetParameterValue("@FrDate", Convert.ToDateTime(utilityService.DateTimeFormat(vmodel.FDate, false, 1)).ToString("dd-MMM-yyyy"));
                    if (!string.IsNullOrEmpty(vmodel.TDate))
                        //rd.SetParameterValue("@ToDate", Convert.ToDateTime(vmodel.TDate));
                        rd.SetParameterValue("@ToDate", Convert.ToDateTime(utilityService.DateTimeFormat(vmodel.TDate, false, 1)).ToString("dd-MMM-yyyy"));
                    rd.SetParameterValue("@RegionId", Convert.ToInt32(vmodel.RegionId));
                   // break;
                    CrystalReportViewer.ReportSource = rd;


                }
                else
                {
                    Response.Write("Nothing found/ No report found");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        protected void CrystalReportViewer_Unload(object sender, EventArgs e)
        {

            rd.Close();
            rd.Dispose();
            GC.Collect();
        }
    }
}