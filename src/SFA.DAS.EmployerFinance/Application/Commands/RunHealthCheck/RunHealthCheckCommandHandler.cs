using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.RunHealthCheck
{
    public class RunHealthCheckCommandHandler : AsyncRequestHandler<RunHealthCheckCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;
        private readonly IEmployerFinanceApiClient _employerFinanceApiClient;

        public RunHealthCheckCommandHandler(Lazy<EmployerFinanceDbContext> db, IEmployerFinanceApiClient employerFinanceApiClient)
        {
            _db = db;
            _employerFinanceApiClient = employerFinanceApiClient;
        }

        protected override async Task Handle(RunHealthCheckCommand request, CancellationToken cancellationToken)
        {
            var healthCheck = new HealthCheck();

            await healthCheck.Run(_employerFinanceApiClient.Ping);

            _db.Value.HealthChecks.Add(healthCheck);
        }
    }
}