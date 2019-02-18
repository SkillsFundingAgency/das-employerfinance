using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance
{
    public class UpdateAccountBalanceCommandHandler : AsyncRequestHandler<UpdateAccountBalanceCommand>
    {
        protected override Task Handle(UpdateAccountBalanceCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}