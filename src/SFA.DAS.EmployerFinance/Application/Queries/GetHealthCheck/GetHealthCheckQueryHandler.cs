using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck.Dtos;
using SFA.DAS.EmployerFinance.Data;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck
{
    public class GetHealthCheckQueryHandler : IRequestHandler<GetHealthCheckQuery, GetHealthCheckQueryResult>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetHealthCheckQueryHandler(Lazy<EmployerFinanceDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetHealthCheckQueryResult> Handle(GetHealthCheckQuery request, CancellationToken cancellationToken)
        {
            var healthCheck = await _db.Value.HealthChecks
                .OrderByDescending(h => h.Id)
                .ProjectTo<HealthCheckDto>(_configurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return new GetHealthCheckQueryResult(healthCheck);
        }
    }
}