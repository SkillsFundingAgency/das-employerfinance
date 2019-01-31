using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Web.Extensions;
using SFA.DAS.EmployerFinance.Web.Models;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompositeViewEngine _viewEngine;

        public HomeController(ILogger logger, ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            _viewEngine = viewEngine;
        }
        
        public IActionResult Index()
        {
            _logger.LogDebug("Index page has been viewed");
            
            return View();
        }

        [AllowAnonymous]
        [Route("error/{code}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {
            return View(this.ViewExists(_viewEngine, $"Errors/{code}") ? $"Errors/{code}" : "Errors/Error");
        }
    }
}