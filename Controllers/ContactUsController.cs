using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dqcsweb.Models;

namespace dqcsweb.Controllers
{
    public class ContactUsController : Controller
    {
        //
        // GET: /ContactUs/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Send email to the Hylander Team
        /// </summary>
        /// <param name="cum"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SendEmail")]
        public async Task<ActionResult> SendEmail(ContactUsModel cum)
        {
            try
            {
                string Body = string.Empty;
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress(cum.Email, cum.Name));

                var recipients = new List<EmailAddress>
                {
                    new EmailAddress("webmaster@dqcs.com", "Ken Dudley"),
                };

                msg.AddTos(recipients);

                msg.SetSubject("Web-email: " + cum.Subject);
                
                StringBuilder sb = new StringBuilder("<html><body><table border='0'  cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td width='8%'><b>Phone:</b></td><td width='92%'>");
                sb.Append(cum.Phone);
                sb.Append("</td></tr></table><p>");
                sb.Append(cum.Message);
                sb.Append("</p></body></html>");
                msg.AddContent(MimeType.Html, sb.ToString());

                var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_APIKEY"));
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ViewBag.SentMessage = "Your email was sent, we will answer you shortly!!";
                }
                else
                {
                    ViewBag.SentMessage = "There was and error Sending the email please email webmaster@dqcs.com ";
                }
            }
            catch (Exception ex)
            {
                ViewBag.SentMessage = "There was and error Sending the email please email webmaster@dqcs.com " + ex.Message;
            }
            return View("index", cum);
        }
    }
}