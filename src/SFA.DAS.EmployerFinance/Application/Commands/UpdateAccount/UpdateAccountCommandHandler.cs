using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        protected override Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}