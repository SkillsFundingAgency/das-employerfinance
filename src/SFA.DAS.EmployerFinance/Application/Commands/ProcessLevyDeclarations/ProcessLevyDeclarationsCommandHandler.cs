using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations
{
    public class ProcessLevyDeclarationsCommandHandler : AsyncRequestHandler<ProcessLevyDeclarationsCommand>
    {
        private readonly EmployerFinanceDbContext _db;

        public ProcessLevyDeclarationsCommandHandler(EmployerFinanceDbContext db)
        {
            _db = db;
        }
        
        protected override async Task Handle(ProcessLevyDeclarationsCommand request, CancellationToken cancellationToken)
        {
            var accountPayeSchemes = await _db.AccountPayeSchemes.AsNoTracking().ToListAsync(cancellationToken);
            var saga = new LevyDeclarationSaga(request.PayrollPeriod, accountPayeSchemes);

            _db.LevyDeclarationSagas.Add(saga);
        }
    }
}