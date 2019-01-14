using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Web.Models;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    //todo: ssl cert. auth (using new core startup?)
    //todo: fonts issue (edge)
    //todo: minification (https://docs.microsoft.com/en-us/aspnet/core/client-side/using-gulp?view=aspnetcore-2.2)
    //todo: document/script setting up node modules (switch git to e:)
    //todo: typescript (use BuildBundlerMinifier-Typescript?
    //todo: gov.uk design frontend? js
    
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