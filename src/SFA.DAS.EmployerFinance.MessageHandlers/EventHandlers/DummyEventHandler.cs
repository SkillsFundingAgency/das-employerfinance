using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers
{
    public class DummyEventHandler : IHandleMessages<DummyEvent>
    {
        private readonly ILogger _logger;

        public DummyEventHandler(ILogger logger)
        {
            _logger = logger;
        }
        
        public Task Handle(DummyEvent @event, IMessageHandlerContext context)
        {
            _logger.LogDebug($"Received dummy message with payload: {@event.Payload}");
            
            Console.WriteLine($"Received dummy message with payload: {@event.Payload}");
            
            return Task.CompletedTask;
        }
    }
}