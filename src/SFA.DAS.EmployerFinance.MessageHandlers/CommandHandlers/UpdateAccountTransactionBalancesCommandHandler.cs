using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class UpdateAccountTransactionBalancesCommandHandler : IHandleMessages<UpdateAccountTransactionBalancesCommand>
    {
        private readonly IMediator _mediator;

        public UpdateAccountTransactionBalancesCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdateAccountTransactionBalancesCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}