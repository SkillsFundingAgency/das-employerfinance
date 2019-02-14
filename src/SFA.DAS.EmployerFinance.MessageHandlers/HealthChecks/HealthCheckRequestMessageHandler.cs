using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Messages;

namespace SFA.DAS.EmployerFinance.MessageHandlers.HealthChecks
{
    public class HealthCheckRequestMessageHandler : IHandleMessages<HealthCheckRequestMessage>
    {
        private readonly ILogger _logger;

        public HealthCheckRequestMessageHandler(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task Handle(HealthCheckRequestMessage message, IMessageHandlerContext context)
        {
            _logger.LogInformation($"Received health check request ID: {message.Id}");
           
            await context.Reply(HealthStatus.Healthy).ConfigureAwait(false);
        }
    }
}