using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Web.Models;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    //todo: minification (https://docs.microsoft.com/en-us/aspnet/core/client-side/using-gulp?view=aspnetcore-2.2)
    //todo: typescript
    //todo: gov.uk design frontend? js + favicon from images & remove template 1
    
    [Route("")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}