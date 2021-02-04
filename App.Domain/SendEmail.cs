using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public static class SendEmail
    {
      
        public static bool SendByGmail(string subject, string body, string toEmail)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            MailMessage mailMessage = new MailMessage();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("postofficeban@gmail.com", "postofficeban");
            mailMessage.From = new MailAddress("postofficeban@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml=true;
            mailMessage.Body= body;
            client.Send(mailMessage);
            return true;
        }
        
    }
}
