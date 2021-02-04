using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Domain.Interface.VModel;
using App.Domain.ViewModel;

namespace App.Domain.Interface.VModel
{
  public interface ICommonSearchModel
    {
      
      
      SelectList Region { get; set; }
      SelectList SupCode { get; set; }
      IComSearchName SearchName { get; set; }
      string FDate { get; set; }
      string TDate { get; set; }
      string ReportTitle { get; set; }
      string RPTFileName { get; set; }
      string RegionId { get; set; }
    

     // List<RegionWiseReport> RegionWiseReportList { get; set; }
    }
}
