using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcclineERP.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Requisition() 
        {
            ViewBag.Location = LoadEmpDlList();
            ViewBag.User = LoadEmpDlList();
            ViewBag.ItemType = LoadEmpDlList();
            ViewBag.Group = LoadEmpDlList();
            return View();
        }

        public static SelectList LoadEmpDlList()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("", "---- Select ----");
            return new SelectList(items, "Key", "Value");
        }
    }
}