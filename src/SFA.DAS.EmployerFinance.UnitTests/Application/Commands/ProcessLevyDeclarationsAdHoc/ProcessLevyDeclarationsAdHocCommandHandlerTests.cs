using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsAdHoc;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessLevyDeclarationsAdHoc
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsAdHocCommandHandlerTests : FluentTest<ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldCreateSaga()
        {
            return TestAsync(f => f.Handle(), f => f.Db.LevyDeclarationSagas.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSaga>(j =>
                    j.PayrollPeriod == f.Command.PayrollPeriod &&
                    j.HighWaterMarkId == f.AccountPayeScheme.Id &&
                    j.ImportPayeSchemeLevyDeclarationsTasksCount == 1 &&
                    j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == 0 &&
                    j.UpdateAccountTransactionBalancesTasksCount == 1 &&
                    j.UpdateAccountTransactionBalancesTasksCompleteCount == 0 &&
                    j.Created >= f.Now &&
                    j.Updated == null &&
                    !j.IsComplete));
        }
        
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldPublishEvent()
        {
            return TestAsync(f => f.Handle(), f =>
            {
                var saga = f.Db.LevyDeclarationSagas.Single();
                
                f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                    .And.Match<StartedProcessingLevyDeclarationsAdHocEvent>(e =>
                        e.SagaId == saga.Id &&
                        e.PayrollPeriod == saga.PayrollPeriod &&
                        e.AccountPayeSchemeId == saga.HighWaterMarkId &&
                        e.Started == saga.Created);
            });
        }
    }

    public class ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public ProcessLevyDeclarationsAdHocCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public IRequestHandler<ProcessLevyDeclarationsAdHocCommand> Handler { get; set; }
        public AccountPayeScheme AccountPayeScheme { get; set; }

        public ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            UnitOfWorkContext = new UnitOfWorkContext();
            AccountPayeScheme = Fixture.Create<AccountPayeScheme>().Set(aps => aps.Id, 1);
            Command = new ProcessLevyDeclarationsAdHocCommand(Now, AccountPayeScheme.Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            Db.AccountPayeSchemes.Add(AccountPayeScheme);
            Db.SaveChanges();
            
            Handler = new ProcessLevyDeclarationsAdHocCommandHandler(Db);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}