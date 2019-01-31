using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Mappings;
using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck;
using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck.Dtos;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.EmployerFinance.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetHealthCheckQueryHandlerTests : FluentTest<GetHealthCheckQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAGetHealthCheckQuery_ThenShouldReturnAGetHealthCheckQueryResult()
        {
            return TestAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.HealthCheck.Should().NotBeNull().And.Match<HealthCheckDto>(d => d.Id == f.HealthChecks[1].Id);
            });
        }
    }

    public class GetHealthCheckQueryHandlerTestsFixture
    {
        public GetHealthCheckQuery GetHealthCheckQuery { get; set; }
        public IRequestHandler<GetHealthCheckQuery, GetHealthCheckQueryResult> Handler { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }

        public GetHealthCheckQueryHandlerTestsFixture()
        {
            GetHealthCheckQuery = new GetHealthCheckQuery();
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(HealthCheckMappings)));

            HealthChecks = new List<HealthCheck>
            {
                EntityActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 1),
                EntityActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 2)
            };
            
            Db.HealthChecks.AddRange(HealthChecks);
            Db.SaveChanges();

            Handler = new GetHealthCheckQueryHandler(new Lazy<EmployerFinanceDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetHealthCheckQueryResult> Handle()
        {
            return Handler.Handle(GetHealthCheckQuery, CancellationToken.None);
        }
    }
}