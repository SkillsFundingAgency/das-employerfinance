using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Api.Client.Http;
using SFA.DAS.EmployerFinance.Web.Extensions;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Finance Api check";
        
        private readonly IEmployerFinanceApiClient _apiClient;
        private readonly ILogger<ApiHealthCheck> _logger;

        public ApiHealthCheck(IEmployerFinanceApiClient apiClient, ILogger<ApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging Employer Finance API");
            
            try
            {
                var timer = Stopwatch.StartNew();
                await _apiClient.Ping();
                timer.Stop();

                var durationString = timer.Elapsed.ToHumanReadableString();
                
                _logger.LogInformation($"Employer Finance API ping successful and took {durationString}");
                
                return HealthCheckResult.Healthy(HealthCheckResultDescription, 
                    new Dictionary<string, object>(){{"Duration", durationString}});
            }
            catch (RestHttpClientException e)
            {
                _logger.LogWarning($"Employer Finance API ping failed : [Code: {e.StatusCode}] - {e.ReasonPhrase}");
                return HealthCheckResult.Unhealthy(HealthCheckResultDescription, e);
            }
        }
    }
}