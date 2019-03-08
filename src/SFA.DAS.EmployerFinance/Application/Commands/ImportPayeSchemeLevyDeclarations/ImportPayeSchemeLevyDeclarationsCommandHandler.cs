using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations
{
    public class ImportPayeSchemeLevyDeclarationsCommandHandler : AsyncRequestHandler<ImportPayeSchemeLevyDeclarationsCommand>
    {
        protected override Task Handle(ImportPayeSchemeLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}