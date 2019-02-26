using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccount;

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
            return _mediator.Send(new UpdateAccountCommand());
        }
    }
}