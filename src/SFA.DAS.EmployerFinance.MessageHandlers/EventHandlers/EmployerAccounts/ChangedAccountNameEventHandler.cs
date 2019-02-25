using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
    {
        private readonly IMediator _mediator;

        public ChangedAccountNameEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}