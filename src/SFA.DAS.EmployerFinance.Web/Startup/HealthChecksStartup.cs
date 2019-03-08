using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Web.HealthChecks;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class HealthChecksStartup
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>("API health check")
                .AddCheck<NServiceBusHealthCheck>("Service bus health check")
                .AddSqlServer(databaseConnectionString, name: "DB health check");
            
            return services;
        }
    }
}