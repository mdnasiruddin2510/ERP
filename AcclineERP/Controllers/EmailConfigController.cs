using App.Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoIpaWeb.Controllers
{
   
    public class EmailConfigController : Controller
    {
        private readonly IEmailConfigAppService _emailService;
        public EmailConfigController(IEmailConfigAppService _emailService)
        {
            this._emailService = _emailService;
        }
        public ActionResult GetEmailPartialView()
        {
            return PartialView("_EmailConfigPartial",new EmailConfig());
        }
        public ActionResult CreateEmailConfig()
        {
           return View(_emailService.All().ToList());
        }
        public ActionResult SaveEmailConfig(EmailConfig emailConfig)
        {
            EmailConfig model = new EmailConfig();
            model.TemplateHeader = emailConfig.TemplateHeader.ToString();
            model.TempFixedTopDes = emailConfig.TempFixedTopDes.ToString();
            model.ThanksGiving = emailConfig.ThanksGiving.ToString();
            model.FooterThanks = emailConfig.FooterThanks.ToString();
            model.Department = emailConfig.Department.ToString();
            model.FooterConcern = emailConfig.FooterConcern.ToString();
            model.FooterPhone = emailConfig.FooterPhone.ToString();
            model.FooterFax = emailConfig.FooterFax.ToString();
            model.FooterEmail = emailConfig.FooterEmail.ToString();
            model.FooterWeb = emailConfig.FooterWeb.ToString();
            model.IsActive =(bool) emailConfig.IsActive;
            _emailService.Add(model);
            _emailService.Save();

            return Json("",JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditEmailConfig(int id)
        {
            EmailConfig model = _emailService.All().FirstOrDefault(e => e.Id == id);
            if (model != null)
            {
                return PartialView("_EmailConfigPartial", model);
            }
            else 
            {
                return Json("Nothing Found!!!", JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult UpdateEmailConfig(EmailConfig emailConfig)
        {
            EmailConfig model = _emailService.All().FirstOrDefault(e => e.Id == emailConfig.Id);
            model.TemplateHeader = emailConfig.TemplateHeader.ToString();
            model.TempFixedTopDes = emailConfig.TempFixedTopDes.ToString();
            model.ThanksGiving = emailConfig.ThanksGiving.ToString();
            model.FooterThanks = emailConfig.FooterThanks.ToString();
            model.Department = emailConfig.Department.ToString();
            model.FooterConcern = emailConfig.FooterConcern.ToString();
            model.FooterPhone = emailConfig.FooterPhone.ToString();
            model.FooterFax = emailConfig.FooterFax.ToString();
            model.FooterEmail = emailConfig.FooterEmail.ToString();
            model.FooterWeb = emailConfig.FooterWeb.ToString();
            model.IsActive = (bool)emailConfig.IsActive;
            _emailService.Update(model);
            _emailService.Save();

            return Json("Updated Successfully", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmailConfig(int id) 
        {
            EmailConfig model = _emailService.All().FirstOrDefault(e => e.Id == id);
            if (model != null)
            {
                _emailService.Delete(model);
                _emailService.Save();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }
        }
        

    }
}
