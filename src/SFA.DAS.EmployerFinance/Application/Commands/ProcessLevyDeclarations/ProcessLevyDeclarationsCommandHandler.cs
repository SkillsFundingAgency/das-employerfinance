using System.Linq;
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
            var job = new ProcessLevyDeclarationsJob(request.PayrollPeriod, accountPayeSchemes);
            var importCommands = accountPayeSchemes.Select(aps => new ImportLevyDeclarationsCommand(job.Id, request.PayrollPeriod, aps.Id));
            var processTimeoutCommand = new ProcessLevyDeclarationsTimeoutCommand(job.Id);
            var sendImportCommandTasks = importCommands.Select(_uniformSession.SendLocal);
            var sendProcessTimeoutCommandTask = _uniformSession.SendLocal(processTimeoutCommand, job.Timeout);
            var tasks = sendImportCommandTasks.Append(sendProcessTimeoutCommandTask);

            _db.ProcessLevyDeclarationsJobs.Add(job);
            
            await Task.WhenAll(tasks);
        }
    }
}