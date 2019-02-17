using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.EmployerFinance.Web.Filters;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services
                .AddMvc(o =>
                {
                    o.RequireAuthenticatedUser();
                    o.AddAuthorization();
                    o.Filters.Add<UrlsViewBagFilter>();
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }
    }
}