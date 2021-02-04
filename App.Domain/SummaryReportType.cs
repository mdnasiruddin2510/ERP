using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class SummaryReportType
    {
        [Key]
        public string ReportTypeCode { set; get; }
        public string ReportTypeName { set; get; }
    }
}
