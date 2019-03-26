using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.UniformSession;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationTransactionBalances
{
    public class UpdateLevyDeclarationTransactionBalancesCommandHandler : AsyncRequestHandler<UpdateLevyDeclarationTransactionBalancesCommand>
    {
        private readonly EmployerFinanceDbContext _db;
        private readonly IUniformSession _uniformSession;

        public UpdateLevyDeclarationTransactionBalancesCommandHandler(EmployerFinanceDbContext db, IUniformSession uniformSession)
        {
            _db = db;
            _uniformSession = uniformSession;
        }

        protected override async Task Handle(UpdateLevyDeclarationTransactionBalancesCommand request, CancellationToken cancellationToken)
        {
            var accountIds = await _db.LevyDeclarationSagaTasks
                .Where(t => t.SagaId == request.SagaId && t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                .Select(t => t.AccountPayeScheme.AccountId)
                .Distinct()
                .ToListAsync(cancellationToken);
            
            var commands = accountIds.Select(i => new UpdateAccountLevyDeclarationTransactionBalancesCommand(request.SagaId, i));
            
            var tasks = commands.Select(c =>
            {
                var sendOptions = new SendOptions();
                
                sendOptions.RequireImmediateDispatch();
                sendOptions.RouteToThisEndpoint();
                sendOptions.SetMessageId($"{nameof(UpdateAccountLevyDeclarationTransactionBalancesCommand)}-{c.SagaId}-{c.AccountId}");
                
                return _uniformSession.Send(c, sendOptions);
            });

            await Task.WhenAll(tasks);
        }
    }
}