using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Client;
using SFA.DAS.EmployerFinance.Application.Commands.RunHealthCheck;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class RunHealthCheckCommandHandlerTests : FluentTest<RunHealthCheckCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingARunHealthCheckCommand_ThenShouldAddAHealthCheck()
        {
            return TestAsync(f => f.Handle(), f => f.Db.HealthChecks.SingleOrDefault().Should().NotBeNull());
        }
    }

    public class RunHealthCheckCommandHandlerTestsFixture
    {
        public EmployerFinanceDbContext Db { get; set; }
        public RunHealthCheckCommand RunHealthCheckCommand { get; set; }
        public IRequestHandler<RunHealthCheckCommand, Unit> Handler { get; set; }
        public UnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IEmployerFinanceApiClient> EmployerFinanceApiClient { get; set; }

        public RunHealthCheckCommandHandlerTestsFixture()
        {
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            RunHealthCheckCommand = new RunHealthCheckCommand();
            UnitOfWorkContext = new UnitOfWorkContext();
            EmployerFinanceApiClient = new Mock<IEmployerFinanceApiClient>();

            Db.SaveChanges();
            
            EmployerFinanceApiClient.Setup(c => c.HealthCheck()).Returns(Task.CompletedTask);

            Handler = new RunHealthCheckCommandHandler(new Lazy<EmployerFinanceDbContext>(() => Db), EmployerFinanceApiClient.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(RunHealthCheckCommand, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}