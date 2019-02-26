using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme
{
    public class RemoveAccountPayeSchemeCommandHandler : AsyncRequestHandler<RemoveAccountPayeSchemeCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public RemoveAccountPayeSchemeCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(RemoveAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Value.Accounts.FirstAsync(a => a.Id == request.AccountId, cancellationToken);
            
            var accountPayeScheme = await _db.Value.AccountPayeSchemes
                .FirstAsync(aps => aps.AccountId == request.AccountId && aps.EmployerReferenceNumber == request.EmployerReferenceNumber, cancellationToken);

            account.RemovePayeScheme(accountPayeScheme, request.Removed);
        }
    }
}