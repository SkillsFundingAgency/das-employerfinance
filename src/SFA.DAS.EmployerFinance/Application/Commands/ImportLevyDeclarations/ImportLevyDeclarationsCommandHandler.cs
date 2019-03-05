using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations
{
    public class ImportLevyDeclarationsCommandHandler : AsyncRequestHandler<ImportLevyDeclarationsCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public ImportLevyDeclarationsCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }

        protected override async Task Handle(ImportLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            var saga = await _db.LevyDeclarationSagas.SingleAsync(s => s.Id == request.SagaId, cancellationToken);
            
            var accountPayeSchemeIds = await _db.AccountPayeSchemes
                .Where(aps => aps.Id <= saga.AccountPayeSchemeHighWaterMarkId.Value)
                .Select(aps => aps.Id)
                .ToListAsync(cancellationToken);
                    
            var commands = accountPayeSchemeIds.Select(i => new ImportPayeSchemeLevyDeclarationsCommand(saga.Id, saga.PayrollPeriod, i));
            var tasks = commands.Select(_uniformSession.SendLocal);
            
            await Task.WhenAll(tasks);
        }
    }
}