using AcclineERPApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AcclineERPApi.Models
{
    public class SaleViewModel
    {        
        public SalesMain SalesMain { get; set; }
        public IEnumerable<SalesDetail> SalesDetail { get; set; }
        public IssueMain IssueMain { get; set; }
        public IEnumerable<IssueDetail> IssueDetail { get; set; }
        public IEnumerable<CurrentStock> CurrentStock { get; set; }
        public IEnumerable<CostLedger> CostLedger { get; set; }
    }
}