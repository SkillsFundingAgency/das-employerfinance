using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme
{
    public class AddAccountPayeSchemeCommandHandler : AsyncRequestHandler<AddAccountPayeSchemeCommand>
    {
        public AddAccountPayeSchemeCommandHandler()
        {
        }

        protected override Task Handle(AddAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}