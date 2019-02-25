using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccount
{
    public class RemoveAccountCommandHandler : AsyncRequestHandler<RemoveAccountCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public RemoveAccountCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override Task Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}