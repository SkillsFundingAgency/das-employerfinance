using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFinance.Api.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services
                .AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }
        
        public static IServiceCollection AddApiHealthChecks(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddHealthChecks().AddSqlServer(databaseConnectionString);

            return services;
        }
    }
}