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
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeImportLevyDeclarationTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == f.Tasks.Count(t => t.Finished != null) &&
                    s.ImportPayeSchemeLevyDeclarationsTasksErroredCount == f.Tasks.Count(t => t.Errored != null) &&
                    !s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == 0 &&
                    !s.IsStage2Complete &&
                    !s.IsComplete &&
                    s.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeImportLevyDeclarationTasksCompleted(),
                f => f.Handle(), 
                f => f.UnitOfWorkContext.GetEvents().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenSomeImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeImportLevyDeclarationTasksCompleted(),
                f => f.Handle(), 
                f => f.UniformSession.Verify(s => s
                    .Send(
                        It.Is<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Saga.Id),
                        It.IsAny<SendOptions>()),
                    Times.Once));
        }
        
        [Test]
        public Task Handle_WhenAllImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllImportLevyDeclarationTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == f.Tasks.Count(t => t.Finished != null) &&
                    s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == 0 &&
                    !s.IsStage2Complete &&
                    !s.IsComplete &&
                    s.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllImportPayeSchemeLevyDeclarationsTasksCompleted_ThenShouldPublishEvent()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllImportLevyDeclarationTasksCompleted(),
                f => f.Handle(),
                f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                    .And.Match<UpdatedLevyDeclarationSagaProgressEvent>(e => e.SagaId == f.Saga.Id));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == f.AccountPayeSchemes.Count &&
                    s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == f.Tasks.Count(t => t.Finished != null) &&
                    !s.IsStage2Complete &&
                    !s.IsComplete &&
                    s.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.UnitOfWorkContext.GetEvents().OfType<FinishedProcessingLevyDeclarationsEvent>().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountTransactionBalancesTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetSomeUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.UniformSession.Verify(s => s
                    .Send(
                        It.Is<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Saga.Id),
                        It.IsAny<SendOptions>()),
                    Times.Once));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompletedAndSagaTypeIsScheduled_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == f.AccountPayeSchemes.Count &&
                    s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == f.Tasks.Count(t => t.Finished != null) &&
                    s.IsStage2Complete &&
                    s.IsComplete &&
                    s.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompletedAndSagaTypeIsScheduled_ThenShouldPublishEvent()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                    .And.Match<FinishedProcessingLevyDeclarationsEvent>(e => 
                        e.SagaId == f.Saga.Id &&
                        e.PayrollPeriod == f.Saga.PayrollPeriod &&
                        e.AccountPayeSchemeHighWaterMarkId == f.Saga.AccountPayeSchemeHighWaterMarkId.Value &&
                        e.Finished == f.Saga.Updated.Value));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompletedAndSagaTypeIsAdHoc_ThenShouldUpdateSagaProgress()
        {
            return TestAsync(
                f => f.SetAdHocSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == 1 &&
                    s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == 1 &&
                    s.IsStage2Complete &&
                    s.IsComplete &&
                    s.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompletedAndSagaTypeIsAdHoc_ThenShouldPublishEvent()
        {
            return TestAsync(
                f => f.SetAdHocSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                    .And.Match<FinishedProcessingLevyDeclarationsAdHocEvent>(e => 
                        e.SagaId == f.Saga.Id &&
                        e.PayrollPeriod == f.Saga.PayrollPeriod &&
                        e.AccountPayeSchemeId == f.Saga.AccountPayeSchemeId &&
                        e.Finished == f.Saga.Updated.Value));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountTransactionBalancesTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                f => f.UniformSession.Verify(s => s
                    .Send(
                        It.IsAny<UpdateLevyDeclarationSagaProgressCommand>(),
                        It.IsAny<SendOptions>()),
                    Times.Never));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotUpdateSagaProgress()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllTasksCompleted(),
                f => f.Handle(),
                f => f.Saga.Should().Match<LevyDeclarationSaga>(s =>
                    s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount == f.AccountPayeSchemes.Count &&
                    s.IsStage1Complete &&
                    s.UpdateAccountTransactionBalancesTasksFinishedCount == f.Accounts.Count &&
                    s.IsStage2Complete &&
                    s.IsComplete &&
                    s.Updated == f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotPublishEvent()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllTasksCompleted(),
                f => f.Handle(),
                f => f.UnitOfWorkContext.GetEvents().Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(
                f => f.SetScheduledSagaType().SetAllTasksCompleted(),
                f => f.Handle(),
                f => f.UniformSession.Verify(s => s
                    .Send(
                        It.IsAny<UpdateLevyDeclarationSagaProgressCommand>(),
                        It.IsAny<SendOptions>()), 
                    Times.Never));
        }

        [Test]
        public Task Handle_WhenImportPayeSchemeLevyDeclarationsTaskCountsDoNotBalance_ThenShouldThrowException()
        {
            return TestExceptionAsync(
                f => f.SetScheduledSagaType().SetTooManyImportLevyDeclarationsTasksCompleted(),
                f => f.Handle(),
                (f, r) => r.Should().Throw<InvalidOperationException>()
                    .WithMessage($"Requires {nameof(LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)} task counts balance"));
        }

        [Test]
        public Task Handle_WhenUpdateAccountTransactionBalancesTaskCountsDoNotBalance_ThenShouldThrowException()
        {
            return TestExceptionAsync(
                f => f.SetScheduledSagaType().SetTooManyUpdateAccountTransactionBalancesTasksCompleted(),
                f => f.Handle(),
                (f, r) => r.Should().Throw<InvalidOperationException>()
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
                .Set(s => s.PayrollPeriod, Now);
            
            Command = new UpdateLevyDeclarationSagaProgressCommand(Saga.Id);
            
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();

            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.LevyDeclarationSagas.Add(Saga);
            Db.SaveChanges();
            
            Handler = new UpdateLevyDeclarationSagaProgressCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetScheduledSagaType()
        {
            Saga.Set(s => s.Type, LevyDeclarationSagaType.Planned)
                .Set(s => s.AccountPayeSchemeHighWaterMarkId, AccountPayeSchemes.Max(aps => aps.Id))
                .Set(s => s.ImportPayeSchemeLevyDeclarationsTasksCount, AccountPayeSchemes.Select(aps => aps.EmployerReferenceNumber).Count())
                .Set(s => s.UpdateAccountTransactionBalancesTasksCount, AccountPayeSchemes.Select(aps => aps.AccountId).Distinct().Count());
            
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAdHocSagaType()
        {
            Saga.Set(s => s.Type, LevyDeclarationSagaType.AdHoc)
                .Set(s => s.AccountPayeSchemeId, AccountPayeSchemes[0].Id)
                .Set(s => s.ImportPayeSchemeLevyDeclarationsTasksCount, 1)
                .Set(s => s.UpdateAccountTransactionBalancesTasksCount, 1);
            
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetSomeImportLevyDeclarationTasksCompleted()
        {
            Tasks = new List<LevyDeclarationSagaTask>
            {
                ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, AccountPayeSchemes[0].Id)
                    .Set(t => t.Finished, Now),
                ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, AccountPayeSchemes[1].Id)
                    .Set(t => t.Errored, Now)
                    .Set(t => t.ErrorMessage, "Foobar")
            };
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllImportLevyDeclarationTasksCompleted()
        {
            Tasks = AccountPayeSchemes
                .Take(Saga.ImportPayeSchemeLevyDeclarationsTasksCount)
                .Select(aps => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, aps.Id)
                    .Set(t => t.Finished, Now))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetSomeUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(s => s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount, AccountPayeSchemes.Count);
            
            Tasks = new List<LevyDeclarationSagaTask>
            {
                ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, AccountPayeSchemes[0].AccountId)
                    .Set(t => t.Finished, Now),
                ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, AccountPayeSchemes[1].AccountId)
                    .Set(t => t.Errored, Now)
                    .Set(t => t.ErrorMessage, "Foobar")
            };
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(s => s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount, Saga.ImportPayeSchemeLevyDeclarationsTasksCount);
            
            Tasks = Accounts
                .Take(Saga.UpdateAccountTransactionBalancesTasksCount)
                .Select(a => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, a.Id)
                    .Set(t => t.Finished, Now))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetAllTasksCompleted()
        {
            Saga.Set(s => s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount, Saga.ImportPayeSchemeLevyDeclarationsTasksCount)
                .Set(s => s.UpdateAccountTransactionBalancesTasksFinishedCount, Saga.UpdateAccountTransactionBalancesTasksCount)
                .Set(s => s.IsComplete, true)
                .Set(s => s.Updated, Now);
            
            var importPayeSchemeLevyDeclarationsTasks = AccountPayeSchemes
                .Take(Saga.ImportPayeSchemeLevyDeclarationsTasksCount)
                .Select(aps => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, aps.Id)
                    .Set(t => t.Finished, Now));
            
            var updateAccountTransactionBalancesTasks = Accounts
                .Take(Saga.UpdateAccountTransactionBalancesTasksCount)
                .Select(a => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, a.Id)
                    .Set(t => t.Finished, Now));

            Tasks = importPayeSchemeLevyDeclarationsTasks.Concat(updateAccountTransactionBalancesTasks).ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetTooManyImportLevyDeclarationsTasksCompleted()
        {
            Tasks = AccountPayeSchemes
                .Select(aps => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, aps.Id)
                    .Set(t => t.Finished, Now))
                .Append(ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, AccountPayeSchemes[0].Id)
                    .Set(t => t.Finished, Now))
                .Append(ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)
                    .Set(t => t.AccountPayeSchemeId, AccountPayeSchemes[0].Id)
                    .Set(t => t.Errored, Now)
                    .Set(t => t.ErrorMessage, "Foobar"))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public UpdateProcessLevyDeclarationsSagaProgressCommandHandlerTestsFixture SetTooManyUpdateAccountTransactionBalancesTasksCompleted()
        {
            Saga.Set(s => s.ImportPayeSchemeLevyDeclarationsTasksFinishedCount, AccountPayeSchemes.Count);
            
            Tasks = Accounts
                .Select(a => ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, a.Id)
                    .Set(t => t.Finished, Now))
                .Append(ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, Accounts[0].Id)
                    .Set(t => t.Finished, Now))
                .Append(ObjectActivator.CreateInstance<LevyDeclarationSagaTask>()
                    .Set(t => t.SagaId, Saga.Id)
                    .Set(t => t.Type, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)
                    .Set(t => t.AccountId, Accounts[0].Id)
                    .Set(t => t.Errored, Now)
                    .Set(t => t.ErrorMessage, "Foobar"))
                .ToList();
            
            Db.LevyDeclarationSagaTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }
    }
}