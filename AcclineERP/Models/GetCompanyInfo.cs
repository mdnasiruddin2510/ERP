using App.Domain;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using AcclineSysSet;

namespace AcclineERP.Models
{
    public static class GetCompanyInfo
    {
        static AcclineERPContext db = new AcclineERPContext();
        public static  App.Domain.CompanyInfo GetComInfo()
        {
            App.Domain.CompanyInfo CompanyInfo = db.CompanyInfo.FirstOrDefault();
            AcclineSysSet.CompanyInfo cinfo = new AcclineSysSet.CompanyInfo();
            CompanyInfo.CompName = cinfo.CName;
            CompanyInfo.Addr1 = cinfo.CAdd;
            return CompanyInfo;
        }

        public static string GetReportCriteria(string ProjName, string BrName, string UnitCode, string DeptCode, string FinYear)
        {
            string projNameOnly = "";
            string brName = "";
            string FY = FinYear;
            string proj = "";
            string brNa = "";
            string Yr = " Year: ";
            string uName = "";
            string uLavel = "";
            string dName = "";
            string dLavel = "";
            var dynaCap = db.DynaCap.FirstOrDefault();
            var sysSet = db.SysSet.FirstOrDefault();
            if (sysSet.HasBranch == true)
            {
                if (BrName == null)
                {
                    brName = " ";
                    brNa = " ";
                }
                else if (BrName == "" || BrName == " ")
                {
                    brName = "All ,";
                    brNa = dynaCap.Branch + " : ";
                }
                else
                {
                    brName = db.Branch.Where(x => x.BranchCode == BrName).FirstOrDefault().BranchName + ", ";
                    brNa = dynaCap.Branch + " : ";
                }
            }
            //if (sysSet.HasProject == true)
            //{
            //    if (ProjName == null)
            //    {
            //        projNameOnly = " ";
            //        proj = " ";
            //    }
            //    else if (ProjName == "" || ProjName == " ")
            //    {
            //        projNameOnly = "All ,";
            //        proj = dynaCap.Proj + " : ";
            //    }
            //    else
            //    {
            //        projNameOnly = db.ProjInfo.FirstOrDefault(x => x.ProjCode == ProjName).ProjName + " ,";
            //        proj = dynaCap.Proj + " : ";
            //    }
            //}
            if (FinYear == null)
            {
                FY = " ";
                Yr = " ";
            }
            else if (FinYear == "")
            {
                FY = "All";
                Yr = " Year : ";
            }
            //if (sysSet.HasUnit == true)
            //{
            //    if (UnitCode == null)
            //    {
            //        uName = " ";
            //        uLavel = " ";
            //    }
            //    else if (UnitCode == "" || UnitCode == " ")
            //    {
            //        uName = "All ,";
            //        uLavel = dynaCap.Unit + " : ";
            //    }
            //    else
            //    {
            //        uName = db.UnitInfo.FirstOrDefault(s => s.UnitCode == UnitCode).UnitName + " ,";
            //        uLavel = dynaCap.Unit + " : ";
            //    }

            //}
            if (sysSet.HasDept == true)
            {
                if (DeptCode == null)
                {
                    dName = " ";
                    dLavel = " ";
                }
                else if (DeptCode == "" || DeptCode == " ")
                {
                    dName = "All ,";
                    dLavel = dynaCap.Dept + " : ";
                }
                else
                {
                    dName = db.Department.FirstOrDefault(s => s.DeptCode == DeptCode).DeptDesc + " ,";
                    dLavel = dynaCap.Dept + " : ";
                }

            }

            //string Criteria = proj + projNameOnly + brNa + brName + uLavel + uName + dLavel + dName + Yr + FY;
            string Criteria = proj + projNameOnly + brNa + brName + uLavel + uName + dLavel + dName;
            return Criteria;
        }

        public static string ValidateFinYearDateRange(string fDate, string tDate, string FinYear)
        {
            StringBuilder builder = new StringBuilder();
            DateTime YMf = DateTime.ParseExact(Convert.ToDateTime(fDate).ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime YMt = DateTime.ParseExact(Convert.ToDateTime(tDate).ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            bool HasWConv = false;
            var FYDD = db.FYDD.FirstOrDefault(x => x.FinYear == FinYear);

            var SDate = FYDD.FYDF;
            var EDate = FYDD.FYDT;

            if (!((YMf >= SDate && YMf <= EDate) && (YMt >= SDate && YMt <= EDate)))
            {
                HasWConv = true;
            }
            builder.Append("\n");
            builder.Append("These Dates are Out of FinYear Date Range");
            builder.Append("\n");

            if (HasWConv == false)
            {
                builder.Length = 0;
            }

            return builder.ToString();
        }

        public static string GetVTypeNameByTVoucher(string VchrNo)
        {
            var vtypeName = (from mi in db.TVchrMain
                             join md in db.VoucherType on mi.VType equals md.VInitial
                             where mi.VchrNo == VchrNo
                             select new
                             {
                                 TypeDesc = md.TypeDesc

                             }).FirstOrDefault();

            return vtypeName.TypeDesc;
        }
        public static string GetVTypeNameByPVoucher(string VchrNo)
        {
            var vtypeName = default(dynamic);
            vtypeName = (from mi in db.PVchrMain
                         join md in db.VoucherType on mi.VType equals md.VInitial
                         where mi.VchrNo == VchrNo
                         select new
                         {
                             TypeDesc = md.TypeDesc

                         }).FirstOrDefault();
            if (vtypeName == null)
            {
                vtypeName = (from mi in db.VchrMain
                             join md in db.VoucherType on mi.VType equals md.VInitial
                             where mi.VchrNo == VchrNo
                             select new
                             {
                                 TypeDesc = md.TypeDesc

                             }).FirstOrDefault();
            }
            return vtypeName.TypeDesc;
        }

        //public static bool? getByPassProc()
        //{
        //    var sysSet = db.SysSet.FirstOrDefault();
        //    return sysSet.ByPassProc;
        //}

        public static string JvWaitingPostSQL(string pageType)
        {
            string sql = string.Format("select pvd.PVchrDetailId,pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName,(select SubName "
                + "from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, "
                + "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate from PVchrMain as pvm "
                + "join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear join JTrGrp as jt on pvm.VType = jt.VType where jt.TranGroup = '" + pageType + "'"
                + "group by pvd.PVchrDetailId, pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac order by pvd.PVchrDetailId");
            return sql;

            //string sql = string.Format("select pvm.VchrNo,(select AcName from NewChart where Accode = pvd.Accode)as AcName," +
            //                           "(select SubName from SubsidiaryInfo where SubCode = pvd.sub_Ac) as SubSidiary, " +
            //                           "pvd.Narration, pvd.Accode, pvd.CrAmount, pvd.DrAmount, pvm.Posted,pvm.VDate, pme.vchrSrc +  pme.VchrSrcRef  as glSource " +
            //                           "from PVchrMain as pvm inner join PVchrDetail as pvd on pvm.VchrNo = pvd.VchrNo and pvm.FinYear = pvd.FinYear" +
            //                           "inner join JTrGrp as jt on pvm.VType = jt.VType inner join PVchrMainExt as pme on pvm.VchrNo = pme.vchrNo" +
            //                           "where jt.TranGroup ='" + pageType + "' and pvm.VDate = '" + date.ToString("dd/MM/yyyy") + "'  group by pvm.VchrNo, pvm.BranchCode, pvm.Posted, pvm.VDate,pvd.Narration,pvd.Accode, " +
            //                           "pvd.CrAmount, pvd.DrAmount, pvd.sub_Ac, pme.vchrSrc, pme.VchrSrcRef");


        }
    }
}