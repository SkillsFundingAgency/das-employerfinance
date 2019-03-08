using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SFA.DAS.EmployerFinance.HealthChecks;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class HealthChecksStartup
    {
        public static IApplicationBuilder UseDasHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });
            
            app.UseHealthChecks("/ping", new HealthCheckOptions
            {
                Predicate = _ => false
            });

            return app;
        }
    }
}