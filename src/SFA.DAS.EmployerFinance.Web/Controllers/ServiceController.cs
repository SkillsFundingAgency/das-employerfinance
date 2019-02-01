using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("service")]
    public class ServiceController : Controller
    {
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(); //CookieAuthenticationDefaults.AuthenticationScheme);

            //todo: config    "LogoutEndpoint": "/connect/endsession?id_token_hint={0}",
            return new RedirectResult("/connect/endsession?id_token_hint={0}");
        }

        [Route("signoutcleanup")]
        public IActionResult SignOutCleanup()
        {
            //_authenticationService.SignOutUser();
            return new SignOutResult();
        }
    }
}