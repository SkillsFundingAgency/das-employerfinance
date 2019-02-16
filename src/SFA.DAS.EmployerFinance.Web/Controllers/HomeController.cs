using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
    }
}