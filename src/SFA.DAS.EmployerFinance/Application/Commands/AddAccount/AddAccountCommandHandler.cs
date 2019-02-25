using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccount
{
    public class AddAccountCommandHandler : AsyncRequestHandler<AddAccountCommand>
    {
        public AddAccountCommandHandler()
        {
        }

        protected override Task Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}