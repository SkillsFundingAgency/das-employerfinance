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
            if (!User.Identity.IsAuthenticated)
                return;
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "/" });
        }

        //todo: shouldn't need this if we supply /signout-oidc as the LogoutUrl in the RelyingParty table
        // (which won't work locally on chrome because 'Refused to display '' in a frame because it set 'X-Frame-Options' to 'sameorigin'', but should work in the environment), but works in edge
        [Route("signoutcleanup")]
        [AllowAnonymous]
        public void SignOutCleanup()
        {
            // look at recruit, see what they're doing differently
            // what's wsfederated??
            
            Response.Cookies.Delete(CookieNames.Authentication);
        }
    }
}