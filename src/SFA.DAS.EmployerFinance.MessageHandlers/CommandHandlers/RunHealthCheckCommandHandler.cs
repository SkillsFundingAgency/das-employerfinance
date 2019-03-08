using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Commands;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class RunHealthCheckCommandHandler : IHandleMessages<RunHealthCheckCommand>
    {
        private readonly ILogger _logger;

        public RunHealthCheckCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(RunHealthCheckCommand message, IMessageHandlerContext context)
        {
            _logger.LogInformation($"Handled NServiceBus health check message with ID '{message.Id}'");

            return Task.CompletedTask;
        }
    }
}