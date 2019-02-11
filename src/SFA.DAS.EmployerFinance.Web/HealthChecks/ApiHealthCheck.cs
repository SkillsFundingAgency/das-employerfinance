using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Api.Client.Http;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IEmployerFinanceApiClient _apiClient;
        private readonly ILogger _logger;

        public ApiHealthCheck(IEmployerFinanceApiClient apiClient, ILogger logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging Employer Finance API");
            
            try
            {
                await _apiClient.Ping();
                _logger.LogInformation("Employer Finance API ping successful");
                
                return HealthCheckResult.Healthy("Api ping succeeded");
            }
            catch (RestHttpClientException e)
            {
                _logger.LogWarning($"Employer Finance API ping failed : [Code: {e.StatusCode}] - {e.ReasonPhrase}");
                return HealthCheckResult.Unhealthy("Api ping failed", e);
            }
        }
    }
}