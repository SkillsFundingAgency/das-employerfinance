using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations
{
    public class ImportLevyDeclarationsCommandHandler : AsyncRequestHandler<ImportLevyDeclarationsCommand>
    {
        protected override Task Handle(ImportLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}