using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interface.VModel
{
   public interface IRegionWiseReport
    {
       //DateTime SearchDate { get; set; }
       //DateTime Searchmonth { get; set; }
       //string Region { get; set; }
       //string EngineerName { get; set; }
       //int NewsiteQty { get; set; }
       //int NewSiteSale { get; set; }
       //int ConversionSiteQty { get; set; }
       //int ConversionSaleQty { get; set; }
       //int RepeatSaleSiteQty { get; set; }
       //int RepeatSaleQty { get; set; }
       string CusCode { get; set; }
       string CustomerName { get; set; }
       int NewsiteQty { get; set; }
       int NSSaleQty { get; set; }
       int BrandConQty { get; set; }
       int BrandSaleQty { get; set; }
       int RepSiteQty { get; set; }
       int RepSaleQty { get; set; }
       string RegCode { get; set; }
       string RegName { get; set; }
       DateTime SDate { get; set; }
    }
}
