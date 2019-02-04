using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Commands.ReceiveEmployerFinanceHealthCheckEvent
{
    public class ReceiveEmployerFinanceHealthCheckEventCommandHandler : AsyncRequestHandler<ReceiveEmployerFinanceHealthCheckEventCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public ReceiveEmployerFinanceHealthCheckEventCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(ReceiveEmployerFinanceHealthCheckEventCommand request, CancellationToken cancellationToken)
        {
            var healthCheck = await _db.Value.HealthChecks.SingleAsync(h => h.Id == request.Id, cancellationToken);

            healthCheck.ReceiveEmployerFinanceEvent();
        }
    }
}