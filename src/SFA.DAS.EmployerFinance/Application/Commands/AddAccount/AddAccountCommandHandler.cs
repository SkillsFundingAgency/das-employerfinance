using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccount
{
    public class AddAccountCommandHandler : AsyncRequestHandler<AddAccountCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public AddAccountCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override Task Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}