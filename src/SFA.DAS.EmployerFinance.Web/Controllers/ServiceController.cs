using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("service")]
    public class ServiceController : Controller
    {
        private readonly IOidcConfiguration _config;
        public ServiceController(IOidcConfiguration config)
        {
            _config = config;
        }
        
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new RedirectResult(_config.LogoutEndpoint);
        }

        [Route("signoutcleanup")]
        public IActionResult SignOutCleanup()
        {
            return new SignOutResult(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}