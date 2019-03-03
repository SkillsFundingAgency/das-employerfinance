using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.RunLevyDeclarationSagaTask
{
    public class RunLevyDeclarationSagaTaskCommandHandler : IPipelineBehavior<ImportPayeSchemeLevyDeclarationsCommand, Unit>, IPipelineBehavior<UpdateAccountLevyDeclarationTransactionBalancesCommand, Unit>
    {
        private readonly EmployerFinanceDbContext _db;

        public RunLevyDeclarationSagaTaskCommandHandler(EmployerFinanceDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(ImportPayeSchemeLevyDeclarationsCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var task = LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(request.SagaId, request.AccountPayeSchemeId);
            var result = await Run(task, next);

            return result;
        }

        public async Task<Unit> Handle(UpdateAccountLevyDeclarationTransactionBalancesCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var task = LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(request.SagaId, request.AccountId);
            var result = await Run(task, next);

            return result;
        }

        private async Task<T> Run<T>(LevyDeclarationSagaTask task, RequestHandlerDelegate<T> next)
        {
            task.Start();

            var result = await next();

            task.Finish();

            _db.LevyDeclarationSagaTasks.Add(task);

            return result;
        }
    }
}