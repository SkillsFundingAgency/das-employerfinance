using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class AddedPayeSchemeEventHandler : IHandleMessages<AddedPayeSchemeEvent>
    {
        private readonly IMediator _mediator;

        public AddedPayeSchemeEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(AddedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}