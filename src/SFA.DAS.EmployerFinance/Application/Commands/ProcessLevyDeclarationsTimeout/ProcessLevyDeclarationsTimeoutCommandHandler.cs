using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout
{
    public class ProcessLevyDeclarationsTimeoutCommandHandler : AsyncRequestHandler<ProcessLevyDeclarationsTimeoutCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public ProcessLevyDeclarationsTimeoutCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }

        protected override async Task Handle(ProcessLevyDeclarationsTimeoutCommand request, CancellationToken cancellationToken)
        {
            var job = await _db.ProcessLevyDeclarationsJobs.SingleAsync(j => j.Id == request.JobId, cancellationToken);
            var tasks = await _db.ProcessLevyDeclarationsJobTasks.Include(t => t.AccountPayeScheme).AsNoTracking().Where(t => t.JobId == job.Id).ToListAsync(cancellationToken);
            var stateChange = job.UpdateProgress(tasks);

            if (stateChange == ProcessLevyDeclarationsJobStateChange.ImportLevyDeclarationsTasksCompleted)
            {
                var accountIds = tasks.Where(t => t.Type == ProcessLevyDeclarationsJobTaskType.ImportLevyDeclarations).Select(t => t.AccountPayeScheme.AccountId).Distinct();
                var updateAccountBalanceCommands = accountIds.Select(i => new UpdateAccountBalanceCommand(request.JobId, i));
                var sendUpdateAccountBalanceCommandTasks = updateAccountBalanceCommands.Select(_uniformSession.SendLocal);

                await Task.WhenAll(sendUpdateAccountBalanceCommandTasks);
            }
            
            if (!job.IsComplete)
            {
                await _uniformSession.SendLocal(new ProcessLevyDeclarationsTimeoutCommand(job.Id), job.Timeout);
            }
        }
    }
}