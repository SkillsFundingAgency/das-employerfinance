using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Web.Extensions;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IEmployerFinanceApiClient _apiClient;
        private readonly ILogger<ApiHealthCheck> _logger;

        public ApiHealthCheck(IEmployerFinanceApiClient apiClient, ILogger<ApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started '{context.Registration.Name}'");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                await _apiClient.Ping();

                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"Finished '{context.Registration.Name}' in '{elapsed}'");

                return HealthCheckResult.Healthy(null, new Dictionary<string, object> { { "elapsed", elapsed } });
            }
            catch (RestHttpClientException ex)
            {
                _logger.LogError($"Failed '{context.Registration.Name}': {nameof(ex.StatusCode)}='{ex.StatusCode}', {nameof(ex.ReasonPhrase)}='{ex.ReasonPhrase}'");

                return HealthCheckResult.Unhealthy(null, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{context.Registration.Name}'", ex);

                return HealthCheckResult.Unhealthy(null, ex);
            }
        }
    }
}