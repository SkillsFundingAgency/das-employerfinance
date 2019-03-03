using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance
{
    public class StartedProcessingLevyDeclarationsAdHocEventHandler : IHandleMessages<StartedProcessingLevyDeclarationsAdHocEvent>
    {
        private readonly IMediator _mediator;

        public StartedProcessingLevyDeclarationsAdHocEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(StartedProcessingLevyDeclarationsAdHocEvent message, IMessageHandlerContext context)
        {
            return Task.WhenAll(
                _mediator.Send(new ImportLevyDeclarationsCommand(message.SagaId)),
                _mediator.Send(new UpdateLevyDeclarationSagaProgressCommand(message.SagaId)));
        }
    }
}