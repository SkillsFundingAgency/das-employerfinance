using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccount
{
    public class RemoveAccountCommandHandler : AsyncRequestHandler<RemoveAccountCommand>
    {
        public RemoveAccountCommandHandler()
        {
        }

        protected override Task Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}