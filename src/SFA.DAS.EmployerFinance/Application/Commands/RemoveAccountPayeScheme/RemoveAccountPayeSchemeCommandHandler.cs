using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme
{
    public class RemoveAccountPayeSchemeCommandHandler : AsyncRequestHandler<RemoveAccountPayeSchemeCommand>
    {
        public RemoveAccountPayeSchemeCommandHandler()
        {
        }

        protected override Task Handle(RemoveAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}