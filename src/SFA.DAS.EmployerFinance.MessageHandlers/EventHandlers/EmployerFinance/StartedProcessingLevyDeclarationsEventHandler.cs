using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance
{
    public class StartedProcessingLevyDeclarationsEventHandler : IHandleMessages<StartedProcessingLevyDeclarationsEvent>
    {
        private readonly IMediator _mediator;

        public StartedProcessingLevyDeclarationsEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(StartedProcessingLevyDeclarationsEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new ImportLevyDeclarationsCommand(message.SagaId));
        }
    }
}