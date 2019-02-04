using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("service")]
    public class ServiceController : Controller
    {
//        private readonly IOidcConfiguration _config;
//        public ServiceController(IOidcConfiguration config)
//        {
//            _config = config;
//        }
        
        [Route("signout")]
        public async Task SignOut()
        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var idToken = await HttpContext.GetTokenAsync("id_token");
//                //hmmm, getting 404 or 'Bad Request - Request Too Long' in edge - url is ~4k with token (see https://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers)
//                //var logoutEndpointWithToken = string.Format(_config.LogoutEndpoint, WebUtility.UrlEncode(idToken));
//
//                var logoutEndpointWithToken = _config.LogoutEndpoint;
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = "/" });
                
//                        new AuthenticationProperties { RedirectUri = logoutEndpointWithToken });
            //}
        }

        // shouldn't need this if we supply /signout-oidc as the LogoutUrl in the RelyingParty table
        // (which won't work locally on chrome because 'Refused to display '' in a frame because it set 'X-Frame-Options' to 'sameorigin'', but should work in the environment), but works in edge
//        [Route("signoutcleanup")]
//        public IActionResult SignOutCleanup()
//        {
//            return new SignOutResult(new List<string> {CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme});
//        }

        [Route("signoutcleanup")]
        [AllowAnonymous]
        public void SignOutCleanup()
        {
            //why is breakpoint not hit, but endpoint called?
            // try deleting cookie manually
            // look at recruit, see what they're doing differently
            // what's wsfederated??
//            if (!User.Identity.IsAuthenticated)
//                return; // Task.CompletedTask;
            
            Response.Cookies.Delete("EmployerFinance.Web.Auth");
            
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
//                new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}