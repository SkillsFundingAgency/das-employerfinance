using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Web.Models;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmployerUrls _employerUrls;

        public HomeController(IHostingEnvironment hostingEnvironment, IEmployerUrls employerUrls)
        {
            _hostingEnvironment = hostingEnvironment;
            _employerUrls = employerUrls;
        }
        
        public IActionResult Index()
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return RedirectToAction("Index", "Transactions", new { accountHashedId = "JRML7V" });
            }

            return Redirect(_employerUrls.Homepage());
        }

        [AllowAnonymous]
        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}