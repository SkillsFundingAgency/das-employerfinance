using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers
{
    public class DummyEventHandler : IHandleMessages<DummyMessage>
    {
        private readonly ILogger _logger;

        public DummyEventHandler(ILogger logger)
        {
            _logger = logger;
        }
        
        public Task Handle(DummyMessage message, IMessageHandlerContext context)
        {
            _logger.LogDebug($"Received dummy message with payload: {message.Payload}");
            
            return Task.CompletedTask;
        }
    }
}