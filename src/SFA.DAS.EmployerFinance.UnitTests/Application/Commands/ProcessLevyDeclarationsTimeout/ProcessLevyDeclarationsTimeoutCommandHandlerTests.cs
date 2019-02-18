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
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessLevyDeclarationsTimeout
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsTimeoutCommandHandlerTests : FluentTest<ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenSomeImportLevyDeclarationsTasksCompleted_ThenShouldUpdateJobProgress()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.Job.Should().Match<ProcessLevyDeclarationsJob>(j =>
                j.ImportLevyDeclarationsTasksCompletedCount == f.Tasks.Count &&
                j.UpdateAccountBalanceTasksCompletedCount == 0 &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeImportLevyDeclarationsTasksCompleted_ThenShouldNotSendUpdateAccountBalanceCommands()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateAccountBalanceCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenSomeImportLevyDeclarationsTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetSomeImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.Is<ProcessLevyDeclarationsTimeoutCommand>(c => c.JobId == f.Job.Id), It.IsAny<SendOptions>()), Times.Once));
        }
        [Test]
        public Task Handle_WhenAllImportLevyDeclarationsTasksCompleted_ThenShouldUpdateJobProgress()
        {
            return TestAsync(f => f.SetAllImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.Job.Should().Match<ProcessLevyDeclarationsJob>(j =>
                j.ImportLevyDeclarationsTasksCompletedCount == f.Tasks.Count &&
                j.UpdateAccountBalanceTasksCompletedCount == 0 &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllImportLevyDeclarationsTasksCompleted_ThenShouldSendUpdateAccountBalanceCommands()
        {
            return TestAsync(f => f.SetAllImportLevyDeclarationTasksCompleted(), f => f.Handle(), f => f.Accounts.ForEach(a => f.UniformSession
                .Verify(s => s.Send(It.Is<UpdateAccountBalanceCommand>(c => c.JobId == f.Job.Id && c.AccountId == a.Id), It.IsAny<SendOptions>()), Times.Once)));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountBalanceTasksCompleted_ThenShouldUpdateJobProgress()
        {
            return TestAsync(f => f.SetSomeUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.Job.Should().Match<ProcessLevyDeclarationsJob>(j =>
                j.ImportLevyDeclarationsTasksCompletedCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountBalanceTasksCompletedCount == f.Tasks.Count &&
                !j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountBalanceTasksCompleted_ThenShouldNotSendUpdateAccountBalanceCommands()
        {
            return TestAsync(f => f.SetSomeUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateAccountBalanceCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenSomeUpdateAccountBalanceTasksCompleted_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetSomeUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.Is<ProcessLevyDeclarationsTimeoutCommand>(c => c.JobId == f.Job.Id), It.IsAny<SendOptions>()), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountBalanceTasksCompleted_ThenShouldUpdateJobProgress()
        {
            return TestAsync(f => f.SetAllUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.Job.Should().Match<ProcessLevyDeclarationsJob>(j =>
                j.ImportLevyDeclarationsTasksCompletedCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountBalanceTasksCompletedCount == f.Tasks.Count &&
                j.IsComplete &&
                j.Updated >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountBalanceTasksCompleted_ThenShouldNotSendUpdateAccountBalanceCommands()
        {
            return TestAsync(f => f.SetAllUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateAccountBalanceCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenAllUpdateAccountBalanceTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetAllUpdateAccountBalanceTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<ProcessLevyDeclarationsTimeoutCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotUpdateJobProgress()
        {
            return TestAsync(f => f.SetAllTasksCompleted(), f => f.Handle(), f => f.Job.Should().Match<ProcessLevyDeclarationsJob>(j =>
                j.ImportLevyDeclarationsTasksCompletedCount == f.AccountPayeSchemes.Count &&
                j.UpdateAccountBalanceTasksCompletedCount == f.Accounts.Count &&
                j.IsComplete &&
                j.Updated == f.Now));
        }
        
        [Test]
        public Task Handle_WhenAllAllTasksCompleted_ThenShouldNotSendUpdateAccountBalanceCommands()
        {
            return TestAsync(f => f.SetAllTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<UpdateAccountBalanceCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
        
        [Test]
        public Task Handle_WhenAllTasksCompleted_ThenShouldNotSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.SetAllTasksCompleted(), f => f.Handle(), f => f.UniformSession
                .Verify(s => s .Send(It.IsAny<ProcessLevyDeclarationsTimeoutCommand>(), It.IsAny<SendOptions>()), Times.Never));
        }
    }

    public class ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public List<string> PayeSchemeRefs { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }
        public ProcessLevyDeclarationsJob Job { get; set; }
        public List<ProcessLevyDeclarationsJobTask> Tasks { get; set; }
        public ProcessLevyDeclarationsTimeoutCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<ProcessLevyDeclarationsTimeoutCommand> Handler { get; set; }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            
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
            
            Job = new ProcessLevyDeclarationsJob(Now, AccountPayeSchemes);
            
            Command = new ProcessLevyDeclarationsTimeoutCommand(Job.Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();

            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.ProcessLevyDeclarationsJobs.AddRange(Job);
            Db.SaveChanges();
            
            Handler = new ProcessLevyDeclarationsTimeoutCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture SetSomeImportLevyDeclarationTasksCompleted()
        {
            Tasks = new List<ProcessLevyDeclarationsJobTask>
            {
                ProcessLevyDeclarationsJobTask.CreateImportLevyDeclarationsTask(Job.Id, AccountPayeSchemes[0].Id),
                ProcessLevyDeclarationsJobTask.CreateImportLevyDeclarationsTask(Job.Id, AccountPayeSchemes[2].Id)
            };
            
            Db.ProcessLevyDeclarationsJobTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture SetAllImportLevyDeclarationTasksCompleted()
        {
            Tasks = AccountPayeSchemes.Select(aps => ProcessLevyDeclarationsJobTask.CreateImportLevyDeclarationsTask(Job.Id, aps.Id)).ToList();
            
            Db.ProcessLevyDeclarationsJobTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture SetSomeUpdateAccountBalanceTasksCompleted()
        {
            Job.Set(j => j.ImportLevyDeclarationsTasksCompletedCount, AccountPayeSchemes.Count);
            
            Tasks = new List<ProcessLevyDeclarationsJobTask>
            {
                ProcessLevyDeclarationsJobTask.CreateUpdateAccountBalanceTask(Job.Id, AccountPayeSchemes[0].AccountId)
            };
            
            Db.ProcessLevyDeclarationsJobTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture SetAllUpdateAccountBalanceTasksCompleted()
        {
            Job.Set(j => j.ImportLevyDeclarationsTasksCompletedCount, AccountPayeSchemes.Count);
            
            Tasks = Accounts.Select(a => ProcessLevyDeclarationsJobTask.CreateUpdateAccountBalanceTask(Job.Id, a.Id)).ToList();;
            
            Db.ProcessLevyDeclarationsJobTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture SetAllTasksCompleted()
        {
            Job.Set(j => j.ImportLevyDeclarationsTasksCompletedCount, AccountPayeSchemes.Count);
            Job.Set(j => j.UpdateAccountBalanceTasksCompletedCount, Accounts.Count);
            Job.Set(j => j.IsComplete, true);
            Job.Set(j => j.Updated, Now);
            
            var importLevyDeclarationsTasks = AccountPayeSchemes.Select(aps => ProcessLevyDeclarationsJobTask.CreateImportLevyDeclarationsTask(Job.Id, aps.Id));
            var updateAccountBalanceTasks = Accounts.Select(a => ProcessLevyDeclarationsJobTask.CreateUpdateAccountBalanceTask(Job.Id, a.Id));

            Tasks = importLevyDeclarationsTasks.Concat(updateAccountBalanceTasks).ToList();
            
            Db.ProcessLevyDeclarationsJobTasks.AddRange(Tasks);
            Db.SaveChanges();
            
            return this;
        }
    }
}