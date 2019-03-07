using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances
{
    public class UpdateAccountTransactionBalancesCommandHandler : AsyncRequestHandler<UpdateAccountTransactionBalancesCommand>
    {
        protected override Task Handle(UpdateAccountTransactionBalancesCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}