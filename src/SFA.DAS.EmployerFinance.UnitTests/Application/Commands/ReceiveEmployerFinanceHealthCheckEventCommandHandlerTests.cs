using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ReceiveEmployerFinanceHealthCheckEvent;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class ReceiveEmployerFinanceHealthCheckEventCommandHandlerTests : FluentTest<ReceiveEmployerFinanceHealthCheckEventCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingReceiveEmployerFinanceHealthCheckEventCommand_ThenShouldUpdateHealthCheck()
        {
            return TestAsync(f => f.Handle(), f => f.HealthChecks[1].ReceivedEmployerFinanceEvent.Should().NotBeNull());
        }
    }

    public class ReceiveEmployerFinanceHealthCheckEventCommandHandlerTestsFixture
    {
        public List<HealthCheck> HealthChecks { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public ReceiveEmployerFinanceHealthCheckEventCommand Command { get; set; }
        public IRequestHandler<ReceiveEmployerFinanceHealthCheckEventCommand, Unit> Handler { get; set; }

        public ReceiveEmployerFinanceHealthCheckEventCommandHandlerTestsFixture()
        {
            HealthChecks = new List<HealthCheck>
            {
                ObjectActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 1),
                ObjectActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 2)
            };
            
            Command = new ReceiveEmployerFinanceHealthCheckEventCommand(HealthChecks[1].Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            
            Db.HealthChecks.AddRange(HealthChecks);
            Db.SaveChanges();
            
            Handler = new ReceiveEmployerFinanceHealthCheckEventCommandHandler(new Lazy<EmployerFinanceDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}