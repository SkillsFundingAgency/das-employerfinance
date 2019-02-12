using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Messages;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public class HealthCheckResponseMessageHandler : IHandleMessages<HealthCheckResponseMessage>, INServiceBusHealthCheckResponseHandler
    {
        private readonly ILogger _logger;
        public event EventHandler<Guid> ReceivedResponse;

        public HealthCheckResponseMessageHandler(ILogger logger)
        {
            _logger = logger;
        }
        
        public Task Handle(HealthCheckResponseMessage message, IMessageHandlerContext context)
        {
            _logger.LogInformation($"Received health check response ID: {message.Id}");
            ReceivedResponse?.Invoke(this, message.Id);
            
            return Task.CompletedTask;
        }
    }
}