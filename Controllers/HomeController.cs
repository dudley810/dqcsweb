using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dqcsweb.Models;
using System.Net.Http;

namespace dqcsweb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            var client = new HttpClient();
            string bloghtml = client.GetStringAsync("http://blog.dqcs.com").Result;
            bloghtml = bloghtml.Replace("Awesome Inc. theme. Powered by <a href='https://www.blogger.com' target='_blank'>Blogger</a>.", "");
            ViewBag.bloghtml = bloghtml;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
