using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessAdHocLevyDeclarations;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ProcessAdHocLevyDeclarationsCommandHandler : IHandleMessages<ProcessAdHocLevyDeclarationsCommand>
    {
        private readonly IMediator _mediator;

        public ProcessAdHocLevyDeclarationsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(ProcessAdHocLevyDeclarationsCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}