using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations
{
    public class ProcessLevyDeclarationsCommandHandler : AsyncRequestHandler<ProcessLevyDeclarationsCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public ProcessLevyDeclarationsCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }
        
        protected override async Task Handle(ProcessLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            var accountPayeSchemes = await _db.AccountPayeSchemes.AsNoTracking().ToListAsync(cancellationToken);
            var saga = new LevyDeclarationSaga(request.PayrollPeriod, accountPayeSchemes);

            _db.LevyDeclarationSagas.Add(saga);
            
            await _uniformSession.SendLocal(new UpdateLevyDeclarationSagaProgressCommand(saga.Id), saga.Timeout);
        }
    }
}