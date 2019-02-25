using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

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
            throw new System.NotImplementedException();
        }
    }
}