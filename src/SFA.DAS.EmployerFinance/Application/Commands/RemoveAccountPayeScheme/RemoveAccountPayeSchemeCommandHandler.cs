using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        protected override Task Handle(RemoveAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}