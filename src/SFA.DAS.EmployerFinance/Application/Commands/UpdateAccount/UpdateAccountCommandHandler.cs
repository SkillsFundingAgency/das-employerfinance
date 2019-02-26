using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandler : AsyncRequestHandler<UpdateAccountCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public UpdateAccountCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Value.Accounts.SingleAsync(a => a.Id == request.AccountId, cancellationToken);

            account.UpdateName(request.Name, request.Updated);
        }
    }
}