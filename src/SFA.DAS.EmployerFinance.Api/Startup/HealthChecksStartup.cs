using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFinance.Api.Startup
{
    public static class HealthChecksStartup
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddHealthChecks()
                .AddSqlServer(databaseConnectionString, name: "DB health check");

            return services;
        }
    }
}