using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ProcessLevyDeclarationsCommandHandler : IHandleMessages<ProcessLevyDeclarationsCommand>
    {
        private readonly IMediator _mediator;

        public ProcessLevyDeclarationsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(ProcessLevyDeclarationsCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}