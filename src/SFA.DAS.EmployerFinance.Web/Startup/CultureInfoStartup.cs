using System.Globalization;
using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class CultureInfoStartup
    {
        public static IApplicationBuilder UseDasCultureInfo(this IApplicationBuilder app)
        {
            var cultureInfo = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            
            return app;
        }
    }
}