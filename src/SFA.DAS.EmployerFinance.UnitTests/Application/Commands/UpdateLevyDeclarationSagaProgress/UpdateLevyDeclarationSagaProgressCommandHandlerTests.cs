using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NServiceBus;
using NServiceBus.UniformSession;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.UpdateLevyDeclarationSagaProgress
{
    [TestFixture]
    [Parallelizable]
    public class UpdateLevyDeclarationSagaProgressCommandHandlerTests : FluentTest<UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenSomeImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.Saga.Should().Match<LevyDeclarationSaga>(j =>
                j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == f.Tasks.Count &&
                j.UpdateAccountTransactionBalancesTasksCompleteCount == 0 &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(),f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenSomeImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.Is<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Saga.Id), It.IsAny<SendOptions>()), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenAllImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(f => f.SetAllImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.Saga.Should().Match<LevyDeclarationSaga>(j =>
                j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == f.Tasks.Count &&
                j.UpdateAccountTransactionBalancesTasksCompleteCount == 0 &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldPublishEvent()
        {
            return TestAsync(f => f.SetAllImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdatedLevyDeclarationSagaProgressEvent>(e => e.SagaId == f.Saga.Id));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(f => f.SetSomeUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.Saga.Should().Match<LevyDeclarationSaga>(j =>
                j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountTransactionBalancesTasksCompleteCount == f.Tasks.Count &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(f => f.SetSomeUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().OfType<FinishedProcessingLevyDeclarationsEvent>().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetSomeUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.Is<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Saga.Id), It.IsAny<SendOptions>()), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(f => f.SetAllUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.Saga.Should().Match<LevyDeclarationSaga>(j =>
                j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountTransactionBalancesTasksCompleteCount == f.Tasks.Count &&
                j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompleted_ThenShouldPublishEvent()
        {
            return TestAsync(f => f.SetAllUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<FinishedProcessingLevyDeclarationsEvent>(e => 
                    e.SagaId == f.Saga.Id &&
                    e.PayrollPeriod == f.Saga.PayrollPeriod &&
                    e.Finished == f.Saga.Updated.Value));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompletedAndSagaTypeIsAdHoc_ThenShouldPublishEvent()
        {
            return TestAsync(f => f.SetAdHocSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<FinishedProcessingLevyDeclarationsAdHocEvent>(e => 
                    e.SagaId == f.Saga.Id &&
                    e.PayrollPeriod == f.Saga.PayrollPeriod &&
                    e.AccountPayeSchemeId == f.Saga.HighWaterMarkId &&
                    e.Finished == f.Saga.Updated.Value));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetAllUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateLevyDeclarationSagaProgressCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotUpdateSagaProgress()
        {
            return TestAsync(f => f.SetAllTasksCompleted(), f => f.Handle(), f => f.Saga.Should().Match<LevyDeclarationSaga>(j =>
                j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountTransactionBalancesTasksCompleteCount == f.Accounts.Count &&
                j.IsComplete &&
                j.Updated == f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(f => f.SetAllTasksCompleted(),f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetAllTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateLevyDeclarationSagaProgressCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }

        [Test]
        public Task Handle_WhenImportPayeSchemeLevyDeclarationsTaskCountsDoNotBalance_ThenShouldThrowException()
        {
            return TestExceptionAsync(f => f.SetTooManyImportLevyDeclarationsTasksCompleted(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>()
                .WithMessage($"Requires {nameof(LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)} task counts balance"));
        }

        [Test]
        public Task Handle_WhenUpdateAccountTransactionBalancesTaskCountsDoNotBalance_ThenShouldThrowException()
        {
            return TestExceptionAsync(f => f.SetTooManyUpdateAccountTransactionBalancesTasksCompleted(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>()
                .WithMessage($"Requires {nameof(LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)} task counts balance"));
        }
    }

    public class UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public List<string> PayeSchemeRefs { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }
        public LevyDeclarationSaga Saga { get; set; }
        public List<LevyDeclarationSagaTask> Tasks { get; set; }
        public UpdateLevyDeclarationSagaProgressCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<UpdateLevyDeclarationSagaProgressCommand> Handler { get; set; }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            UnitOfWorkContext = new UnitOfWorkContext();
            
            PayeSchemeRefs = new List<string>
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
                new AccountPayeScheme(Accounts[0], PayeSchemeRefs[0]).Set(aps => aps.Id, 1),
                new AccountPayeScheme(Accounts[0], PayeSchemeRefs[1]).Set(aps => aps.Id, 2),
                new AccountPayeScheme(Accounts[1], PayeSchemeRefs[2]).Set(aps => aps.Id, 3)
            };

            Saga = ObjectActivator.CreateInstance<LevyDeclarationSaga>()
                .Set(s => s.Id, 4)
                .Set(s => s.Type, LevyDeclarationSagaType.All)
                .Set(s => s.PayrollPeriod, Now)
                .Set(s => s.HighWaterMarkId, AccountPayeSchemes.OrderByDescending(aps => aps.Id).Select(aps => aps.Id).First())
                .Set(s => s.ImportPayeSchemeLevyDeclarationsTasksCount, AccountPayeSchemes.Select(aps => aps.EmployerReferenceNumber).Distinct().Count())
                .Set(s => s.UpdateAccountTransactionBalancesTasksCount, AccountPayeSchemes.Select(aps => aps.AccountId).Distinct().Count());
            
            Command = new UpdateLevyDeclarationSagaProgressCommand(Saga.Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();

            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.LevyDeclarationSagas.AddRange(Saga);
            Db.SaveChanges();
            
            Handler = new UpdateLevyDeclarationSagaProgressCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAdHocSagaType()
        {
            Saga.Set(s => s.Type, LevyDeclarationSagaType.AdHoc);
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetSomeImportLevyDeclarationTasksCompleted()
        {
            Tasks = new List<LevyDeclarationSagaTask>
            {
                LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, AccountPayeSchemes[0].Id),
                LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, AccountPayeSchemes[2].Id)
            };
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllImportLevyDeclarationTasksCompleted()
        {
            Tasks = AccountPayeSchemes.Select(aps => LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, aps.Id)).ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetSomeUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(j => j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount, AccountPayeSchemes.Count);
            
            Tasks = new List<LevyDeclarationSagaTask>
            {
                LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(Saga.Id, AccountPayeSchemes[0].AccountId)
            };
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(j => j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount, AccountPayeSchemes.Count);
            
            Tasks = Accounts.Select(a => LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(Saga.Id, a.Id)).ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllTasksCompleted()
        {
            Saga.Set(j => j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount, AccountPayeSchemes.Count);
            Saga.Set(j => j.UpdateAccountTransactionBalancesTasksCompleteCount, Accounts.Count);
            Saga.Set(j => j.IsComplete, true);
            Saga.Set(j => j.Updated, Now);
            
            var importPayeSchemeLevyDeclarationsTasks = AccountPayeSchemes.Select(aps => LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, aps.Id));
            var updateAccountTransactionBalancesTasks = Accounts.Select(a => LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(Saga.Id, a.Id));

            Tasks = importPayeSchemeLevyDeclarationsTasks.Concat(updateAccountTransactionBalancesTasks).ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetTooManyImportLevyDeclarationsTasksCompleted()
        {
            Tasks = AccountPayeSchemes
                .Select(aps => LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, aps.Id))
                .Append(LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, AccountPayeSchemes[0].Id))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetTooManyUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(j => j.ImportPayeSchemeLevyDeclarationsTasksCompleteCount, AccountPayeSchemes.Count);
            
            Tasks = Accounts
                .Select(a => LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(Saga.Id, a.Id))
                .Append(LevyDeclarationSagaTask.CreateUpdateAccountTransactionBalancesTask(Saga.Id, Accounts[0].Id))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }
    }
}