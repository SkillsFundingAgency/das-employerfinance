using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ProcessLevyDeclarationsTimeoutCommandHandler : IHandleMessages<ProcessLevyDeclarationsTimeoutCommand>
    {
        private readonly IMediator _mediator;

        public ProcessLevyDeclarationsTimeoutCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(ProcessLevyDeclarationsTimeoutCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}