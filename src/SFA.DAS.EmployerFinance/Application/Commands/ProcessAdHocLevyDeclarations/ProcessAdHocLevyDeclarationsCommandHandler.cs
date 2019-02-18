using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessAdHocLevyDeclarations
{
    public class ProcessAdHocLevyDeclarationsCommandHandler : AsyncRequestHandler<ProcessAdHocLevyDeclarationsCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public ProcessAdHocLevyDeclarationsCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }
        
        protected override async Task Handle(ProcessAdHocLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            var accountPayeScheme = await _db.AccountPayeSchemes.AsNoTracking().SingleAsync(aps => aps.Id == request.AccountPayeSchemeId, cancellationToken);
            var job = new ProcessLevyDeclarationsJob(request.PayrollPeriod, accountPayeScheme);
            var importCommand = new ImportLevyDeclarationsCommand(job.Id, request.PayrollPeriod, accountPayeScheme.Id);
            var processTimeoutCommand = new ProcessLevyDeclarationsTimeoutCommand(job.Id);
            var sendImportCommandTask = _uniformSession.SendLocal(importCommand);
            var sendProcessTimeoutCommandTask = _uniformSession.SendLocal(processTimeoutCommand, job.Timeout);

            _db.ProcessLevyDeclarationsJobs.Add(job);
            
            await Task.WhenAll(sendImportCommandTask, sendProcessTimeoutCommandTask);
        }
    }
}