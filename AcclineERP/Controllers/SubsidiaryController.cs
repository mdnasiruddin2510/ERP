using App.Domain;
using App.Domain.ViewModel;
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
    public class SubsidiaryController : Controller
    {
        private readonly ISubsidiaryTypeAppService _subsidiaryTypeAppService;
        private readonly ISubsidiaryInfoAppService _subsidiaryInfoAppService;
        private readonly ITransactionLogAppService _transactionLogService;
        private readonly ISubsidiaryExtAppService _subsidiaryExtSevevice;
        private readonly ISubOpenBalAppService _SubOpenBalService;
        public SubsidiaryController(ISubsidiaryTypeAppService _subsidiaryTypeAppService,
            ITransactionLogAppService _transactionLogService,
            ISubsidiaryInfoAppService _subsidiaryInfoAppService, ITransactionLogAppService _transactionLogServic,
            ISubsidiaryExtAppService _subsidiaryExtSevevice, ISubOpenBalAppService _SubOpenBalService)
        {
            this._subsidiaryTypeAppService = _subsidiaryTypeAppService;
            this._subsidiaryInfoAppService = _subsidiaryInfoAppService;
            this._transactionLogService = _transactionLogService;
            this._subsidiaryExtSevevice = _subsidiaryExtSevevice;
            this._SubOpenBalService = _SubOpenBalService;
        }
        public ActionResult Create(string errMsg)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.SubCode = GenerateSubsidiaryCode();
                ViewBag.Items = GetSubInfo();
                ViewBag.SubType = new SelectList(_subsidiaryTypeAppService.All().ToList(), "TypeCode", "SubType");
                ViewBag.CountryCode = LoadDropDown.LoadCountryDDL();
                ViewBag.SubGrp = LoadDropDown.LoadSubGrpDDL();  
                ViewBag.errMsg = errMsg;
                return View();
            }
            else
            {
                return RedirectToAction("SecUserLogin", "SecUserLogin");
            }
        }

        public List<SubsidiaryInfo> GetSubInfo()
        {
            List<SubsidiaryType> subTypeInfo = _subsidiaryTypeAppService.All().ToList();
            List<SubsidiaryInfo> subInfo = _subsidiaryInfoAppService.All().ToList();
            var subsidiaryInfo = from st in subTypeInfo
                                 join si in subInfo
                                 on st.TypeCode equals si.SubType
                                 join sie in _subsidiaryExtSevevice.All().ToList()
                                 on si.SubCode equals sie.SubCode
                                 select new
                                 {
                                     SubCode = si.SubCode,
                                     SubName = si.SubName,
                                     SubType = st.SubType,
                                     SubCategory = sie.SubCategory 
                                 };
            List<SubsidiaryInfo> sInfos = new List<SubsidiaryInfo>();
            foreach (var item in subsidiaryInfo)
            {
                SubsidiaryInfo sInfo = new SubsidiaryInfo();
                sInfo.SubCode = item.SubCode;
                sInfo.SubName = item.SubName;
                sInfo.SubType = item.SubType;
                sInfo.SubCategory = item.SubCategory;
                sInfos.Add(sInfo);
            }
            return sInfos;
        }


        [HttpPost]
        public ActionResult SaveSubsidary(SubsidiaryInfo subsidiaryInfo, SubsidiaryInfo_Ext SubsisiaryExt, List<SubsidiaryInfo_Ext> SubsisiaryExtList)
        {
            try
            {
                RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                if (!rUser.HasPermission("Subsidiary_Insert"))
                {
                    return Json("X", JsonRequestBehavior.AllowGet);
                }

                if (SubsisiaryExtList != null)
                {
                    foreach (var item in SubsisiaryExtList)
                    {
                        item.SubCode = SubsisiaryExt.SubCode;
                        item.SubCategory = SubsisiaryExt.SubCategory;
                        item.Tel = SubsisiaryExt.Tel;
                        item.SubEmail = SubsisiaryExt.SubEmail;
                        item.Fax = SubsisiaryExt.Fax;
                        item.OpenBal = SubsisiaryExt.OpenBal;
                        item.OpenDate = SubsisiaryExt.OpenDate;
                        item.TIN = SubsisiaryExt.TIN;
                        item.BIN = SubsisiaryExt.BIN;
                        item.CountryCode = SubsisiaryExt.CountryCode;
                        item.VATRegNo = SubsisiaryExt.VATRegNo;
                        _subsidiaryExtSevevice.Add(item);
                    }
                    _subsidiaryExtSevevice.Save();
                }
                else if (SubsisiaryExt != null)
                {
                    SubsidiaryInfo_Ext subsidiaryExt = new SubsidiaryInfo_Ext();
                    subsidiaryExt.SubTypeExtID = SubsisiaryExt.SubTypeExtID;
                    subsidiaryExt.SubCode = SubsisiaryExt.SubCode;
                    subsidiaryExt.SubCategory = SubsisiaryExt.SubCategory;
                    subsidiaryExt.Tel = SubsisiaryExt.Tel;
                    subsidiaryExt.SubEmail = SubsisiaryExt.SubEmail;
                    subsidiaryExt.Fax = SubsisiaryExt.Fax;
                    subsidiaryExt.OpenBal = SubsisiaryExt.OpenBal;
                    subsidiaryExt.OpenDate = SubsisiaryExt.OpenDate;
                    subsidiaryExt.TIN = SubsisiaryExt.TIN;
                    subsidiaryExt.BIN = SubsisiaryExt.BIN;
                    subsidiaryExt.CountryCode = SubsisiaryExt.CountryCode;
                    subsidiaryExt.VATRegNo = SubsisiaryExt.VATRegNo;
                    _subsidiaryExtSevevice.Add(subsidiaryExt);
                    _subsidiaryExtSevevice.Save();
                }
                SubOpenBal subOpnBal = new SubOpenBal();
                subOpnBal.SubCode = subsidiaryInfo.SubCode;
                subOpnBal.FinYear = Session["FinYear"].ToString();
                subOpnBal.OpenBal = Convert.ToDouble(SubsisiaryExt.OpenBal);
                subOpnBal.BranchCode = Session["BranchCode"].ToString();
                subOpnBal.ProjCode = Session["ProjCode"].ToString();

                _SubOpenBalService.Add(subOpnBal);
                _subsidiaryInfoAppService.Add(subsidiaryInfo);
                _subsidiaryInfoAppService.Save();
                _SubOpenBalService.Save();
                TransactionLogService.SaveTransactionLog(_transactionLogService, "Subsidiary", "Save", subsidiaryInfo.SubCode, Session["UserName"].ToString());
                return Json("1", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpdateSubsidiary(SubsidiaryInfo subsidiaryInfo, SubsidiaryInfo_Ext SubExt, List<SubsidiaryInfo_Ext> SubsisiaryExtList)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    RBACUser rUser = new RBACUser(Session["UserName"].ToString());
                    if (!rUser.HasPermission("Subsidiary_Update"))
                    {
                        return Json("U", JsonRequestBehavior.AllowGet);
                    }
                    if (subsidiaryInfo.SubGrp == null)
                    {
                        subsidiaryInfo.SubGrp = "";
                    }
                    _subsidiaryExtSevevice.All().ToList().Where(y => y.SubCode.Trim() == SubExt.SubCode).ToList().ForEach(x => _subsidiaryExtSevevice.Delete(x));
                    if (SubsisiaryExtList != null)
                    {
                        foreach (var item in SubsisiaryExtList)
                        {
                            item.SubTypeExtID = SubExt.SubTypeExtID;
                            item.SubCode = SubExt.SubCode;
                            item.SubCategory = SubExt.SubCategory;
                            item.Tel = SubExt.Tel;
                            item.SubEmail = SubExt.SubEmail;
                            item.Fax = SubExt.Fax;
                            item.OpenBal = SubExt.OpenBal;
                            item.OpenDate = SubExt.OpenDate;
                            item.TIN = SubExt.TIN;
                            item.BIN = SubExt.BIN;
                            item.VATRegNo = SubExt.VATRegNo;
                            item.CountryCode = SubExt.CountryCode;
                            _subsidiaryExtSevevice.Add(item);
                        }
                        _subsidiaryExtSevevice.Save();
                    }
                    else if (SubExt != null)
                    {
                        SubsidiaryInfo_Ext subsidiaryExt = new SubsidiaryInfo_Ext();
                        subsidiaryExt.SubTypeExtID = SubExt.SubTypeExtID;
                        subsidiaryExt.SubCode = SubExt.SubCode;
                        subsidiaryExt.SubCategory = SubExt.SubCategory;
                        subsidiaryExt.Tel = SubExt.Tel;
                        subsidiaryExt.SubEmail = SubExt.SubEmail;
                        subsidiaryExt.Fax = SubExt.Fax;
                        subsidiaryExt.OpenBal = SubExt.OpenBal;
                        subsidiaryExt.OpenDate = SubExt.OpenDate;
                        subsidiaryExt.TIN = SubExt.TIN;
                        subsidiaryExt.BIN = SubExt.BIN;
                        subsidiaryExt.CountryCode = SubExt.CountryCode;
                        subsidiaryExt.VATRegNo = SubExt.VATRegNo;
                        _subsidiaryExtSevevice.Add(subsidiaryExt);
                        _subsidiaryExtSevevice.Save();
                    }

                    SubOpenBal subOpnBal = _SubOpenBalService.All().Where(s => s.SubCode == subsidiaryInfo.SubCode).FirstOrDefault();
                    if (subOpnBal != null)
                    {
                        subOpnBal.OpenBal = Convert.ToDouble(SubExt.OpenBal);
                        _SubOpenBalService.Update(subOpnBal);
                        _SubOpenBalService.Save();
                    }
                    else
                    {
                        SubOpenBal subOBal = new SubOpenBal();
                        subOBal.SubCode = subsidiaryInfo.SubCode;
                        subOBal.FinYear = Session["FinYear"].ToString();
                        subOBal.OpenBal = Convert.ToDouble(SubExt.OpenBal);
                        subOBal.BranchCode = Session["BranchCode"].ToString();
                        subOBal.ProjCode = Session["ProjCode"].ToString();

                        _SubOpenBalService.Add(subOBal);
                        _SubOpenBalService.Save();
                    }

                    _subsidiaryInfoAppService.Update(subsidiaryInfo);
                    _subsidiaryInfoAppService.Save();
                    TransactionLogService.SaveTransactionLog(_transactionLogService, "Subsidiary", "Update", subsidiaryInfo.SubCode, Session["UserName"].ToString());
                    transaction.Complete();
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult GetAllBySubCode(string subCode)
        {
            var subsidiaryInfo = _subsidiaryInfoAppService.All().FirstOrDefault(x => x.SubCode == subCode.Trim());
            var SubExt = _subsidiaryExtSevevice.All().Where(s => s.SubCode.Trim() == subCode.Trim()).ToList();
            return Json(new { data = subsidiaryInfo, SubExt = SubExt }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSubsidiaryById(string SubCode)
        {
            var sub = _subsidiaryInfoAppService.All().ToList().FirstOrDefault(x => x.SubCode == SubCode);
            var SubExt = _subsidiaryExtSevevice.All().FirstOrDefault(s => s.SubCode.Trim() == SubCode.Trim());
            SubVModel vmodel = new SubVModel();
            vmodel.SubCode = sub.SubCode;
            vmodel.SubName = sub.SubName;
            vmodel.SubType = sub.SubType;
            vmodel.SubGrp = sub.SubGrp;
            if (SubExt != null)
            {
                vmodel.OpenBal = (SubExt.OpenBal == null) ? 0 : (decimal)SubExt.OpenBal;
                vmodel.OpenDate = (SubExt.OpenDate == null) ? null : SubExt.OpenDate;
                vmodel.SubAddress = (SubExt.SubAddress == null) ? "" : SubExt.SubAddress;
                vmodel.Tel = (SubExt.Tel == null) ? "" : SubExt.Tel;
                vmodel.SubEmail = (SubExt.SubEmail == null) ? "" : SubExt.SubEmail;
                vmodel.Fax = (SubExt.Fax == null) ? "" : SubExt.Fax;
                vmodel.SubCategory = (SubExt.SubCategory == null) ? "" : SubExt.SubCategory;
                vmodel.VATRegNo = (SubExt.VATRegNo == null) ? "" : SubExt.VATRegNo;
                vmodel.TIN = (SubExt.TIN == null) ? "" : SubExt.TIN;
                vmodel.BIN = (SubExt.BIN == null) ? "" : SubExt.BIN;
                vmodel.CountryCode = (SubExt.CountryCode == null) ? "" : SubExt.CountryCode;
                vmodel.SubTypeExtID = SubExt.SubTypeExtID;
            }

            return Json(vmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubsiiaryCode()
        {
            return Json(GenerateSubsidiaryCode(), JsonRequestBehavior.AllowGet);
        }
        public string GenerateSubsidiaryCode()
        {
            string subCode = "";
            var item = _subsidiaryInfoAppService.All().ToList().LastOrDefault();
            if (item != null)
            {
                subCode = (Convert.ToInt32(item.SubCode) + 1).ToString().PadLeft(5, '0');
            }
            else
            {
                subCode = "00001";
            }
            return subCode;
        }

        public ActionResult GetAllSubsidiary()
        {
            RBACUser rUser = new RBACUser(Session["UserName"].ToString());
            if (!rUser.HasPermission("Subsidiary_Preview"))
            {
                string errMsg = "P";
                return RedirectToAction("Create", "Subsidiary", new { errMsg });
            }

            string sql = string.Format(" EXEC AllSubsidiary ");
            List<SubsidiaryInfo> subsidiary = _subsidiaryInfoAppService.SqlQueary(sql).ToList();
            ViewBag.PrintDate = DateTime.Now.ToShortDateString();
            return new Rotativa.ViewAsPdf("GetAllSubsidiary", subsidiary) { CustomSwitches = "--footer-left \"Reporting Date: " + DateTime.Now.ToString("dd-MM-yyyy") + "\" " + "--footer-right \"Page: [page] of [toPage]\"        --footer-font-size \"9\" --footer-spacing 5  --footer-font-name \"calibri light\"" };
        }

    }
}
