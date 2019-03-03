using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress
{
    public class UpdateLevyDeclarationSagaProgressCommandHandler : AsyncRequestHandler<UpdateLevyDeclarationSagaProgressCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public UpdateLevyDeclarationSagaProgressCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }

        protected override async Task Handle(UpdateLevyDeclarationSagaProgressCommand request, CancellationToken cancellationToken)
        {
            var saga = await _db.LevyDeclarationSagas.SingleAsync(j => j.Id == request.SagaId, cancellationToken);
            var tasks = await _db.LevyDeclarationSagaTasks.Include(t => t.AccountPayeScheme).AsNoTracking().Where(t => t.SagaId == saga.Id).ToListAsync(cancellationToken);
            
            saga.UpdateProgress(tasks);
            
            if (!saga.IsComplete)
            {
                await _uniformSession.SendLocal(new UpdateLevyDeclarationSagaProgressCommand(saga.Id), saga.Timeout);
            }
        }
    }
}