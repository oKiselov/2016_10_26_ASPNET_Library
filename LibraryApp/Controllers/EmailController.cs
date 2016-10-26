using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LibraryApp.Models;

namespace LibraryApp.Controllers
{
    public class EmailController : Controller
    {
        /// <summary>
        /// Method which send messages to all users accounts
        /// </summary>
        /// <param name="strUserMail"></param> - User's mail 
        /// <param name="strMessage"></param> - Message Text 
        /// <param name="strRedirectAction"></param> - Action for redirection after sending 
        /// <param name="strRedirectController"></param>
        /// <returns></returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public async Task<ActionResult> Contact(string strUserMail, string strMessage, string strRedirectAction, string strRedirectController)
        {
            EmailSender model = new EmailSender();
            model.FromEmail = strUserMail;
            model.Message = strMessage; 
            if (ModelState.IsValid)
            {
                string body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(strUserMail)); 
                message.Subject = "Notification";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction(strRedirectAction, strRedirectController);
                }
            }
            // exception 
            return RedirectToAction("Index", "Entrance");
        }
    }
}
