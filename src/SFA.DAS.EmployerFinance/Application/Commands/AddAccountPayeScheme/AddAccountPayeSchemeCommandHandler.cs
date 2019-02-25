using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        protected override Task Handle(AddAccountPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}