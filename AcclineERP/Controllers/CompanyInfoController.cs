using App.Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AcclineERP.Models;

namespace AcclineERP.Controllers
{
    public class CompanyInfoController : Controller
    {
        private readonly ICompanyInfoAppService _companyinfoService;

        public CompanyInfoController(ISubsidiaryInfoAppService _subsidiaryInfoService,

         ICompanyInfoAppService _companyinfoService)
        {

            this._companyinfoService = _companyinfoService;
        }
        // GET: value
        public ActionResult CompanyInfo()
        {
            return View();
        }

        public ActionResult GetAllDataForCompanyInfo()
        {
            try
            {

                List<CompanyInfo> CompanyInfoList = new List<CompanyInfo>();

                CompanyInfoList = companyInfoDest();


                if (CompanyInfoList != null)
                {
                    return Json(new { data = CompanyInfoList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = new EmptyResult() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
        private List<CompanyInfo> companyInfoDest()
        {

            List<CompanyInfo> companyInfoList = new List<CompanyInfo>();

            var companyInfoListCheck = _companyinfoService.All().ToList();

            if (companyInfoListCheck.Count > 0)
            {

                foreach (var value in companyInfoListCheck)
                {
                    CompanyInfo CompanyinfoAdd = new CompanyInfo();
                    CompanyinfoAdd.CompanyID = 1;
                    CompanyinfoAdd.CompCode = value.CompCode;
                    CompanyinfoAdd.CompName = value.CompName;
                    CompanyinfoAdd.ShortName = value.ShortName;
                    CompanyinfoAdd.CompNameBN = value.CompNameBN;
                    CompanyinfoAdd.Addr1 = value.Addr1;

                    CompanyinfoAdd.Addr2 = value.Addr2;
                    CompanyinfoAdd.AddrBN = value.AddrBN;
                    CompanyinfoAdd.Tel = value.Tel;
                    CompanyinfoAdd.Fax = value.Fax;
                    CompanyinfoAdd.Email = value.Email;
                    CompanyinfoAdd.TINNo = value.TINNo;
                    CompanyinfoAdd.BIN = value.BIN;
                    CompanyinfoAdd.BINBN = value.BINBN;
                    CompanyinfoAdd.VATRegNo = value.VATRegNo;  

                    companyInfoList.Add(CompanyinfoAdd);
                }

                if (companyInfoList != null)
                {
                    companyInfoList = companyInfoList.OrderBy(x => x.CompName).ToList();
                }

                return companyInfoList;
            }
            else
            {
                return null;
            }

        }
        public ActionResult SaveCompanyInfo(CompanyInfo CompanyInfo)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("CompanyInfo_Insert"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    CompanyInfo IsExistComInfo = _companyinfoService.All().FirstOrDefault();
                    if (IsExistComInfo == null)
                    {
                        _companyinfoService.Add(CompanyInfo);
                        _companyinfoService.Save();                       
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        IsExistComInfo.CompCode = CompanyInfo.CompCode;
                        IsExistComInfo.CompName = CompanyInfo.CompName;
                        IsExistComInfo.ShortName = CompanyInfo.ShortName;
                        IsExistComInfo.CompNameBN = CompanyInfo.CompNameBN;
                        IsExistComInfo.Addr1 = CompanyInfo.Addr1;
                        IsExistComInfo.Addr2 = CompanyInfo.Addr2;
                        IsExistComInfo.AddrBN = CompanyInfo.AddrBN;
                        IsExistComInfo.Tel = CompanyInfo.Tel;
                        IsExistComInfo.Fax = CompanyInfo.Fax;
                        IsExistComInfo.Email = CompanyInfo.Email;
                        IsExistComInfo.TINNo = CompanyInfo.TINNo;
                        IsExistComInfo.BIN = CompanyInfo.BIN;
                        IsExistComInfo.BINBN = CompanyInfo.BINBN;
                        IsExistComInfo.VATRegNo = CompanyInfo.VATRegNo;
                        // AcBrList.Add(DynaCaplist);
                        _companyinfoService.Update(IsExistComInfo);
                        _companyinfoService.Save();
                        transaction.Complete();
                        return Json("1", JsonRequestBehavior.AllowGet);

                    }                    
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult CompanyInfoforEdit(int CompanyID)
        {
            using (var transaction = new TransactionScope())
            {

                try
                {

                    //RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    //if (!rUser.HasPermission("CompanyInfo_Edit"))
                    //{
                    //    return Json("X", JsonRequestBehavior.AllowGet);
                    //}


                    CompanyInfo CompanyInfoAdd = new CompanyInfo();
                    var CompanyInfocheck = _companyinfoService.All().ToList().FirstOrDefault(x => x.CompanyID == CompanyID);
                    if (CompanyInfocheck != null)
                    {
                        CompanyInfoAdd.CompCode = CompanyInfocheck.CompCode;
                        CompanyInfoAdd.CompName = CompanyInfocheck.CompName;
                        CompanyInfoAdd.ShortName = CompanyInfocheck.ShortName;
                        CompanyInfoAdd.CompNameBN = CompanyInfocheck.CompNameBN;
                        CompanyInfoAdd.Addr1 = CompanyInfocheck.Addr1;

                        CompanyInfoAdd.Addr2 = CompanyInfocheck.Addr2;
                        CompanyInfoAdd.AddrBN = CompanyInfocheck.AddrBN;
                        CompanyInfoAdd.Tel = CompanyInfocheck.Tel;
                        CompanyInfoAdd.Fax = CompanyInfocheck.Fax;
                        CompanyInfoAdd.Email = CompanyInfocheck.Email;
                        CompanyInfoAdd.TINNo = CompanyInfocheck.TINNo;
                        CompanyInfoAdd.BIN = CompanyInfocheck.BIN;
                        CompanyInfoAdd.BINBN = CompanyInfocheck.BINBN;
                        CompanyInfoAdd.VATRegNo = CompanyInfocheck.VATRegNo;

                    }
                    //  return Json(new { GgName = GgName, GroupTypeId = GroupTypeId }, JsonRequestBehavior.AllowGet);
                    return Json(CompanyInfoAdd, JsonRequestBehavior.AllowGet);

                    // return Json(JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

            }
         
        }
        public ActionResult UpdateCompanyInfo(CompanyInfo CompanyInfo, int CompanyID)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {

                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("CompanyInfo_Update"))
                    {
                        return Json("X", JsonRequestBehavior.AllowGet);
                    }
                    // IssRecSrcDest GgName = new IssRecSrcDest();
                    var CompanyInfocheck = _companyinfoService.All().ToList().FirstOrDefault(x => x.CompanyID == CompanyID);
                    if (CompanyInfocheck != null)
                    {
                        //Misclns.CopyPropertiesTo(OpnBal, UpOpnBal);
                        CompanyInfocheck.CompanyID = _companyinfoService.All().Select(x => x.CompanyID).LastOrDefault();

                        CompanyInfocheck.CompCode = CompanyInfo.CompCode;
                        CompanyInfocheck.CompName = CompanyInfo.CompName;
                        CompanyInfocheck.ShortName = CompanyInfo.ShortName;
                        CompanyInfocheck.CompNameBN = CompanyInfo.CompNameBN;
                        CompanyInfocheck.Addr1 = CompanyInfo.Addr1;

                        CompanyInfocheck.Addr2 = CompanyInfo.Addr2;
                        CompanyInfocheck.AddrBN = CompanyInfo.AddrBN;
                        CompanyInfocheck.Tel = CompanyInfo.Tel;
                        CompanyInfocheck.Fax = CompanyInfo.Fax;
                        CompanyInfocheck.Email = CompanyInfo.Email;
                        CompanyInfocheck.TINNo = CompanyInfo.TINNo;
                        CompanyInfocheck.BIN = CompanyInfo.BIN;
                        CompanyInfocheck.BINBN = CompanyInfo.BINBN;
                        CompanyInfocheck.VATRegNo = CompanyInfo.VATRegNo;

                        _companyinfoService.Update(CompanyInfocheck);
                        _companyinfoService.Save();

                        //transaction.Complete();
                    }
                    var ecode = 1;
                    return Json(ecode, JsonRequestBehavior.AllowGet);
                }
                //return Json(JsonRequestBehavior.AllowGet);

                catch (Exception ex)
                {
                    //transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }
        

    }
}