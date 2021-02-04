using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AcclineERPApi.Models;

namespace AcclineERPApi.Controllers
{
    public class CustomerWiseSaleRptController : ApiController
    {
        private ASPLEntities db = new ASPLEntities();

        // GET: api/CustomerWiseSaleRpt/GetCustomerWiseSaleRpt

        [Authorize]
        [ResponseType(typeof(CustWiseSummSale_Result))]
        [HttpGet]
        
        public List<CustWiseSummSale_Result> GetCustomerWiseSaleRpt(string finYear, string locCode, DateTime fdate, DateTime tdate)
        {
            List<CustWiseSummSale_Result> res = new List<CustWiseSummSale_Result>();
            using (var dbContext = new ASPLEntities())
            {
                foreach (var item in dbContext.CustWiseSummSale(finYear, locCode, fdate, tdate))
                {
                    res.Add(item);
                }
            }
            return res;
        }

    }
}
