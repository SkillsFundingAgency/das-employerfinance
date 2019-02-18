using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class UpdateAccountBalanceCommandHandler : IHandleMessages<UpdateAccountBalanceCommand>
    {
        private readonly IMediator _mediator;

        public UpdateAccountBalanceCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdateAccountBalanceCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}