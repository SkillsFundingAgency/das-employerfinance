using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class UpdateLevyDeclarationSagaProgressCommandHandler : IHandleMessages<UpdateLevyDeclarationSagaProgressCommand>
    {
        private readonly IMediator _mediator;

        public UpdateLevyDeclarationSagaProgressCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdateLevyDeclarationSagaProgressCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}