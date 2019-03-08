using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsAdHoc;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ProcessLevyDeclarationsAdHocCommandHandler : IHandleMessages<ProcessLevyDeclarationsAdHocCommand>
    {
        private readonly IMediator _mediator;

        public ProcessLevyDeclarationsAdHocCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(ProcessLevyDeclarationsAdHocCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}