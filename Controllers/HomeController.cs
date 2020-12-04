using dqcsweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace dqcsweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
