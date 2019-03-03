using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsAdHoc
{
    public class ProcessLevyDeclarationsAdHocCommandHandler : AsyncRequestHandler<ProcessLevyDeclarationsAdHocCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public ProcessLevyDeclarationsAdHocCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }
        
        protected override async Task Handle(ProcessLevyDeclarationsAdHocCommand request, CancellationToken cancellationToken)
        {
            var accountPayeScheme = await _db.AccountPayeSchemes.AsNoTracking().SingleAsync(aps => aps.Id == request.AccountPayeSchemeId, cancellationToken);
            var saga = new LevyDeclarationSaga(request.PayrollPeriod, accountPayeScheme);

            _db.LevyDeclarationSagas.Add(saga);
            
            await _uniformSession.SendLocal(new UpdateLevyDeclarationSagaProgressCommand(saga.Id), saga.Timeout);
        }
    }
}