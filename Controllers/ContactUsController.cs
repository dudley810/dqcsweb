using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dqcsweb.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace dqcsweb.Controllers
{
   public class ContactUsController : Controller
    {
        private readonly WebSettings appSet;

        public ContactUsController(IOptions<WebSettings> appSettings)
        {
            appSet = appSettings.Value;
        }

        //
        // GET: /ContactUs/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Send email to the Hylander Team
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SendEmail")]
        public async Task<ActionResult> SendEmail(ContactUsModel c)
        {
            try
            {
                c.recaptcharesponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(c.recaptcharesponse))
                {
                    ViewBag.SentMessage = "Please tell me if you are a robot or not, thanks.";
                    return View("index", c);
                }

                using (var httpc = new HttpClient())
                {
                    httpc.BaseAddress = new System.Uri(appSet.GOOGLEBASEURL);
                    httpc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string requrl = appSet.RECAPTCHAAPI + "?secret=" + Environment.GetEnvironmentVariable("GOOGLE_RECAPTCHA") + "&response=" + c.recaptcharesponse;
                    HttpResponseMessage resp = httpc.PostAsync(requrl, new StringContent("")).Result;
                    if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ReCaptcha r = JsonConvert.DeserializeObject<ReCaptcha>(resp.Content.ReadAsStringAsync().Result);
                        if (!r.success)
                        {
                            ViewBag.SentMessage = "google failed: " + r.errorcodes;
                            return View("index", c);
                        }
                    }
                    else
                    {
                        ViewBag.SentMessage = "You are a robot or google failed please try again.";
                        return View("index", c);
                    }
                }

                string Body = string.Empty;
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress(c.Email, c.Name));

                var recipients = new List<EmailAddress>
                {
                    new EmailAddress("webmaster@dqcs.com", "Ken Dudley"),
                };

                msg.AddTos(recipients);

                msg.SetSubject("Web-email: " + c.Subject);

                StringBuilder sb = new StringBuilder("<html><body><table border='0'  cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td width='8%'><b>Phone:</b></td><td width='92%'>");
                sb.Append(c.Phone);
                sb.Append("</td></tr></table><p>");
                sb.Append(c.Message);
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
            return View("index", c);
        }
    }
}