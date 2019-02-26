using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class DeletedPayeSchemeEventHandler : IHandleMessages<DeletedPayeSchemeEvent>
    {
        private readonly IMediator _mediator;

        public DeletedPayeSchemeEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(DeletedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new RemoveAccountPayeSchemeCommand());
        }
    }
}