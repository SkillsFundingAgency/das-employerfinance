using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Web.Cookies;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("service")]
    public class ServiceController : Controller
    {
        [Route("signout")]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
        }
        
        [AllowAnonymous]
        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            Response.Cookies.Delete(CookieNames.Authentication);
        }
    }
}