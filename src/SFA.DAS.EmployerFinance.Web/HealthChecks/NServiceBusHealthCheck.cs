using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Messages;
using SFA.DAS.EmployerFinance.Web.Extensions;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class NServiceBusHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "NServiceBus message handler check";
        
        private readonly IMessageSession _messageSession;
        private readonly ILogger<NServiceBusHealthCheck> _logger;

        private readonly TimeSpan ResponseTimeout;

        public NServiceBusHealthCheck(IMessageSession messageSession, ILogger<NServiceBusHealthCheck> logger, int timeoutMills = 3000)
        {
            _messageSession = messageSession;
            _logger = logger;
            ResponseTimeout = TimeSpan.FromMilliseconds(timeoutMills);
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var messageGuid = Guid.NewGuid();

            try
            {
                _logger.LogInformation($"Sending health check NServiceBus request message with Id: {messageGuid}");
                
                var messageCancellationTokenSource = CreateMessageCancellationTokenSource(cancellationToken);

                var (status, duration) = await RequestMessage(messageGuid, messageCancellationTokenSource.Token);
                
                if (status == HealthStatus.Healthy)
                {
                    return HealthCheckResult.Healthy(HealthCheckResultDescription, 
                        new Dictionary<string, object>{{"Duration:", duration}});
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation(cancellationToken.IsCancellationRequested
                    ? "NServiceBus health check was cancelled."
                    : $"Failed to get health check response before timeout for request ID: {messageGuid}");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get NServiceBus health check response", e);
            }
            
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }

        private async Task<Tuple<HealthStatus, string>> RequestMessage(Guid messageId, CancellationToken cancellationToken)
        {
            var option = new SendOptions();
            
            //TODO: The destination needs to be extracting out into a central place. Need to review how we want to do this
            option.SetDestination("SFA.DAS.EmployerFinanceV2.MessageHandlers");
                
            var stopwatch = Stopwatch.StartNew();
                
            var status = await _messageSession.Request<HealthStatus>(
                    new HealthCheckRequestMessage {Id = messageId}, option, cancellationToken)
                .ConfigureAwait(false);
                
            stopwatch.Stop();
            
            return new Tuple<HealthStatus, string>(status, stopwatch.Elapsed.ToHumanReadableString());
        }
        
        private CancellationTokenSource CreateMessageCancellationTokenSource(CancellationToken cancellationToken)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(ResponseTimeout);

            var combinedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationTokenSource.Token);
            return combinedTokenSource;
        }
    }
}