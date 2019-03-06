using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFinance.Api.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services
                .AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }
    }
}