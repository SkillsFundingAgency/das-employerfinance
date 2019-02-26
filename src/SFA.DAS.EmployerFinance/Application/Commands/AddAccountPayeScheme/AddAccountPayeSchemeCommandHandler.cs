using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme
{
    public class AddAccountPayeSchemeCommandHandler : AsyncRequestHandler<AddAccountPayeSchemeCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public AddAccountPayeSchemeCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Value.Accounts.FirstAsync(a => a.Id == request.AccountId, cancellationToken);
            account.AddPayeScheme(request.EmployerReferenceNumber, request.Created);
        }
    }
}