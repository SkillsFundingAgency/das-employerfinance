using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTask
{
    public class ProcessLevyDeclarationsTaskCommandHandler : IPipelineBehavior<ImportLevyDeclarationsCommand, Unit>, IPipelineBehavior<UpdateAccountBalanceCommand, Unit>
    {
        private readonly EmployerFinanceDbContext _db;

        public ProcessLevyDeclarationsTaskCommandHandler(EmployerFinanceDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(ImportLevyDeclarationsCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var task = ProcessLevyDeclarationsJobTask.CreateImportLevyDeclarationsTask(request.JobId, request.AccountPayeSchemeId);
            var result = await ProcessTask(task, next);

            return result;
        }

        public async Task<Unit> Handle(UpdateAccountBalanceCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var task = ProcessLevyDeclarationsJobTask.CreateUpdateAccountBalanceTask(request.JobId, request.AccountId);
            var result = await ProcessTask(task, next);

            return result;
        }

        private async Task<T> ProcessTask<T>(ProcessLevyDeclarationsJobTask task, RequestHandlerDelegate<T> next)
        {
            task.Start();

            var result = await next();

            task.Finish();

            _db.ProcessLevyDeclarationsJobTasks.Add(task);

            return result;
        }
    }
}