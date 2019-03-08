using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Commands;
using SFA.DAS.EmployerFinance.Web.Extensions;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class NServiceBusHealthCheck : IHealthCheck
    {
        private readonly IMessageSession _messageSession;
        private readonly ILogger<NServiceBusHealthCheck> _logger;

        public NServiceBusHealthCheck(IMessageSession messageSession, ILogger<NServiceBusHealthCheck> logger)
        {
            _messageSession = messageSession;
            _logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var id = Guid.NewGuid();
            
            _logger.LogInformation($"Started '{context.Registration.Name}' with ID '{id}'");

            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                await _messageSession.Send(new RunHealthCheckCommand(id));
                
                stopwatch.Stop();
                
                var elapsed = stopwatch.Elapsed.ToHumanReadableString();
                
                _logger.LogInformation($"Finished '{context.Registration.Name}' with ID '{id}' in '{elapsed}'");
                
                return HealthCheckResult.Healthy(null, new Dictionary<string, object> { { "elapsed", elapsed } });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{context.Registration.Name}' with ID '{id}'", ex);
                
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}