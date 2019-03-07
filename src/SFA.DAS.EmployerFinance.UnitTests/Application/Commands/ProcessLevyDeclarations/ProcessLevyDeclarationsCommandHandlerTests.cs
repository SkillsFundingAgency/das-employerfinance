using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessLevyDeclarations
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsCommandHandlerTests : FluentTest<ProcessLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldCreateSaga()
        {
            return TestAsync(f => f.Handle(), f => f.Db.LevyDeclarationSagas.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSaga>(j =>
                    j.PayrollPeriod == f.Command.PayrollPeriod &&
                    j.AccountPayeSchemeHighWaterMarkId == f.AccountPayeSchemes.Max(aps => aps.Id) &&
                    j.AccountPayeSchemeId == null &&
                    j.ImportPayeSchemeLevyDeclarationsTasksCount == f.EmployerReferenceNumbers.Count &&
                    j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == 0 &&
                    !j.IsStage1Complete &&
                    j.UpdateAccountTransactionBalancesTasksCount == f.Accounts.Count &&
                    j.UpdateAccountTransactionBalancesTasksCompleteCount == 0 &&
                    !j.IsStage2Complete &&
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
                    .And.Match<StartedProcessingLevyDeclarationsEvent>(e =>
                        e.SagaId == saga.Id &&
                        e.PayrollPeriod == saga.PayrollPeriod &&
                        e.AccountPayeSchemeHighWaterMarkId == saga.AccountPayeSchemeHighWaterMarkId.Value &&
                        e.Started == saga.Created);
            });
        }
    }

    public class ProcessLevyDeclarationsCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public ProcessLevyDeclarationsCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public IRequestHandler<ProcessLevyDeclarationsCommand> Handler { get; set; }
        public List<string> EmployerReferenceNumbers { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }

        public ProcessLevyDeclarationsCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            UnitOfWorkContext = new UnitOfWorkContext();
            
            EmployerReferenceNumbers = new List<string>
            {
                "AAA111",
                "BBB222",
                "CCC333"
            };
            
            Accounts = new List<Account>
            {
                Fixture.Create<Account>(),
                Fixture.Create<Account>()
            };
            
            AccountPayeSchemes = new List<AccountPayeScheme>
            {
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[0], Now).Set(aps => aps.Id, 1),
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[1], Now).Set(aps => aps.Id, 2),
                new AccountPayeScheme(Accounts[1], EmployerReferenceNumbers[2], Now).Set(aps => aps.Id, 3)
            };
            
            Command = new ProcessLevyDeclarationsCommand(Now);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.SaveChanges();
            
            Handler = new ProcessLevyDeclarationsCommandHandler(Db);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}