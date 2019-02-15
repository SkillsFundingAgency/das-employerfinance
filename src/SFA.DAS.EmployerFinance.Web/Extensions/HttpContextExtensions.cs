using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SFA.DAS.EmployerFinance.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool TryGetRouteValue(this HttpContext httpContext, string key, out object value)
        {
            value = httpContext.GetRouteValue(key);

            return value != null;
        }
    }
}