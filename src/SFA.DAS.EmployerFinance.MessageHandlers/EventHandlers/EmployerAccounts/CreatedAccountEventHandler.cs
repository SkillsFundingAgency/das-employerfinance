using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.AddAccount;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        private readonly IMediator _mediator;

        public CreatedAccountEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new AddAccountCommand());
        }
    }
}