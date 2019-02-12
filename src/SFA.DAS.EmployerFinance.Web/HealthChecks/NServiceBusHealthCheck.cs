using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Messages;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class NServiceBusHealthCheck : IHealthCheck
    {
        public int MessageResponseTimeoutMilliseconds { get; set; }
        
        private readonly IMessageSession _messageSession;
        private readonly INServiceBusHealthCheckResponseHandler _responseHandler;
        private readonly ILogger<NServiceBusHealthCheck> _logger;
        private readonly ManualResetEventSlim _resetEvent;

        public NServiceBusHealthCheck(IMessageSession messageSession, INServiceBusHealthCheckResponseHandler responseHandler, ILogger<NServiceBusHealthCheck> logger)
        {
            _messageSession = messageSession;
            _responseHandler = responseHandler;
            _logger = logger;

            _resetEvent = new ManualResetEventSlim();
            MessageResponseTimeoutMilliseconds = 3000;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var messageGuid = Guid.NewGuid();
            
            var action = new EventHandler<Guid>((sender, messageId) =>
            {
                if (messageId == messageGuid)
                {
                    _resetEvent.Set();
                }
            });
            
            _responseHandler.ReceivedResponse += action;

            try
            {
                await _messageSession.SendLocal(new HealthCheckRequestMessage {Id = messageGuid});

                if (_resetEvent.Wait(MessageResponseTimeoutMilliseconds))
                {
                    return HealthCheckResult.Healthy();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get NServiceBus health check response", e);
            }
            finally
            {
                _responseHandler.ReceivedResponse -= action;
            }
            
            return HealthCheckResult.Unhealthy();
        }
    }
}