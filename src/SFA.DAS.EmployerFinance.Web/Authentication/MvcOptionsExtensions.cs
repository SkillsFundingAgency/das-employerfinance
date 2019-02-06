using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace SFA.DAS.EmployerFinance.Web.Authentication
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Default to all pages/actions requiring authentication and allow opt-out with [AllowAnonymous], rather than opting in with [Authorize]
        /// </summary>
        public static void RequireAuthorizationByDefault(this MvcOptions mvcOptions)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            mvcOptions.Filters.Add(new AuthorizeFilter(policy));
        }
    }
}