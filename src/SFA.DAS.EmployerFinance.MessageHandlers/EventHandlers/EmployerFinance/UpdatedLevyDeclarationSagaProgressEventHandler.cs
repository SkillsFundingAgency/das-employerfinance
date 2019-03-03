using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance
{
    public class UpdatedLevyDeclarationSagaProgressEventHandler : IHandleMessages<UpdatedLevyDeclarationSagaProgressEvent>
    {
        private readonly IMediator _mediator;

        public UpdatedLevyDeclarationSagaProgressEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdatedLevyDeclarationSagaProgressEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new UpdateLevyDeclarationTransactionBalancesCommand(message.SagaId));
        }
    }
}